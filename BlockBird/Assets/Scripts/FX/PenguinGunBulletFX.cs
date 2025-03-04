using System.Collections;
using UnityEngine;

public class PenguinGunBulletFX : BulletFX
{
    private float freezeTime = 1;
    private float slowRate = 1;

    public void init(float damage, float size, float freezeTime, float slowRate)
    {
        base.init(damage, size);
        this.freezeTime = freezeTime;
        this.slowRate = slowRate;
    }

    protected override float SpecialEffect(BlockBase block)
    {
        block.Freeze(freezeTime, slowRate);

        if (block.IsBoss)
        {
            return damage * 5f;
        }
        else
        {
            return damage;
        }
    }


}