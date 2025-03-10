using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Runtime.InteropServices;
using Unity.Services.Core;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public IAPManager IAPManager;

    public bool IsNoAd { get; set; }
    public static float RewardedAdRate = 100f;   //3;

    public InterstitialAdManager interstitialAdManager;
    public RewardedAdManager rewardedAdManager;

    public AudioSource[] backgroundAudioSources;
    private int currentSound;
    private int nextSound;


    public GameObject spinningLoadingPrefab;
    private GameObject spinningLoading;

    public MessagePopup messagePopupPrefab;
    public WaitMessagePopup firstLoginMessagePopupPrefab;

    public SettingData setting;

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            // �� ��ȯ �� �� ������Ʈ�� �ı����� �ʵ��� ����

            DontDestroyOnLoad(gameObject);


            setting = PlayerPrefsManager.LoadSetting();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void OnDestroy()
    {
        // ���� ����� �� �̺�Ʈ �ߺ� ��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
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




        SaveLoadManager.LoadNoADData();

        //�ֵ�� �ʱ�ȭ
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("[AdMob initialized]");

            if (PersistentObject.Instance.IsNoAd == true)
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


        //�α���
        GetComponent<Login>().StartLogin();

        //�۾�����Ʈ üũ
#if UNITY_ANDROID
        InAppUpdateManager inAppUpdateManager = GetComponent<InAppUpdateManager>();
        inAppUpdateManager.CheckAppUpdate();
#elif UNITY_IOS
        iOSUpdateChecker iOSUpdateChecker = GetComponent<iOSUpdateChecker>();
        iOSUpdateChecker.CheckAppUpdate();
#endif
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundManager.Instance.SetBGMVolume(setting.BGMVolume);
        SoundManager.Instance.SetSFXVolume(setting.SFXVolume);

        if (SoundManager.Instance.isBGMOn)
        {
            StartCoroutine(PlayBackgroundSound());
        }
    }


    public IEnumerator PlayBackgroundSound()
    {
        int nextPlaySound = UnityEngine.Random.Range(0, backgroundAudioSources.Length);
        backgroundAudioSources[nextPlaySound].Pause();  

        foreach (AudioSource audioSource in backgroundAudioSources)
        {
            if (audioSource.isPlaying)
            {
                for (int i = 0; i < 5; i++)
                {
                    audioSource.volume -= 0.1f;
                    yield return new WaitForSeconds(0.6f);
                }

            }
            audioSource.Stop();
        }

        backgroundAudioSources[nextPlaySound].volume = 0.1f;
        backgroundAudioSources[nextPlaySound].Play();
    }


    public void StartSpinningLoading()
    {
        if (spinningLoading == null)
        {
            spinningLoading = Instantiate(spinningLoadingPrefab);
        }
    }

    public void StopSpinningLoading()
    {
        if (spinningLoading != null)
        {
            Destroy(spinningLoading);
        }
    }


    public void ShowMessagePopup(int index, Action okCallback = null, Action closeCallback = null)
    {
        MessagePopup messagePopup = Instantiate(messagePopupPrefab);
        messagePopup.ShowMessage(index, okCallback, closeCallback);
    }

    public IEnumerator FirstLoginMessagePopup(Action action1, Action action2)
    {
        WaitMessagePopup messagePopup = Instantiate(firstLoginMessagePopupPrefab);
        yield return messagePopup.ShowPopupAndWait(action1, action2);
    }




    private User userData;
    public User UserData
    {
        get => userData;
        set
        {
            userData = value;
            UserDataChanged?.Invoke(); // ���� ����� ������ �̺�Ʈ ȣ��
        }
    }

    public delegate void OnUserDataChanged();
    public event OnUserDataChanged UserDataChanged;
    public void UpdateUserData()
    {
        UserDataChanged?.Invoke(); // ���� ����� ������ �̺�Ʈ ȣ��
    }



    public bool IsLogin { get; set; } = false;

    public IEnumerator LoadLoginUserData(string userId)
    {
        userData = SaveLoadManager.LoadUserData();

        userData.isGuest = false;

        bool saveToServer = false;
        User serverUser = null;

        //ó�� �α���
        if (string.IsNullOrEmpty(userData.lastUserId))
        {
            yield return SaveLoadManager.LoadUserDataFromServer(userId, (result) => {
                serverUser = result;
            });

            if (serverUser == null)
            {
                //������ ���� ������ ����
                //�Խ�Ʈ ������ ���
                saveToServer = true;
            }
            else
            {
                //������ ���� ������ ����(�� �缳ġ ��)

                //�Խ�Ʈ�� ������� �÷��� ������ �Խ�Ʈ ������ ���, ���� ���� ������ ��� ���� �˾�
                if (userData.stage < 5)
                {
                    //���� ���� ������ ���
                    UserData = serverUser;
                }
                else
                {
                    yield return PersistentObject.Instance.FirstLoginMessagePopup(() => {
                        //���� ���� ������ ���
                        UserData = serverUser;


                    }, () => {
                        //�Խ�Ʈ ������ ���
                        saveToServer = true;

                        //�͸� ���� ����
                    });

                }
            }

            userData.userId = userId;
            userData.lastUserId = userId;
        }
        //������ �α��� ���̵�� ���� �α��� ���̵� ������
        else if (userData.lastUserId == userId)
        {
            //�������� ���� ������ ������
            yield return SaveLoadManager.LoadUserDataFromServer(userId, (result) => {
                serverUser = result;
            });
            if (serverUser != null)
            {
                UserData = serverUser;
            }

            userData.userId = userId;
            userData.lastUserId = userId;
        }
        //������ �ٸ� ���ο� ���̵�� �α���
        else
        {
            //���� ���̵�� ��� ����Ǿ����Ƿ� ���� ���̵�� �α����ϸ� ��� ������
            //�׷��Ƿ� ���� �α��� ���̵�� �������� �������ų� ������ ���� �ִ� �����Ͱ� �ƴ� �ʱ�ȭ �� ���ο� ������ ���

            yield return SaveLoadManager.LoadUserDataFromServer(userId, (result) => {
                serverUser = result;
            });
            if (serverUser != null)
            {
                //������ ����
                UserData = serverUser;
            }
            else
            {
                //ó�� �÷����ϴ� ����
                //�ʱ�ȭ
                userData = SaveLoadManager.GetDefaultData();
                userData.userId = userId;
                userData.lastUserId = userId;
                userData.isGuest = false;
                saveToServer = true;
            }
        }

        SaveLoadManager.SaveUserData(UserData);
        if (saveToServer)
        {
            StartCoroutine(SaveLoadManager.SendUserDataToServer(PersistentObject.Instance.UserData));
        }

        IsLogin = true;
    }

    public void LoadGuestUserData()
    {
        userData = SaveLoadManager.LoadUserData();
        IsLogin = true;
    }

    private bool isGemLoaded { get; set; } = false;
    public void LoadGuestGem()
    {
        if (UserData.isGuest && !isGemLoaded)
        {
            string data = "{\"userId\": \"" + PersistentObject.Instance.UserData.userId + "\", " +
                           "\"type\": \"CHECK\" }";

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
                    isGemLoaded = true;
                    PersistentObject.Instance.UserData.gem = (int)result.GetValue("amount");
                    SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
                    UpdateUserData();
                }
            }));
        }
    }


    public string GetStoreURL()
    {
#if UNITY_ANDROID
        return "https://play.google.com/store/apps/details?id=com.Creaturesoft.BlockBird";
#elif UNITY_IOS
        return "https://apps.apple.com/app/id6741071606";
#else
        return "";
#endif

    }
}


