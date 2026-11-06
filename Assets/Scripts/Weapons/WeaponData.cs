using UnityEngine;

// Données d'une arme (réglables dans l'inspecteur, sans toucher au code).
[CreateAssetMenu(fileName = "NouvelleArme", menuName = "ZOMBIEGAME/Arme")]
public class WeaponData : ScriptableObject
{
    [Header("Tir")]
    public string weaponName = "Pistolet";
    public float damage = 10f;
    [Tooltip("Temps minimum entre deux tirs, en secondes.")]
    public float fireInterval = 0.25f;
    public float range = 60f;
    [Tooltip("Tir automatique si on garde le bouton appuyé.")]
    public bool automatic = false;

    [Header("Effet spécial")]
    [Tooltip("Si plus de 0 : la cible touchée est immobilisée ce nombre de secondes (taser).")]
    public float immobilizeSeconds = 0f;

    [Header("Recul")]
    [Tooltip("Degrés dont la caméra se relève à chaque tir.")]
    public float cameraKickDegrees = 2.5f;
    [Tooltip("Distance dont l'arme recule visuellement, en mètres.")]
    public float gunKickDistance = 0.12f;

    [Header("Apparence")]
    public Color gunColor = Color.white;
}
