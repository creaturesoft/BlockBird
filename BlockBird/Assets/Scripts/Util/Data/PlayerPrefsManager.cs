using I2.Loc.SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public List<int> scoreList;
}


[System.Serializable]
public class SettingData
{
    public float SFXVolume;
    public float BGMVolume;
}

public class PlayerPrefsManager : MonoBehaviour
{
    public static int InsertScoreAndGetRank(int newScore)
    {
        List<int> scores = LoadScore();

        // �ߺ� ���� Ȯ��
        if (scores.Contains(newScore))
        {
            Debug.Log($"���� {newScore}�� �̹� ����Ʈ�� �����մϴ�.");
            return scores.Count - scores.IndexOf(newScore);
        }

        // ������ ������ �ùٸ� ��ġ ã�� (���� Ž��)
        int index = scores.BinarySearch(newScore);
        if (index < 0)
            index = ~index;

        // ������ ���ĵ� ��ġ�� ����
        scores.Insert(index, newScore);

        // ��� ��ȯ
        int rank = scores.Count - index;
        Debug.Log($"���� {newScore}�� ���ԵǾ����ϴ�. ���� ���: {rank}");

        SaveScore(scores);
        return rank;
    }


    // ����Ʈ ����
    static private void SaveScore(List<int> list)
    {
        ScoreData data = new ScoreData { scoreList = list };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("ScoreData", json);
        PlayerPrefs.Save();
    }

    // ����Ʈ �ҷ�����
    static public List<int> LoadScore()
    {
        if (PlayerPrefs.HasKey("ScoreData"))
        {
            string json = PlayerPrefs.GetString("ScoreData");
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            return data.scoreList;
        }

        return new List<int>(); // �⺻�� ��ȯ
    }


    // ����Ʈ �ҷ�����
    static public string GetUserDataPath()
    {
        if (PlayerPrefs.HasKey("UserDataPath"))
        {
            return AESEncryption.Decrypt(PlayerPrefs.GetString("UserDataPath"));
        }
        else
        {
            string path = Path.Combine(Application.persistentDataPath, Guid.NewGuid().ToString());
            PlayerPrefs.SetString("UserDataPath", AESEncryption.Encrypt(path));
            PlayerPrefs.Save();
            return path;
        }
    }

    static public string GetNoADPath()
    {
        if (PlayerPrefs.HasKey("NoADPath"))
        {
            return AESEncryption.Decrypt(PlayerPrefs.GetString("NoADPath"));
        }
        else
        {
            string path = Path.Combine(Application.persistentDataPath, Guid.NewGuid().ToString());
            PlayerPrefs.SetString("NoADPath", AESEncryption.Encrypt(path));
            PlayerPrefs.Save();
            return path;
        }
    }

    // ���� ����
    static public void SaveSetting(SettingData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SettingData", json);
        PlayerPrefs.Save();
    }

    // ���� �ҷ�����
    static public SettingData LoadSetting()
    {
        if (PlayerPrefs.HasKey("SettingData"))
        {
            string json = PlayerPrefs.GetString("SettingData");
            SettingData data = JsonUtility.FromJson<SettingData>(json);
            return data;
        }

        SettingData defaultData = new SettingData() { SFXVolume = 0.8f, BGMVolume = 0.14f };
        SaveSetting(defaultData);

        return defaultData;// �⺻�� ��ȯ
    }
}
