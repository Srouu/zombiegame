using UnityEngine;

// Fait apparaître des zombies en cercle autour de ce point, jusqu'à un maximum en vie.
// En multijoueur, seul l'hôte fera apparaître les zombies.
public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private int _maxAlive = 5;
    [SerializeField] private float _interval = 3f;
    [SerializeField] private float _radius = 15f;
    [SerializeField] private float _spawnHeight = 1.1f;
    [SerializeField] private float _minDistanceFromPlayer = 8f;

    private float _nextSpawn;
    private int _alive;

    private void Update()
    {
        if (_zombiePrefab == null || _alive >= _maxAlive || Time.time < _nextSpawn) return;
        _nextSpawn = Time.time + _interval;

        // Quelques essais pour ne pas faire apparaître un zombie collé au joueur.
        for (int attempt = 0; attempt < 5; attempt++)
        {
            Vector2 circle = Random.insideUnitCircle.normalized * _radius;
            Vector3 position = transform.position + new Vector3(circle.x, _spawnHeight, circle.y);

            PlayerController player = PlayerController.Nearest(position);
            if (player != null && Vector3.Distance(player.transform.position, position) < _minDistanceFromPlayer)
                continue;

            GameObject zombie = Instantiate(_zombiePrefab, position, Quaternion.identity);
            Health health = zombie.GetComponent<Health>();
            if (health != null)
            {
                _alive++;
                health.Died += OnZombieDied;
            }
            return;
        }
    }

    private void OnZombieDied(Health health)
    {
        _alive--;
    }
}
