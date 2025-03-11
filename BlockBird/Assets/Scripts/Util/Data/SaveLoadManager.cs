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
        // AES�� ��ȣȭ
        string encryptedData = AESEncryption.Encrypt(data);

        // ��ȣȭ�� �����͸� ���Ͽ� ����
        File.WriteAllText(path, encryptedData);
    }

    public static string LoadData(string path)
    {
        try
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(path);

            // AES�� ��ȣȭ
            return AESEncryption.Decrypt(encryptedData);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("LoadData exception : " + ex.Message);
            return null;
        }
    }


    // ���� �����͸� ��ȣȭ�Ͽ� JSON ���Ϸ� ����
    public static void SaveUserData(User user)
    {
        // JSON���� ����ȭ
        string jsonData = JsonUtility.ToJson(user, true);

        SaveData(PlayerPrefsManager.GetUserDataPath(), jsonData);

        //������ ������Ʈ
        PersistentObject.Instance.UpdateUserData();

    }

    // JSON ���Ͽ��� �����͸� �о� ��ȣȭ�� �� ���� �����ͷ� �ҷ�����
    public static User LoadUserData()
    {
            // AES�� ��ȣȭ
            string jsonData = LoadData(PlayerPrefsManager.GetUserDataPath());
            if(jsonData == null)
            {
                return GetDefaultData();
            }


            // JSON�� ���� �����ͷ� ������ȭ
            User data = JsonUtility.FromJson<User>(jsonData);
            //Debug.Log("Game data loaded and decrypted from " + userDataPath);

            //foreach(BirdData birdData in data.birdList)
            //{
            //        birdData.pullLevel = 20;
            //}

            return data;
    }

    public static User GetDefaultData()
    {
        //������ ���� ���
        List<BirdData> birdList = new List<BirdData>();
        birdList.Add(new BirdData() { name = "BirdyGun", expLevel = 1 });
        birdList.Add(new BirdData() { name = "Eagle", expLevel = 1 });

        string userId = System.Guid.NewGuid().ToString();

        User user = new User()
        {
            userId = userId,
            stage = 1,
            maxStage = 1,
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
                    //����
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
                "\"maxStage\": " + user.maxStage + ", " +
                "\"isReviewed\": " + user.isReviewed.ToString().ToLower() + ", " +
                 "\"highScore\": " + PlayerPrefsManager.LoadScore().Last() + ", " +
                 "\"birdCount\": " + user.birdList.Count() + ", " +
                 "\"currentLifeWeight\": " + user.currentLifeWeight + ", " +
                 "\"currentBossMapIndex\": " + user.currentBossMapIndex + ", " +
                "\"birdList\": " + JsonConvert.SerializeObject(user.birdList, Formatting.None) + "}";


        yield return WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            if (result == null)
            {
                //����
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
    public int maxStage;
    public bool isGuest;
    public bool isReviewed;
    public int gem;
    public int currentLifeWeight;
    public int currentBossMapIndex;
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
