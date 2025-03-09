using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class iOSUpdateChecker : MonoBehaviour
{
    // iTunes Lookup API URL
    // TODO! : YOUR_BUNDLE_ID, YOUR_APP_ID ����
    private string appStoreURL = "https://itunes.apple.com/lookup?bundleId=com.Creaturesoft.BlockBird";
    private string appStoreOpenURL = "https://apps.apple.com/app/id6741071606";

    void Start()
    {
#if UNITY_IOS
        StartCoroutine(CheckForiOSUpdate());
#endif
    }

    IEnumerator CheckForiOSUpdate()
    {
        UnityWebRequest request = UnityWebRequest.Get(appStoreURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResult = request.downloadHandler.text;
            var appInfo = JsonUtility.FromJson<AppStoreInfoWrapper>(jsonResult);

            if (appInfo.resultCount > 0)
            {
                string appStoreVersion = appInfo.results[0].version;
                string currentVersion = Application.version;

                Debug.Log("���� �� ����: " + currentVersion);
                Debug.Log("App Store ����: " + appStoreVersion);

                if (IsNewVersion(appStoreVersion, currentVersion))
                {
                    ShowUpdateDialog();
                }
                else
                {
                    Debug.Log("�ֽ� ������ ��� ���Դϴ�.");
                }
            }
            else
            {
                Debug.LogError("App Store���� �� ������ ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("���� Ȯ�� ����: " + request.error);
        }
    }

    // ���� ��
    bool IsNewVersion(string appStoreVersion, string currentVersion)
    {
        System.Version storeVersion = new System.Version(appStoreVersion);
        System.Version localVersion = new System.Version(currentVersion);
        return storeVersion.CompareTo(localVersion) > 0;
    }

    // ������Ʈ ���� �˾�
    void ShowUpdateDialog()
    {
        // Unity�� �⺻ �˾� â
        Debug.Log("���ο� ������ �ֽ��ϴ�. ������Ʈ�Ͻðڽ��ϱ�?");

        // ����: ����ڰ� '������Ʈ' ��ư�� ������ �� App Store�� �̵�
        PersistentObject.Instance.ShowMessagePopup(2, () =>
        {
            Application.OpenURL(appStoreOpenURL);
        }, null);

    }

    // iTunes Search API ������ ó���ϱ� ���� Ŭ����
    [Serializable]
    public class AppStoreInfoWrapper
    {
        public int resultCount;
        public AppStoreInfo[] results;
    }

    [Serializable]
    public class AppStoreInfo
    {
        public string version;
    }
}
