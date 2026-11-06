using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Tir du joueur : clic gauche (ou gâchette droite de la manette). Touches 1 / 2 pour changer d'arme.
// Le tir part du centre de l'écran. Recul : la caméra se relève et l'arme recule.
public class PlayerShooter : MonoBehaviour
{
    [Header("Armes")]
    [SerializeField] private WeaponData[] _weapons;

    [Header("Références")]
    [SerializeField] private ThirdPersonCamera _camera;
    [Tooltip("Le modèle de l'arme (pour l'effet de recul visuel).")]
    [SerializeField] private Transform _gunVisual;

    [Header("Effets")]
    [SerializeField] private float _gunRecoverySpeed = 14f;
    [SerializeField] private float _hitMarkerSize = 0.2f;
    [SerializeField] private float _hitMarkerLifetime = 0.4f;

    private InputAction _fireAction;
    private float _nextFireTime;
    private Vector3 _gunRestPosition;
    private int _currentIndex;
    private Renderer _gunRenderer;

    public WeaponData Current => (_weapons != null && _weapons.Length > 0) ? _weapons[_currentIndex] : null;
    public event Action<WeaponData> WeaponChanged;

    private void Awake()
    {
        _fireAction = new InputAction("Fire", InputActionType.Button);
        _fireAction.AddBinding("<Mouse>/leftButton");
        _fireAction.AddBinding("<Gamepad>/rightTrigger");

        if (_camera == null && Camera.main != null)
            _camera = Camera.main.GetComponent<ThirdPersonCamera>();
        if (_gunVisual != null)
        {
            _gunRestPosition = _gunVisual.localPosition;
            _gunRenderer = _gunVisual.GetComponent<Renderer>();
        }
    }

    private void Start()
    {
        ApplyWeaponLook();
    }

    private void OnEnable() { _fireAction.Enable(); }
    private void OnDisable() { _fireAction.Disable(); }

    private void Update()
    {
        // Retour en place de l'arme après le recul.
        if (_gunVisual != null)
        {
            _gunVisual.localPosition = Vector3.Lerp(
                _gunVisual.localPosition, _gunRestPosition, _gunRecoverySpeed * Time.deltaTime);
        }

        HandleWeaponSwitch();

        WeaponData weapon = Current;
        if (weapon == null) return;

        // Pas de tir tant que la souris n'est pas verrouillée dans le jeu.
        if (Cursor.lockState != CursorLockMode.Locked) return;

        bool wantsToFire = weapon.automatic ? _fireAction.IsPressed() : _fireAction.WasPressedThisFrame();
        if (wantsToFire && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + weapon.fireInterval;
            Fire(weapon);
        }
    }

    private void HandleWeaponSwitch()
    {
        if (_weapons == null || Keyboard.current == null) return;
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectWeapon(1);
    }

    private void SelectWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Length || index == _currentIndex) return;
        _currentIndex = index;
        ApplyWeaponLook();
    }

    private void ApplyWeaponLook()
    {
        WeaponData weapon = Current;
        if (weapon == null) return;
        if (_gunRenderer != null) _gunRenderer.material.color = weapon.gunColor;
        WeaponChanged?.Invoke(weapon);
    }

    private void Fire(WeaponData weapon)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Rayon du centre de l'écran. On ignore le joueur lui-même.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.RaycastAll(ray, weapon.range, ~0, QueryTriggerInteraction.Ignore);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform)) continue;

            if (weapon.damage > 0f)
            {
                IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
                if (target != null) target.TakeDamage(weapon.damage, hit.point);
            }

            if (weapon.immobilizeSeconds > 0f)
            {
                IImmobilizable stunTarget = hit.collider.GetComponentInParent<IImmobilizable>();
                if (stunTarget != null) stunTarget.Immobilize(weapon.immobilizeSeconds);
            }

            SpawnHitMarker(hit.point);
            break;
        }

        // Recul.
        if (_camera != null) _camera.AddRecoil(weapon.cameraKickDegrees);
        if (_gunVisual != null) _gunVisual.localPosition -= Vector3.forward * weapon.gunKickDistance;
    }

    private void SpawnHitMarker(Vector3 position)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.name = "ImpactTir";
        marker.transform.position = position;
        marker.transform.localScale = Vector3.one * _hitMarkerSize;
        Destroy(marker.GetComponent<Collider>());
        Destroy(marker, _hitMarkerLifetime);
    }
}
