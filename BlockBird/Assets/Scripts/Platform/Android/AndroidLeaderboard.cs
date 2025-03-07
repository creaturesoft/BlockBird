using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using BlockBirds.Platform.Interface;
using GooglePlayGames;
using System;

namespace BlockBirds.Platform.Android
{
    public class AndroidLeaderboard : ILeaderboard
    {

        static string LeaderboardID;

        public AndroidLeaderboard(string id)
        {
            LeaderboardID = id;
        }

        public void SubmitScore(int score, Action callback)
        {
            bool isAuthenticated = PlayGamesPlatform.Instance.localUser.authenticated;

            if (isAuthenticated)
            {
                Social.ReportScore(score, LeaderboardID, success =>
                {
                    callback();
                });
            }
        }

        public IEnumerator ShowLeaderboard()
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
        }

    }

}