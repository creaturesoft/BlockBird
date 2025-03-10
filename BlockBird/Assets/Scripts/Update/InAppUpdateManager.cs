using UnityEngine;

#if UNITY_ANDROID
using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
#endif
public class InAppUpdateManager : MonoBehaviour
{
#if UNITY_ANDROID
    private AppUpdateManager _appUpdateManager;

    public void CheckAppUpdate()
    {
        _appUpdateManager = new AppUpdateManager();
        //StartCoroutine(CheckForAndroidUpdate());

        StartCoroutine(CheckForImmediateUpdate());
    }

    private IEnumerator CheckForAndroidUpdate()
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
            PersistentObject.Instance.ShowMessagePopup(2, () =>
            {
                _appUpdateManager.CompleteUpdate();
            }, null);

        }
        else
        {
            Debug.LogError("업데이트 중 오류 발생: " + startUpdateRequest.Error);
        }
    }

    IEnumerator CheckForImmediateUpdate()
    {
        var appUpdateInfoTask = _appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoTask;

        if (appUpdateInfoTask.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoTask.GetResult();

            // 강제 업데이트가 필요하면 즉시 업데이트 진행
            if (appUpdateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable &&
                appUpdateInfo.IsUpdateTypeAllowed(AppUpdateOptions.ImmediateAppUpdateOptions()))
            {
                PersistentObject.Instance.ShowMessagePopup(2, () =>
                {
                    StartCoroutine(StartImmediateUpdate(appUpdateInfo));
                }, null);

            }
        }
    }

    IEnumerator StartImmediateUpdate(AppUpdateInfo appUpdateInfo)
    {
        var startUpdateTask = _appUpdateManager.StartUpdate(appUpdateInfo, AppUpdateOptions.ImmediateAppUpdateOptions());
        yield return startUpdateTask;

        if (!startUpdateTask.IsDone)
        {
            Debug.LogError("즉시 업데이트 실패!");
        }
    }

#endif
}
