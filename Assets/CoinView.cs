using UnityEngine;
using UnityEngine.UI;

public class CoinView : MonoBehaviour {

	public Text textObj = null;	
	
	void Start(){
		OnNewPurchaseFunction(DataManager.getCoins());
		EventsSystem.onNewPurchase += OnNewPurchaseFunction;
	}

	void OnNewPurchaseFunction(int c){
			textObj.text = c.ToString();
	}
}
