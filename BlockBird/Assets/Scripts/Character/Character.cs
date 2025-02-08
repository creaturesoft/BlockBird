using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;

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
    int hp = 5;               // 플레이어 또는 객체의 HP
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            hpText.text = Hp.ToString();
        }
    }

    [SerializeField]
    int maxHp = 5;               // 플레이어 또는 객체의 HP
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

    int damagePerSecond = 1;   // 1초마다 감소할 HP
    float lastDamageTime = 0f; // 마지막으로 데미지를 준 시간


    public GameObject[] initGunPrefabList;
    private List<GunBase> useGunPrefabList;
    private List<GunBase> useGunList;

    private Rigidbody2D rb;
    private TextMeshPro hpText; // HP를 표시할 텍스트
    public bool isActive = false;

    void Start()
    {
        useGunPrefabList = new List<GunBase>();
        useGunList = new List<GunBase>();

        rb = GetComponent<Rigidbody2D>();
        hpText = GetComponentInChildren<TextMeshPro>();

        Hp = MaxHp;


        DisableCharacter();
    }

    public void DisableCharacter()
    {
        isActive = false;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        if (hpText != null)
        {
            hpText.text = string.Empty;
        }
    }

    public void EnableCharacter()
    {
        isActive = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        hpText.text = Hp.ToString();

        foreach (GameObject gunPrefab in initGunPrefabList)
        {
            GunBase gunBasePrefab = gunPrefab.GetComponent<GunBase>();

            useGunList.Add(Instantiate(gunBasePrefab, transform).init());
            useGunPrefabList.Add(gunBasePrefab);
        }
    }


    public void TakeBulletItem(GunBase[] gunPrefabList)
    {
        if (gunPrefabList == null || gunPrefabList.Length == 0)
        {
            System.Type[] useLastGuns = useGunList.FindAll(x => x.isLastGun).Select(x => x.prefabType).ToArray();
            gunPrefabList = useGunPrefabList.FindAll(x => useLastGuns.Contains(x.GetType())).ToArray();
        }

        foreach (GunBase gun in useGunList)
        {
            gun.isLastGun = false;
        }

        foreach (GunBase gunPrefab in gunPrefabList)
        {
            GunBase addedGun = useGunPrefabList.Find(x => x.name == gunPrefab.name);

            if (addedGun == null)
            {
                //아이템 추가
                useGunList.Add(Instantiate(gunPrefab, transform).init());
                useGunPrefabList.Add(gunPrefab);
            }
            else
            {
                foreach (GunBase checkGun in useGunList)
                {
                    if (checkGun.GetType() == addedGun.GetType())
                    {
                        checkGun.isLastGun = true;
                        checkGun.LevelUp();
                    }
                }

            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isActive) return;
        if (GameManager.Instance.IsGameOver) return;
        if (IsDie) return;

        if (transform.position.x < -0.001f || transform.position.x > 0.001f)
        {
            Vector3 originalPosition = new Vector3(0f, transform.position.y, transform.position.z);

            // 일정 속도로 원래 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Speed * Time.deltaTime);

            // 정밀하게 목표 위치에 도달했는지 확인
            if (transform.position.x > -0.01f && transform.position.x < 0.01f)
            {
                transform.position = originalPosition; // 위치 고정
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
            // 적과의 충돌일 경우 데미지 처리
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
        DisableCharacter();
    }
}
