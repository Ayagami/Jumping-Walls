using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	public Transform[] backgrounds;				// Array of all the backgrounds to be parallaxed.
	public float parallaxScale;					// The proportion of the camera's movement to move the backgrounds by.
	public float parallaxReductionFactor;		// How much less each successive layer should parallax.
	public float smoothing;						// How smooth the parallax effect should be.


	private Transform cam;						// Shorter reference to the main camera's transform.
	private Vector3 previousCamPos;				// The postion of the camera in the previous frame.

	private Vector3[] onEnablePos;
	private Bounds[]  boundsPointer;


	void Awake ()
	{
		// Setting up the reference shortcut.
		cam = Camera.main.transform;
		onEnablePos = new Vector3[backgrounds.Length];
		boundsPointer = new Bounds[backgrounds.Length];

		for (int i=0; i < backgrounds.Length; i++) {
			onEnablePos[i] = backgrounds[i].localPosition;	
		}
	}


	void OnEnable ()
	{
		// The 'previous frame' had the current frame's camera position.
		previousCamPos = cam.position;
		for (int i = 0; i < onEnablePos.Length; i++) {
			backgrounds[i].localPosition = onEnablePos[i];
			boundsPointer[i] = backgrounds[i].gameObject.GetComponent<Renderer>().bounds;
		}
	}


	void LateUpdate () {
		// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
		float parallax = (previousCamPos.y - cam.position.y) * parallaxScale;

		// For each successive background...
		for(int i = 0; i < backgrounds.Length; i++)
		{
			Vector3 aux  = Camera.main.WorldToViewportPoint( boundsPointer[i].min );
			Debug.Log(aux);
			if( aux.y > 1f)
				backgrounds[i].localPosition = onEnablePos[i];

			// ... set a target x position which is their current position plus the parallax multiplied by the reduction.
			float backgroundTargetPosY = backgrounds[i].position.y + parallax * (i * parallaxReductionFactor + 1);

			// Create a target position which is the background's current position but with it's target x position.
			Vector3 backgroundTargetPos = new Vector3(backgrounds[i].position.x, backgroundTargetPosY, backgrounds[i].position.z);

			// Lerp the background's position between itself and it's target position.
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		// Set the previousCamPos to the camera's position at the end of this frame.
		previousCamPos = cam.position;
	}
}
