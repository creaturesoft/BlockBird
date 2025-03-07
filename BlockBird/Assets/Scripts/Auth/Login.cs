using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif

public class Login : MonoBehaviour
{

    public void StartLogin()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PersistentObject.Instance.LoadGuestUserData();
            return;
        }

        //PlayGamesPlatform.DebugLogEnabled = true;
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate((status) => {
            if (status == SignInStatus.Success)
            {
                StartCoroutine(PersistentObject.Instance.LoadLoginUserData(PlayGamesPlatform.Instance.GetUserId()));
            }
            else
            {
                PersistentObject.Instance.LoadGuestUserData();
            }
        });
#elif UNITY_IOS
        GameCenterLogin();
#endif

    }

    /// <summary>
    /// Apple GameCenter Login
    /// </summary>
    public void GameCenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
            StartCoroutine(PersistentObject.Instance.LoadLoginUserData(Social.localUser.id));
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success to authenticate");
                    StartCoroutine(PersistentObject.Instance.LoadLoginUserData(Social.localUser.id));
                }
                else
                {
                    Debug.Log("Faile to login");
                    PersistentObject.Instance.LoadGuestUserData();
                }
            });
        }
    }
}