using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    public GameObject claimButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rankText;

    public GameObject restartImage;
    public GameObject nextStageImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Init(bool isNextStage)
    {

        scoreText.text = GameManager.Instance.Score.ToString();

        if (Random.Range(0, PersistentObject.RewardedAdRate) != 0)
        {
            claimButton.SetActive(false);
        }

        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            claimButton.SetActive(false);
        }

        rankText.text = PlayerPrefsManager.InsertScoreAndGetRank(GameManager.Instance.Score).ToString();

        if (isNextStage)
        {
            nextStageImage.SetActive(true);
            restartImage.SetActive(false);
        }
        else
        {
            nextStageImage.SetActive(false);
            restartImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnRestart()
    {
        GameManager.IsRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClaim()
    {
        claimButton.SetActive(false);
        PersistentObject.Instance.rewardedAdManager.ShowRewardedAd();
    }

    public void OnWorldRank()
    {
        PageController.Instance.ShowWorldRank();
    }
}
