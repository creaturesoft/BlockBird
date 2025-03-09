using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
    public List<Rigidbody2D> friendRbList;
    public GameObject[] resultHideList;

    
    public float ResultDelay { get; set; } = 2f;

    public static bool IsRestart { get; set; } = false;

    public PageController pageController;

    public GameObject[] itemPrefabList;

    private int stage { get; set; } = 1;

    public int Stage
    {
        get
        {
            return stage;
        }
        set
        {
            stage = value;
            stageText.text = "Stage " + stage;
        }
    }
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;

    public CharacterSelect characterSelect;


    public GameObject[] clearEmojiList;

    public GameObject SpeedUpItem;
    public GameObject FriendsItem;
    public GameObject LevelUpItem;

    public GameObject map;
    public int blockLayer { get; set; }

    public static float attackSoundLastPlayTime = 0f;
    public static float attackSoundPlayCooldown = 0.12f; // 0.1초 쿨타임 (필요에 따라 조절)

    public static float effectSoundLastPlayTime = 0f;
    public static float effectSoundPlayCooldown = 0.12f; // 0.1초 쿨타임 (필요에 따라 조절)

    public static float blockSoundLastPlayTime = 0f;
    public static float blockSoundPlayCooldown = 0.12f; // 0.1초 쿨타임 (필요에 따라 조절)

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

//#if UNITY_EDITOR
//        GetComponent<FPSDisplay>().enabled = true;
//#endif

        StartCoroutine(Init());

        blockLayer = LayerMask.NameToLayer("Block");

        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    LoadGuestUserData();
        //}
        //else
        //{
        //    //테스트
        //    StartCoroutine(LoadLoginUserData("test3"));
        //    //LoadGuestUserData();
        //}

        //로그아웃이면
        //userData.isGuest = true;
        //SaveLoadManager.SaveUserData(UserData);
    }

    IEnumerator Init()
    {
        while (!PersistentObject.Instance.IsLogin)
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }

        Stage = PersistentObject.Instance.UserData.stage;
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


        if (isClear)
        {
            //성공 소리재생
            SoundManager.Instance.PlaySuccessAudio();
        }

        yield return new WaitForSeconds(2f);

        if (isClear)
        {
            Character.Clear();
        }

        pageController.ShowResultPopup(isClear);
    }


}
