using System.Collections;
using UnityEngine;

public class ChickBullet: Bullet
{
    
    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            return Damage * 1.8f;
        }
        else
        {
            return Damage;
        }
    }

}
