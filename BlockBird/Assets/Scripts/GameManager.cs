using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Character { get; private set; }

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
        Character = FindObjectOfType<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
