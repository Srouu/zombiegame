using UnityEngine;

// Zombie-capsule : poursuit le joueur le plus proche, subit des dégâts, meurt en lâchant des pièces.
[RequireComponent(typeof(CharacterController), typeof(Health))]
public class ZombieController : MonoBehaviour, IKnockable, IImmobilizable
{
    [Header("Déplacement")]
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _turnSpeed = 8f;
    [SerializeField] private float _stopDistance = 1.2f;
    [SerializeField] private float _gravity = -25f;

    [Header("Mort")]
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private int _coinsDropped = 3;
    [SerializeField] private int _coinValue = 5;
    [SerializeField] private Color _deathColor = new Color(0.4f, 0.8f, 0.3f);

    private CharacterController _controller;
    private Health _health;
    private ColorFlash _flash;
    private Vector3 _impulse;
    private float _stunnedUntil;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _flash = GetComponent<ColorFlash>();
    }

    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Damaged -= OnDamaged;
        _health.Died -= OnDied;
    }

    public void AddImpulse(Vector3 velocity)
    {
        _impulse += velocity;
    }

    public void Immobilize(float seconds)
    {
        _stunnedUntil = Time.time + seconds;
        if (_flash != null) _flash.Flash(Color.yellow, seconds);
    }

    private void Update()
    {
        if (_health.IsDead) return;

        Vector3 move = Vector3.zero;
        if (Time.time >= _stunnedUntil)
        {
            PlayerController target = PlayerController.Nearest(transform.position);
            if (target != null)
            {
                Vector3 toTarget = target.transform.position - transform.position;
                toTarget.y = 0f;
                if (toTarget.sqrMagnitude > 0.01f)
                {
                    Quaternion look = Quaternion.LookRotation(toTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, look, _turnSpeed * Time.deltaTime);
                }
                if (toTarget.magnitude > _stopDistance) move = toTarget.normalized * _speed;
            }
        }

        if (_controller.isGrounded && _verticalVelocity < 0f) _verticalVelocity = -2f;
        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 velocity = move + _impulse + Vector3.up * _verticalVelocity;
        _controller.Move(velocity * Time.deltaTime);
        _impulse = Vector3.Lerp(_impulse, Vector3.zero, 6f * Time.deltaTime);
    }

    private void OnDamaged(float amount)
    {
        if (_flash != null) _flash.Flash(Color.white, 0.1f);
    }

    private void OnDied(Health health)
    {
        ImpactEffects.Burst(transform.position, _deathColor, 25, 5f, 0.25f);

        if (_coinPrefab != null)
        {
            for (int i = 0; i < _coinsDropped; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), -0.5f, Random.Range(-0.8f, 0.8f));
                Coin coin = Instantiate(_coinPrefab, transform.position + offset, _coinPrefab.transform.rotation);
                coin.SetValue(_coinValue);
            }
        }

        Destroy(gameObject);
    }
}
