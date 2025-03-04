using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class DragonGun : GunBase
{
    DragonBullet dragonBullet;

    private void OnDestroy()
    {
        Destroy(dragonBullet.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragonBullet = Instantiate(bulletListPrefab[0]
            , transform.position
            , Quaternion.identity, GetBulletsTransform()).GetComponent<DragonBullet>();

        dragonBullet.Life = 100;
        dragonBullet.gunTransform = transform;

        for (int i = 1; i < characterLevel; i++)
        {
            LevelUp();
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
        delay = dragonBullet.Delay - (float)level/50f;
        if(delay <= 0.1f)
        {
            delay = 0.1f;
        }
        dragonBullet.LevelUp(
            (float)(level + 1) * 1.5f, delay);
    }
}
