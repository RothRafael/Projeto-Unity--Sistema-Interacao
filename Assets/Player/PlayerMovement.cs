using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool enableSprinting = true;
    [SerializeField] private bool canCrouch = true;
    #endregion

    #region Mouse Look Settings
    [Header("Mouse Look Settings")]
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float cameraZRotationSense = 5f;
    [SerializeField] private float cameraZRotationMovementSense = 5f;
    [SerializeField] private float cameraZDamping = 5f;
    [SerializeField] [Range(0, 6)] private float cameraBobSpeed = 0.1f;
    [SerializeField] [Range(0, 0.2f)] private float cameraBobAmmount = 0.1f;
    [SerializeField] private Transform cameraTransform;
    #endregion

    #region Ground Check Settings
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    #endregion

    // Component references
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    // Mouse rotation
    private float xRotation = 0f;
    private float zRotation = 0f;

    // Movement flags
    private bool isGrounded;
    private bool isCrouching;

    #region Unity Callbacks
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Evita rotações indesejadas pela física
        rb.freezeRotation = true;

        // Bloqueia o cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ProcessInput();
        HandleMouseLook();
        HandleCrouching();
        HandleMovement();
        HandleJump();
    }

    private void FixedUpdate()
    {
        UpdateGroundStatus();
    }
    #endregion

    #region Input Processing
    /// <summary>
    /// Centraliza a leitura dos inputs (pode ser expandida futuramente para suportar diferentes fontes).
    /// </summary>
    private void ProcessInput()
    {
        // Aqui, futuramente, podemos gerenciar outros inputs ou abstrair para uma classe InputManager.
    }
    #endregion

    #region Mouse Look
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // Atualiza rotação vertical e limita para evitar olhar para trás
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Calcula a rotação no eixo Z com base no movimento do mouse e aplica suavização
        float targetZRotation = -mouseX * cameraZRotationSense;
        targetZRotation += horizontal * cameraZRotationMovementSense;
        zRotation = Mathf.Lerp(zRotation, targetZRotation, Time.deltaTime * cameraZDamping);

        // Sensação de passos ao se mover SEN
        if (canHeadBob && (horizontal != 0 || vertical != 0))
        {
            float step = Mathf.Sin(Time.time * Mathf.PI * cameraBobSpeed) * cameraBobAmmount;
            Debug.Log(step);
            Vector3 localPosition = cameraTransform.localPosition;
            localPosition.y = step + 0.526f;
            cameraTransform.localPosition = localPosition;
        }

        // Aplica as rotações na câmera e no jogador
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        transform.Rotate(Vector3.up * mouseX);
    }
    #endregion

    #region Movement
    private void HandleMovement()
    {
        // Obtém os inputs de movimento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        float currentSpeed = GetCurrentSpeed();

        // Preserva a velocidade vertical para não interferir com a física (gravidade ou pulo)
        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// Retorna a velocidade atual baseada nos estados de sprint e agachamento.
    /// </summary>
    /// <returns>Velocidade de movimento</returns>
    private float GetCurrentSpeed()
    {
        if (enableSprinting && Input.GetKey(KeyCode.LeftShift))
        {
            return sprintSpeed;
        }
        else if (canCrouch && Input.GetKey(KeyCode.LeftControl))
        {
            return moveSpeed / 2f;
        }
        else
        {
            return moveSpeed;
        }
    }
    #endregion

    #region Jump
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion

    #region Ground Check
    private void UpdateGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
    #endregion

    #region Crouch
    private void HandleCrouching()
    {
        if (canCrouch && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            AdjustColliderHeight();
        }
        if (isCrouching && Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            AdjustColliderHeight();
        }
    }

    /// <summary>
    /// Ajusta a altura do collider de acordo com o estado de agachado.
    /// </summary>
    private void AdjustColliderHeight()
    {
        if (isCrouching)
        {
            capsuleCollider.height /= 2f;
        }
        else
        {
            capsuleCollider.height *= 2f;
        }
    }
    #endregion
}
