using GooglePlayGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leaderboard : MonoBehaviour
{

    static string LeaderboardID = "CgkI-8WL140ZEAIQAQ";

    void Start()
    {
#if UNITY_ANDROID
        LeaderboardID = GPGSIds.leaderboard_top_flyers;
#else
        LeaderboardID = "top_flyers";
#endif

    }

    public void SubmitScore()
    {
        bool isAuthenticated;
        if (Application.platform == RuntimePlatform.Android)
        {
            isAuthenticated = PlayGamesPlatform.Instance.localUser.authenticated;
        }
        else
        {
            isAuthenticated = Social.localUser.authenticated;
        }

        if (isAuthenticated)
        {
            Social.ReportScore(GameManager.Instance.Score, LeaderboardID, success =>
            {
                ShowLeaderboard();
            });
        }
        else
        {
        }
    }

    IEnumerator LeaderboardCoroutine()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                ScreenOrientation orgOrientation = Screen.orientation;
                Screen.orientation = ScreenOrientation.Portrait;
                yield return null;

                PlayGamesPlatform.Instance.ShowLeaderboardUI(LeaderboardID);

                yield return new WaitForSeconds(1f);
                Screen.orientation = orgOrientation;
            }
            else
            {
                Debug.Log("Google Play Games");
            }
        }
        else
        {
            if (Social.localUser.authenticated)
            {
                ScreenOrientation orgOrientation = Screen.orientation;
                Screen.orientation = ScreenOrientation.Portrait;
                yield return null;

                Social.ShowLeaderboardUI();

                yield return new WaitForSeconds(1f);
                Screen.orientation = orgOrientation;
            }
            else
            {
                Debug.Log("Game Center");
            }
        }
    }

    public void ShowLeaderboard()
    {
        StartCoroutine(LeaderboardCoroutine());
    }
}
