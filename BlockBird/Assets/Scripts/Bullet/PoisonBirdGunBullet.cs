using System.Collections;
using UnityEngine;

public class PoisonBirdGunBullet : Bullet
{
    public PoisonBirdBulletFX effectPrefab;


    private float period;
    public float Period
    {
        get { return period; }
        set { period = value; }
    }


    protected override float SpecialEffect(BlockBase block)
    {
        //if (!block.IsPoison)
        {
            block.IsPoison = true;
            Instantiate(effectPrefab, block.gameObject.transform).init(Size, block, Period);
        }

        return Damage;
    }

}
