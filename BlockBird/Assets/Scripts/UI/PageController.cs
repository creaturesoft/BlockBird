using UnityEngine;

public class PageController : MonoBehaviour
{
    public static PageController Instance { get; private set; }

    public GameObject ui;
    public GameObject resultPopupPrefab;
    public GameObject noInternetPopupPrefab;
    public GameObject characterDrawPrefab;
    public GameObject shopPrefab;
    public GameObject worldRankPrefab;

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
    }

    public bool CheckInternetConnection()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowNoInternetPopupPopup();
            return false;
        }

        return true;
    }

    void ShowNoInternetPopupPopup()
    {
        Instantiate(noInternetPopupPrefab, ui.transform);
    }
    

    public void ShowResultPopup()
    {
        Instantiate(resultPopupPrefab, ui.transform);
    }

    public void ShowCharacterDraw()
    {
        if (CheckInternetConnection())
        {
            Time.timeScale = 0;
            Instantiate(characterDrawPrefab, ui.transform);
        }
    }


    public void ShowShop()
    {
        if (CheckInternetConnection())
        {
            Time.timeScale = 0;
            Instantiate(shopPrefab, ui.transform);
        }
    }

    public void ShowSetting()
    {
    }

    public void ShowWorldRank()
    {
        if (CheckInternetConnection())
        {
            Instantiate(worldRankPrefab, ui.transform);
        }
    }
}
