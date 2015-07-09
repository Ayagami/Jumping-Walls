using UnityEngine;
using System.Collections.Generic;
using MiniJSON;
using GoogleMobileAds.Api;
public class DataManager : MonoBehaviour {

	public static DataManager instance = null;
    private static string token        = "87859b6921509e0Au9sjR4ep8H9T1FED0g2JH65E";
    private static string androidAdT   = "ca-app-pub-2407766428808821/7321542790";
    public string saveFile             = "jumpingWalls.txt";
    public string NameTag              = "dictionary";
    public bool useEncrypt             = true;

    private Dictionary<string, System.Object> m_dDictionary = null;
    private BannerView bw = null;
    
    private GameObject g = null;
    private GameObject shop = null;
    
    private BuyableManager m_bmManager = null;
    
	void Awake () {
		if (instance == null){
            
		    DontDestroyOnLoad (this.gameObject);
            
            instance = this;
            
            m_bmManager = new BuyableManager();
            
            g = GameObject.FindGameObjectWithTag("PPYN") as GameObject;
            
            load();
 
            // Cheking Properties...
            
            if(m_dDictionary != null && g != null){     //Cheking first time running the app.
                g.SetActive(!m_dDictionary.ContainsKey("NickName"));
            }
            
            #if UNITY_ANDROID
            if(Application.isMobilePlatform){
                if( !ExistsDataOnDictionary(BuyableManager.BUYABLE_NOADS) )
                    createAds();
            }
            #endif
		}
	}

