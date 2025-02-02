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

    [SerializeField]
    int life = 1;
    public int Life
    {
        get { return life; }
        set { life = value; }
    }

    [SerializeField]
    float size = 1f;
    public float Size
    {
        get { return size; }
        set { size = value; }
    }

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ���� �ð��� ������ �Ѿ��� ����
        //Destroy(gameObject, lifeTime);
    }

    protected virtual void FixedUpdate()
    {
        // Kinematic Rigidbody�� �̵�
        rb.MovePosition(rb.position + (Vector2)(transform.right * Speed * Time.fixedDeltaTime));
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
                ); // 1 ������
            }

            // �Ѿ� ����
            if(Life > 0)
            {
                Life--;
            }
            
            if(Life <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            // �Ѿ� ����
            Destroy(gameObject);
        }
    }

    protected virtual float SpecialEffect(BlockBase block)
    {
        // Ư�� ȿ��

        return Damage;
    }
}
