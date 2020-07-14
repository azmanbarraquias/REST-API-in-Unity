using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class YuGiOhAPIManager : MonoBehaviour
{
	// https://documenter.getpostman.com/view/10808728/SzS8rjbc?version=latest

	public TextMeshProUGUI nameTMP, descriptionTMP, atckTMP, defTMP, lvlTMP, typeTMP;
	public RawImage imagePlaceHolder;
	public string url = "https://db.ygoprodeck.com/api/v6/cardinfo.php";

	public Cards cards;

	private void Start()
	{
		nameTMP.text = "";
		descriptionTMP.text = "";
		atckTMP.text = "";
		defTMP.text = "";
		lvlTMP.text = "";
		typeTMP.text = "";
		StartCoroutine(GetCardInfo(url));
	}

	private IEnumerator GetCardInfo(string url)
	{
		nameTMP.text = "Loading. . . .";
		descriptionTMP.text = ". . . .";

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

		var jsonData = FixJson("cards", www.downloadHandler.text);
		Debug.Log(jsonData);
		cards = JsonUtility.FromJson<Cards>(jsonData);
		Resizer.Resize(cards.cards, 100, true);

		var card = cards.cards[Random.Range(0, cards.cards.Count)];
		nameTMP.text = card.name;
		descriptionTMP.text = card.desc;
		atckTMP.text = card.atk.ToString();
		defTMP.text = card.def.ToString();
		lvlTMP.text = "Level: " + card.level.ToString();
		typeTMP.text = !string.IsNullOrWhiteSpace(card.archetype) ? "[" + card.archetype + "]" : "";

		UnityWebRequest wwwTexture = UnityWebRequestTexture.GetTexture(card.card_images[0].image_url);

		var asyncOperation2 = wwwTexture.SendWebRequest();

		if (wwwTexture.isNetworkError || wwwTexture.isHttpError)
		{
			Debug.Log("Error has occur: " + www.error);
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

		imagePlaceHolder.texture = DownloadHandlerTexture.GetContent(wwwTexture);
		imagePlaceHolder.texture.filterMode = FilterMode.Point;

	}


	public string FixJson(string name, string value)
	{
		value = "{\"" + name + "\": " + value + "}";
		return value;
	}

}

public static class Resizer
{
	public static void Resize<T>(this List<T> list, int newCount, bool removeRandomly = false)
	{
		if (newCount <= 0)
		{
			list.Clear();
		}
		else
		{
			while (list.Count > newCount)
			{
				if (removeRandomly)
					list.RemoveAt(Random.Range(0, list.Count - 1));
				else
					list.RemoveAt(list.Count - 1);
			}
			//while (list.Count < newCount)
			//{
			//    list.Add(default(T));
			//}
		}
	}
}
