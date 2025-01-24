using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AutoIncrementPlatformVersion : IPreprocessBuildWithReport
{
    // 빌드 전에 실행되는 우선순위
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // 플랫폼에 따라 다른 동작 수행
        if (report.summary.platform == BuildTarget.Android)
        {
            IncrementAndroidVersion();
        }
        else if (report.summary.platform == BuildTarget.iOS)
        {
            IncrementiOSVersion();
        }
    }

    // 안드로이드 Version 및 Version Code 증가
    private void IncrementAndroidVersion()
    {
        // Version Code 증가
        int currentVersionCode = PlayerSettings.Android.bundleVersionCode;
        PlayerSettings.Android.bundleVersionCode = currentVersionCode + 1;

        // Version 증가
        IncrementBundleVersion();
    }

    // iOS Version 및 Build Number 증가
    private void IncrementiOSVersion()
    {
        // Build Number 증가
        int currentBuildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
        PlayerSettings.iOS.buildNumber = (currentBuildNumber + 1).ToString();

        // Version 증가
        IncrementBundleVersion();
    }

    // Version 증가 로직 (모든 플랫폼 공통)
    private void IncrementBundleVersion()
    {
        string currentVersion = PlayerSettings.bundleVersion;

        // 현재 버전을 "."으로 나누기
        string[] versionParts = currentVersion.Split('.');

        // 버전의 마지막 숫자 증가
        if (versionParts.Length > 2)
        {
            int patchNumber = int.Parse(versionParts[2]) + 1;
            versionParts[2] = patchNumber.ToString();
        }

        // 새 버전 설정
        PlayerSettings.bundleVersion = string.Join(".", versionParts);
    }
}
