using System.Collections;
using UnityEngine;

public class FireRingBullet : Bullet
{
    public BulletFX bulletFXPrefab;
    private float period = 0.06f;  // 1�ʿ� �� ��
    public float Period
    {
        get { return period; }
        set { period = value; }
    }

    private float nextActionTime = 0f;

    private float rotationSpeed = 100;

    private float amplitude = 0.03f; // ������ ���� (����)

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
        nextActionTime = Time.time + period;  // ���� ȣ�� �ð� ����

        if (collision.CompareTag("Block"))
        {
            BlockBase block = collision.GetComponent<BlockBase>();
            if (block != null)
            {
                block.TakeDamage(
                    SpecialEffect(block)
                ); // 1 ������
            }

        }
        else if (collision.CompareTag("Wall"))
        {
            // �Ѿ� ����
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {

        // �ð��� ���� x�� ���� �̵�
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
