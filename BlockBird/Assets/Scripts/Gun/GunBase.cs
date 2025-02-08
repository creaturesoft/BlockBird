using System;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GameObject[] bulletListPrefab;
    public int level;
    public bool isLastGun;
    public Type prefabType;
    protected float delay;


    public GunBase init()
    {
        isLastGun = true;
        prefabType = this.GetType();
        return this;
    }

    public virtual void LevelUp()
    {
        level++;
    }


}
