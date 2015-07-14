using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	public Transform[] backgrounds;				// Array de los BG para hacerle Parallax.
	public float parallaxScale;					// La proporcion de movimiento con respecto a la camara
	public float parallaxReductionFactor;		// Resto de las capas que se le aplicaran parallax (Es consecutivo).
	public float smoothing;						// Lerp time del parallax


	private Transform cam;						// Referencia al transform de la camara
	private Vector3 previousCamPos;				// Posición de la camara en el frame anterior

	private Vector3[] onEnablePos;
	private Bounds[]  boundsPointer;

	private Vector3 auxVector;

	void Awake ()
	{
		// Seteando fors y referencias que voy a usar despues.
		cam = Camera.main.transform;
		onEnablePos = new Vector3[backgrounds.Length];
		boundsPointer = new Bounds[backgrounds.Length];

		for (int i=0; i < backgrounds.Length; i++) {
			onEnablePos[i] = backgrounds[i].localPosition;	
		}
	}


	void OnEnable ()
	{
		// PreviousFrame ahora es mi actual frame, y reinicio posiciones de los objetos parallax (En este caso, lo necesito porque las nubes se trasladan como trolas)
		previousCamPos = cam.position;
		for (int i = 0; i < onEnablePos.Length; i++) {
			backgrounds[i].localPosition = onEnablePos[i];
			boundsPointer[i] = backgrounds[i].gameObject.GetComponent<Renderer>().bounds;
		}
	}


	void LateUpdate () {
		// Parallax es el movimiento contrario a la camara, So... Obtengo un vector negativo e.e
		float parallax = (previousCamPos.y - cam.position.y) * parallaxScale;

		// Por cada BG a aplicarle Parallax.
		for(int i = 0; i < backgrounds.Length; i++) {
			Vector3 aux  = Camera.main.WorldToViewportPoint( boundsPointer[i].min );
			if( aux.y > 1f)
				backgrounds[i].localPosition = onEnablePos[i];

			// Obtengo desplazamiento, la posicion temporal con el movimiento en Y mas la reduccion de parallax consecutiva.
			float backgroundTargetPosY = backgrounds[i].position.y + parallax * (i * parallaxReductionFactor + 1);

			// Creo la posición final del target.
			//Vector3 backgroundTargetPos = new Vector3(backgrounds[i].position.x, backgroundTargetPosY, backgrounds[i].position.z);
			auxVector.x = backgrounds[i].position.x;
			auxVector.y = backgroundTargetPosY;
			auxVector.z = backgrounds[i].position.z;

			// Lerpeo, obviamente.
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, auxVector, smoothing * Time.deltaTime);
		}

		// Updateo previousCamPos...
		previousCamPos = cam.position;
	}
}
