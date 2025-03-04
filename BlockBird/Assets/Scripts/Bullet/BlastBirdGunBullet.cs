using System.Collections;
using UnityEngine;

public class BlastBirdGunBullet : Bullet
{
    public BulletFX bulletFXPrefab;

    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            Damage *= 2.5f;
        }
        Instantiate(bulletFXPrefab, transform.position, Quaternion.identity, transform.parent).init(Damage, Size);
        return 0;
    }


}
