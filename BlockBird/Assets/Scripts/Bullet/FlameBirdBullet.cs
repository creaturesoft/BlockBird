using System.Collections;
using UnityEngine;

public class FlameBirdBullet : Bullet
{
    public FlameBirdBulletFX bulletFXPrefab;
    private float goalPositionX = 5f; // 물결의 진폭 (높이)

    private float destroyTime;
    public float DestroyTime
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }

    protected override void Start()
    {
        base.Start();

        transform.localScale = new Vector3(0.3f, 0.3f, 1);
        goalPositionX = Random.Range(1f, 12f);
    }

    protected override float SpecialEffect(BlockBase block)
    {
        Instantiate(bulletFXPrefab, transform.position, Quaternion.identity, transform.parent).init(Damage, Size, DestroyTime);
        return 0;
    }

}
