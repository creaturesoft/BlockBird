using UnityEngine;
using UnityEditor;
using System.IO;

public static class AppInfoMenu
{
    private const string assetPath = "Assets/AppInfo.asset";

    [MenuItem("App Tools/AppInfo")]
    public static void OpenOrCreateAppInfoAsset()
    {
        AppInfoSO asset = AssetDatabase.LoadAssetAtPath<AppInfoSO>(assetPath);

        if (asset == null)
        {
            // 없으면 새로 생성
            asset = ScriptableObject.CreateInstance<AppInfoSO>();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log("AppInfo.asset 새로 생성됨");
        }
        else
        {
            Debug.Log("기존 AppInfo.asset 열기");
        }

        // 선택해서 인스펙터에서 바로 보이게
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
