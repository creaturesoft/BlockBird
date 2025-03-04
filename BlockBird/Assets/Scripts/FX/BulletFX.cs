using System.Collections;
using UnityEngine;

public class BulletFX : MonoBehaviour
{
    protected float damage;
    protected Collider2D damageCollider;

    public float effectDuration = 5.0f;  // 이펙트가 끝나는 예상 시간

    public AudioSource defaultAudio;


    public virtual void init(float damage, float size = 1f)
    {
        if (Time.time - GameManager.effectSoundLastPlayTime >= GameManager.effectSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
        {
            defaultAudio?.PlayOneShot(defaultAudio.clip);
            GameManager.effectSoundLastPlayTime = Time.time;
        }


        this.damage = damage;

        damageCollider = GetComponent<Collider2D>();

        transform.localScale = new Vector3(size, size, 1);

        StartCoroutine(DestroyCollider());
        if (effectDuration > 0f)
        {
            Destroy(gameObject, effectDuration);
        }
    }

    protected virtual IEnumerator DestroyCollider()
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