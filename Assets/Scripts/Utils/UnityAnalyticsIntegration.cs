using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	private static string projectId = "032e2591-7be4-434c-9c6b-88c9030d3675";
	static bool enabled = false;
	// Use this for initialization
	void Start () {
		if (DataManager.instance && !enabled) {
			UnityAnalytics.StartSDK (projectId);
			enabled = true;
		}
	}
	
}