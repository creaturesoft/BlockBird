using System.Collections;
using UnityEngine;

public class EagleBullet : Bullet
{
    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            return Damage * 1.2f;
        }
        else
        {
            return Damage;
        }
    }

}
