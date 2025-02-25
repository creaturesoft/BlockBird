using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class Character : MonoBehaviour
{
    string characterName;
    public string CharacterName
    {
        get { return characterName; }
        set
        {
            characterName = value;
        }
    }

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

    float originalSpeed = 1f;
    public float OriginalSpeed
    {
        get { return originalSpeed; }
        set
        {
            originalSpeed = value;
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

    private int newGunCount = 0;
    public int NewGunCount
    {
        get { return newGunCount; }
        set
        {
            newGunCount = value;
        }
    }

    private int level = 0;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
        }
    }

    private int expLevel = 0;
    public int ExpLevel
    {
        get { return expLevel; }
        set
        {
            expLevel = value;
        }
    }

    private int pullLevel = 0;
    public int PullLevel
    {
        get { return pullLevel; }
        set
        {
            pullLevel = value;
        }
    }


    private double exp = 0;
    public double Exp
    {
        get { return exp; }
        set
        {
            exp = value;
        }
    }



    public TextMeshPro hpText; // HP를 표시할 텍스트

    int damagePerSecond = 1;   // 1초마다 감소할 HP
    float lastDamageTime = 0f; // 마지막으로 데미지를 준 시간


    public GameObject[] initGunPrefabList;
    private List<GunBase> useGunPrefabList;
    private List<GunBase> useGunList;

    private Rigidbody2D rb;
    public bool isActive = false;

    public GameObject clearEmoji;

    public bool isFriend;


    public Character Init(Character character, bool isFriend = false)
    {
        characterName = character.name;

        useGunPrefabList = new List<GunBase>();
        useGunList = new List<GunBase>();
        clearEmoji.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        Hp = MaxHp;
        OriginalSpeed = Speed;

        ExpLevel = character.ExpLevel;
        PullLevel = character.PullLevel;

        Level = character.PullLevel + character.ExpLevel;

        Exp = character.Exp;


        this.isFriend = isFriend;
        if (isFriend)
        {
            EnableCharacter();
            hpText.enabled = false;
        }
        else
        {
            DisableCharacter();
        }

        return this;
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
            hpText.enabled = false;
        }
    }

    public void EnableCharacter()
    {
        isActive = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        hpText.enabled = true;

        foreach (GameObject gunPrefab in initGunPrefabList)
        {
            GunBase gunBasePrefab = gunPrefab.GetComponent<GunBase>();

            useGunList.Add(Instantiate(gunBasePrefab, transform).init(this));

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
                NewGunCount++;

                //아이템 추가
                useGunList.Add(Instantiate(gunPrefab, transform).init(1));
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
        if (isFriend) return;
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

    public void Die(bool isClear = false)
    {
        IsDie = true;
        GameManager.Instance.CheckGameOver(isClear);

        if (isClear)
        {
            Exp += GameManager.Instance.Score;
            
            if (GetExpForLevel() < Exp)
            {
                Level += 1;
                ExpLevel += 1;
                Exp = 0;
            }

            //경험치, 레벨 저장
            BirdData birdData = GameManager.Instance.UserData.birdList.Where(x => x.name == characterName).FirstOrDefault();

            birdData.expLevel = ExpLevel;
            birdData.exp = Exp;
            GameManager.Instance.UserData.stage++;

            SaveLoadManager.SaveUserData(GameManager.Instance.UserData);
            StartCoroutine(SaveLoadManager.SendUserDataToServer(GameManager.Instance.UserData));
            //GameManager.Instance.UserData = SaveLoadManager.LoadUserData();


            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            Speed = 1f;
            rb.freezeRotation = false;
            rb.AddTorque(Random.Range(-50f, 50f));
        }

        StartCoroutine(AfterDie());
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(GameManager.Instance.ResultDelay);
        DisableCharacter();
    }

    public void Clear()
    {
        Instantiate(GameManager.Instance.clearEmojiList[Random.Range(0, GameManager.Instance.clearEmojiList.Length)]
            , clearEmoji.transform);
        clearEmoji.SetActive(true);
    }

    public double GetExpForLevel()
    {
        double a = 100000; // 초기값
        double b = 1.2; // 증가율
        return (a * System.Math.Pow(b, (double)expLevel));
    }


    /// <summary>
    /// 스피드업 아이템
    /// </summary>
    public bool isSpeedUp;
    float speedUpElapseTime;
    List<GameObject> speedUpGuns = new List<GameObject>();
    public void TakeSpeedUpItem()
    {
        if (isSpeedUp)
        {
            speedUpElapseTime = 0;
            return;
        }


        SoundManager.Instance.PlaySpeedUpItemAudio();

        isSpeedUp = true;
        StartCoroutine(FinishSpeedUp());

        originalSpeed = Speed;
        Speed = 12f;

        //아이템 추가
        for (int i = 0; i < initGunPrefabList.Length; i++)
        {
            GameObject gun = Instantiate(initGunPrefabList[i], transform);
            speedUpGuns.Add(gun);
            useGunList.Add(gun.GetComponent<GunBase>()
                .init(useGunList[i].level * 10));
        }

    }

    IEnumerator FinishSpeedUp()
    {
        while (6f > speedUpElapseTime)
        {
            speedUpElapseTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }


        Speed = originalSpeed;
        foreach (GameObject gun in speedUpGuns)
        {
            Destroy(gun);
        }

        isSpeedUp = false;
    }

    /// <summary>
    /// 친구 소환 아이템
    /// </summary>
    public bool isFriendsItem;
    float friendsElapseTime;
    public void TakeFriendsItem()
    {
        friendsElapseTime = 0;
        if (isFriendsItem)
        {
            return;
        }

        GameManager.Instance.friendBulletGameObject = new GameObject("Friends");

        isFriendsItem = true;
        StartCoroutine(CallFriends());
    }
    IEnumerator CallFriends()
    {

        for (int i = 0; i < 20; i++)
        {
            SoundManager.Instance.PlayFriendsItemAudio();

            Character friend = Instantiate(
                GameManager.Instance.characterSelect.CharactersPrefabs[Random.Range(2, GameManager.Instance.characterSelect.CharactersPrefabs.Count)]
                //, new Vector3(Random.Range(-2f, 12f), Random.Range(-4f, 4f), 0)
                , new Vector3(Random.Range(3f, 10f), 0, 0)
                , Quaternion.identity
            );


            friend.transform.parent = GameManager.Instance.friendBulletGameObject.transform;
            friend.gameObject.layer = LayerMask.NameToLayer("Friend");

            friend.PullLevel /= 10;
            friend.ExpLevel /= 10;
            if (friend.ExpLevel <= 0)
            {
                friend.ExpLevel = 1;
            }


            friend.Init(friend, true);

            GameManager.Instance.friendRbList.Add(friend.GetComponent<Rigidbody2D>());

            yield return new WaitForSeconds(0.3f);
        }


        while (5f > friendsElapseTime)
        {
            friendsElapseTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GameManager.Instance.friendRbList.Clear();
        Destroy(GameManager.Instance.friendBulletGameObject);

        isFriendsItem = false;
    }


}
