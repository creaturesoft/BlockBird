using Google.Play.AppUpdate;
using Google.Play.Common;
using UnityEngine;
using System.Collections;

public class InAppUpdateManager : MonoBehaviour
{
    private AppUpdateManager _appUpdateManager;

    void Start()
    {
#if UNITY_ANDROID
        _appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
#endif
    }

    private IEnumerator CheckForUpdate()
    {
        var appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfo = appUpdateInfoOperation.GetResult();

            if (appUpdateInfo.AppUpdateStatus == AppUpdateStatus.Downloaded)
            {
                Debug.Log("이전 세션에서 다운로드된 업데이트가 있습니다.");
                PersistentObject.Instance.ShowMessagePopup(2, () =>
                {
                    _appUpdateManager.CompleteUpdate();
                }, null);
            }
            else if (appUpdateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable &&
                appUpdateInfo.IsUpdateTypeAllowed(AppUpdateOptions.FlexibleAppUpdateOptions()))
            {
                StartCoroutine(StartFlexibleUpdate(appUpdateInfo));
            }
        }
        else
        {
            Debug.LogError("업데이트 정보 가져오기 실패: " + appUpdateInfoOperation.Error);
        }
    }

    private IEnumerator StartFlexibleUpdate(AppUpdateInfo appUpdateInfo)
    {
        var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();
        var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfo, appUpdateOptions);

        while (!startUpdateRequest.IsDone)
        {
            if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
            {
                Debug.Log($"다운로드 진행 중: {startUpdateRequest.DownloadProgress * 100}%");
            }
            yield return null;
        }

        if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
        {
            Debug.Log("업데이트 다운로드 완료. 앱 재실행 시 설치를 진행합니다.");
            //PersistentObject.Instance.ShowMessagePopup(2, () =>
            //{
            //    _appUpdateManager.CompleteUpdate();
            //}, null);

        }
        else
        {
            Debug.LogError("업데이트 중 오류 발생: " + startUpdateRequest.Error);
        }
    }
}
