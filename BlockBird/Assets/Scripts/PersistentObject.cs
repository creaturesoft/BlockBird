using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Runtime.InteropServices;
using Unity.Services.Core;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void RequestTrackingAuthorization();
#endif

    // 인스턴스를 저장하는 정적 변수
    private static PersistentObject _instance;

    // 외부에서 싱글톤 인스턴스에 접근할 수 있도록 하는 프로퍼티
    public static PersistentObject Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool IsNoAd { get; set; }
    public static int InterstitialAdRate = 3;   //4;
    public static int RewardedAdRate = 4;   //4;

    public InterstitialAdManager interstitialAdManager;
    public RewardedAdManager rewardedAdManager;

    public AudioSource[] backgroundAudioSources;


    public GameObject spinningLoadingPrefab;
    private GameObject spinningLoading;

    public GameObject messagePopupPrefab;


    void Awake()
    {
        // 씬 전환 시 이 오브젝트가 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            RequestTrackingAuthorization();
        }
#endif
        ConsentRequestParameters requestParameters = new ConsentRequestParameters();

        // 동의 정보 업데이트
        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            if (updateError != null)
            {
                Debug.LogError($"동의 정보 업데이트 실패: {updateError.Message}");
                return;
            }

            // 동의 상태가 이미 동의된 경우 폼을 표시하지 않음
            if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
            {
                Debug.Log("동의 상태가 이미 동의로 설정되었습니다. 동의 창을 표시하지 않습니다.");
                return;
            }

            // 동의 폼 사용 가능 여부 확인
            if (ConsentInformation.IsConsentFormAvailable())
            {
                // 동의 폼 로드
                ConsentForm.Load((ConsentForm consentForm, FormError loadError) =>
                {
                    if (loadError != null)
                    {
                        Debug.LogError($"동의 폼 로드 실패: {loadError.Message}");
                        return;
                    }

                    // 동의 폼이 성공적으로 로드되었으므로 표시
                    consentForm.Show((FormError showError) =>
                    {
                        if (showError != null)
                        {
                            Debug.LogError($"동의 폼 표시 실패: {showError.Message}");
                        }
                        else
                        {
                            Debug.Log("동의 폼이 성공적으로 표시되었습니다.");
                        }
                    });
                });
            }
            else
            {
                Debug.Log("동의 폼을 사용할 수 없습니다.");
            }
        });

        //애드몹 초기화
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("[AdMob initialized]");

            if (SaveLoadManager.LoadNoADData() == "true")
            {
                IsNoAd = true;
            }
            else
            {
                interstitialAdManager.LoadInterstitialAd();
            }

            rewardedAdManager.LoadRewardedAd();
        });

        //유니티 게임 서비스 초기화
        try
        {
            UnityServices.InitializeAsync();
            Debug.Log("Unity Gaming Services initialized successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Gaming Services: {e.Message}");
        }

        //배경음악 재생
        backgroundAudioSources[UnityEngine.Random.Range(0, backgroundAudioSources.Length)].Play();
    }


    public void StartSpinningLoading()
    {
        spinningLoading = Instantiate(spinningLoadingPrefab);
    }

    public void StopSpinningLoading()
    {
        Destroy(spinningLoading);
    }


    public void ShowMessagePopup(int index, Action okCallback = null, Action closeCallback = null)
    {
        MessagePopup messagePopup = Instantiate(messagePopupPrefab).GetComponent<MessagePopup>();
        messagePopup.ShowMessage(index, okCallback, closeCallback);
    }

}

