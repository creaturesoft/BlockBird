using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultPopup : MonoBehaviour
{
    public GameObject claimButton;
    public GameObject rewardedShareButton;
    public GameObject claim;

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

        if (Random.Range(0, PersistentObject.RewardedAdRate) < 40f)
        {
            claimButton.SetActive(true);
        }
        else if(Random.Range(0, 100) < 8)
        {
            rewardedShareButton.SetActive(true);
        }
        else
        {
            claim.SetActive(false);
        }

        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            claimButton.SetActive(false);
            rewardedShareButton.SetActive(false);
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
        claim.SetActive(false);
        PersistentObject.Instance.rewardedAdManager.ShowRewardedAd();

    }

    public void OnRewardedShare()
    {
        rewardedShareButton.SetActive(false);
        claim.SetActive(false);

        //°øÀ¯
        OnShare();

        int amount = UnityEngine.Random.Range(1, 6);
        PersistentObject.Instance.rewardedAdManager.GetRewardGem(amount);
        ToastNotification.Show("+" + amount);
    }



    public void OnRank()
    {
        

        //PageController.Instance.ShowWorldRank();
    }


    public void OnShare()
    {
        SunShineNativeShare.instance.ShareText(PersistentObject.Instance.GetStoreURL(), "Block Birds");

        //string text = "I got " + GameManager.Instance.Score + " points in the game! Can you beat me?";
        //string url = "https://play.google.com/store/apps/details?id=com.unity3d.korea";

        //ShareManager.Share(text, url);
    }

    public void OnScreenShotShare()
    {
        StartCoroutine(takeScreenshotAndSave());
    }

    private IEnumerator takeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();
        string img_name = "ScreenShot.png";
        string destination_path = Application.persistentDataPath + "/" + img_name; ;
        System.IO.File.WriteAllBytes(destination_path, imageBytes);

        //Call Share Function
        SunShineNativeShare.instance.ShareSingleFile(destination_path, SunShineNativeShare.TYPE_IMAGE, PersistentObject.Instance.GetStoreURL(), "Block Birds");
    }
}
