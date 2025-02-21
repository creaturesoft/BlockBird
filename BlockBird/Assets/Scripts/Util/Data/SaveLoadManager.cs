using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    public static void SaveNoADData()
    {
        SaveData(PlayerPrefsManager.GetNoADPath(), "true");
        PersistentObject.Instance.IsNoAd = true;
    }
    public static void DeleteNoADData()
    {
        SaveData(PlayerPrefsManager.GetNoADPath(), "false");
        PersistentObject.Instance.IsNoAd = true;
    }

    public static string LoadNoADData()
    {
        string noAd = LoadData(PlayerPrefsManager.GetNoADPath());
        PersistentObject.Instance.IsNoAd = noAd == "true";

        return noAd;
    }

    public static void SaveData(string path, string data)
    {
        // AES로 암호화
        string encryptedData = AESEncryption.Encrypt(data);

        // 암호화된 데이터를 파일에 저장
        File.WriteAllText(path, encryptedData);
    }

    public static string LoadData(string path)
    {
        if (File.Exists(path))
        {
            // 파일에서 암호화된 데이터 읽기
            string encryptedData = File.ReadAllText(path);

            // AES로 복호화
            return AESEncryption.Decrypt(encryptedData);
        }
        else
        {
            Debug.LogWarning("No save file found at " + path);
            return null;
        }
    }


    // 게임 데이터를 암호화하여 JSON 파일로 저장
    public static void SaveUserData(User user)
    {
        // JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(user, true);

        SaveData(PlayerPrefsManager.GetUserDataPath(), jsonData);

        //데이터 업데이트
        GameManager.Instance.UpdateUserData();

    }

    // JSON 파일에서 데이터를 읽어 복호화한 후 게임 데이터로 불러오기
    public static User LoadUserData()
    {
        if (File.Exists(PlayerPrefsManager.GetUserDataPath()))
        {
            // AES로 복호화
            string jsonData = LoadData(PlayerPrefsManager.GetUserDataPath());

            // JSON을 게임 데이터로 역직렬화
            User data = JsonUtility.FromJson<User>(jsonData);
            //Debug.Log("Game data loaded and decrypted from " + userDataPath);

            return data;
        }

        User user = GetDefaultData();
        SaveUserData(user);
        return user;
        
    }

    public static User GetDefaultData()
    {
        //데이터 없을 경우
        List<BirdData> birdList = new List<BirdData>();
        birdList.Add(new BirdData() { name = "BirdyGun", expLevel = 1 });

        string userId = System.Guid.NewGuid().ToString();

        User user = new User()
        {
            userId = userId,
            stage = 1,
            gem = 0,
            isGuest = true,
            birdList = birdList
        };

        return user;
    }

    public static IEnumerator LoadUserDataFromServer(string userId, Action<User> callback)
    {
        string data = "{\"userId\": \"" + userId + "\", " +
        "\"type\": \"GET_USER\"}";

        User serverUser = null;

        yield return WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            try
            {
                if (result == null)
                {
                    //실패
                    PersistentObject.Instance.ShowMessagePopup(0, () => { });
                    serverUser = null;
                }
                else
                {


                    if (result["user"] == null)
                    {
                        serverUser = null;
                    }
                    else
                    {
                        serverUser = DynamoDBHelper.ConvertDynamoDBItem<User>(result["user"]);

                        //User user = new User();
                        //user.userId = (string)result["user"]["userId"]["S"];
                        //user.gem = (int)result["user"]["gem"]["N"];
                        //serverUser = user;
                    }
                }
            }catch (Exception e)
            {
                Debug.Log(e);
                serverUser = null;
            }

        });

        callback?.Invoke(serverUser);
    }

    public static IEnumerator SendUserDataToServer(User user, Action callback = null)
    {
        if (user.isGuest)
        {
            yield break;
        }

        string data = "{\"userId\": \"" + user.userId + "\", " +
                "\"type\": \"UPDATE_USER\", " +
                "\"stage\": " + user.stage + ", " +
                "\"isReviewed\": " + user.isReviewed.ToString().ToLower() + ", " +
                 "\"highScore\": " + PlayerPrefsManager.LoadScore().Last() + ", " +
                 "\"birdCount\": " + user.birdList.Count() + ", " +
                "\"birdList\": " + JsonConvert.SerializeObject(user.birdList, Formatting.None) + "}";


        yield return WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            if (result == null)
            {
                //실패
                PersistentObject.Instance.ShowMessagePopup(0, () => { });
            }

            if(callback != null)
            {
                callback();
            }
        });
    }


}


[Serializable]
public class User
{
    public string userId;
    public string lastUserId;
    public int stage;
    public bool isGuest;
    public bool isReviewed;
    public int gem;
    public List<BirdData> birdList;
}

[Serializable]
public class BirdData
{
    public string name;
    public int expLevel;
    public int pullLevel;
    public double exp;
}
