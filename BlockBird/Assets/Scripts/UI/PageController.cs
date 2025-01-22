using UnityEngine;

public class PageController : MonoBehaviour
{
    public static PageController Instance { get; private set; }

    public GameObject ui;
    public GameObject resultPopupPrefab;
    public GameObject noInternetPopupPrefab;
    public GameObject characterDrawPrefab;

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
        }
    }

    public void ShowSetting()
    {
    }
}
