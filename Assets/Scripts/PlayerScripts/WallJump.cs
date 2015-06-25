using UnityEngine;
using System.Collections;

//[SerializePrivateVariables] // Showing Variables via Inspector... I need to disable this later!!
public class WallJump : MonoBehaviour {
	public float maxSpeed = 10f;
	public float jumpForce = 4f;
	public float jumpPushForce = 10f;
	public bool facingRight = true;
	bool doubleJump = true;
	bool wallJumped = false;
	bool wallJumping = false;

	[SerializeField] bool grounded;
	[SerializeField] bool touchingWall;
	float checkRadius = 0.2f;
	public Transform groundCheck;
	public Transform wallCheck;
	public LayerMask whatIsGround;
	public LayerMask whatIsWall;


	// Private Vectors to Avoid new Objects.
	Vector2 AuxVector  = Vector2.zero;


	// Rigidbody 2D Reference.
	public Rigidbody2D rigidbody2D = null;
	Transform tr = null;

	private int maxY = 0;
	
	private bool needToApplyjump = false;

	void Start () {
		rigidbody2D = this.GetComponent<Rigidbody2D> ();
		tr = transform;

		maxY = (int) (tr.position.y + 1f);

		EventsSystem.onGameChanged += OnGameStateChanged;
		EventsSystem.onNewInputSystem += InputMangement;
	}
	
	void FixedUpdate () {

		grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
		touchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsWall);

		float move = GameManager.State == GameManager.GameState.INTRO ? 1 : 0; 

		//float move = 0f;
		if (grounded) {
			AuxVector.x = move * maxSpeed;
			AuxVector.y = rigidbody2D.velocity.y;
			rigidbody2D.velocity = AuxVector;
		} else if ((move != 0 && !wallJumping)) {
			AuxVector.x = move * maxSpeed;
			AuxVector.y = rigidbody2D.velocity.y;
			rigidbody2D.velocity = AuxVector;
		}

		if(wallJumped){
			Walljump();
		}

		if (GameManager.State == GameManager.GameState.INTRO && touchingWall) {
			GameManager.State = GameManager.GameState.STARTED;
		}

		if(rigidbody2D.velocity.y < 0){	// Causes falling...

			if(touchingWall){
				Vector2 v = rigidbody2D.velocity;
				v.y = v.y * 0.8f;
				rigidbody2D.velocity = v;
			}
			wallJumping = false;
		}
		if (grounded){
			doubleJump = true;
			wallJumping = false;
		}
		if (rigidbody2D.velocity.x > 0 && !facingRight){
			Flip();
		}
		else if (rigidbody2D.velocity.x < 0 && facingRight){
			Flip();
		}
	}
	
	void Update(){
		//bool jump = Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0);
		/*bool jump = InputManager.isTriggeringDown(InputManager.JUMP_ACTION);
		if (jump){
			if (grounded){
				Jump ();
			}
			else if (touchingWall){
				wallJumped = true;
			}
			else if (doubleJump){
				DoubleJump();
			}

			GameManager.instance.checkPlayerPosition(tr.position);

			if(maxY <= tr.position.y){
				GameManager.instance.AddScore( (int)tr.position.y - maxY );
				maxY = (int) tr.position.y;
			}
		} 
		*/
		
		if(needToApplyjump){
			ApplyJump();
			needToApplyjump = false;
		}
	}
	
	void ApplyJump(){
		if (grounded){
				Jump ();
		}
		else if (touchingWall){
				wallJumped = true;
		}
		else if (doubleJump){
				DoubleJump();
		}

		GameManager.instance.checkPlayerPosition(tr.position);

		if(maxY <= tr.position.y){
			GameManager.instance.AddScore( (int)tr.position.y - maxY );
			maxY = (int) tr.position.y;
		}
	}
	
	void InputMangement(InputManager.MainControls control){
		switch(control){
			case InputManager.MainControls.JUMP_ACTION:
				needToApplyjump = true;
                break;
            case InputManager.MainControls.JUMP_ACTION_LEFT:
                if (facingRight)
                    needToApplyjump = true;
                break;
            case InputManager.MainControls.JUMP_ACTION_RIGHT:
                if (!facingRight)
                    needToApplyjump = true;
			    break;
		}
	}
	
	void Flip(){      
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Jump(){
		AuxVector.x = rigidbody2D.velocity.x;
		AuxVector.y = jumpForce;
		rigidbody2D.velocity = AuxVector;
	}

	void Walljump(){
		AuxVector.x = jumpPushForce * (facingRight ? -1:1);
		AuxVector.y = jumpForce;
		rigidbody2D.velocity = AuxVector;
		wallJumping = true;
		wallJumped = false;
	}
	
	void DoubleJump(){
		AuxVector.x = rigidbody2D.velocity.x;
		AuxVector.y = jumpForce / 1.1f;
		rigidbody2D.velocity = AuxVector;
		doubleJump = false;
	}

	void OnGameStateChanged(GameManager.GameState state){

	}
}
