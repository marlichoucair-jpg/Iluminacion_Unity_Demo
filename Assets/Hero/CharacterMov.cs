using UnityEngine;

public class CharacterMov : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    [Header("Cámara")]
    public Transform cameraTarget;
    public float cameraSmoothSpeed = 0.125f;
    public Vector3 cameraOffset = new Vector3(0, 3, -6);
    public float mouseSensitivity = 2f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    private float horizontalInput;
    private float verticalInput;

    private float mouseX;
    private float mouseY;
    private float currentYaw = 0f;
    private float currentPitch = 20f;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (cameraTarget != null)
        {
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 desiredPosition = cameraTarget.position + rotation * cameraOffset;

            if (desiredPosition.y < cameraTarget.position.y)
            {
                desiredPosition.y = cameraTarget.position.y;
            }

            // Suavizado de cámara
            Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, cameraSmoothSpeed);
            Camera.main.transform.position = smoothedPosition;
            Camera.main.transform.LookAt(cameraTarget);
        }
    }
}
