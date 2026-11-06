using UnityEngine;

// Change la couleur d'un objet un court instant (coup reçu, immobilisé...), puis la remet.
public class ColorFlash : MonoBehaviour
{
    private Renderer _renderer;
    private Color _baseColor;
    private float _flashUntil;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer != null) _baseColor = _renderer.material.color;
    }

    public void Flash(Color color, float seconds)
    {
        if (_renderer == null) return;
        // Un effet plus long déjà en cours n'est pas écrasé par un effet plus court.
        if (Time.time + seconds < _flashUntil) return;
        _renderer.material.color = color;
        _flashUntil = Time.time + seconds;
    }

    private void Update()
    {
        if (_flashUntil > 0f && Time.time >= _flashUntil)
        {
            _renderer.material.color = _baseColor;
            _flashUntil = 0f;
        }
    }
}
