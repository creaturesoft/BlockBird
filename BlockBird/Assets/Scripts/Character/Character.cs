using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    public float Speed 
    {
        get { return speed; }
        set
        {
            speed = value;
        }
    }

    [SerializeField]
    float attackSpeed = 1f;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;
        }
    }

    [SerializeField]
    float jumpForce = 7f;
    public float JumpForce
    {
        get { return jumpForce; }
        set
        {
            jumpForce = value;
        }
    }

    [SerializeField]
    int hp = 5;               // �÷��̾� �Ǵ� ��ü�� HP
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }

    [SerializeField]
    int maxHp = 5;               // �÷��̾� �Ǵ� ��ü�� HP
    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
        }
    }

    private bool isDie = false;
    public bool IsDie
    {
        get { return isDie; }
        set
        {
            isDie = value;
        }
    }

    int damagePerSecond = 1;   // 1�ʸ��� ������ HP
    float lastDamageTime = 0f; // ���������� �������� �� �ð�


    public List<GameObject> initBulletPrefabList;
    private Rigidbody2D rb;
    private TextMeshPro hpText; // HP�� ǥ���� �ؽ�Ʈ


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hpText = GetComponentInChildren<TextMeshPro>();

        Hp = MaxHp;

        DisableCharacter();

        foreach (GameObject bulletPrefab in initBulletPrefabList)
        {
            StartCoroutine(FireContinuously(bulletPrefab));
        }
    }

    public void DisableCharacter()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        hpText.text = string.Empty;
    }

    public void EnableCharacter()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        hpText.text = Hp.ToString();
    }



    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        while (!IsDie)
        {
            if (GameManager.IsPaused)
            {
                yield return null;
                continue;
            }

            Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform);
            yield return new WaitForSeconds(bullet.Delay / AttackSpeed);
        }
    }

    public void TakeBulletItem(GameObject bulletPrefab)
    {
        StartCoroutine(FireContinuously(bulletPrefab));
    }


    void FixedUpdate()
    {
        if (GameManager.IsPaused) return;

        if (transform.position.x < -0.001f || transform.position.x > 0.001f)
        {
            Vector3 originalPosition = new Vector3(0f, transform.position.y, transform.position.z);

            // ���� �ӵ��� ���� ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Speed * Time.deltaTime);

            // �����ϰ� ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (transform.position.x > -0.01f && transform.position.x < 0.01f)
            {
                transform.position = originalPosition; // ��ġ ����
            }
        }
        else
        {
            Hp = MaxHp;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(Hp <= 0 || transform.position.x > -0.5f && transform.position.x < 0.5f)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            // ������ �浹�� ��� ������ ó��
            if (Time.time >= lastDamageTime + 1f)
            {
                Hp -= damagePerSecond;
                lastDamageTime = Time.time;

                if (Hp <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        IsDie = true;
        GameManager.Instance.CheckGameOver();

        rb.freezeRotation = false;
        Speed = 1f;

        rb.AddTorque(Random.Range(-50f, 50f));

        StartCoroutine(AfterDie());
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(GameManager.Instance.ResultDelay);

        hpText.text = string.Empty;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
