using UnityEngine;

// Tout ce qui peut recevoir des dégâts (zombie, baril, cible...).
// Plus tard, en multijoueur, seul l'hôte appliquera réellement les dégâts.
public interface IDamageable
{
    void TakeDamage(float amount, Vector3 hitPoint);
}
