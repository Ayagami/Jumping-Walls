using UnityEngine;
using System.Collections;

public class CreditsVertical : MonoBehaviour
{
    public Texture2D gameLogo;

    public Role[] Roles;

    public string sceneToGoBackTo;

    // Credits scrolling
    private Vector2 scrollPosition;
    private float prevScrollPosition = 0f;
    private float currScrollPosition = -1f;
    private float autoScrollingSpeed = 40f;

    public GUIStyle titleStyle;
    public GUIStyle roleStyle;
    public GUIStyle nameStyle;
    public GUIStyle backButtonStyle;
    public GUIStyle logoStyle;

    // Touch controls
    private float scrollSpeed = 0f;
    private float timeTouchPhaseEnded = 0f;
    private const float inertiaDuration = 0.75f;
    private float lastDeltaPosition = 0.0f;

    // Font resizing according to screen size
    private float oldWidth;
    private float oldHeight;
    private float fontSize = 16;
    private const float ratio = 320;

    // Back button
    public Texture2D backButton;
    private float backButtonRatio = 1;
    private float backButtonWidth;
    private float backButtonHeight;
    private float backButtonOffset = 10;

    void Start()
    {
        /*backButtonRatio = (float)backButton.width / (float)backButton.height;
        backButtonWidth = Screen.width / 3;
        backButtonHeight = backButtonWidth / backButtonRatio;*/
    }

    // Scrolls the credits with touch
    // http://stackoverflow.com/a/20631963
    // http://www.mindthecube.com/blog/2010/09/adding-iphone-touches-to-unitygui-scrollview
    void scrollWithTouch()
    {
        if (Input.touchCount > 0)
        {
            autoScrollingSpeed = 0;

            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                scrollSpeed = 0f; // fully stop drag when new touch begins
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                scrollPosition.y += touch.deltaPosition.y; // drag

                // touch.deltaPosition must be saved for scrolling to work in Android
                // this happens because touch.deltaPosition is reset to 0
                // when touch.phase is Ended instead of keeping the last value
                lastDeltaPosition = touch.deltaPosition.y; // save deltaPosition.y
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // impart momentum, using last delta as the starting velocity
                // ignore delta < 10; precision issues can cause ultra-high velocity
                if (Mathf.Abs(lastDeltaPosition) >= 10)
                    scrollSpeed = (int)(lastDeltaPosition / touch.deltaTime);

                if (scrollSpeed == 0.0f)
                {
                    autoScrollingSpeed = 40f;
                    currScrollPosition = -1f;
                }

                timeTouchPhaseEnded = Time.time; // save time when touch eded
            }
        }
        else
        {
            if (scrollSpeed != 0.0f)
            {
                // slow down over time
                float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;

                /*if (scrollPosition.y <= 0 || scrollPosition.y >= (numRows*rowSize.y - listSize.y))
{
    // bounce back if top or bottom reached
    scrollVelocity = -scrollVelocity;
}*/

                float frameVelocity = Mathf.Lerp(scrollSpeed, 0, t);
                scrollPosition.y += frameVelocity * Time.deltaTime;

                // after N seconds, we've stopped
                if (t >= 1.0f)
                {
                    scrollSpeed = 0.0f;
                    autoScrollingSpeed = 30f;
                    currScrollPosition = -1f;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        scrollWithTouch();

        // Change text size according to screen size
        // http://forum.unity3d.com/threads/102876-Changing-Text-Size-Relative-To-Screen
        if (oldWidth != Screen.width || oldHeight != Screen.height)
        {
            oldWidth = Screen.width;
            oldHeight = Screen.height;
            fontSize = Mathf.Min(Screen.width, Screen.height) / ratio;

            titleStyle.fontSize = Mathf.CeilToInt(20 * fontSize);
            roleStyle.fontSize = Mathf.CeilToInt(16 * fontSize);
            nameStyle.fontSize = Mathf.CeilToInt(16 * fontSize);

            /*if (Screen.width < Screen.height)
            {
                backButtonWidth = Screen.width / 3;
                backButtonHeight = backButtonWidth / backButtonRatio;
            }
            else
            {
                backButtonHeight = Screen.height / 3;
                backButtonWidth = backButtonHeight * backButtonRatio;
            }*/
        }

        prevScrollPosition = scrollPosition.y;
        scrollPosition.y += Time.deltaTime * autoScrollingSpeed;

        if (prevScrollPosition == currScrollPosition && autoScrollingSpeed > 0)
        {
            scrollPosition.y = 0;
        }

        currScrollPosition = prevScrollPosition;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(sceneToGoBackTo); 
        }

    }

   void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);

        GUILayout.Space(Screen.height);
        
        GUILayout.Label("Development Team", titleStyle);
		 
        foreach (Role item in Roles)
        {
           GUILayout.Label(item.roleTitle, roleStyle);
           
           for(int i=0; i < item.personName.Length; i++){
               GUILayout.Label(item.personName[i], nameStyle);
           }
        }
         
        GUILayout.Label(gameLogo, logoStyle, GUILayout.Width(Screen.width), GUILayout.Height(gameLogo.height * Screen.width / gameLogo.width));


        GUILayout.Space(Screen.height);

        GUILayout.EndScrollView();

        GUILayout.EndArea();


        // Back Button

        if (GUI.Button(new Rect(Screen.width / 2 - ((Screen.width / 3) / 2), Screen.height - ((Screen.height / 2)), Screen.width / 3, Screen.height / 2), backButton, backButtonStyle))
        {
         Application.LoadLevel(sceneToGoBackTo);
        }
    }
}
