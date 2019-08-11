using UnityEngine;

public class Player : MonoBehaviour {
	public float angleLimit;

	private Camera cam;
	private CapsuleCollider col;
	public float jumpHeight;
	private Rigidbody rb;
	public float runSpeed;
	public float sensitivity;
	private Transform tr;
	public float walkSpeed;
	private bool locked;
	public bool jumped;

	public static Player instance;
	private float moveSpeed;
	private bool jumpedWhileSprinting;
	private EnergyController energyController;
	

// Start is called before the first frame update
	private void Start() {
		cam = GetComponentInChildren<Camera>();
		rb = GetComponent<Rigidbody>();
		tr = transform;
		col = GetComponent<CapsuleCollider>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		jumped = false;
		jumpedWhileSprinting = false;
		energyController = EnergyController.instance;
	}

	private void Awake() {
		if (!instance)
			instance = this;
		else if (instance!=this)
			Destroy(gameObject);
	}

	private void Update()
	{
		jumped = false;
		CamLook();
		Jump();
	}

	private void FixedUpdate() {
		Move();
	}

	private void CamLook() {
		//TODO: interpolate cam movement: http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8?fbclid=IwAR09F2udjxU9i_D7VuLsjHUXPco0tGQ4Gnf96emKsRcBNCt6ubW43xnlO-c

		if (locked)
			return;
		
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		tr.RotateAround(tr.position, tr.up, mouseX * sensitivity);

		float currentAngle = Vector3.Angle(-tr.up, cam.transform.forward) - 90; //makes currentAngle [0,90] when looking up and [0,-90] when looking down
		float angle = mouseY * sensitivity;

		if (Mathf.Abs(currentAngle + angle) < angleLimit) {
			Transform camTransform = cam.transform;
			cam.transform.RotateAround(camTransform.position, -camTransform.right, angle);
		}
	}

	private void Move() {
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		Vector3 moveDirection = Vector3.Normalize(moveX * tr.right + moveY * tr.forward);

		//check whether player is running and set his speed
		if (!IsGrounded())
		{
			moveSpeed = jumpedWhileSprinting ? runSpeed : walkSpeed;
		}
		else
		{
			moveSpeed = IsRunning() ? runSpeed : walkSpeed;
		}
		

		Vector3 moveVector = moveDirection * moveSpeed;

		rb.velocity = moveVector + rb.velocity.y * tr.up; //moveVector is set in 2d, missing the Y axis, which should remain unchanged
	}

	private void Jump() {
		if (Input.GetButtonDown("Jump") && IsGrounded())
		{
			jumped = true;
			rb.velocity += Vector3.up * jumpHeight;
			jumpedWhileSprinting = IsRunning();
		}
	}

	public bool IsRunning()
	{
		if (energyController.LockState) return false;
		return Input.GetButton("Run");
	}

	public bool IsGrounded() {
		return Physics.Raycast(tr.TransformPoint(col.center), -Vector3.up, col.height / 2 + 0.1f);
	}

	public bool camLockState {
		get { return locked; }
		set { locked = value; }
	}
}