using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectToRapidAPI : MonoBehaviour
{
    // Start is called before the first frame update

    public string _URL = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/jokes/random";
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest(_URL));

    }

    IEnumerator GetRequest(string uri)
    {

        UnityWebRequest w1 = new UnityWebRequest(uri) { downloadHandler = new DownloadHandlerBuffer() };
        w1.SetRequestHeader("x-rapidapi-key", "69c64357e6mshfafd205e447e98bp1b0617jsn84b39a79606d");
        w1.SetRequestHeader("x-rapidapi-host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");

        yield return w1.SendWebRequest();

        if (w1.isNetworkError || w1.isHttpError)
        {
            Debug.Log(w1.error);
        }
        else
        {
            Debug.Log($"Finished");
        }
        var jokeJson = w1.downloadHandler.text;
        var joke = JsonUtility.FromJson<ChuckNorrisJoke>(jokeJson);
        Debug.Log(joke.value);
    }

}