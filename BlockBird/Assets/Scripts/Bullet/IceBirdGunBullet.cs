using System.Collections;
using UnityEngine;

public class IceBirdGunBullet : Bullet
{
    private float freezeTime = 1;
    public float FreezeTime
    {
        get { return freezeTime; }
        set { freezeTime = value; }
    }

    private float slowRate = 1;
    public float SlowRate
    {
        get { return slowRate; }
        set { slowRate = value; }
    }

    protected override float SpecialEffect(BlockBase block)
    {
        block.Freeze(FreezeTime, SlowRate);

        if (block.IsBoss)
        {
            return Damage * 3f;
        }
        else
        {
            return Damage;
        }
    }

}
