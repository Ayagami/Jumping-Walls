using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyButtonBehaviour : ButtonAction {
	public Color _disabledColor = Color.gray;
	private Color _originalColor;
	public BuyableManager.BuyableId id;
	
	private Image imgPtr = null;
	
	public override void OnStart(){
		base.OnStart();
		imgPtr = this.GetComponentInChildren<Image>() as Image;
		_originalColor = imgPtr.color;
		OnNewPurchase(DataManager.getCoins());
		
		EventsSystem.onNewPurchase += OnNewPurchase;
	}
	
	public override void execute(){
		DataManager.buyItem(id);
	}
	
	void OnNewPurchase(int coinsLeft){
		if(imgPtr != null){
			if(!DataManager.canBuyItem(id)){
				imgPtr.color = _disabledColor;
				bt.interactable = false;
			}else{
				imgPtr.color = _originalColor;
				bt.interactable = true;
			}
		}
	}
}
