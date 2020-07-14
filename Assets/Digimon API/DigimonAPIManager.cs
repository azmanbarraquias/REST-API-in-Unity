using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

//public enum Level { Champion, Fresh, InTraining, Mega, Rookie, Ultimate };

public class DigimonAPIManager : MonoBehaviour
{
	// https://digimon-api.herokuapp.com/index.html

	public readonly string url = "https://digimon-api.herokuapp.com/api/digimon/";

	[Header("Display")]
	public TextMeshProUGUI digimonNameTMP, digimonLevelTMP;
	public RawImage digimonImagePlaceholder;

	[Space]
	public DigimonList digimonList;

	private void Start()
	{
		StartCoroutine(GetDigimon());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StopAllCoroutines();
			StartCoroutine(GetDigimon());
		}
	}

	public void GetDigimonButton()
	{
		StopAllCoroutines();
		StartCoroutine(GetDigimon());
	}

	private IEnumerator GetDigimon()
	{
		digimonNameTMP.text = "Loading. . .";
		digimonLevelTMP.text = "Loading. . .";

		var www = new UnityWebRequest(url)
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		var asyncOperation1 = www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			//error
			yield break;
		}
		else
		{
			while (!www.isDone)
			{
				Debug.Log("getting data progress: " + asyncOperation1.progress);
				yield return null;
			}
		}


		var FinalizeJsonData
			= FixJson("digimons", www.downloadHandler.text);
		Debug.Log(FinalizeJsonData);
		digimonList = JsonUtility.FromJson<DigimonList>(FinalizeJsonData);
		int index = Random.Range(0, digimonList.digimons.Count);
		Digimon digimon = digimonList.digimons[index];
		SaveToSystem(digimon);

		digimonNameTMP.text = digimon.name;
		digimonLevelTMP.text = digimon.level;

		UnityWebRequest digimonSpriteRequest = UnityWebRequestTexture.GetTexture(digimon.img);

		var asyncOperation2 = digimonSpriteRequest.SendWebRequest();

		if (digimonSpriteRequest.isNetworkError || digimonSpriteRequest.isHttpError)
		{
			Debug.Log("Error has occur: " + digimonSpriteRequest.error);
			yield break;
		}
		else
		{
			while (!asyncOperation2.isDone)
			{
				Debug.Log("Image progress: " + asyncOperation2.progress);
				yield return null;
			}
		}

		digimonImagePlaceholder.texture = DownloadHandlerTexture.GetContent(digimonSpriteRequest);
		digimonImagePlaceholder.texture.filterMode = FilterMode.Point;

	}

	public void SaveToSystem(Digimon digimon)
	{
		Debug.Log("On Create");
		var path = Application.dataPath + "/Digimon API/DigimonJson/" + digimon.name + ".json";

		Debug.Log(path);
		if (!File.Exists(path))
		{
			Debug.Log("Created");
			File.WriteAllText(path, JsonUtility.ToJson(digimon, true));
		}
		else
		{
			Debug.Log("FailExist");
		}
	}

	public string FixJson(string name, string value)
	{
		value = "{\"" + name + "\": " + value + "}";
		return value;
	}

	//public string LevelConverter(Level level)
	//{
	//    switch (level)
	//    {
	//        case Level.Champion:
	//            return Level.Champion.ToString();
	//        case Level.Fresh:
	//            return Level.Fresh.ToString();
	//        case Level.InTraining:
	//            return Level.InTraining.ToString();
	//        case Level.Mega:
	//            return Level.Mega.ToString();
	//        case Level.Rookie:
	//            return Level.Rookie.ToString();
	//        case Level.Ultimate:
	//            return Level.Ultimate.ToString();
	//        default:
	//            return Level.Champion.ToString();
	//    }
	//}

}

[System.Serializable]
public class Digimon
{
	public string name;
	public string img;
	public string level;
}

[System.Serializable]
public class DigimonList
{
	public List<Digimon> digimons;
}
