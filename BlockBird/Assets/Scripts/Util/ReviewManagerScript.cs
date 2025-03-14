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
        // ReviewManager 인스턴스 생성
#if UNITY_ANDROID
        _reviewManager = new ReviewManager();
#endif

    }

    public void Close()
    {
        IsPlaying = false;
        Destroy(gameObject);
    }

    // 인앱 리뷰 요청 코루틴
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
        //2초 후에 ok 누르면 리뷰 작성한 것으로 간주
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
        // 리뷰 플로우 요청
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // 에러 처리
            Debug.LogError($"리뷰 플로우 요청 실패: {requestFlowOperation.Error}");
            OpenAppStore();

            yield break;
        }

        // PlayReviewInfo 객체 획득
        _playReviewInfo = requestFlowOperation.GetResult();

        // 리뷰 플로우 시작
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // 에러 처리
            Debug.LogError($"리뷰 플로우 시작 실패: {launchFlowOperation.Error}");
            OpenAppStore();

            yield break;
        }

        // 리뷰 플로우 완료
        Debug.Log("리뷰 플로우 완료");
    }
#endif

    private void OpenAppStore()
    {
#if UNITY_ANDROID
            string packageName = "com.Creaturesoft.BlockBird"; // 앱 패키지명 변경
            string reviewUrl = "https://play.google.com/store/apps/details?id=" + packageName + "&reviewId=0";
            Application.OpenURL(reviewUrl);
#elif UNITY_IOS
            string appId = "6741071606"; // iOS 앱스토어 ID 입력
            string reviewUrl = "itms-apps://itunes.apple.com/app/id" + appId + "?action=write-review";
            Application.OpenURL(reviewUrl);
#endif
    }
}
