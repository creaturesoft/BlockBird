using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GameObject[] bulletListPrefab; 
    private int level;

    public virtual void LevelUp()
    {
        level++;
    }
}
