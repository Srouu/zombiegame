using UnityEngine;

// Peut être poussé (explosion, recul). La vitesse donnée s'estompe toute seule.
public interface IKnockable
{
    void AddImpulse(Vector3 velocity);
}

// Peut être immobilisé un moment (taser).
public interface IImmobilizable
{
    void Immobilize(float seconds);
}