    void createAds(){  
        bw = new BannerView(androidAdT, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bw.LoadAd(request);
        bw.Hide();
    }

    void OnApplicationQuit() {
        save();
    }
	public void GetData(){
	}
    
    public static void enableAds(bool toggle){
        if(instance == null) return;
        
        if(instance.bw == null) return;
        
        if(ExistsDataOnDictionary(BuyableManager.BUYABLE_NOADS)) return;
        
        if(toggle)
            instance.bw.Show();
        else
            instance.bw.Hide();
    }

	public void OnSceneChange(){
         g = GameObject.FindGameObjectWithTag("PPYN") as GameObject;
         if(m_dDictionary != null && g != null){
            g.SetActive(!m_dDictionary.ContainsKey("NickName"));
         }
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
    
    public static bool BuyCoins(int cuantity){
       /* #if UNITY_ANDROID
        addCoins(cuantity);
        EventsSystem.makeNewPurchaseTrigger(getCoins());
        return true; 
        #else
        if(Application.isMobilePlatform){
            using (AndroidJavaClass ajo = new AndroidJavaClass("com.ligool.plugin.Main")) {
                    bool c = ajo.CallStatic<bool>("buyCoins");
                    Debug.Log("Boolean en Unity es " + c);
                    if(c){
                        addCoins(cuantity);
                        EventsSystem.makeNewPurchaseTrigger(getCoins());
                        return true;
                    }
                    
                    Debug.Log("No pudiste comprar amigo.");
                    return false;
            }
        }
        
        return false;
       #endif*/
       
       if(Application.isMobilePlatform){
           using (AndroidJavaClass ajo = new AndroidJavaClass("com.ligool.plugin.Main")) {
                    bool c = ajo.CallStatic<bool>("buyCoins");
                    Debug.Log("Boolean en Unity es " + c);
                    if(c){
                        addCoins(cuantity);
                        EventsSystem.makeNewPurchaseTrigger(getCoins());
                        return true;
                    }
                    
                    Debug.Log("No pudiste comprar amigo.");
                    return false;
            }
       }else{
            addCoins(cuantity);
            EventsSystem.makeNewPurchaseTrigger(getCoins());
            return true; 
       }
   }
    public static void addCoins(int newCoins){
        if (instance.m_dDictionary.ContainsKey("Coins")){
            int s = System.Convert.ToInt32( instance.m_dDictionary["Coins"] );
            instance.m_dDictionary["Coins"] = newCoins + s;
        }
        else
            instance.m_dDictionary["Coins"] = newCoins;
    }
    
    public static int getCoins(){
        if(ExistsDataOnDictionary("Coins"))
            return System.Convert.ToInt32( instance.m_dDictionary["Coins"]);
        else
            return 0;
    }
    
    public static bool canBuyItem(BuyableManager.BuyableId bID){
        if(instance == null) return false;
        if(instance.m_dDictionary == null) return false;
        if(!instance.m_dDictionary.ContainsKey("Coins")) return false;
        
        if(instance.m_bmManager.canBuy(bID)){
            Buyable buy = instance.m_bmManager.getBuyable(bID);
            if(buy != null){
               int cost  = buy.getCost();
               int myCoins = System.Convert.ToInt32(instance.m_dDictionary["Coins"]);
               return (cost <= myCoins);
            }
        }
        
        return false;
    }
    
    public static bool buyItem(BuyableManager.BuyableId bID){
        if(instance.m_bmManager.buy(bID)){
           Debug.Log("Inside if");
           Buyable buy = instance.m_bmManager.getBuyable(bID);
           instance.m_dDictionary[buy.getTag()] = 1;
           
           int cost  = buy.getCost();
           int myCoins = System.Convert.ToInt32(instance.m_dDictionary["Coins"]);
           int newCoins = myCoins - cost;
           instance.m_dDictionary["Coins"] = newCoins;
           EventsSystem.makeNewPurchaseTrigger(newCoins);
           return true;
        }
        
        Debug.Log("outside if");
        return false;
    }
    
    public static Buyable getBuyable(BuyableManager.BuyableId bID){
        
        if(instance == null) return null;
        if(instance.m_bmManager == null) return null;
        
        return instance.m_bmManager.getBuyable(bID);
    }

    public static bool ExistsDataOnDictionary(string key){
        if(instance == null) return false;
        if(instance.m_dDictionary == null)
            return false;
            
       return instance.m_dDictionary.ContainsKey(key);
    }

    public static void setNickName(string s){
        instance.m_dDictionary["NickName"] = s;
        instance.g.SetActive(false);
    }
    
	private void save(){
		// Converting Dictionary to json...
        string s = Json.Serialize(m_dDictionary);
        Debug.Log("Saving..." + s);
        string encodedS = Base64Encode(s);
        ES2.Save(encodedS, calculateString(NameTag));
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
            string load = ES2.Load<string>(calculateString(NameTag));
            string decodedLoad = Base64Decode(load);
            m_dDictionary = Json.Deserialize(decodedLoad) as Dictionary<string, System.Object>;
            Debug.Log("Load... " + decodedLoad);
            if(decodedLoad == null || decodedLoad == "null"){
                Debug.LogError("Hey, decoded load cannot be NULL!!!");
                ES2.Delete(saveFile);
            }
            // Cargo las In-Apps desde mi dictionary.
            Buyable b = null;
            // ------------------------
            if(ExistsDataOnDictionary(BuyableManager.BUYABLE_DESTROYBLOCK)){
               b =  m_bmManager.getBuyable(BuyableManager.BUYABLE_DESTROYBLOCK);
               if(b != null){
                    int v = System.Convert.ToInt32(instance.m_dDictionary[BuyableManager.BUYABLE_DESTROYBLOCK]); 
                    b.setLevel(v);
                    b = null; 
               }
            }
            
            if(ExistsDataOnDictionary(BuyableManager.BUYABLE_NOADS)){
               b =  m_bmManager.getBuyable(BuyableManager.BUYABLE_NOADS);
               if(b != null){
                    int v = System.Convert.ToInt32(instance.m_dDictionary[BuyableManager.BUYABLE_NOADS]); 
                    b.setLevel(v);
                    b = null; 
               }
            }
            // ---------------------------- 
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