using System.Collections;
using UnityEngine;

public class FireRingBullet : Bullet
{
    public BulletFX bulletFXPrefab;
    private float period = 0.06f;  // 1초에 한 번
    public float Period
    {
        get { return period; }
        set { period = value; }
    }

    private float nextActionTime = 0f;

    private float rotationSpeed = 100;

    private float amplitude = 0.03f; // 물결의 진폭 (높이)

    private float startTime;

    protected override void Start()
    {
        base.Start();
        startTime = Time.time;
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(Random.Range(8f, 14f));
        Instantiate(bulletFXPrefab, transform.position, Quaternion.identity, transform.parent).init(Damage*10, Size);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time < nextActionTime)
        {
            return;
        }
        nextActionTime = Time.time + period;  // 다음 호출 시간 설정

        if (collision.CompareTag("Block"))
        {
            BlockBase block = collision.GetComponent<BlockBase>();
            if (block != null)
            {
                block.TakeDamage(
                    SpecialEffect(block)
                ); // 1 데미지
            }

        }
        else if (collision.CompareTag("Wall"))
        {
            // 총알 제거
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {

        // 시간에 따라 x축 물결 이동
        float wave = Mathf.Sin((Time.time - startTime) * Speed + Time.time) * amplitude;

        //transform.Rotate(0f, 0f, rotationSpeed * Speed * Time.fixedDeltaTime);

        rb.MovePosition(rb.position
            + (Vector2)(transform.parent.right * Speed * Time.fixedDeltaTime)
            + new Vector2(0f, wave));
    }

    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            return Damage * 4.0f;
        }
        else
        {
            return Damage;
        }
    }
}
