using Assets.Scripts.Player.Condition;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour {
        public float angleLimit;

        private Camera cam;
        private CapsuleCollider col;
        public float jumpHeight;
        private Rigidbody rb;
        public float runSpeed;
        public float sensitivity;
        public float walkSpeed;
        public bool camLockState { get; set; }

        public static Player instance;
        private float moveSpeed;
        private EnergyController energyController;
	

// Start is called before the first frame update
        private void Start() {
            cam = GetComponentInChildren<Camera>();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            energyController = GetComponent<EnergyController>();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Awake() {
            if (!instance)
                instance = this;
            else if (instance!=this)
                Destroy(gameObject);
        }

        private void Update()
        {
            CamLook();

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                Jump();
            }
        }

        private void FixedUpdate() {
            Move();
        }


        private void CamLook() {
            //TODO: interpolate cam movement: http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8?fbclid=IwAR09F2udjxU9i_D7VuLsjHUXPco0tGQ4Gnf96emKsRcBNCt6ubW43xnlO-c

            if (camLockState) return;
		
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            transform.RotateAround(transform.position, transform.up, mouseX * sensitivity);

            var currentAngle = Vector3.Angle(-transform.up, cam.transform.forward) - 90; //makes currentAngle [0,90] when looking up and [0,-90] when looking down
            var angle = mouseY * sensitivity;

            if (!(Mathf.Abs(currentAngle + angle) < angleLimit)) return;

            var camTransform = cam.transform;
            cam.transform.RotateAround(camTransform.position, -camTransform.right, angle);
        }

        private void Move() {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");

            var moveDirection = Vector3.Normalize(moveX * transform.right + moveY * transform.forward);

            //check whether player is running and set his speed
            //during a jump moveSpeed remains unchanged
            if (IsGrounded())
            {
                moveSpeed = IsRunning() ? runSpeed : walkSpeed;
            }

            var moveVector = moveDirection * moveSpeed;

            rb.velocity = moveVector + rb.velocity.y * transform.up; //moveVector is set in 2d, missing the Y axis, which should remain unchanged
        }

        private void Jump()
        {
            if (!energyController.canJump) return;
            rb.velocity += Vector3.up * jumpHeight;
            energyController.ExecuteJumpLoss();
        }

        public bool IsRunning()
        {
            return energyController.canRun && IsGrounded() && Input.GetButton("Run");
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(transform.TransformPoint(col.center), -Vector3.up, col.height / 2 + 0.1f);
        }

        public bool IsResting()
        {
            return IsGrounded() && !Input.GetButton("Run");
        }
    }
}