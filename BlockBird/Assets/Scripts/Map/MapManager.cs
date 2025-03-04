using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public GameObject[] mapList;
    public GameObject[] bossMapList;

    public int bossMapFrequency { get; set; } = 2;

    void Awake()
    {
        // �̱��� �ν��Ͻ��� �̹� �����ϸ� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���� ��ü�� ����
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
            //������
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
            //�Ϲݸ�
            mapList[Random.Range(0, mapList.Length)].SetActive(true);
        }
    }
}