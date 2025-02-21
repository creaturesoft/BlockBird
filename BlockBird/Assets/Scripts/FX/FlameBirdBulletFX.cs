using System.Collections;
using UnityEngine;

public class FlameBirdBulletFX : BulletFX
{
    private float delay;


    private float destroyTime;
    public float DestroyTime
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }


    public void init(float damage, float size, float destroyTime)
    {
        base.init(damage, size);
        this.DestroyTime = destroyTime;
        this.effectDuration = destroyTime;
        StartCoroutine(Explosion());
    }


    protected override IEnumerator DestroyCollider()
    {
        while (true)
        {
            damageCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            damageCollider.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }

}