using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Character { get; private set; }

    public TextMeshProUGUI scoreText;
    public int Score { get; set; }

    public bool IsGameOver { get; set; } = false;
    public bool IsResultProcess { get; set; } = false;


    public GameObject bulletGameObject;
    public GameObject[] resultHideList;
    public GameObject resultPopup;

    public float ResultDelay { get; set; } = 2f;

    public static bool IsPaused { get; set; } = true;

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
    }

    void Start()
    {
        Character = GameObject.FindWithTag("Player").GetComponent<Character>();
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

        yield return new WaitForSeconds(2.5f);
        resultPopup.SetActive(true);
    }
}
