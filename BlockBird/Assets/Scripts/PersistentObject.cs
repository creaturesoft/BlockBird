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
            // 씬 전환 시 이 오브젝트가 파괴되지 않도록 설정

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
        // 씬이 변경될 때 이벤트 중복 등록 방지
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




        SaveLoadManager.LoadNoADData();

        //애드몹 초기화
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


        //로그인
        GetComponent<Login>().StartLogin();

        //앱업데이트 체크
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
            UserDataChanged?.Invoke(); // 값이 변경될 때마다 이벤트 호출
        }
    }

    public delegate void OnUserDataChanged();
    public event OnUserDataChanged UserDataChanged;
    public void UpdateUserData()
    {
        UserDataChanged?.Invoke(); // 값이 변경될 때마다 이벤트 호출
    }



    public bool IsLogin { get; set; } = false;

    public IEnumerator LoadLoginUserData(string userId)
    {
        userData = SaveLoadManager.LoadUserData();

        userData.isGuest = false;

        bool saveToServer = false;
        User serverUser = null;

        //처음 로그인
        if (string.IsNullOrEmpty(userData.lastUserId))
        {
            yield return SaveLoadManager.LoadUserDataFromServer(userId, (result) => {
                serverUser = result;
            });

            if (serverUser == null)
            {
                //서버에 유저 데이터 없음
                //게스트 데이터 사용
                saveToServer = true;
            }
            else
            {
                //서버에 유저 데이터 있음(앱 재설치 등)

                //게스트로 어느정도 플레이 했으면 게스트 데이터 사용, 기존 유저 데이터 사용 선택 팝업
                if (userData.stage < 5)
                {
                    //서버 서버 데이터 사용
                    UserData = serverUser;
                }
                else
                {
                    yield return PersistentObject.Instance.FirstLoginMessagePopup(() => {
                        //서버 유저 데이터 사용
                        UserData = serverUser;


                    }, () => {
                        //게스트 데이터 사용
                        saveToServer = true;

                        //익명 계정 연결
                    });

                }
            }

            userData.userId = userId;
            userData.lastUserId = userId;
        }
        //마지막 로그인 아이디와 현재 로그인 아이디가 동일함
        else if (userData.lastUserId == userId)
        {
            //서버에서 유저 데이터 가져옴
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
        //이전과 다른 새로운 아이디로 로그인
        else
        {
            //이전 아이디는 모두 저장되었으므로 이전 아이디로 로그인하면 모두 복구됨
            //그러므로 지금 로그인 아이디는 서버에서 가져오거나 없으면 원래 있던 데이터가 아닌 초기화 후 새로운 데이터 사용

            yield return SaveLoadManager.LoadUserDataFromServer(userId, (result) => {
                serverUser = result;
            });
            if (serverUser != null)
            {
                //데이터 있음
                UserData = serverUser;
            }
            else
            {
                //처음 플레이하는 계정
                //초기화
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
                    //실패
                    PersistentObject.Instance.ShowMessagePopup(0, () => { });
                }
                else
                {
                    //성공
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


