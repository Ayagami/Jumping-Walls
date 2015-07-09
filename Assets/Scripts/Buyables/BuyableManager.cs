using System.Collections.Generic;
using UnityEngine;

public class BuyableManager {
	public static string BUYABLE_DESTROYBLOCK = "DESTROY_BLOCK";
	public static string BUYABLE_NOADS = "NO_ADS";
	Dictionary<BuyableId, Buyable> m_dBuyables = null;
	
	public enum BuyableId{
		NoAds,
		DestroyBlock,
		None
	}
	
	public BuyableManager(){
		m_dBuyables = new Dictionary<BuyableId, Buyable> ();
		m_dBuyables[BuyableId.NoAds] = new Buyable(500000, BuyableId.NoAds, BUYABLE_NOADS);
		m_dBuyables[BuyableId.DestroyBlock] = new Buyable(20000, BuyableId.DestroyBlock, BUYABLE_DESTROYBLOCK);	
	}
	
	public bool canBuy(BuyableId obj){
		if(m_dBuyables == null) 
			return false;
			
		if(!m_dBuyables.ContainsKey(obj))
			return false;
			
		Buyable currentBuy = m_dBuyables[obj];
		
		return currentBuy.isMaxed();
	}
	
	public bool buy(BuyableId obj){
		if(canBuy(obj)){
			m_dBuyables[obj].addLevel();
			return true;
		}
		
		return false;
		/*
		using (AndroidJavaClass ajo = new AndroidJavaClass("com.ligool.plugin.Main")) {
                return ajo.CallStatic<bool>("buyObject", (int)obj);
        }*/
	}
	
	public Buyable getBuyable(BuyableId id){
		if(m_dBuyables.ContainsKey(id)){
			return m_dBuyables[id];
		}
		
		return null;
	}
	
	public Buyable getBuyable(string _ID){
		BuyableId bID = BuyableId.None;
		if(_ID == BUYABLE_DESTROYBLOCK)
			bID = BuyableId.DestroyBlock;
			
		if(_ID == BUYABLE_NOADS)
			bID = BuyableId.NoAds;
		
		return getBuyable(bID);
		
	}
}

public class Buyable {
	private int m_iCost;
	private BuyableManager.BuyableId m_iID;
	
	private bool m_bLevelable;
	
	private int m_iCurentlevel = 0;
	private int m_iMaxLevel = 1;
	
	private string m_sTag = "";
	
	public Buyable(int _cost, BuyableManager.BuyableId _id, string _tag, bool _levelable = false){
		this.m_iCost = _cost;
		this.m_iID   = _id;
		this.m_sTag  = _tag;
		this.m_bLevelable = _levelable;
	}
	
	public bool isLevelable() { return m_bLevelable; }
	
	public bool isMaxed(){
		return !(m_iCurentlevel >= m_iMaxLevel);
	}
	
	public string getTag(){
		return m_sTag;
	}
	
	public int getCost(){
		return m_iCost;
	}
	
	public void addLevel(){
		m_iCurentlevel++;
	}
	
	public void setLevel(int _level){
		m_iCurentlevel = _level;
	}
}
