using UnityEngine;
using System.Collections;

public class BuyCoins : ButtonAction {
	public int cuantity = 1000;
	public override void execute(){
		DataManager.BuyCoins(cuantity);
	}
}
