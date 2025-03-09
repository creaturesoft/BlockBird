using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using Google.Play.Review;
#endif

using System.Runtime.InteropServices;


public class ReviewManagerScript : MonoBehaviour
{

#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
#endif

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern bool RequestReviewiOS();
#endif

    private void Start()
    {
        // ReviewManager �ν��Ͻ� ����
#if UNITY_ANDROID
        _reviewManager = new ReviewManager();
#endif

    }

    public void Close()
    {
        IsPlaying = false;
        Destroy(gameObject);
    }

    // �ξ� ���� ��û �ڷ�ƾ
    public void RequestInAppReview()
    {
#if UNITY_ANDROID
        StartCoroutine(RequestReviewFlow());
#elif UNITY_IOS
        try
        {
            if (RequestReviewiOS() == false)
            {
                OpenAppStore();
            }
        }
        catch
        {
            OpenAppStore();
        }
#endif

        StartCoroutine(CheckTime());
        PersistentObject.Instance.ShowMessagePopup(3, CompleteReview, CompleteReview);
    }

    public bool IsPlaying { get; set; } = true;
    private bool isDone;
    IEnumerator CheckTime()
    {
        //2�� �Ŀ� ok ������ ���� �ۼ��� ������ ����
        yield return new WaitForSecondsRealtime(2.0f);
        isDone = true;
    }

    void CompleteReview()
    {
        if (!isDone)
        {
            OpenAppStore();
        }

        PersistentObject.Instance.UserData.isReviewed = true;
        SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
        StartCoroutine(SaveLoadManager.SendUserDataToServer(PersistentObject.Instance.UserData, Close));

    }

#if UNITY_ANDROID
    private IEnumerator RequestReviewFlow()
    {
        // ���� �÷ο� ��û
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // ���� ó��
            Debug.LogError($"���� �÷ο� ��û ����: {requestFlowOperation.Error}");
            OpenAppStore();

            yield break;
        }

        // PlayReviewInfo ��ü ȹ��
        _playReviewInfo = requestFlowOperation.GetResult();

        // ���� �÷ο� ����
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // ���� ó��
            Debug.LogError($"���� �÷ο� ���� ����: {launchFlowOperation.Error}");
            OpenAppStore();

            yield break;
        }

        // ���� �÷ο� �Ϸ�
        Debug.Log("���� �÷ο� �Ϸ�");
    }
#endif

    private void OpenAppStore()
    {
#if UNITY_ANDROID
            string packageName = "com.Creaturesoft.BlockBird"; // �� ��Ű���� ����
            string reviewUrl = "https://play.google.com/store/apps/details?id=" + packageName + "&reviewId=0";
            Application.OpenURL(reviewUrl);
#elif UNITY_IOS
            string appId = "6741071606"; // iOS �۽���� ID �Է�
            string reviewUrl = "itms-apps://itunes.apple.com/app/id" + appId + "?action=write-review";
            Application.OpenURL(reviewUrl);
#endif
    }
}
