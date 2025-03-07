using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

using BlockBirds.Platform.Interface;

#if UNITY_ANDROID
    using BlockBirds.Platform.Android;
#else
    using BlockBirds.Platform.iOS;
#endif

public class Leaderboard : MonoBehaviour
{
    ILeaderboard leaderBoard;

    void Start()
    {
#if UNITY_ANDROID
        leaderBoard = new AndroidLeaderboard(GPGSIds.leaderboard_top_flyers);
#else
        leaderBoard = new IOSLeaderboard("top_flyers");
#endif

    }

    public void SubmitScore()
    {
        leaderBoard.SubmitScore(GameManager.Instance.Score, ShowLeaderboard);
    }

    public void ShowLeaderboard()
    {
        StartCoroutine(leaderBoard.ShowLeaderboard());
    }
}
