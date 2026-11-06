using System;
using UnityEngine;

// La cagnotte commune de l'équipe (monnaie 100 % virtuelle).
// En multijoueur, seul l'hôte modifiera ce montant ; les autres joueurs ne feront que l'afficher.
public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    public int Money { get; private set; }
    public event Action<int> Changed;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Add(int amount)
    {
        Money += amount;
        Changed?.Invoke(Money);
    }

    public bool TrySpend(int amount)
    {
        if (amount > Money) return false;
        Money -= amount;
        Changed?.Invoke(Money);
        return true;
    }
}
