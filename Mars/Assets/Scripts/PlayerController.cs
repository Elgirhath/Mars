using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float angleLimit;

	private Camera cam;
	private CapsuleCollider col;
	public float jumpHeight;
	private Rigidbody rb;
	public float runSpeed;
	public float sensitivity;
	private Transform tr;
	public float walkSpeed;

// Start is called before the first frame update
	private void Start() {
		cam = GetComponentInChildren<Camera>();
		rb = GetComponent<Rigidbody>();
		tr = transform;
		col = GetComponent<CapsuleCollider>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}


	private void Update() {
		CamLook();
		Jump();
	}

	private void FixedUpdate() {
		Move();
	}

	private void CamLook() {
		//TODO: interpolate cam movement: http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8?fbclid=IwAR09F2udjxU9i_D7VuLsjHUXPco0tGQ4Gnf96emKsRcBNCt6ubW43xnlO-c
		
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

		float moveSpeed = IsRunning() ? runSpeed : walkSpeed; //check whether player is running and set his speed

		Vector3 moveVector = moveDirection * moveSpeed;

		rb.velocity = moveVector + rb.velocity.y * tr.up; //moveVector is set in 2d, missing the Y axis, which should remain unchanged
	}

	private void Jump() {
		if (Input.GetButtonDown("Jump") && IsGrounded())
			rb.velocity += Vector3.up * jumpHeight;
	}

	private bool IsRunning() {
		return Input.GetButton("Run");
	}

	private bool IsGrounded() {
		return Physics.Raycast(tr.TransformPoint(col.center), -Vector3.up, col.height / 2 + 0.1f);
	}
}