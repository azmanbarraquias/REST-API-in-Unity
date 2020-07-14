using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadLocalJsonData : MonoBehaviour
{
    [Header("LoadLocalJson1")]
    public TextMeshProUGUI textMeshProGUI;

    public string path;

    public string fileName;

    public string jsonString;

    public DigimonList digimonList;

    [Header("LoadLocalJson2")]
    public string longPath;

    private void Start()
    {
        //Debug.Log(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
        Debug.Log(Application.dataPath);// game folder
        Debug.Log(Application.persistentDataPath); // secure folder

        //LoadLocalJson1();
        LoadLocalJson2();
    }
   
    public void LoadLocalJson1()
    {
        path = Application.streamingAssetsPath + "/" + fileName + ".json";
        Debug.Log(Application.streamingAssetsPath);
        jsonString = File.ReadAllText(path);
        digimonList = JsonUtility.FromJson<DigimonList>(jsonString);
        var digimonJson = JsonUtility.ToJson(digimonList);
        Debug.Log(digimonJson);
        foreach (var digimon in digimonList.digimons)
        {
            textMeshProGUI.text += digimon.name + ",     ";
        }
    }

    public void LoadLocalJson2()
    {
        // Read
        using (StreamReader streamRead = new StreamReader(longPath))
        {
            string json = streamRead.ReadToEnd();
            digimonList = JsonUtility.FromJson<DigimonList>(json);
            streamRead.Close();
        }

        foreach (var digimon in digimonList.digimons)
        {
            textMeshProGUI.text += digimon.name + ",     ";
        }

        // Write
        using (StreamWriter streamWrite = new StreamWriter(longPath))
        {
            string json = JsonUtility.ToJson(digimonList);
            streamWrite.Write(json);
            streamWrite.Close();
        }

    }

    //public static void SaveDigimonr(Digimon digimon)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    // Application.persistentDataPath this will get a path to a data directory on the operating system that will not change
    //    // string path = Path.Combine(Application.persistentDataPath, "/player.fun");
    //   // string path = Application.persistentDataPath + "/player.fun";
    //    string path = Application.dataPath + "/Digimon API/" + digimon.name + ".json";


    //    // Filestream is a stream of data contain in a file, and we can use a particular file steam to read and write form a file.
    //    // FileMode is a an action you want to do the the file
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    // data to write 
    //    Digimon digimonData = new Digimon();

    //    // incert to a file, convert the data to binary, Serialize write down to the file
    //    formatter.Serialize(stream, digimonData);

    //    Debug.Log("File has being save to " + path);

    //    stream.Close(); // close the stream after writing data
    //}

    //public static Digimon LoadDigimon()
    //{
    //    // Application.persistentDataPath this will get a path to a data directory on the operating system that will not change
    //    string path = Application.persistentDataPath + "/player.fun";
    //    //string path = Path.Combine(Application.persistentDataPath, "/player.fun");


    //    if (File.Exists(path))
    //    {
    //        //check if the file exist
    //        BinaryFormatter formatter = new BinaryFormatter();

    //        // Open the file
    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        // read the file stream, the convert to the player data
    //        Digimon playerData = formatter.Deserialize(stream) as Digimon;
    //        stream.Close(); // close the stream after writing data
    //        Debug.Log("File has being loaded from " + path);
    //        return playerData;

    //    }
    //    else
    //    {
    //        Debug.Log("Save file not found in " + path);
    //        Debug.LogError("Save file not found in " + path);
    //        return null;
    //    }
    //}

}
