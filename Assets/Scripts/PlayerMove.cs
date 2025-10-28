using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField, Min(0f)] private float moveSpeed = 10f;
    [SerializeField, Range(0f, 20f)] private float inputDamping = 8f;   // Mayor = responde más rápido
    [SerializeField] private float gravity = -9.81f;                     // Usa Physics.gravity sugerencia

    [Header("Dependencies")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;                           // Puede estar en la cámara o en el player
    [SerializeField] private string walkParamName = "isWalking";

    private static int isWalkingHash;

    // Estado interno
    private Vector2 targetInput;      // Lectura cruda: WASD
    private Vector2 smoothedInput;    // Suavizada (momentum)
    private float verticalVelocity;   // Solo componente Y

    private void Awake()
    {
        controller ??= GetComponent<CharacterController>();
        if (animator == null) TryGetComponent(out animator);

        isWalkingHash = Animator.StringToHash(walkParamName);
        // Asegura que la gravedad sea negativa
        if (gravity > -0.001f) gravity = -9.81f;
    }

    private void Update()
    {
        ReadInput();
        ApplyMovement();
        SyncAnimation();
    }

    private void ReadInput()
    {
        // Input crudo (old Input Manager)
        targetInput.x = Input.GetAxisRaw("Horizontal");
        targetInput.y = Input.GetAxisRaw("Vertical");

        // Evita boost en diagonal
        if (targetInput.sqrMagnitude > 1f)
            targetInput.Normalize();

        // Exponential smoothing hacia el objetivo (mejor que Lerp lineal por frame)
        float k = 1f - Mathf.Exp(-inputDamping * Time.deltaTime);
        smoothedInput = Vector2.Lerp(smoothedInput, targetInput, k);
    }

    private void ApplyMovement()
    {
        // Movimiento horizontal relativo al forward/right del player
        Vector3 horizontal =
            (transform.right * smoothedInput.x + transform.forward * smoothedInput.y) * moveSpeed;

        // Gravedad y anclaje al suelo
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // pequeño empuje hacia abajo para permanecer grounded

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 motion = horizontal;
        motion.y = verticalVelocity;

        controller.Move(motion * Time.deltaTime);
    }

    private void SyncAnimation()
    {
        if (animator == null) return;
        bool walking = smoothedInput.sqrMagnitude > 0.0005f;
        animator.SetBool(isWalkingHash, walking);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (moveSpeed < 0f) moveSpeed = 0f;
        if (gravity > -0.001f) gravity = -9.81f;
        if (inputDamping < 0f) inputDamping = 0f;
    }
#endif
}
