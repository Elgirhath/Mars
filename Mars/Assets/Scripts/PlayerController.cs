using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float sensitivity;
	public float moveSpeed;
	public float jumpHeight;
	public float angleLimit;
	
	private Camera cam;
	private Rigidbody rBody;
	private Transform tr;
	private CapsuleCollider col;


	// Start is called before the first frame update
	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		cam = GetComponentInChildren<Camera>();
		tr = transform;
		rBody = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
	}

	private void Update()
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");
		float angle = mouseY * sensitivity;
		float currentAngle = Vector3.Angle(-tr.up, cam.transform.forward) - 90;

		
		transform.RotateAround(tr.position, tr.up, mouseX * sensitivity);
		if (Mathf.Abs(currentAngle + angle) < angleLimit)
		{
			cam.transform.RotateAround(cam.transform.position, -cam.transform.right, angle);
		}
		
		// Jumping
		if (Input.GetButtonDown("Jump") && Physics.Raycast(tr.TransformPoint(col.center), -Vector3.up, out var hitInfo,col.height/2 + 0.1f))
		{
			rBody.velocity = Vector3.up * jumpHeight;
		}
	}

	private void FixedUpdate()
	{
		// Player movement
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		Vector3 moveVector = moveX * tr.right + moveY * tr.forward;
		rBody.velocity = moveVector.normalized * Time.deltaTime * moveSpeed * 100 + tr.up * rBody.velocity.y;

	}
}