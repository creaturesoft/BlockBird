using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;


public class Login : MonoBehaviour
{

    // Start is called before the first frame update
    public string TryLogin()
    {
        //SignIn();

        return string.Empty;
    }

    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }

    // ���� �α���!
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("��");
            Debug.Log(PlayGamesPlatform.Instance.GetUserId());
        }
        else
        {

            Debug.Log("����");
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
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success to authenticate");
                }
                else
                {
                    Debug.Log("Faile to login");
                }
            });
        }
    }
}