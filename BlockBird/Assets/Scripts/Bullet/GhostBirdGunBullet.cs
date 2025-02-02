using System.Collections;
using UnityEngine;

public class GhostBirdGunBullet : Bullet
{
    public NoColliderBulletFX effectPrefab;
    private int destroyRate = 15;
    public int DestroyRate
    {
        get { return destroyRate; }
        set { destroyRate = value; }
    }

    protected override float SpecialEffect(BlockBase block)
    {
        
        if (destroyRate < 2)
        {
            destroyRate = 2;
        }

        if (Random.Range(0, destroyRate) == 0)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity, transform.parent).init(Size);
            return block.Life/2;
        }
        else
        {
            return 1;
        }
    }

}
