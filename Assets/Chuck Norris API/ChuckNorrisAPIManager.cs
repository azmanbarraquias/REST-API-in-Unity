using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public enum Category { animal, career, celebrity, dev, @explicit, fashion, food, history, 
    money, movie, music, political, religion, science, sport, travel }

public class ChuckNorrisAPIManager : MonoBehaviour
{
    //https://api.chucknorris.io/
    private readonly string _URL = "https://api.chucknorris.io/jokes/";

    [Header("Display")]
    public TextMeshProUGUI categoryTMP;
    public TextMeshProUGUI jokePlaceholderTMP;
    public TMP_InputField searchFieldTMP;

    [Header("Name")]
    public new string name = "Azman";

    [Space]
    public ChuckNorrisJoke chucknorrisJoke;
    public SearchJoke chucknorrisJokes;


    public void GetJoke(int categoryIndex)
    {
        //string asd = categoryIndex.ToString();
        //foreach (var item in asd)
        //{
        //    Debug.Log()
        //}
        //https://api.chucknorris.io/jokes/random?category=dev,explicit
        //https://api.chucknorris.io/jokes/random?name=Bob&category=dev,explicit
        Category category = (Category)categoryIndex;
        string url = "random?category=" + category.ToString();
        StartCoroutine(SearchJoke(url));
    }

    private void Start()
    {
        jokePlaceholderTMP.text = "";
        GetRandomJoke();
    }

    public void GetRandomJoke()
    {
        //StartCoroutine(SearchJoke("random"));
        StartCoroutine(SearchJoke("random?name=" + name));
    }

    public void SearchJokes()
    {
        var toSearch = searchFieldTMP.text;
        if (!string.IsNullOrEmpty(toSearch))
        {
            StartCoroutine(SearchJoke("search?query=" + toSearch, true));
        }
       
    }

    private IEnumerator SearchJoke(string search, bool searchJokes = false)
    {
        jokePlaceholderTMP.text = "Loading. . .";

        string finalURL = _URL + search;

        UnityWebRequest www = new UnityWebRequest(finalURL)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {

            jokePlaceholderTMP.text = "Error";
            Debug.Log("Error has occur: " + www.error);
            yield break;
        }

        if (searchJokes == false)
        {
            chucknorrisJoke = JsonUtility.FromJson<ChuckNorrisJoke>(www.downloadHandler.text);
            categoryTMP.text = chucknorrisJoke.categories.Count > 0 ? chucknorrisJoke.categories[0] : "";
            jokePlaceholderTMP.text = chucknorrisJoke.value;
            Debug.Log(chucknorrisJoke.value);
        }
        else
        {
            chucknorrisJokes = JsonUtility.FromJson<SearchJoke>(www.downloadHandler.text);
            if (chucknorrisJokes.total > 0)
            {
                int rand = Random.Range(0, chucknorrisJokes.result.Count);
                ChuckNorrisJoke joke = chucknorrisJokes.result[rand];
                jokePlaceholderTMP.text = joke.value;
                categoryTMP.text = joke.categories.Count > 0 ? joke.categories[0] : "";
            }
        }
       
    }
}
