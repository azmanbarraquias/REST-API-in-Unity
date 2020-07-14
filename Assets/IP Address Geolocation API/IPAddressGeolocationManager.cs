using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public enum Localization { English, Deutsch, Español, PortuguêsBrasil, Français, Japanese, Chinese, Russian }
public class IPAddressGeolocationManager : MonoBehaviour
{
    // https://ip-api.com/
    // http://bot.whatismyipaddress.com/

    [Header("IP Address Geolocation Manager")]
    public string _IPAddress;

    [Header("Setting")]
    public Localization localization;
    public bool autoGetIP = true;
    public bool viewMoreDetail = false;
    [Space]
    public GeolocationInfo geolocationInfo;

    [Header("Display")]
    public TMP_InputField inputField;
    public TextMeshProUGUI geolocationInfooTMP1, geolocationInfooTMP2, ipAddressTMP;

    private void Start()
    {
        if (autoGetIP)
            StartCoroutine(GetIPAddress());
        //else
        //    StartCoroutine(GetGeolocation(_IPAddress));
    }

    public void SearchIPAndGeolocation()
    {
        string toSearch = inputField.text;
        if (!string.IsNullOrWhiteSpace(toSearch))
        {
            StartCoroutine(GetGeolocation(toSearch));
        }
    }

    public IEnumerator GetIPAddress()
    {
        var www = new UnityWebRequest("http://bot.whatismyipaddress.com/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //error
            yield break;
        }

        _IPAddress = www.downloadHandler.text;
        Debug.Log(_IPAddress);
        StartCoroutine(GetGeolocation(_IPAddress));
    }

    public IEnumerator GetGeolocation(string ipAddress)
    {
        geolocationInfooTMP1.text = "Loading. . . .";
        geolocationInfooTMP2.text = "Loading. . . .";
        ipAddressTMP.text = "IP Address: . . .";
        inputField.text = "";

        if (viewMoreDetail)
        { ipAddress += "?fields=status,message,continent,continentCode,country,countryCode,region,regionName,city,district,zip,lat,lon,timezone,currency,isp,org,as,asname,reverse,mobile,proxy,hosting,query"; }

        ipAddress += (viewMoreDetail ? "&" : "?") + "lang=" + LocalizationString(localization);

        var www = new UnityWebRequest("http://ip-api.com/json/" + ipAddress)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        Debug.Log("http://ip-api.com/json/" + ipAddress);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //error
            yield break;
        }
        geolocationInfo = JsonUtility.FromJson<GeolocationInfo>(www.downloadHandler.text);
        DisplayInfo(geolocationInfo);
    }

    public void DisplayInfo(GeolocationInfo _geolocationInfo)
    {
        ipAddressTMP.text = "IP Address: " + _geolocationInfo.query;
        inputField.text = _geolocationInfo.query;

        geolocationInfooTMP1.text =
            $"status: {_geolocationInfo.status}\n" +
            $"continent: {_geolocationInfo.continent}\n" +
            $"continentCode: {_geolocationInfo.continentCode}\n" +
            $"country: {_geolocationInfo.country}\n" +
            $"countryCode: {_geolocationInfo.countryCode}\n" +
            $"region: {_geolocationInfo.region}\n" +
            $"regionName: {_geolocationInfo.regionName}\n" +
            $"city: {_geolocationInfo.city}\n" +
            $"district: {_geolocationInfo.district}\n" +
            $"zip: {_geolocationInfo.zip}\n" +
            $"lat: {_geolocationInfo.lat}\n" +
            $"lon: {_geolocationInfo.lon}";
           
        geolocationInfooTMP2.text = 
            $"timezone: {_geolocationInfo.timezone}\n" +
            $"currency: {_geolocationInfo.currency}\n" +
            $"isp: {_geolocationInfo.isp}\n" +
            $"org: {_geolocationInfo.org}\n" +
            $"as: {_geolocationInfo.@as}\n" +
            $"asname: {_geolocationInfo.asname}\n" +
            $"reverse: {_geolocationInfo.reverse}\n" +
            $"mobile: {_geolocationInfo.mobile}\n" +
            $"proxy: {_geolocationInfo.proxy}\n" +
            $"hosting: {_geolocationInfo.hosting}\n" +
            $"query: {_geolocationInfo.query}";
    }

    private string LocalizationString(Localization localization)
    {
        switch (localization)
        {
            case Localization.English:
                return "en";
            case Localization.Deutsch:
                return "de";
            case Localization.Español:
                return "es";
            case Localization.PortuguêsBrasil:
                return "pt-BR";
            case Localization.Français:
                return "fr";
            case Localization.Japanese:
                return "ja";
            case Localization.Chinese:
                return "zh-CN";
            case Localization.Russian:
                return "ru";
            default:
                return "en";
        }
    }
}
