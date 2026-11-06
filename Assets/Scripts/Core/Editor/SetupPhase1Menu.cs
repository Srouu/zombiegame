using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Outil d'éditeur : installe dans la scène tout ce qu'il faut pour la phase 1
// (zombies, pièces, baril, coéquipier de test, taser, cagnotte, affichage).
// Menu : ZOMBIEGAME > Phase 1 : installer la scène de test
public static class SetupPhase1Menu
{
    private const string MaterialsFolder = "Assets/Art/Materials";
    private const string PrefabsFolder = "Assets/Prefabs";
    private const string WeaponsFolder = "Assets/Weapons";

    [MenuItem("ZOMBIEGAME/Phase 1 : installer la scène de test")]
    private static void Install()
    {
        if (GameObject.Find("GameManager") != null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "La scène de test est déjà installée.", "OK");
            return;
        }

        PlayerShooter shooter = Object.FindAnyObjectByType<PlayerShooter>();
        if (shooter == null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "Installe d'abord le joueur et son pistolet.", "OK");
            return;
        }

        EnsureFolder("Assets/Art", "Materials");

        Material green = MakeMaterial("Zombie", new Color(0.4f, 0.8f, 0.3f));
        Material yellow = MakeMaterial("Piece", new Color(1f, 0.85f, 0.1f));
        Material red = MakeMaterial("Baril", new Color(0.85f, 0.15f, 0.1f));
        Material blue = MakeMaterial("Coequipier", new Color(0.2f, 0.45f, 0.9f));

        // --- Pièce (prefab) ---
        GameObject coinObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        coinObject.name = "Piece";
        Object.DestroyImmediate(coinObject.GetComponent<Collider>());
        coinObject.transform.localScale = new Vector3(0.4f, 0.04f, 0.4f);
        coinObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        coinObject.GetComponent<Renderer>().sharedMaterial = yellow;
        coinObject.AddComponent<Coin>();
        Coin coinPrefab = SavePrefab(coinObject, "Piece").GetComponent<Coin>();

        // --- Zombie (prefab) ---
        GameObject zombieObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        zombieObject.name = "Zombie";
        Object.DestroyImmediate(zombieObject.GetComponent<Collider>());
        zombieObject.GetComponent<Renderer>().sharedMaterial = green;
        zombieObject.AddComponent<ZombieController>(); // ajoute aussi CharacterController et Health
        zombieObject.AddComponent<ColorFlash>();
        SerializedObject zombieSo = new SerializedObject(zombieObject.GetComponent<ZombieController>());
        zombieSo.FindProperty("_coinPrefab").objectReferenceValue = coinPrefab;
        zombieSo.ApplyModifiedProperties();
        GameObject zombiePrefab = SavePrefab(zombieObject, "Zombie");

        // --- Spawner de zombies ---
        GameObject spawner = new GameObject("ZombieSpawner");
        spawner.transform.position = new Vector3(0f, 0f, 0f);
        ZombieSpawner spawnerScript = spawner.AddComponent<ZombieSpawner>();
        SerializedObject spawnerSo = new SerializedObject(spawnerScript);
        spawnerSo.FindProperty("_zombiePrefab").objectReferenceValue = zombiePrefab;
        spawnerSo.ApplyModifiedProperties();

        // --- Barils explosifs ---
        MakeBarrel(new Vector3(-4f, 0.5f, 6f), red);
        MakeBarrel(new Vector3(-5.6f, 0.5f, 6.4f), red);

        // --- Coéquipier de test (tir allié) ---
        GameObject mate = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        mate.name = "CoequipierTest";
        mate.transform.position = new Vector3(6f, 1f, 10f);
        mate.GetComponent<Renderer>().sharedMaterial = blue;
        mate.AddComponent<TeammateDummy>();

        // --- Cagnotte ---
        GameObject manager = new GameObject("GameManager");
        manager.AddComponent<Wallet>();

        // --- Taser (arme 2) ---
        EnsureFolder("Assets", "Weapons");
        WeaponData pistol = AssetDatabase.LoadAssetAtPath<WeaponData>(WeaponsFolder + "/Pistolet.asset");
        WeaponData taser = ScriptableObject.CreateInstance<WeaponData>();
        taser.weaponName = "Taser";
        taser.damage = 0f;
        taser.fireInterval = 0.8f;
        taser.range = 30f;
        taser.immobilizeSeconds = 3f;
        taser.cameraKickDegrees = 0.4f;
        taser.gunKickDistance = 0.04f;
        taser.gunColor = new Color(1f, 0.9f, 0.1f);
        AssetDatabase.CreateAsset(taser, WeaponsFolder + "/Taser.asset");

        SerializedObject shooterSo = new SerializedObject(shooter);
        SerializedProperty weapons = shooterSo.FindProperty("_weapons");
        weapons.arraySize = 2;
        weapons.GetArrayElementAtIndex(0).objectReferenceValue = pistol;
        weapons.GetArrayElementAtIndex(1).objectReferenceValue = taser;
        shooterSo.ApplyModifiedProperties();

        // --- Affichage (argent + arme) ---
        CreateHud();

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    private static void MakeBarrel(Vector3 position, Material material)
    {
        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrel.name = "BarilExplosif";
        barrel.transform.position = position;
        barrel.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
        barrel.GetComponent<Renderer>().sharedMaterial = material;
        barrel.AddComponent<ExplosiveBarrel>(); // ajoute aussi Health
        SerializedObject so = new SerializedObject(barrel.GetComponent<Health>());
        so.FindProperty("_maxHealth").floatValue = 20f;
        so.ApplyModifiedProperties();
    }

    private static void CreateHud()
    {
        GameObject canvasObject = new GameObject("Hud");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        Text money = MakeText(canvasObject.transform, "TexteArgent", new Vector2(0f, 1f), new Vector2(30f, -30f), TextAnchor.UpperLeft);
        Text weapon = MakeText(canvasObject.transform, "TexteArme", new Vector2(0f, 0f), new Vector2(30f, 30f), TextAnchor.LowerLeft);

        Hud hud = canvasObject.AddComponent<Hud>();
        SerializedObject so = new SerializedObject(hud);
        so.FindProperty("_moneyText").objectReferenceValue = money;
        so.FindProperty("_weaponText").objectReferenceValue = weapon;
        so.ApplyModifiedProperties();
    }

    private static Text MakeText(Transform parent, string name, Vector2 anchor, Vector2 position, TextAnchor alignment)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        Text text = go.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 42;
        text.fontStyle = FontStyle.Bold;
        text.color = Color.white;
        text.alignment = alignment;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        RectTransform rect = text.rectTransform;
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(900f, 80f);
        return text;
    }

    private static Material MakeMaterial(string name, Color color)
    {
        string path = MaterialsFolder + "/" + name + ".mat";
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) return existing;

        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static GameObject SavePrefab(GameObject sceneObject, string name)
    {
        string path = PrefabsFolder + "/" + name + ".prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(sceneObject, path);
        Object.DestroyImmediate(sceneObject);
        return prefab;
    }

    private static void EnsureFolder(string parent, string folder)
    {
        if (!AssetDatabase.IsValidFolder(parent + "/" + folder))
            AssetDatabase.CreateFolder(parent, folder);
    }
}
