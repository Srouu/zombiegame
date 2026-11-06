using UnityEngine;

// Pièce ramassable : tourne sur elle-même, est attirée par le joueur proche, puis ajoute à la cagnotte.
public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 5;
    [SerializeField] private float _pickupRadius = 0.9f;
    [SerializeField] private float _magnetRadius = 3f;
    [SerializeField] private float _magnetSpeed = 10f;
    [SerializeField] private float _spinSpeed = 180f;
    [SerializeField] private float _lifetime = 40f;

    private float _despawnTime;

    private void Start()
    {
        _despawnTime = Time.time + _lifetime;
    }

    public void SetValue(int value)
    {
        _value = value;
    }

    private void Update()
    {
        transform.Rotate(0f, _spinSpeed * Time.deltaTime, 0f, Space.World);

        if (Time.time >= _despawnTime)
        {
            Destroy(gameObject);
            return;
        }

        PlayerController player = PlayerController.Nearest(transform.position);
        if (player == null) return;

        Vector3 target = player.transform.position;
        float distance = Vector3.Distance(transform.position, target);

        if (distance <= _pickupRadius)
        {
            if (Wallet.Instance != null) Wallet.Instance.Add(_value);
            Destroy(gameObject);
        }
        else if (distance <= _magnetRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _magnetSpeed * Time.deltaTime);
        }
    }
}
