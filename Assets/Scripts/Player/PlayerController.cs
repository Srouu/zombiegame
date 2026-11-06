using UnityEngine;
using UnityEngine.InputSystem;

// Déplacement et saut d'un joueur (capsule pour l'instant).
// Les touches sont créées ici en code : rien à brancher dans l'inspecteur.
// Le déplacement se fait par rapport à la direction de la caméra.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _turnSpeed = 12f;

    [Header("Saut et gravité")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -25f;

    [Header("Références")]
    [Tooltip("La caméra qui suit le joueur. Si vide, on prend la caméra principale.")]
    [SerializeField] private Transform _cameraTransform;

    private CharacterController _controller;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;

        _moveAction = new InputAction("Move", InputActionType.Value);
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        _moveAction.AddBinding("<Gamepad>/leftStick");

        _jumpAction = new InputAction("Jump", InputActionType.Button);
        _jumpAction.AddBinding("<Keyboard>/space");
        _jumpAction.AddBinding("<Gamepad>/buttonSouth");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
    }

    private void Update()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();

        // Directions "avant" et "droite" de la caméra, mises à plat (sans l'inclinaison).
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        if (_cameraTransform != null)
        {
            forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
            right = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;
        }

        Vector3 move = forward * input.y + right * input.x;
        if (move.sqrMagnitude > 1f) move.Normalize();

        // Le personnage se tourne vers la direction où il avance.
        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, _turnSpeed * Time.deltaTime);
        }

        // Gravité et saut.
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        if (_controller.isGrounded && _jumpAction.WasPressedThisFrame())
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 velocity = move * _walkSpeed + Vector3.up * _verticalVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }
}
