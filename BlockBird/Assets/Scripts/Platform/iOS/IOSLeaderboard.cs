using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using BlockBirds.Platform.Interface;
using System;

namespace BlockBirds.Platform.iOS
{

    public class IOSLeaderboard : ILeaderboard
    {
        static string LeaderboardID;

        public IOSLeaderboard(string id)
        {
            LeaderboardID = id;
        }

        public void SubmitScore(int score, Action callback)
        {
            bool isAuthenticated = Social.localUser.authenticated;

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
            if (Social.localUser.authenticated)
            {
                ScreenOrientation orgOrientation = Screen.orientation;
                Screen.orientation = ScreenOrientation.Portrait;
                yield return null;

                Social.ShowLeaderboardUI();

                yield return new WaitForSeconds(1f);
                Screen.orientation = orgOrientation;
            }
        }
    }

}
