using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float damage = 1f; // 총알 데미지
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    [SerializeField]
    float speed = 20f; // 총알 속도
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    float delay = 1f; // 연사 속도
    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }

    [SerializeField]
    float lifeTime = 5f; // 없어지기 전까지의 시간
    public float LifeTime
    {
        get { return lifeTime; }
        set { lifeTime = value; }
    }

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 일정 시간이 지나면 총알을 제거
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Kinematic Rigidbody를 이동
        rb.MovePosition(rb.position + (Vector2)(transform.right * Speed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            BlockBase block = collision.GetComponent<BlockBase>();
            if (block != null)
            {
                block.TakeDamage(Damage); // 1 데미지
            }

            // 총알 제거
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            // 총알 제거
            Destroy(gameObject);
        }
    }

}
