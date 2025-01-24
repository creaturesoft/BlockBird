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
        // AES�� ��ȣȭ
        string encryptedData = AESEncryption.Encrypt(data);

        // ��ȣȭ�� �����͸� ���Ͽ� ����
        File.WriteAllText(path, encryptedData);
    }

    public static string LoadData(string path)
    {
        if (File.Exists(path))
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(path);

            // AES�� ��ȣȭ
            return AESEncryption.Decrypt(encryptedData);
        }
        else
        {
            Debug.LogWarning("No save file found at " + path);
            return null;
        }
    }


    // ���� �����͸� ��ȣȭ�Ͽ� JSON ���Ϸ� ����
    public static void SaveUserData(User user)
    {
        // JSON���� ����ȭ
        string jsonData = JsonUtility.ToJson(user, true);

        // AES�� ��ȣȭ
        string encryptedData = AESEncryption.Encrypt(jsonData);

        // ��ȣȭ�� �����͸� ���Ͽ� ����
        File.WriteAllText(userDataPath, encryptedData);
    }

    // JSON ���Ͽ��� �����͸� �о� ��ȣȭ�� �� ���� �����ͷ� �ҷ�����
    public static User LoadUserData()
    {
        if (File.Exists(userDataPath))
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(userDataPath);

            // AES�� ��ȣȭ
            string jsonData = AESEncryption.Decrypt(encryptedData);

            // JSON�� ���� �����ͷ� ������ȭ
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


    // ���� �����͸� ��ȣȭ�Ͽ� JSON ���Ϸ� ����
    public void SaveGameData(GameData data)
    {
        // JSON���� ����ȭ
        string jsonData = JsonUtility.ToJson(data, true);

        // AES�� ��ȣȭ
        string encryptedData = AESEncryption.Encrypt(jsonData);

        // ��ȣȭ�� �����͸� ���Ͽ� ����
        File.WriteAllText(userDataPath, encryptedData);
        Debug.Log("Game data saved and encrypted to " + userDataPath);
    }

    // JSON ���Ͽ��� �����͸� �о� ��ȣȭ�� �� ���� �����ͷ� �ҷ�����
    public GameData LoadGameData()
    {
        if (File.Exists(userDataPath))
        {
            // ���Ͽ��� ��ȣȭ�� ������ �б�
            string encryptedData = File.ReadAllText(userDataPath);

            // AES�� ��ȣȭ
            string jsonData = AESEncryption.Decrypt(encryptedData);

            // JSON�� ���� �����ͷ� ������ȭ
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
    /// ����->Ŭ���̾�Ʈ ����ȭ
    /// 
    /// 1. ù ���� ����ȭ
    /// 2. ����, ����� ������ ����ȭ
    /// </summary>
    public int gems;

    /// <summary>
    /// Ŭ���̾�Ʈ->���� ����ȭ
    /// 
    /// 1. ù ���� ����ȭ(lv1�� ���� ����ȭ)
    /// 2. ���� �̵��� ������ ����ȭ
    /// </summary>
    public string currentLevel;     //Ŭ���̾�Ʈ->���� ����ȭ(lv1�� ���� ����ȭ)

    /// <summary>
    /// ����, Ŭ���̾�Ʈ �� ���� ������ ����ȭ
    /// 
    /// 1. ù ���� ����ȭ
    /// 2. ���� �̵��� ������ ����ȭ
    /// </summary>
    public string lastLevel;        //����, Ŭ���̾�Ʈ �� ���� ������ ����ȭ

    /// <summary>
    /// ����->Ŭ���̾�Ʈ ����ȭ
    /// 
    /// 1. ù ���� ����ȭ
    /// 2. ���� �� ������ ����ȭ
    /// </summary>
    public GemHistory[] gemHistory; //����->Ŭ���̾�Ʈ ����ȭ


    /// <summary>
    /// ����->Ŭ���̾�Ʈ ����ȭ
    /// 
    ///  1. ù ���� ����ȭ
    ///  2. ��Ʈ ���� �� ������ ����ȭ
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

