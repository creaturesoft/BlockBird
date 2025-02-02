using System.Collections;
using UnityEngine;

public class BulletFX : MonoBehaviour
{
    protected float damage;
    private CircleCollider2D damageCollider;
    protected float effectDuration = 5.0f;  // 이펙트가 끝나는 예상 시간

    virtual public void init(float damage, float size = 1f)
    {
        this.damage = damage;
        damageCollider = GetComponent<CircleCollider2D>();
        transform.localScale = new Vector3(size, size, 1);

        StartCoroutine(DestroyCollider());
        Destroy(gameObject, effectDuration);
    }

    IEnumerator DestroyCollider()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(damageCollider);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            BlockBase block = collision.GetComponent<BlockBase>();
            if (block != null)
            {
                block.TakeDamage(
                    SpecialEffect(block)
                );
            }
        }
    }

    protected virtual float SpecialEffect(BlockBase block)
    {
        // 특수 효과
        return damage;
    }

}