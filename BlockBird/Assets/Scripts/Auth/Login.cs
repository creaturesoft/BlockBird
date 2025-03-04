using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;


public class Login : MonoBehaviour
{

    public void StartLogin()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PersistentObject.Instance.LoadGuestUserData();
            return;
        }


        PlayGamesPlatform.DebugLogEnabled = true;
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#elif UNITY_IOS
        GameCenterLogin()
#endif

    }

    // 구글 로그인!
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("굳");
            Debug.Log(PlayGamesPlatform.Instance.GetUserId());


            StartCoroutine(PersistentObject.Instance.LoadLoginUserData(PlayGamesPlatform.Instance.GetUserId()));
        }
        else
        {

            Debug.Log("실패");

            PersistentObject.Instance.LoadGuestUserData();
        }
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