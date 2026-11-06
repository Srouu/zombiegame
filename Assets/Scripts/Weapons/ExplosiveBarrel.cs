using System.Collections.Generic;
using UnityEngine;

// Baril explosif : explose quand sa vie tombe à zéro, avec une onde de choc qui blesse et projette.
[RequireComponent(typeof(Health))]
public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float _radius = 6f;
    [SerializeField] private float _maxDamage = 60f;
    [SerializeField] private float _impulseStrength = 14f;
    [SerializeField] private Color _explosionColor = new Color(1f, 0.55f, 0.1f);

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable() { _health.Died += Explode; }
    private void OnDisable() { _health.Died -= Explode; }

    private void Explode(Health health)
    {
        Vector3 center = transform.position;
        ImpactEffects.Burst(center, _explosionColor, 60, 9f, 0.4f);

        Collider[] colliders = Physics.OverlapSphere(center, _radius, ~0, QueryTriggerInteraction.Ignore);
        HashSet<Object> alreadyHit = new HashSet<Object>();

        foreach (Collider col in colliders)
        {
            Vector3 closest = col.ClosestPoint(center);
            float strength = 1f - Mathf.Clamp01(Vector3.Distance(center, closest) / _radius);

            IDamageable damageable = col.GetComponentInParent<IDamageable>();
            if (damageable is Object damageObject && damageObject != _health && alreadyHit.Add(damageObject))
                damageable.TakeDamage(_maxDamage * strength, closest);

            IKnockable knockable = col.GetComponentInParent<IKnockable>();
            if (knockable is Object knockObject && alreadyHit.Add(knockObject))
            {
                Vector3 direction = col.transform.position - center;
                direction.y = Mathf.Max(direction.y, 0.6f);
                knockable.AddImpulse(direction.normalized * _impulseStrength * strength);
            }
        }

        Destroy(gameObject);
    }
}
