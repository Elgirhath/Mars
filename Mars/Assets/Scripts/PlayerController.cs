using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float sensitivity;
	public float walkSpeed;
	public float runSpeed;
	public float jumpHeight;
	public float angleLimit;
	
	private Camera cam;
	private Rigidbody rb;
	private Transform tr;
	private CapsuleCollider col;

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

	void CamLook() {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		tr.RotateAround(tr.position, tr.up, mouseX * sensitivity);

		float currentAngle = Vector3.Angle(-tr.up, cam.transform.forward) - 90;
		float angle = mouseY * sensitivity;
		
		if (Mathf.Abs(currentAngle + angle) < angleLimit)
			cam.transform.RotateAround(cam.transform.position, -cam.transform.right, angle);
	}

	void Move() {
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		Vector3 moveDirection = Vector3.Normalize(moveX * tr.right + moveY * tr.forward);
		
		float moveSpeed = isRunning() ? runSpeed : walkSpeed;  			//check whether player is running and set his speed
		moveSpeed *= Time.fixedDeltaTime * 100;								//apply time to moveSpeed
		
		Vector3 moveVector = moveDirection * moveSpeed;

		rb.velocity = moveVector + rb.velocity.y * tr.up; 				//moveVector is set in 2d, missing the Y axis, which should remain unchanged
	}

	void Jump() {
		if (Input.GetButtonDown("Jump") && isGrounded()) {
			rb.velocity += Vector3.up * jumpHeight;
		}
	}

	bool isRunning() {
		return Input.GetButton("Run");
	}
	bool isGrounded() {
		return Physics.Raycast(tr.TransformPoint(col.center), -Vector3.up, out var hitInfo, col.height / 2 + 0.1f);
	}
}