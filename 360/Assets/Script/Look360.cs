using UnityEngine;
using UnityEngine.InputSystem;
public class Look360 : MonoBehaviour
{
    private NIS inputActions;

    [Header("Configuración")]
    public float sensitivity = 0.1f;
    public Transform playerBody;

    private float xRotation = 0f;
    private Vector2 lookInput;

    private void Awake()
    {
      inputActions = new NIS();
    }

    private void OnEnable()
    {
        inputActions.View.Enable();

        inputActions.View.Look.performed += OnLook;
        inputActions.View.Look.canceled += OnLook;
    }
    private void OnDisable()
    {
        inputActions.View.Look.performed -= OnLook;
        inputActions.View.Look.canceled -= OnLook;

        inputActions.View.Disable();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        RotateCamera();
    }
    private void RotateCamera()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // rotación vertical (arriba / abajo)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotacion horizontal (izquierda / derecha)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

}
