using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CoronaInfoManager : MonoBehaviour
{
    // https://documenter.getpostman.com/view/10808728/SzS8rjbc?version=latest

    public TextMeshProUGUI Display1TMP, Display2TMP;
    public string url = "https://api.covid19api.com/summary";
    public CoronaInfo coronaInfo;
    public int countryCode = 132;
    private void Start()
    {
        OnButtonGetInfo();
    }
    public void OnButtonGetInfo()
    {
        StartCoroutine(GetInfo(url));
    }

    private IEnumerator GetInfo(string url)
    {

        UnityWebRequest www = new UnityWebRequest(url)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error has occur: " + www.error);
            yield break;
        }

        coronaInfo = JsonUtility.FromJson<CoronaInfo>(www.downloadHandler.text);

        Display1TMP.text =
             $"Global Corona Statistic: \n\n" +
           $"Date :{coronaInfo.Date}\n" +
           $"New Confirmed:{coronaInfo.Global.NewConfirmed}\n" +
           $"Total Confirmed: {coronaInfo.Global.TotalConfirmed}\n" +
           $"New Death: {coronaInfo.Global.NewDeaths}\n" +
           $"Total Death: {coronaInfo.Global.TotalDeaths}\n" +
           $"New Recovered: {coronaInfo.Global.NewRecovered}\n" +
           $"Total Recovered: {coronaInfo.Global.TotalRecovered}";

        Display2TMP.text = $"Country: {coronaInfo.Countries[countryCode].Slug}\n" +
             $"Country Code: {coronaInfo.Countries[countryCode].CountryCode}\n" +
             $"Date : {coronaInfo.Countries[countryCode].Date}\n\n" +
             $"New Confirmed Cases : {coronaInfo.Countries[countryCode].NewConfirmed}\n" +
             $"Total Confirmed Cases: {coronaInfo.Countries[countryCode].TotalConfirmed}\n" +
             $"New Deaths: {coronaInfo.Countries[countryCode].NewDeaths}\n" +
             $"Total Deaths: {coronaInfo.Countries[countryCode].TotalDeaths}\n" +
             $"New Recovered: {coronaInfo.Countries[countryCode].NewRecovered}\n" +
             $"Total Recovered: {coronaInfo.Countries[countryCode].TotalRecovered}";
    }

}
