using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    public bool IsPlaying { get; set; }

    void Start()
    {
    }

    public void LoadInterstitialAd()
    {
        if(PersistentObject.Instance.IsNoAd == true)
        {
            return;
        }

#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-9125545321048807/4926614968";
        string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // 테스트용 전면 광고 ID
#elif UNITY_IPHONE
        //string adUnitId = "ca-app-pub-9125545321048807/8807247707";
        string adUnitId = "ca-app-pub-3940256099942544/4411468910"; // 테스트용 전면 광고 ID
#else
        string adUnitId = "unexpected_platform";
#endif

        // 전면 광고 로드
        InterstitialAd.Load(adUnitId, new AdRequest(),
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Failed to load interstitial ad: " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded successfully.");
                interstitialAd = ad;

                // 이벤트 핸들러 등록
                interstitialAd.OnAdFullScreenContentOpened += HandleOnAdOpened;
                interstitialAd.OnAdFullScreenContentClosed += HandleOnAdClosed;
                interstitialAd.OnAdFullScreenContentFailed += HandleOnAdFailedToShow;
            });
    }

    // 전면 광고 표시 메서드
    public void ShowInterstitial()
    {
        if (PersistentObject.Instance.IsNoAd == true)
        {
            return;
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            IsPlaying = true;
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    // 이벤트 핸들러 메서드
    private void HandleOnAdOpened()
    {
        Debug.Log("Interstitial ad opened.");
    }

    private void HandleOnAdClosed()
    {
        Debug.Log("Interstitial ad closed.");
        IsPlaying = false;
        Destroy();
        // 광고가 닫힐 때 새로운 광고 로드
        LoadInterstitialAd();
    }

    private void HandleOnAdFailedToShow(AdError adError)
    {
        Debug.LogError("Failed to show interstitial ad: " + adError.GetMessage());
        IsPlaying = false;
        Destroy();
        LoadInterstitialAd();
    }

    // 광고 객체 파괴
    private void Destroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.OnAdFullScreenContentOpened -= HandleOnAdOpened;
            interstitialAd.OnAdFullScreenContentClosed -= HandleOnAdClosed;
            interstitialAd.OnAdFullScreenContentFailed -= HandleOnAdFailedToShow;
            interstitialAd.Destroy();
        }
    }
}
