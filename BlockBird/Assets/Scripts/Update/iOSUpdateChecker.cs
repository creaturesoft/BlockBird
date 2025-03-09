using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class iOSUpdateChecker : MonoBehaviour
{
    // iTunes Lookup API URL
    // TODO! : YOUR_BUNDLE_ID, YOUR_APP_ID 수정
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

                Debug.Log("현재 앱 버전: " + currentVersion);
                Debug.Log("App Store 버전: " + appStoreVersion);

                if (IsNewVersion(appStoreVersion, currentVersion))
                {
                    ShowUpdateDialog();
                }
                else
                {
                    Debug.Log("최신 버전을 사용 중입니다.");
                }
            }
            else
            {
                Debug.LogError("App Store에서 앱 정보를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("버전 확인 실패: " + request.error);
        }
    }

    // 버전 비교
    bool IsNewVersion(string appStoreVersion, string currentVersion)
    {
        System.Version storeVersion = new System.Version(appStoreVersion);
        System.Version localVersion = new System.Version(currentVersion);
        return storeVersion.CompareTo(localVersion) > 0;
    }

    // 업데이트 유도 팝업
    void ShowUpdateDialog()
    {
        // Unity의 기본 팝업 창
        Debug.Log("새로운 버전이 있습니다. 업데이트하시겠습니까?");

        // 예시: 사용자가 '업데이트' 버튼을 눌렀을 때 App Store로 이동
        PersistentObject.Instance.ShowMessagePopup(2, () =>
        {
            Application.OpenURL(appStoreOpenURL);
        }, null);

    }

    // iTunes Search API 응답을 처리하기 위한 클래스
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
