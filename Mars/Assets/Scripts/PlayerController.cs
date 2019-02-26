using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float sensitivity;
	public float moveSpeed;
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
		camLook();
		jump();
	}

	private void FixedUpdate() {
		move();
	}

	bool isGrounded() {
		return Physics.Raycast(tr.TransformPoint(col.center), -Vector3.up, out var hitInfo, col.height / 2 + 0.1f);
	}

	void camLook() {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		tr.RotateAround(tr.position, tr.up, mouseX * sensitivity);

		float currentAngle = Vector3.Angle(-tr.up, cam.transform.forward) - 90;
		float angle = mouseY * sensitivity;
		
		if (Mathf.Abs(currentAngle + angle) < angleLimit)
			cam.transform.RotateAround(cam.transform.position, -cam.transform.right, angle);
	}

	void move() {
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		Vector3 moveVector = moveX * tr.right + moveY * tr.forward;

		rb.velocity = moveVector * Time.deltaTime * moveSpeed * 100 + rb.velocity.y * tr.up;
	}

	void jump() {
		if (Input.GetButtonDown("Jump") && isGrounded()) {
			rb.velocity += Vector3.up * jumpHeight;
		}
	}
}