using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class DataManager : MonoBehaviour {

	public static DataManager instance = null;
    private static string token        = "87859b6921509e0Au9sjR4ep8H9T1FED0g2JH65E";
    public string saveFile             = "jumpingWalls.txt";
    public string tag                  = "dictionary";
    public bool useEncrypt             = true;

    private Dictionary<string, System.Object> m_dDictionary;
	// Use this for initialization
	void Start () {
		if (instance == null){
			instance = this;
            load();
		}
	}

    void OnApplicationQuit()
    {
        save();
    }

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	public void GetData(){
	}

	public void Clean(){
	}

    public static void setHighScore(int score){
        if (instance.m_dDictionary.ContainsKey("HighScore")){
            int s = System.Convert.ToInt32( instance.m_dDictionary["HighScore"] );
            if (score > s)
                instance.m_dDictionary["HighScore"] = score;
        }
        else
            instance.m_dDictionary["HighScore"] = score;
    }

	private void save(){
		// Converting Dictionary to json...
        string s = Json.Serialize(m_dDictionary);
        Debug.Log("Saving..." + s);
        string encodedS = Base64Encode(s);
        ES2.Save(encodedS, calculateString(tag));
	}

    string calculateString(string tag){
        string str = "";
        str += saveFile + "?tag=";
        str += tag;

        if (useEncrypt){
            str += "&encrypt=true&password=";
            str += token;
        }
        return str;
    }

	private void load(){
		if(ES2.Exists(saveFile)){
            string load = ES2.Load<string>(calculateString(tag));
            string decodedLoad = Base64Decode(load);
            m_dDictionary = Json.Deserialize(decodedLoad) as Dictionary<string, System.Object>;
            Debug.Log("Load... " + decodedLoad);
        }
        else{
            Debug.Log("Creating empty Dictionary");
            m_dDictionary = new Dictionary<string, System.Object>();
        }
	}

    private static string Base64Encode(string plainText) {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData) {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}