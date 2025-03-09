using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class RewardedAdManager : MonoBehaviour
{
    public RewardedAd rewardedAd; //보상형 광고 관리 변수
    public bool IsPlaying { get; set; }

    public void LoadRewardedAd()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-9125545321048807/3746492714";
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // 테스트용 전면 광고 ID
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-9125545321048807/9464625771";
        //string adUnitId = "ca-app-pub-3940256099942544/4411468910"; // 테스트용 전면 광고 ID
#else
        string adUnitId = "unexpected_platform";
#endif

        // 전면 광고 로드
        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Failed to load rewarded ad: " + error);
                    return;
                }

                Debug.Log("rewarded ad loaded successfully.");
                rewardedAd = ad;

                // 이벤트 핸들러 등록
                rewardedAd.OnAdFullScreenContentOpened += HandleOnAdOpened;
                rewardedAd.OnAdFullScreenContentClosed += HandleOnAdClosed;
                rewardedAd.OnAdFullScreenContentFailed += HandleOnAdFailedToShow;

            });
    }

    // 전면 광고 표시 메서드
    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            IsPlaying = true;
            rewardedAd.Show((Reward reward) => {
                Debug.Log("ad rewarded with: " + reward.Type + " " + reward.Amount);
                //int rewardAmount = int.Parse(reward.Amount.ToString());
                int amount = UnityEngine.Random.Range(1, 6);
                GetRewardGem(amount);

                ToastNotification.Show("+" + amount);

            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    public void GetRewardGem(int amount)
    {
        string data = "{\"userId\": \"" + PersistentObject.Instance.UserData.userId + "\", " +
                "\"type\": \"REWARD\", " +
                "\"amount\": " + amount + "}";

        StartCoroutine(WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            if (result == null)
            {
                //실패
                PersistentObject.Instance.ShowMessagePopup(0, () => { });
            }
            else
            {
                //성공
                PersistentObject.Instance.UserData.gem += (int)result.GetValue("amount");
                SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
            }
        }));
    }

    // 이벤트 핸들러 메서드
    private void HandleOnAdOpened()
    {
        Debug.Log("Rewarded ad opened.");
    }

    private void HandleOnAdClosed()
    {
        Debug.Log("Rewarded ad closed.");
        IsPlaying = false;
        Destroy();
        LoadRewardedAd();
    }

    private void HandleOnAdFailedToShow(AdError adError)
    {
        Debug.LogError("Failed to show rewarded ad: " + adError.GetMessage());
        IsPlaying = false;
        Destroy();
        LoadRewardedAd();
    }

    // 광고 객체 파괴
    private void Destroy()
    {
        if (rewardedAd != null)
        {
            rewardedAd.OnAdFullScreenContentOpened -= HandleOnAdOpened;
            rewardedAd.OnAdFullScreenContentClosed -= HandleOnAdClosed;
            rewardedAd.OnAdFullScreenContentFailed -= HandleOnAdFailedToShow;
            rewardedAd.Destroy();
        }
    }
}
