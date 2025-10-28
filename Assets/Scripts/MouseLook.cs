using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;

    private float xMousePos;
    private float yMousePos;
    private float smoothedX;
    private float smoothedY;

    private float currentXRotation;
    private float currentYRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInput();
        ModifyInput();
        MoveCamera();
    }

    void GetInput()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
    }

    void ModifyInput()
    {
        xMousePos *= sensitivity * smoothing;
        yMousePos *= sensitivity * smoothing;

        smoothedX = Mathf.Lerp(smoothedX, xMousePos, 1f / smoothing);
        smoothedY = Mathf.Lerp(smoothedY, yMousePos, 1f / smoothing);

        currentXRotation += smoothedX;
        currentYRotation -= smoothedY; // Invertido para que se mueva natural
        currentYRotation = Mathf.Clamp(currentYRotation, -90f, 90f); // Limita la mirada vertical
    }

    void MoveCamera()
    {
        transform.localRotation = Quaternion.Euler(currentYRotation, currentXRotation, 0f);
    }
}
