using UnityEngine;
using UnityEngine.UI;

public class CoinView : MonoBehaviour {
	
	void Start(){
		OnNewPurchaseFunction(DataManager.getCoins());
		EventsSystem.onNewPurchase += OnNewPurchaseFunction;
	}

	void OnNewPurchaseFunction(int c){
		Text obj = this.GetComponent<Text>();
		if(obj)
			obj.text = c.ToString();
	}
	
	void OnDestroy(){
		EventsSystem.onNewPurchase -= OnNewPurchaseFunction;
	}
}
