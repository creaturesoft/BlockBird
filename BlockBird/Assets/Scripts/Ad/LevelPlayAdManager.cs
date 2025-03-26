using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayAdManager : MonoBehaviour
{
    string appKey;

    bool isBanner;
    bool isInterstitial;
    bool isRewarded;

    public bool IsPlayingInterstitial { get; set; }
    public bool IsPlayingRewarded { get; set; }

    Action onRewardCallback;

    public void InitAds(string ironSourceAppKey, bool useBanner, bool useInterstitial, bool useRewarded)
    {
        appKey = ironSourceAppKey;
        isBanner = useBanner;
        isInterstitial = useInterstitial;
        isRewarded = useRewarded;

        IronSource.Agent.setAdaptersDebug(true);
        IronSource.Agent.validateIntegration();
        IronSourceEvents.onSdkInitializationCompletedEvent += OnIronSourceInitialized;

        List<string> adUnits = new List<string>();
        if (useBanner)
        {
            adUnits.Add("banner");
        }
        if(useInterstitial)
        {
            adUnits.Add("interstitial");
        }
        if (useRewarded)
        {
            adUnits.Add("rewardedVideo");
        }


        RegisterEvents();
        IronSource.Agent.init(appKey, adUnits.ToArray());

    }

    void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }

    void OnIronSourceInitialized()
    {
        Debug.Log("IronSource SDK 초기화 완료");

        if (isBanner) LoadBanner();
        if (isInterstitial) LoadInterstitial();
        if (isRewarded) LoadRewardedAd();
    }

    // ---------------------- 배너 광고 ----------------------

    void LoadBanner()
    {
        IronSource.Agent.destroyBanner();
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
    }

    public void DestroyBanner()
    {
        Debug.Log("DestroyBanner");
        IronSource.Agent.destroyBanner();
        isBanner = false;
    }

    void BannerLoaded(IronSourceAdInfo info)
    {
        Debug.Log("배너 광고 로딩 완료");

        if (!isBanner)
        {
            StartCoroutine(DestroyBannerSafely());
        }
    }

    IEnumerator DestroyBannerSafely()
    {
        yield return new WaitForSeconds(1f); // 살짝 대기

        Debug.Log("DestroyBanner (BannerLoaded)");

        DestroyBanner();
    }

    void BannerLoadFailed(IronSourceError error)
    {
        Debug.LogError($"배너 광고 로딩 실패: {error.getDescription()}");
    }

    // ---------------------- 전면 광고 ----------------------

    void LoadInterstitial()
    {
        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstitial()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IsPlayingInterstitial = true;
            IronSource.Agent.showInterstitial();
        }
        else
        {
            Debug.Log("전면 광고가 아직 준비되지 않았습니다.");
            LoadInterstitial();
        }
    }

    void InterstitialLoaded(IronSourceAdInfo info)
    {
        Debug.Log("전면 광고 로딩 완료");
    }

    void InterstitialLoadFailed(IronSourceError error)
    {
        Debug.LogError($"전면 광고 로딩 실패: {error.getDescription()}");
    }

    void InterstitialClosed(IronSourceAdInfo info)
    {
        Debug.Log("전면 광고 닫힘 → 다시 로드");
        LoadInterstitial();
        IsPlayingInterstitial = false;
    }

    // ---------------------- 보상형 광고 ----------------------

    void LoadRewardedAd()
    {
        IronSource.Agent.loadRewardedVideo();
    }


    public void ShowRewardedAd(Action onReward, bool isAdFailReward)
    {
        onRewardCallback = onReward;

        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
            IsPlayingRewarded = true;
        }
        else
        {
            Debug.Log("보상형 광고가 준비되지 않았습니다. 바로 보상을 지급합니다.");
            if (isAdFailReward)
            {
                Reward();
            }
            LoadRewardedAd();
        }
    }

    void RewardedAdCompleted(IronSourcePlacement placement, IronSourceAdInfo info)
    {
        Debug.Log($"보상형 광고 시청 완료, 보상 지급: {placement.getRewardAmount()} {placement.getRewardName()}");
        Reward();
        LoadRewardedAd();

        IsPlayingRewarded = false;
    }

    void RewardedAdLoadFailed(IronSourceError error)
    {
        Debug.LogError($"보상형 광고 로딩 실패: {error.getDescription()}");
    }

    void Reward()
    {
        Debug.Log("계속하기 보상 지급");
        onRewardCallback?.Invoke();
        onRewardCallback = null;
    }

    // ---------------------- 이벤트 리스너 등록 ----------------------

    void RegisterEvents()
    {
        if (isBanner)
        {
            IronSourceBannerEvents.onAdLoadedEvent += BannerLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerLoadFailed;
        }

        if (isInterstitial)
        {
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialLoaded;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialLoadFailed;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialClosed;
        }

        if (isRewarded)
        {
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedAdCompleted;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += RewardedAdLoadFailed;
        }
    }

    void UnregisterEvents()
    {
        if (isBanner)
        {
            IronSourceBannerEvents.onAdLoadedEvent -= BannerLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent -= BannerLoadFailed;
        }

        if (isInterstitial)
        {
            IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialLoaded;
            IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialLoadFailed;
            IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialClosed;
        }

        if (isRewarded)
        {
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedAdCompleted;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent -= RewardedAdLoadFailed;
        }
    }

    void OnDestroy()
    {
        UnregisterEvents();
    }
}
