using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour {

    public float speed = 5f,
        strafeRatio = .5f,
        maxVelocityChange = 10.0f,
        jumpHeight = 2f,
        groundDistance = 0.2f,
        gravity = 10.0f;

    // Look sensitivity variable
    [Range(0.0f, 5.0f)]
    public float m_LookSensitivity = 1.0f;

    private Rigidbody body;
    private Collider col;
    private Camera cam;

    public bool _isGrounded = true,
        jump;
    private float m_MouseX;
    private float m_MouseY;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        cam = GetComponentInChildren<Camera>();
        body.useGravity = false;
        body.freezeRotation = true;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        // Receive mouse input and modifies
        m_MouseX += Input.GetAxisRaw("Mouse X") * m_LookSensitivity;
        m_MouseY += Input.GetAxisRaw("Mouse Y") * m_LookSensitivity;

        // Keep mouseY between -90 and +90
        m_MouseY = Mathf.Clamp(m_MouseY, -90.0f, 90.0f);

        // Rotate the player object around the y-axis
        transform.localRotation = Quaternion.Euler(Vector3.up * m_MouseX);

        cam.transform.localRotation = Quaternion.Euler(Vector3.left * m_MouseY);

        _isGrounded = true;
        jump = Input.GetMouseButtonDown(1) || Input.GetKeyDown("space") && _isGrounded;
    }


    private void FixedUpdate() {
        var targetVelocity = new Vector3(Input.GetAxis("Horizontal") * strafeRatio, 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity) * speed;
        var velocity = body.velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        body.AddForce(velocityChange, ForceMode.VelocityChange);

        if (jump) {
            jump = false;
            body.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
        }
        body.AddForce(new Vector3(0, -gravity * body.mass, 0));
    }
}
