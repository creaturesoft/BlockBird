using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public GameObject[] mapList;
    public GameObject[] bossMapList;

    public int bossMapFrequency { get; set; } = 2;

    void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스를 현재 객체로 설정
        Instance = this;
    }

    public int GetBossMapIndex()
    {
        return Random.Range(0, bossMapList.Length);
    }

    public void StartMap()
    {
        if(PersistentObject.Instance.UserData.stage % bossMapFrequency == 0)
        {
            //보스맵
            try
            {
                if (PersistentObject.Instance.UserData.stage == PersistentObject.Instance.UserData.maxStage)
                {
                    bossMapList[PersistentObject.Instance.UserData.currentBossMapIndex].SetActive(true);
                }
                else
                {
                    bossMapList[MapManager.Instance.GetBossMapIndex()].SetActive(true);
                }
            }
            catch
            {
                bossMapList[GetBossMapIndex()].SetActive(true);
            }
        }
        else
        {
            //일반맵
            mapList[Random.Range(0, mapList.Length)].SetActive(true);
        }
    }
}