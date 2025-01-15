using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Character { get; private set; }

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
    }

    void Start()
    {
        Character = FindObjectOfType<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
