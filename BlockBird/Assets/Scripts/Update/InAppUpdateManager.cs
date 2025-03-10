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
                Debug.Log("���� ���ǿ��� �ٿ�ε�� ������Ʈ�� �ֽ��ϴ�.");
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
            Debug.LogError("������Ʈ ���� �������� ����: " + appUpdateInfoOperation.Error);
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
                Debug.Log($"�ٿ�ε� ���� ��: {startUpdateRequest.DownloadProgress * 100}%");
            }
            yield return null;
        }

        if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
        {
            Debug.Log("������Ʈ �ٿ�ε� �Ϸ�. �� ����� �� ��ġ�� �����մϴ�.");
            PersistentObject.Instance.ShowMessagePopup(2, () =>
            {
                _appUpdateManager.CompleteUpdate();
            }, null);

        }
        else
        {
            Debug.LogError("������Ʈ �� ���� �߻�: " + startUpdateRequest.Error);
        }
    }

    IEnumerator CheckForImmediateUpdate()
    {
        var appUpdateInfoTask = _appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoTask;

        if (appUpdateInfoTask.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoTask.GetResult();

            // ���� ������Ʈ�� �ʿ��ϸ� ��� ������Ʈ ����
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
            Debug.LogError("��� ������Ʈ ����!");
        }
    }

#endif
}
