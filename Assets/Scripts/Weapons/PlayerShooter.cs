using UnityEngine;
using UnityEngine.InputSystem;

// Tir du joueur : clic gauche (ou gâchette droite de la manette).
// Le tir part du centre de l'écran. Recul : la caméra se relève et l'arme recule.
public class PlayerShooter : MonoBehaviour
{
    [Header("Arme")]
    [SerializeField] private WeaponData _weapon;

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

    private void Awake()
    {
        _fireAction = new InputAction("Fire", InputActionType.Button);
        _fireAction.AddBinding("<Mouse>/leftButton");
        _fireAction.AddBinding("<Gamepad>/rightTrigger");

        if (_camera == null && Camera.main != null)
            _camera = Camera.main.GetComponent<ThirdPersonCamera>();
        if (_gunVisual != null)
            _gunRestPosition = _gunVisual.localPosition;
    }

    private void OnEnable() { _fireAction.Enable(); }
    private void OnDisable() { _fireAction.Disable(); }

    private void Update()
    {
        if (_weapon == null) return;

        // Retour en place de l'arme après le recul.
        if (_gunVisual != null)
        {
            _gunVisual.localPosition = Vector3.Lerp(
                _gunVisual.localPosition, _gunRestPosition, _gunRecoverySpeed * Time.deltaTime);
        }

        // Pas de tir tant que la souris n'est pas verrouillée dans le jeu.
        if (Cursor.lockState != CursorLockMode.Locked) return;

        bool wantsToFire = _weapon.automatic ? _fireAction.IsPressed() : _fireAction.WasPressedThisFrame();
        if (wantsToFire && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + _weapon.fireInterval;
            Fire();
        }
    }

    private void Fire()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Rayon du centre de l'écran. On ignore le joueur lui-même.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.RaycastAll(ray, _weapon.range, ~0, QueryTriggerInteraction.Ignore);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform)) continue;

            IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
            if (target != null) target.TakeDamage(_weapon.damage, hit.point);

            SpawnHitMarker(hit.point);
            break;
        }

        // Recul.
        if (_camera != null) _camera.AddRecoil(_weapon.cameraKickDegrees);
        if (_gunVisual != null) _gunVisual.localPosition -= Vector3.forward * _weapon.gunKickDistance;
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
