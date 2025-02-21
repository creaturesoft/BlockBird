using System.Collections;
using UnityEngine;
using Google.Play.Review;
using System.Runtime.InteropServices;

public class ReviewManagerScript : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern bool RequestReviewiOS();
#endif

    private void Start()
    {
        // ReviewManager �ν��Ͻ� ����
        _reviewManager = new ReviewManager();

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

        GameManager.Instance.UserData.isReviewed = true;
        SaveLoadManager.SaveUserData(GameManager.Instance.UserData);
        StartCoroutine(SaveLoadManager.SendUserDataToServer(GameManager.Instance.UserData, Close));

    }

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

    private void OpenAppStore()
    {
        // TODO! : �۽���� ��ũ�� ����
#if UNITY_ANDROID
            string packageName = "com.yourcompany.yourapp"; // �� ��Ű���� ����
            string reviewUrl = "https://play.google.com/store/apps/details?id=" + packageName + "&reviewId=0";
            Application.OpenURL(reviewUrl);
#elif UNITY_IOS
            string appId = "YOUR_APP_ID"; // iOS �۽���� ID �Է�
            string reviewUrl = "itms-apps://itunes.apple.com/app/id" + appId + "?action=write-review";
            Application.OpenURL(reviewUrl);
#endif
    }
}
