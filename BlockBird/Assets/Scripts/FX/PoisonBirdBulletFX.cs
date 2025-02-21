using System.Collections;
using UnityEngine;

public class PoisonBirdBulletFX : NoColliderBulletFX
{
    BlockBase block;
    
    private float period;


    public void init(float size, BlockBase block, float period)
    {
        base.init(size);
        effectDuration = 20f;
        this.block = block;
        this.period = period;
        StartCoroutine(Poison());
    }


    IEnumerator Poison()
    {
        while (true)
        {
            if (block == null)
            {
                Destroy(gameObject);
                yield break;
            }

            if (block.IsBoss)
            {
                block.TakeDamage(block.Life / period / 2);
            }
            else
            {
                block.TakeDamage(block.Life / period);
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

    }
}