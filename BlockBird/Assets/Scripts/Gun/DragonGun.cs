using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class DragonGun : GunBase
{
    DragonBullet dragonBullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragonBullet = Instantiate(bulletListPrefab[0]
            , transform.position
            , Quaternion.identity, GameManager.Instance.bulletGameObject.transform).GetComponent<DragonBullet>();

        dragonBullet.Life = 100;
    }

    public override void LevelUp()
    {
        base.LevelUp();

        dragonBullet.LevelUp(
            (float)(level + 1) / 5f
            , dragonBullet.Delay / Mathf.Pow(level, 0.2f));
    }
}
