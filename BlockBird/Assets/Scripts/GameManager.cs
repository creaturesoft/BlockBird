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
            UserDataChanged?.Invoke(); // ���� ����� ������ �̺�Ʈ ȣ��
        }
    }

    public delegate void OnUserDataChanged();
    public event OnUserDataChanged UserDataChanged;
    public void UpdateUserData()
    {
        UserDataChanged?.Invoke(); // ���� ����� ������ �̺�Ʈ ȣ��
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
                    //����
                    PersistentObject.Instance.ShowMessagePopup(0, () => { });
                }
                else
                {
                    //����
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
        // �̱��� �ν��Ͻ��� �̹� �����ϸ� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���� ��ü�� ����
        Instance = this;

        // �ٸ� ������ ��ȯ�ص� �ı����� �ʵ��� ����
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
            //�׽�Ʈ
            StartCoroutine(LoadLoginUserData("test3"));
            //LoadGuestUserData();
        }

        //�α׾ƿ��̸�
        //userData.isGuest = true;
        //SaveLoadManager.SaveUserData(UserData);
    }

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
        else if(userData.lastUserId == userId)
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
