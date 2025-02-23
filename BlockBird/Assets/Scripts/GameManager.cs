using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Character { get; set; }

    public TextMeshProUGUI scoreText;
    public int Score { get; set; }

    public int GoalScore { get; set; }

    public bool IsGameOver { get; set; } = false;
    public bool IsResultProcess { get; set; } = false;


    public GameObject bulletGameObject;
    public GameObject friendBulletGameObject;
    public GameObject[] resultHideList;

    
    public float ResultDelay { get; set; } = 2f;

    public static bool IsRestart { get; set; } = false;

    public PageController pageController;

    public GameObject[] itemPrefabList;

    public int stage = 1;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;

    public CharacterSelect characterSelect;


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

    private bool isGemLoaded { get; set; } = false;
    public void LoadGuestGem()
    {
        if(UserData.isGuest && !isGemLoaded)
        {
            string data = "{\"userId\": \"" + GameManager.Instance.UserData.userId + "\", " +
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
                    GameManager.Instance.UserData.gem = (int)result.GetValue("amount");
                    SaveLoadManager.SaveUserData(GameManager.Instance.UserData);
                    UpdateUserData();
                }
            }));
        }
    }

    public GameObject[] clearEmojiList;

    public GameObject SpeedUpItem;
    public GameObject FriendsItem;
    public GameObject LevelUpItem;


    void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스를 현재 객체로 설정
        Instance = this;

        // 다른 씬으로 전환해도 파괴되지 않도록 설정
        //DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        GetComponent<FPSDisplay>().enabled = true;
#endif

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            LoadGuestUserData();
        }
        else
        {
            //테스트
            StartCoroutine(LoadLoginUserData("test3"));
            //LoadGuestUserData();
        }

        //로그아웃이면
        //userData.isGuest = true;
        //SaveLoadManager.SaveUserData(UserData);
    }

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
        else if(userData.lastUserId == userId)
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
       
        stage = userData.stage;
        stageText.text = "Stage " + stage;

        SaveLoadManager.SaveUserData(UserData);
        if (saveToServer)
        {
            StartCoroutine(SaveLoadManager.SendUserDataToServer(GameManager.Instance.UserData));
        }


        characterSelect.Init();
    }

    public void LoadGuestUserData()
    {
        userData = SaveLoadManager.LoadUserData();
        stage = userData.stage;
        stageText.text = "Stage " + stage;

        characterSelect.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int score)
    {
        Score += score;
        scoreText.text = Score.ToString();
    }

    public void CheckGameOver(bool isClear)
    {
        IsGameOver = true;

        StartCoroutine(DoResultProcess(isClear));
    }

    IEnumerator DoResultProcess(bool isClear)
    {
        yield return new WaitForSeconds(ResultDelay);

        IsResultProcess = true;

        foreach (GameObject obj in resultHideList)
        {
            obj.SetActive(false);
        }

        //Time.timeScale = 0;

        yield return new WaitForSeconds(2f);

        if (isClear)
        {
            Character.Clear();
        }

        pageController.ShowResultPopup(isClear);
    }
}
