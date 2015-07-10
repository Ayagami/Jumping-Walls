using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	private static string projectId = "032e2591-7be4-434c-9c6b-88c9030d3675";
	static bool started = false;
	// Use this for initialization
	void Start () {
		if (DataManager.instance && !started) {
			UnityAnalytics.StartSDK (projectId);
			started = true;
		}
	}
	
	public void sendBuyToAnalytics(string object_id, decimal value){
		if(started){
			UnityAnalytics.Transaction(object_id, value, "USD", null, null);
		}else{
			Debug.Log("Not started...");
		}
	}
	
}