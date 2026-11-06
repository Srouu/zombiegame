using UnityEngine;

// Faux coéquipier pour tester le tir allié : il fait des allers-retours,
// ne meurt jamais, et s'arrête quand il est immobilisé par le taser.
[RequireComponent(typeof(ColorFlash))]
public class TeammateDummy : MonoBehaviour, IDamageable, IImmobilizable
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _patrolDistance = 4f;

    private ColorFlash _flash;
    private Vector3 _startPosition;
    private float _immobilizedUntil;
    private float _phase;

    private void Awake()
    {
        _flash = GetComponent<ColorFlash>();
        _startPosition = transform.position;
    }

    // Tir allié : aucun dégât, juste un éclair rouge.
    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        _flash.Flash(Color.red, 0.2f);
    }

    public void Immobilize(float seconds)
    {
        _immobilizedUntil = Time.time + seconds;
        _flash.Flash(Color.yellow, seconds);
    }

    private void Update()
    {
        if (Time.time < _immobilizedUntil) return;
        _phase += _speed / _patrolDistance * Time.deltaTime;
        transform.position = _startPosition + Vector3.right * Mathf.Sin(_phase) * _patrolDistance;
    }
}
