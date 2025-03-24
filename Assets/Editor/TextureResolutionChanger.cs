//using UnityEngine;
//using UnityEditor;

//public class TextureResolutionChanger : EditorWindow
//{
//    private int maxSize = 512; // 変更したい最大解像度

//    [MenuItem("Tools/Reduce Texture Resolution")]
//    static void Init()
//    {
//        TextureResolutionChanger window = (TextureResolutionChanger)EditorWindow.GetWindow(typeof(TextureResolutionChanger));
//        window.Show();
//    }

//    void OnGUI()
//    {
//        GUILayout.Label("テクスチャ解像度の一括変更", EditorStyles.boldLabel);
//        maxSize = EditorGUILayout.IntField("最大解像度", maxSize);

//        if (GUILayout.Button("適用"))
//        {
//            ChangeResolution();
//        }
//    }

//    void ChangeResolution()
//    {
//        // Unityのアセットデータベースを使用してすべてのテクスチャを検索
//        string[] guids = AssetDatabase.FindAssets("t:Texture2D"); // `t:Texture2D` で全テクスチャを検索

//        foreach (string guid in guids)
//        {
//            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
//            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

//            if (importer != null)
//            {
//                importer.maxTextureSize = maxSize; // 解像度変更
//                importer.SaveAndReimport(); // 変更を適用
//            }
//        }

//        Debug.Log($"すべてのテクスチャの解像度を {maxSize}px に変更しました！");
//    }
//}
