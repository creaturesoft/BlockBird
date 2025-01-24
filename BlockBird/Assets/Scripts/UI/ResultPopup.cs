using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    public GameObject claimButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rankText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = GameManager.Instance.Score.ToString();

        if (Random.Range(0,PersistentObject.RewardedAdRate) != 0) {
            claimButton.SetActive(false);
        }

        rankText.text = PlayerPrefsManager.InsertScoreAndGetRank(GameManager.Instance.Score).ToString();
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
        PersistentObject.Instance.rewardedAdManager.ShowRewardedAd();
    }

    public void OnWorldRank()
    {
        PageController.Instance.ShowWorldRank();
    }
}
