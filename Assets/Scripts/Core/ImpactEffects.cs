using UnityEngine;

// Petits effets de particules créés par le code (mort de zombie, explosion).
public static class ImpactEffects
{
    private static Material _particleMaterial;

    public static void Burst(Vector3 position, Color color, int count = 20, float speed = 4f, float size = 0.2f)
    {
        GameObject go = new GameObject("EffetParticules");
        go.transform.position = position;

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 0.5f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = 0.7f;
        main.startSpeed = speed;
        main.startSize = size;
        main.startColor = color;
        main.gravityModifier = 1f;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)count) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        go.GetComponent<ParticleSystemRenderer>().material = GetMaterial();
        ps.Play();
    }

    private static Material GetMaterial()
    {
        if (_particleMaterial == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            _particleMaterial = new Material(shader);
        }
        return _particleMaterial;
    }
}
