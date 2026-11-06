using UnityEngine;
using UnityEngine.UI;

// Affichage minimal : argent de l'équipe et arme en main.
public class Hud : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _weaponText;

    private PlayerShooter _shooter;

    private void Start()
    {
        if (Wallet.Instance != null)
        {
            Wallet.Instance.Changed += OnMoneyChanged;
            OnMoneyChanged(Wallet.Instance.Money);
        }

        _shooter = FindAnyObjectByType<PlayerShooter>();
        if (_shooter != null)
        {
            _shooter.WeaponChanged += OnWeaponChanged;
            OnWeaponChanged(_shooter.Current);
        }
    }

    private void OnDestroy()
    {
        if (Wallet.Instance != null) Wallet.Instance.Changed -= OnMoneyChanged;
        if (_shooter != null) _shooter.WeaponChanged -= OnWeaponChanged;
    }

    private void OnMoneyChanged(int money)
    {
        if (_moneyText != null) _moneyText.text = "Argent : " + money;
    }

    private void OnWeaponChanged(WeaponData weapon)
    {
        if (_weaponText != null && weapon != null)
            _weaponText.text = weapon.weaponName + "  (touches 1 / 2)";
    }
}
