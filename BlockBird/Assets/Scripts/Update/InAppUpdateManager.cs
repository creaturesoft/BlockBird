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
            //PersistentObject.Instance.ShowMessagePopup(2, () =>
            //{
            //    _appUpdateManager.CompleteUpdate();
            //}, null);

        }
        else
        {
            Debug.LogError("������Ʈ �� ���� �߻�: " + startUpdateRequest.Error);
        }
    }
}
