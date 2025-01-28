using System.Collections;
using UnityEngine;

public class PenguinGunBullet : Bullet
{


    private Vector3 fixedWorldPosition;
    public float freezeTime = 1;

    protected override void SpecialEffect(BlockBase block)
    {
        base.SpecialEffect(block);

        block.Freeze(freezeTime);
    }

}
