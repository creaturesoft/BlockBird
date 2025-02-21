using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;


    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    // 익명 로그인 메서드
    public void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("익명 로그인 취소됨");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("익명 로그인 오류: " + task.Exception);
                return;
            }

            AuthResult newUser = task.Result;
            Debug.LogFormat("익명 로그인 성공! User ID: {0}", newUser.User.UserId);
        });
    }

    // Firebase 로그아웃
    public void SignOut()
    {
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("Firebase 로그아웃 완료");
        }
    }

    //계정 연결 여부 확인
    public void CheckLinkedAccounts()
    {
        if (auth.CurrentUser != null)
        {
            foreach (var info in auth.CurrentUser.ProviderData)
            {
                Debug.Log("연결된 계정: " + info.ProviderId);  // google.com, apple.com 등
            }
        }
    }


}
