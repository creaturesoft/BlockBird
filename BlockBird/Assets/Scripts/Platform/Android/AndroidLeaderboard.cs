using System;
using UnityEngine;
using System.Collections;
using System.Collections.Concurrent;
using BlockBirds.Platform.Interface;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

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

#if UNITY_ANDROID
            bool isAuthenticated = PlayGamesPlatform.Instance.localUser.authenticated;

            if (isAuthenticated)
            {
                Social.ReportScore(score, LeaderboardID, success =>
                {
                    callback();
                });
            }

#endif
        }

        public IEnumerator ShowLeaderboard()
        {
#if UNITY_ANDROID
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                ScreenOrientation orgOrientation = Screen.orientation;
                Screen.orientation = ScreenOrientation.Portrait;
                yield return null;

                PlayGamesPlatform.Instance.ShowLeaderboardUI(LeaderboardID);

                yield return new WaitForSeconds(1f);
                Screen.orientation = orgOrientation;
            }
#endif
            yield return null;
        }
    }

}