using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public enum MainControls{
		DESTROY_BLOCK_ACTION,
		JUMP_ACTION
	};
	private static Detector inputDetector = null;
	// Use this for initialization
	void Start () {
			if(Application.isMobilePlatform)
				inputDetector = new SwipeDetector();
			if(Application.isEditor)
				inputDetector = new TouchDetector();		
	}
	void Update(){
		inputDetector.OnUpdate();
	}
}
// -------------------------------------------------------------------------------------
public class Detector{
	public virtual void OnUpdate(){
	}
}
// -------------------------------------------------------------------------------------
public class TouchDetector: Detector {
	public override void OnUpdate(){
		if(Input.GetMouseButtonDown(0)){
			EventsSystem.sendNewInputsystem(InputManager.MainControls.JUMP_ACTION);
			// Click en inspector
		}
		
		if(Input.GetKeyDown(KeyCode.B)){
			// PowerUP en inspector
			EventsSystem.sendNewInputsystem(InputManager.MainControls.DESTROY_BLOCK_ACTION);
		}
	}
}
// -------------------------------------------------------------------------------------
public class SwipeDetector : Detector {

  private float fingerStartTime  = 0.0f;
  private Vector2 fingerStartPos = Vector2.zero;
  
  private bool isSwipe = false;
  private float minSwipeDist  = 50.0f;
  private float maxSwipeTime = 0.5f;

  
  // Update is called once per frame
  public override void OnUpdate () {
    if (Input.touchCount > 0){

      foreach (Touch touch in Input.touches){
      	switch (touch.phase){
	        case TouchPhase.Began :
	          /* this is a new touch */
	          isSwipe = true;
	          fingerStartTime = Time.time;
	          fingerStartPos = touch.position;
	          break;
	          
	        case TouchPhase.Canceled :
	          /* The touch is being canceled */
	          isSwipe = false;
	          break;
	          
	        case TouchPhase.Ended :
	
	          float gestureTime = Time.time - fingerStartTime;
	          float gestureDist = (touch.position - fingerStartPos).magnitude;
	            
	          if (isSwipe && gestureTime < maxSwipeTime){
				  
				if(gestureDist > minSwipeDist){
		            Vector2 direction = touch.position - fingerStartPos;
		            Vector2 swipeType = Vector2.zero;
		            
		            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
		              // the swipe is horizontal:
		              swipeType = Vector2.right * Mathf.Sign(direction.x);
		            }else{
		              // the swipe is vertical:
		              swipeType = Vector2.up * Mathf.Sign(direction.y);
		            }
		
		            if(swipeType.x != 0.0f){
		              if(swipeType.x > 0.0f){
		                Debug.Log("Swipe Right");
		              }else{
		                Debug.Log("Swipe Left");
		              }
		            }
		
		            if(swipeType.y != 0.0f ){
		              if(swipeType.y > 0.0f){
		                EventsSystem.sendNewInputsystem(InputManager.MainControls.DESTROY_BLOCK_ACTION);
		              }else{
		                Debug.Log("Swipe Down");
		              }
		            }
					
				}
				else{
					EventsSystem.sendNewInputsystem(InputManager.MainControls.JUMP_ACTION);
				}
				
	          }
	        
	          break;
        }
      }
    }

  }
}
// -------------------------------------------------------------------------------------