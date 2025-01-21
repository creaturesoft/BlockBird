using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Character { get; set; }

    public TextMeshProUGUI scoreText;
    public int Score { get; set; }

    public bool IsGameOver { get; set; } = false;
    public bool IsResultProcess { get; set; } = false;


    public GameObject bulletGameObject;
    public GameObject[] resultHideList;
    public GameObject resultPopup;

    public float ResultDelay { get; set; } = 2f;

    public static bool IsPaused { get; set; } = true;
    public static bool IsRestart { get; set; } = false;


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

        IsPaused = true;
    }

    void Start()
    {
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

    public void CheckGameOver()
    {
        IsGameOver = true;
        StartCoroutine(DoResultProcess());
    }

    IEnumerator DoResultProcess()
    {
        yield return new WaitForSeconds(ResultDelay);

        IsResultProcess = true;

        foreach (GameObject obj in resultHideList)
        {
            obj.SetActive(false);
        }

        //Time.timeScale = 0;

        yield return new WaitForSeconds(2f);
        resultPopup.SetActive(true);
    }
}
