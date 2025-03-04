using System.Collections;
using UnityEngine;

public class DragonBulletFX : BulletFX
{
    private float delay;

    public void init(float damage, float size, float delay)
    {
        base.init(damage, size);
        this.delay = delay;
    }

    public void LevelUp(float damage, float delay)
    {
        this.damage = damage;
        this.delay = delay;
    }

    protected override IEnumerator DestroyCollider()
    {
        while (true)
        {
            damageCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            damageCollider.enabled = false;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);

            if (GameManager.Instance.Character.IsDie)
            {
                Destroy(gameObject);
                break;
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