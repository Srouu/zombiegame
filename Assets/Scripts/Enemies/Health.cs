using System;
using UnityEngine;

// Points de vie d'un zombie, d'un baril, etc.
// En multijoueur, seul l'hôte appellera TakeDamage.
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 30f;

    private float _current;

    public bool IsDead { get; private set; }
    public event Action<float> Damaged;
    public event Action<Health> Died;

    private void Awake()
    {
        _current = _maxHealth;
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        if (IsDead) return;
        _current -= amount;
        Damaged?.Invoke(amount);
        if (_current <= 0f)
        {
            IsDead = true;
            Died?.Invoke(this);
        }
    }
}
