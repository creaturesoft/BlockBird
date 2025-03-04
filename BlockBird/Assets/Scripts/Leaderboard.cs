using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leaderboard : MonoBehaviour
{

#if UNITY_ANDROID
    static readonly string LeaderboardID = "CgkI-8WL140ZEAIQAQ";
#else
    static readonly string LeaderboardID = "CgkI-8WL140ZEAIQAQ";
#endif


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
            Social.ReportScore(PersistentObject.Instance.UserData.stage, LeaderboardID, success =>
            {
                Debug.Log(success ? "점수 업로드 성공" : "점수 업로드 실패");
                ShowLeaderboard();
            });
        }
        else
        {
            Debug.Log("로그인 필요");
        }
    }
    public void ShowLeaderboard()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(LeaderboardID);
            }
            else
            {
                Debug.Log("Google Play Games 로그인 필요");
            }
        }
        else
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
            else
            {
                Debug.Log("Game Center 로그인 필요");
            }
        }
    }
}
