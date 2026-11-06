using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Déplacement et saut d'un joueur (capsule pour l'instant).
// Les touches sont créées ici en code : rien à brancher dans l'inspecteur.
// Le déplacement se fait par rapport à la direction de la caméra.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IKnockable, IImmobilizable
{
    // Tous les joueurs de la scène (les zombies et les pièces cherchent le plus proche).
    public static readonly List<PlayerController> All = new List<PlayerController>();

    public static PlayerController Nearest(Vector3 position)
    {
        PlayerController best = null;
        float bestDistance = float.MaxValue;
        foreach (PlayerController player in All)
        {
            float distance = (player.transform.position - position).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = player;
            }
        }
        return best;
    }

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
    private Vector3 _impulse;
    private float _immobilizedUntil;

    // Poussée extérieure (explosion). Elle s'estompe toute seule.
    public void AddImpulse(Vector3 velocity)
    {
        _impulse += velocity;
        if (velocity.y > 0f) _verticalVelocity = Mathf.Max(_verticalVelocity, velocity.y);
    }

    // Immobilisé (taser) : ne peut plus se déplacer ni sauter.
    public void Immobilize(float seconds)
    {
        _immobilizedUntil = Time.time + seconds;
    }

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
        All.Add(this);
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        All.Remove(this);
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

        bool immobilized = Time.time < _immobilizedUntil;
        Vector3 move = forward * input.y + right * input.x;
        if (move.sqrMagnitude > 1f) move.Normalize();
        if (immobilized) move = Vector3.zero;

        // Le personnage se tourne vers la direction où il avance.
        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, _turnSpeed * Time.deltaTime);
        }

        // Gravité et saut.
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        if (!immobilized && _controller.isGrounded && _jumpAction.WasPressedThisFrame())
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 horizontalImpulse = new Vector3(_impulse.x, 0f, _impulse.z);
        Vector3 velocity = move * _walkSpeed + horizontalImpulse + Vector3.up * _verticalVelocity;
        _controller.Move(velocity * Time.deltaTime);
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 5f * Time.deltaTime);
    }
}
