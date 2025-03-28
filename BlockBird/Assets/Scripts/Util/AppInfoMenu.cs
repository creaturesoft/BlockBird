using UnityEngine;
using UnityEditor;

public static class AppInfoMenu
{
    [MenuItem("App Tools/Create AppInfo Asset")]
    public static void CreateAppInfoAsset()
    {
        var asset = ScriptableObject.CreateInstance<AppInfoSO>();
        AssetDatabase.CreateAsset(asset, "Assets/AppInfo.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
