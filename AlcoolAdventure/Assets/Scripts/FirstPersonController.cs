using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
	public bool CanMove /*{ get; private set; }*/ = true;
	public bool Paused = false;
	public bool Transition = false;
	
	[Header("Movement Parameters")]
	[SerializeField] private float walkSpeed = 3.0f;
	[SerializeField] private float gravity = 30.0f;
	
	[Header("Look Parameters")]
	[SerializeField, Range(1,10)] private float lookSpeedX = 2.0f;
	[SerializeField, Range(1,10)] private float lookSpeedY = 2.0f;
	[SerializeField, Range(1,180)] private float upperLookLimit = 80.0f;
	[SerializeField, Range(1,180)] private float lowerLookLimit = 80.0f;

	private Animator characterAnimator;
	private Camera playerCamera;
	private CharacterController characterController;
	
	private Vector3 moveDirection;
	private Vector2 currentInput;
	public float forwardSpeed;
	public float sideSpeed;
	
	private float rotationX = 0;
	
	void Awake() {
		playerCamera = GetComponentInChildren<Camera>();
		characterController = GetComponent<CharacterController>();
		characterAnimator = GetComponentInChildren<Animator>();
		characterAnimator.keepAnimatorStateOnDisable = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	void Update()
	{
		if (CanMove & !Paused & !Transition) {
			HandleMovementInput();
			HandleMouseLook();
			HandleMovementAnimation();

			ApplyFinalMovement();


		}
	}
	
	private void HandleMovementInput() {
		currentInput = new Vector2(walkSpeed*Input.GetAxis("Vertical"),walkSpeed*Input.GetAxis("Horizontal"));
		float moveDirectionY = moveDirection.y;
		moveDirection = (transform.TransformDirection(Vector3.forward)*currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
		moveDirection.y = moveDirectionY;
	}

	private void HandleMouseLook() {
		rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
		rotationX = Mathf.Clamp(rotationX,-upperLookLimit, lowerLookLimit);
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX,0,0);
		transform.rotation *= Quaternion.Euler(0,Input.GetAxis("Mouse X")*lookSpeedX,0);
	}

	private void ApplyFinalMovement() {
		if(!characterController.isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}

		characterController.Move(moveDirection * Time.deltaTime);
	} 


	private void HandleMovementAnimation() {
		sideSpeed = walkSpeed*Input.GetAxis("Horizontal");
		forwardSpeed = walkSpeed*Input.GetAxis("Vertical");

		characterAnimator.SetFloat("forwardSpeed", forwardSpeed);
		characterAnimator.SetFloat("sideSpeed", sideSpeed);
		characterAnimator.SetBool("canMove", CanMove);
	}

}
