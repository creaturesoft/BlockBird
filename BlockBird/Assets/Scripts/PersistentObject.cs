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

    // �ν��Ͻ��� �����ϴ� ���� ����
    private static PersistentObject _instance;

    // �ܺο��� �̱��� �ν��Ͻ��� ������ �� �ֵ��� �ϴ� ������Ƽ
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
        // �� ��ȯ �� �� ������Ʈ�� �ı����� �ʵ��� ����
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

        // ���� ���� ������Ʈ
        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            if (updateError != null)
            {
                Debug.LogError($"���� ���� ������Ʈ ����: {updateError.Message}");
                return;
            }

            // ���� ���°� �̹� ���ǵ� ��� ���� ǥ������ ����
            if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
            {
                Debug.Log("���� ���°� �̹� ���Ƿ� �����Ǿ����ϴ�. ���� â�� ǥ������ �ʽ��ϴ�.");
                return;
            }

            // ���� �� ��� ���� ���� Ȯ��
            if (ConsentInformation.IsConsentFormAvailable())
            {
                // ���� �� �ε�
                ConsentForm.Load((ConsentForm consentForm, FormError loadError) =>
                {
                    if (loadError != null)
                    {
                        Debug.LogError($"���� �� �ε� ����: {loadError.Message}");
                        return;
                    }

                    // ���� ���� ���������� �ε�Ǿ����Ƿ� ǥ��
                    consentForm.Show((FormError showError) =>
                    {
                        if (showError != null)
                        {
                            Debug.LogError($"���� �� ǥ�� ����: {showError.Message}");
                        }
                        else
                        {
                            Debug.Log("���� ���� ���������� ǥ�õǾ����ϴ�.");
                        }
                    });
                });
            }
            else
            {
                Debug.Log("���� ���� ����� �� �����ϴ�.");
            }
        });

        //�ֵ�� �ʱ�ȭ
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

        //����Ƽ ���� ���� �ʱ�ȭ
        try
        {
            UnityServices.InitializeAsync();
            Debug.Log("Unity Gaming Services initialized successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Gaming Services: {e.Message}");
        }

        //������� ���
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

