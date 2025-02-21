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

    // �͸� �α��� �޼���
    public void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�͸� �α��� ��ҵ�");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�͸� �α��� ����: " + task.Exception);
                return;
            }

            AuthResult newUser = task.Result;
            Debug.LogFormat("�͸� �α��� ����! User ID: {0}", newUser.User.UserId);
        });
    }

    // Firebase �α׾ƿ�
    public void SignOut()
    {
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("Firebase �α׾ƿ� �Ϸ�");
        }
    }

    //���� ���� ���� Ȯ��
    public void CheckLinkedAccounts()
    {
        if (auth.CurrentUser != null)
        {
            foreach (var info in auth.CurrentUser.ProviderData)
            {
                Debug.Log("����� ����: " + info.ProviderId);  // google.com, apple.com ��
            }
        }
    }


}
