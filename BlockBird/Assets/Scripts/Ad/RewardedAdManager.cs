using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class RewardedAdManager : MonoBehaviour
{
    public RewardedAd rewardedAd; //������ ���� ���� ����
    public bool IsPlaying { get; set; }

    public void LoadRewardedAd()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-9125545321048807/3746492714";
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // �׽�Ʈ�� ���� ���� ID
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-9125545321048807/9464625771";
        //string adUnitId = "ca-app-pub-3940256099942544/4411468910"; // �׽�Ʈ�� ���� ���� ID
#else
        string adUnitId = "unexpected_platform";
#endif

        // ���� ���� �ε�
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

                // �̺�Ʈ �ڵ鷯 ���
                rewardedAd.OnAdFullScreenContentOpened += HandleOnAdOpened;
                rewardedAd.OnAdFullScreenContentClosed += HandleOnAdClosed;
                rewardedAd.OnAdFullScreenContentFailed += HandleOnAdFailedToShow;

            });
    }

    // ���� ���� ǥ�� �޼���
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
                //����
                PersistentObject.Instance.ShowMessagePopup(0, () => { });
            }
            else
            {
                //����
                PersistentObject.Instance.UserData.gem += (int)result.GetValue("amount");
                SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
            }
        }));
    }

    // �̺�Ʈ �ڵ鷯 �޼���
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

    // ���� ��ü �ı�
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
