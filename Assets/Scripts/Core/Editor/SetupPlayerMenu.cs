using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Outil d'éditeur : crée le joueur (capsule) et branche la caméra en un clic.
// Menu : ZOMBIEGAME > Créer le joueur dans la scène
public static class SetupPlayerMenu
{
    [MenuItem("ZOMBIEGAME/Équiper le pistolet du joueur")]
    private static void EquipPistol()
    {
        PlayerController player = Object.FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "Pas de joueur dans la scène.", "OK");
            return;
        }

        WeaponData weapon = AssetDatabase.LoadAssetAtPath<WeaponData>("Assets/Weapons/Pistolet.asset");
        Transform gun = player.transform.Find("Pistolet");
        if (weapon == null || gun == null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "Il manque l'arme (Assets/Weapons/Pistolet) ou le modèle (Player/Pistolet).", "OK");
            return;
        }

        // Le modèle de l'arme ne doit pas gêner le joueur.
        Collider gunCollider = gun.GetComponent<Collider>();
        if (gunCollider != null) Object.DestroyImmediate(gunCollider);

        PlayerShooter shooter = player.GetComponent<PlayerShooter>();
        if (shooter == null) shooter = Undo.AddComponent<PlayerShooter>(player.gameObject);

        SerializedObject so = new SerializedObject(shooter);
        SerializedProperty weapons = so.FindProperty("_weapons");
        weapons.arraySize = 1;
        weapons.GetArrayElementAtIndex(0).objectReferenceValue = weapon;
        so.FindProperty("_gunVisual").objectReferenceValue = gun;
        ThirdPersonCamera cam = Object.FindAnyObjectByType<ThirdPersonCamera>();
        so.FindProperty("_camera").objectReferenceValue = cam;
        so.ApplyModifiedProperties();

        EditorSceneManager.MarkSceneDirty(player.gameObject.scene);
    }

    [MenuItem("ZOMBIEGAME/Créer le joueur dans la scène")]
    private static void CreatePlayer()
    {
        if (Object.FindAnyObjectByType<PlayerController>() != null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "Un joueur existe déjà dans cette scène.", "OK");
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            EditorUtility.DisplayDialog("ZOMBIEGAME", "Pas de Main Camera dans la scène.", "OK");
            return;
        }

        // Capsule "Player".
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, 3f);
        Undo.RegisterCreatedObjectUndo(player, "Créer le joueur");

        // Le Character Controller remplace le collider de la capsule.
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        player.AddComponent<CharacterController>();
        player.AddComponent<PlayerController>();

        // Caméra à la 3e personne, avec le joueur comme cible.
        ThirdPersonCamera follow = cam.GetComponent<ThirdPersonCamera>();
        if (follow == null) follow = cam.gameObject.AddComponent<ThirdPersonCamera>();
        SerializedObject so = new SerializedObject(follow);
        so.FindProperty("_target").objectReferenceValue = player.transform;
        so.ApplyModifiedProperties();

        Selection.activeGameObject = player;
        EditorSceneManager.MarkSceneDirty(player.scene);
    }
}
