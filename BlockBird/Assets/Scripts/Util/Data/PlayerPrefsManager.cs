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

        // 중복 점수 확인
        if (scores.Contains(newScore))
        {
            Debug.Log($"점수 {newScore}는 이미 리스트에 존재합니다.");
            return scores.Count - scores.IndexOf(newScore);
        }

        // 점수를 삽입할 올바른 위치 찾기 (이진 탐색)
        int index = scores.BinarySearch(newScore);
        if (index < 0)
            index = ~index;

        // 점수를 정렬된 위치에 삽입
        scores.Insert(index, newScore);

        // 등수 반환
        int rank = scores.Count - index;
        Debug.Log($"점수 {newScore}가 삽입되었습니다. 현재 등수: {rank}");

        SaveScore(scores);
        return rank;
    }


    // 리스트 저장
    static private void SaveScore(List<int> list)
    {
        ScoreData data = new ScoreData { scoreList = list };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("ScoreData", json);
        PlayerPrefs.Save();
    }

    // 리스트 불러오기
    static public List<int> LoadScore()
    {
        if (PlayerPrefs.HasKey("ScoreData"))
        {
            string json = PlayerPrefs.GetString("ScoreData");
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            return data.scoreList;
        }

        return new List<int>(); // 기본값 반환
    }


    // 리스트 불러오기
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

    // 설정 저장
    static public void SaveSetting(SettingData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SettingData", json);
        PlayerPrefs.Save();
    }

    // 설정 불러오기
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

        return defaultData;// 기본값 반환
    }
}
