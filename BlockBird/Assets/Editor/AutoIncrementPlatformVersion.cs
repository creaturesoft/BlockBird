using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AutoIncrementPlatformVersion : IPreprocessBuildWithReport
{
    // ���� ���� ����Ǵ� �켱����
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // �÷����� ���� �ٸ� ���� ����
        if (report.summary.platform == BuildTarget.Android)
        {
            IncrementAndroidVersion();
        }
        else if (report.summary.platform == BuildTarget.iOS)
        {
            IncrementiOSVersion();
        }
    }

    // �ȵ���̵� Version �� Version Code ����
    private void IncrementAndroidVersion()
    {
        // Version Code ����
        int currentVersionCode = PlayerSettings.Android.bundleVersionCode;
        PlayerSettings.Android.bundleVersionCode = currentVersionCode + 1;

        // Version ����
        IncrementBundleVersion();
    }

    // iOS Version �� Build Number ����
    private void IncrementiOSVersion()
    {
        // Build Number ����
        int currentBuildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
        PlayerSettings.iOS.buildNumber = (currentBuildNumber + 1).ToString();

        // Version ����
        IncrementBundleVersion();
    }

    // Version ���� ���� (��� �÷��� ����)
    private void IncrementBundleVersion()
    {
        string currentVersion = PlayerSettings.bundleVersion;

        // ���� ������ "."���� ������
        string[] versionParts = currentVersion.Split('.');

        // ������ ������ ���� ����
        if (versionParts.Length > 2)
        {
            int patchNumber = int.Parse(versionParts[2]) + 1;
            versionParts[2] = patchNumber.ToString();
        }

        // �� ���� ����
        PlayerSettings.bundleVersion = string.Join(".", versionParts);
    }
}
