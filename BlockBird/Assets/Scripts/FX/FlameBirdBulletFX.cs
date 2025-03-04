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
        this.DestroyTime = destroyTime;
        this.effectDuration = destroyTime;
        base.init(damage, size);
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


    void FixedUpdate()
    {
        if (GameManager.Instance.Character != null)
        {
            if (GameManager.Instance.Character.transform.position.x + 1f < transform.position.x)
            {
                transform.position += Vector3.left * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;
            }

            if (GameManager.Instance.Character.transform.position.x > transform.position.x)
            {
                transform.position += Vector3.right * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;
            }
        }
    }


    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            return damage * 2;
        }
        else
        {
            return damage;
        }

    }
}