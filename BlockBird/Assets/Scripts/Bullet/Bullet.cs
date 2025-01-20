using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float damage = 1f; // �Ѿ� ������
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    [SerializeField]
    float speed = 20f; // �Ѿ� �ӵ�
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    float delay = 1f; // ���� �ӵ�
    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }

    [SerializeField]
    float lifeTime = 5f; // �������� �������� �ð�
    public float LifeTime
    {
        get { return lifeTime; }
        set { lifeTime = value; }
    }

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ���� �ð��� ������ �Ѿ��� ����
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Kinematic Rigidbody�� �̵�
        rb.MovePosition(rb.position + (Vector2)(transform.right * Speed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            BlockBase block = collision.GetComponent<BlockBase>();
            if (block != null)
            {
                block.TakeDamage(Damage); // 1 ������
            }

            // �Ѿ� ����
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            // �Ѿ� ����
            Destroy(gameObject);
        }
    }

}
