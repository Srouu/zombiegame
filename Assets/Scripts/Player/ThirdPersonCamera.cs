using UnityEngine;
using UnityEngine.InputSystem;

// Caméra à la 3e personne : tourne autour du joueur avec la souris ou le stick droit.
// Se place sur la Main Camera.
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform _target;
    [Tooltip("Hauteur du point visé au-dessus des pieds du joueur.")]
    [SerializeField] private float _targetHeight = 1.5f;

    [Header("Distance")]
    [SerializeField] private float _distance = 5f;

    [Header("Sensibilité")]
    [SerializeField] private float _mouseSensitivity = 0.12f;
    [SerializeField] private float _stickSensitivity = 140f;

    [Header("Limites verticales (degrés)")]
    [SerializeField] private float _minPitch = -20f;
    [SerializeField] private float _maxPitch = 60f;

    [Header("Souris")]
    [SerializeField] private bool _lockCursor = true;

    [Header("Recul d'arme")]
    [Tooltip("Vitesse de retour de la caméra après un tir.")]
    [SerializeField] private float _recoilRecovery = 10f;

    private InputAction _lookAction;
    private float _yaw;
    private float _pitch = 20f;
    private float _recoil;

    // Relève la caméra (appelé par l'arme à chaque tir).
    public void AddRecoil(float degrees)
    {
        _recoil += degrees;
    }

    private void Awake()
    {
        // Souris (valeur en pixels par image) et stick droit (valeur de -1 à 1).
        _lookAction = new InputAction("Look", InputActionType.Value);
        _lookAction.AddBinding("<Mouse>/delta");
        _lookAction.AddBinding("<Gamepad>/rightStick");
    }

    private void OnEnable()
    {
        _lookAction.Enable();
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        _lookAction.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        _yaw = transform.eulerAngles.y;
    }

    private void Update()
    {
        // Échap libère la souris pour pouvoir cliquer hors du jeu.
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector2 look = _lookAction.ReadValue<Vector2>();
        bool usingMouse = Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0f;
        if (usingMouse)
        {
            _yaw += look.x * _mouseSensitivity;
            _pitch -= look.y * _mouseSensitivity;
        }
        else
        {
            _yaw += look.x * _stickSensitivity * Time.deltaTime;
            _pitch -= look.y * _stickSensitivity * Time.deltaTime;
        }
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        _recoil = Mathf.Lerp(_recoil, 0f, _recoilRecovery * Time.deltaTime);
        Quaternion rotation = Quaternion.Euler(_pitch - _recoil, _yaw, 0f);
        Vector3 focus = _target.position + Vector3.up * _targetHeight;
        transform.position = focus - rotation * Vector3.forward * _distance;
        transform.rotation = rotation;
    }
}
