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
        string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // �׽�Ʈ�� ���� ���� ID
#elif UNITY_IPHONE
        //string adUnitId = "ca-app-pub-9125545321048807/8807247707";
        string adUnitId = "ca-app-pub-3940256099942544/4411468910"; // �׽�Ʈ�� ���� ���� ID
#else
        string adUnitId = "unexpected_platform";
#endif

        // ���� ���� �ε�
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

                // �̺�Ʈ �ڵ鷯 ���
                interstitialAd.OnAdFullScreenContentOpened += HandleOnAdOpened;
                interstitialAd.OnAdFullScreenContentClosed += HandleOnAdClosed;
                interstitialAd.OnAdFullScreenContentFailed += HandleOnAdFailedToShow;
            });
    }

    // ���� ���� ǥ�� �޼���
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

    // �̺�Ʈ �ڵ鷯 �޼���
    private void HandleOnAdOpened()
    {
        Debug.Log("Interstitial ad opened.");
    }

    private void HandleOnAdClosed()
    {
        Debug.Log("Interstitial ad closed.");
        IsPlaying = false;
        Destroy();
        // ���� ���� �� ���ο� ���� �ε�
        LoadInterstitialAd();
    }

    private void HandleOnAdFailedToShow(AdError adError)
    {
        Debug.LogError("Failed to show interstitial ad: " + adError.GetMessage());
        IsPlaying = false;
        Destroy();
        LoadInterstitialAd();
    }

    // ���� ��ü �ı�
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
