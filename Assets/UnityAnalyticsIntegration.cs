using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	public string projectId = "032e2591-7be4-434c-9c6b-88c9030d3675";
	// Use this for initialization
	void Start () {
		

		UnityAnalytics.StartSDK (projectId);
		Debug.Log ("UnityAnalytics.StartSDK");
	}
	
}