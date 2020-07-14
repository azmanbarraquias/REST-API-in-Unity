using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChuckNorrisJoke
{
    public List<string> categories;
    public string created_at;
    public string icon_url;
    public string id;
    public string updated_at;
    public string url;
    public string value;
}

[System.Serializable]
public class SearchJoke
{
    public int total;
    public List<ChuckNorrisJoke> result;
}

