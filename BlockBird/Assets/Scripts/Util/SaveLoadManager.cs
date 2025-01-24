using JetBrains.Annotations;
using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static string userDataPath = Path.Combine(Application.persistentDataPath, "1.json");
    public static string levelDataPath = Path.Combine(Application.persistentDataPath, "2.json");
    public static string noADPath = Path.Combine(Application.persistentDataPath, "3.json");
    public static string tutorialPath = Path.Combine(Application.persistentDataPath, "4.json");

    public static void SaveTutorialClearData()
    {
        SaveData(tutorialPath, "true");
    }

    public static void DeleteTutorialClearData()
    {
        SaveData(tutorialPath, "false");
    }

    public static string LoadTutorialClearData()
    {
        string tutorialClear = LoadData(tutorialPath);
        return tutorialClear;
    }


    public static void SaveNoADData()
    {
        SaveData(noADPath, "true");
        PersistentObject.Instance.IsNoAd = true;
    }
    public static void DeleteNoADData()
    {
        SaveData(noADPath, "false");
        PersistentObject.Instance.IsNoAd = true;
    }

    public static string LoadNoADData()
    {
        string noAd = LoadData(noADPath);
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

        // AES로 암호화
        string encryptedData = AESEncryption.Encrypt(jsonData);

        // 암호화된 데이터를 파일에 저장
        File.WriteAllText(userDataPath, encryptedData);
    }

    // JSON 파일에서 데이터를 읽어 복호화한 후 게임 데이터로 불러오기
    public static User LoadUserData()
    {
        if (File.Exists(userDataPath))
        {
            // 파일에서 암호화된 데이터 읽기
            string encryptedData = File.ReadAllText(userDataPath);

            // AES로 복호화
            string jsonData = AESEncryption.Decrypt(encryptedData);

            // JSON을 게임 데이터로 역직렬화
            User data = JsonUtility.FromJson<User>(jsonData);
            Debug.Log("Game data loaded and decrypted from " + userDataPath);
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found at " + userDataPath);

            User user = new User() { userId = System.Guid.NewGuid().ToString() };
            SaveUserData(user);
            return user;
        }
    }


    // 게임 데이터를 암호화하여 JSON 파일로 저장
    public void SaveGameData(GameData data)
    {
        // JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(data, true);

        // AES로 암호화
        string encryptedData = AESEncryption.Encrypt(jsonData);

        // 암호화된 데이터를 파일에 저장
        File.WriteAllText(userDataPath, encryptedData);
        Debug.Log("Game data saved and encrypted to " + userDataPath);
    }

    // JSON 파일에서 데이터를 읽어 복호화한 후 게임 데이터로 불러오기
    public GameData LoadGameData()
    {
        if (File.Exists(userDataPath))
        {
            // 파일에서 암호화된 데이터 읽기
            string encryptedData = File.ReadAllText(userDataPath);

            // AES로 복호화
            string jsonData = AESEncryption.Decrypt(encryptedData);

            // JSON을 게임 데이터로 역직렬화
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("Game data loaded and decrypted from " + userDataPath);
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found at " + userDataPath);
            return null;
        }
    }
}


[Serializable]
public class User
{
    public string userId;

    /// <summary>
    /// 서버->클라이언트 동기화
    /// 
    /// 1. 첫 실행 동기화
    /// 2. 충전, 사용할 때마다 동기화
    /// </summary>
    public int gems;

    /// <summary>
    /// 클라이언트->서버 동기화
    /// 
    /// 1. 첫 실행 동기화(lv1은 서버 동기화)
    /// 2. 레벨 이동할 때마다 동기화
    /// </summary>
    public string currentLevel;     //클라이언트->서버 동기화(lv1은 서버 동기화)

    /// <summary>
    /// 서버, 클라이언트 중 높은 레벨로 동기화
    /// 
    /// 1. 첫 실행 동기화
    /// 2. 레벨 이동할 때마다 동기화
    /// </summary>
    public string lastLevel;        //서버, 클라이언트 중 높은 레벨로 동기화

    /// <summary>
    /// 서버->클라이언트 동기화
    /// 
    /// 1. 첫 실행 동기화
    /// 2. 구입 할 때마다 동기화
    /// </summary>
    public GemHistory[] gemHistory; //서버->클라이언트 동기화


    /// <summary>
    /// 서버->클라이언트 동기화
    /// 
    ///  1. 첫 실행 동기화
    ///  2. 힌트 구입 할 떄마다 동기화
    /// </summary>
    public LevelHint[] hints;
}

[Serializable]
public class GemHistory
{
    public string historyId;
    public string userId;
    public string action;
    public int amount;
    public string date;
}

[Serializable]
public class LevelHint
{
    public string hintId;
    public string level;
    public string order;
    public bool isOpen;
}



[Serializable]
public class GameData
{
    public GameSettings settings;
    public LevelData[] levels;
}

[Serializable]
public class GameSettings
{
    public float musicVolume;
    public float sfxVolume;
    public bool isFullscreen;
}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public bool isUnlocked;
    public bool isCompleted;
}

