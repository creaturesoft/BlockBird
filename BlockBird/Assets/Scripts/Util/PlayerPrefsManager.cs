using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public List<int> scoreList;
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
    static private List<int> LoadScore()
    {
        if (PlayerPrefs.HasKey("ScoreData"))
        {
            string json = PlayerPrefs.GetString("ScoreData");
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            return data.scoreList;
        }

        return new List<int>(); // �⺻�� ��ȯ
    }
}
