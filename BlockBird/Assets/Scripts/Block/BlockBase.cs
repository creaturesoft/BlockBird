using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    private float life = 1f;
    public float Life
    {
        get { return life; }
        set
        {

            if (value < 0)
            {
                value = 0;
            }

            life = value;

            lifeText.text = Mathf.CeilToInt(value).ToString();

            if (life <= 9999)
            {
                lifeText.fontSize = 8f;
            }
            else if (life <= 99999)
            {
                lifeText.fontSize = 6.5f;
            }
            else if (life <= 999999)
            {
                lifeText.fontSize = 5.6f;
            }
            else if (life <= 9999999)
            {
                lifeText.fontSize = 4.4f;
            }
            else
            {
                lifeText.fontSize = 4f;
            }
        }
    }

    private float maxLife = 1f;
    public float MaxLife
    {
        get { return maxLife; }
        set { maxLife = value; }
    }

    private float itemDropRate;

    public TextMeshPro lifeText;

    public GameObject iced;

    private bool isBoss = false;
    public bool IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

    public List<BlockBase> BossBlockCheckList { get; set; } = new List<BlockBase>();

    private bool isPoison = false;
    public bool IsPoison
    {
        get { return isPoison; }
        set { isPoison = value; }
    }

    float[] weightList = {
            0f,      //empty
            2f,      //무기
            200f,      //스피드업
            1f,      //친구들
            //0.02f    //레벨업
    };

    //float[] weightList = {
    //        0f,      //empty
    //        2f,      //무기
    //        2f,      //스피드업
    //        0.5f,      //친구들
    //        0.02f    //레벨업
    //};




    void CalculateDropRate(float dropRate)
    {
        for (int i = 1; i < weightList.Length; i++)
        {
            weightList[i] *= dropRate;
        }

        float total = 0;
        for (int i = 1; i < weightList.Length; i++)
        {
            total += weightList[i] * dropRate;
        }
        weightList[0] = 100f - total;
    }

    public BlockBase Init(Transform parent, float life, int type=0, bool isBoss = false)
    {
        transform.SetParent(parent, false);

        MaxLife = life;
        Life = life;
        IsBoss = isBoss;

        Color newColor;

        //무기 하나 먹으면 더 이상 무기 아이템 안나옴.
        if (GameManager.Instance.Character != null && GameManager.Instance.Character.NewGunCount > 0)
        {
            weightList[1] = 0f;
        }

        //스코어 목표 절반 이상이면 스피드업 아이템 나오지 않음
        if (GameManager.Instance.Character != null 
            && GameManager.Instance.Score > GameManager.Instance.GoalScore/2
            || GameManager.Instance.Character.isSpeedUp
            )
        {
            weightList[2] = 0f;
        }

        //한번 사용했으면 안나옴
        //if (GameManager.Instance.Character != null && GameManager.Instance.Character.isFriendsItem)
        //{
        //    weightList[3] = 0f;
        //}

        switch (type)
        {
            case 0:
                CalculateDropRate(0.05f);
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#C57200", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                CalculateDropRate(2f);
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#6F00C5", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                CalculateDropRate(4f);
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#C5009A", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                CalculateDropRate(8f);
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#C60000", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                CalculateDropRate(16f);
                break;
            case 5:
                ColorUtility.TryParseHtmlString("#C60000", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                CalculateDropRate(0f);
                break;
        }

        if (isBoss)
        {
            CalculateDropRate(0f);
        }

        return this;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void TakeDamage(float damage)
    {
        if (Life <= 0)
        {
            return;
        }


        string currentDisplayLife = lifeText.text;
        Life -= damage;
        string afterDisplayLife = lifeText.text;

        int addScore = int.Parse(currentDisplayLife) - int.Parse(afterDisplayLife);
        if (addScore > 0)
        {
            GameManager.Instance.AddScore(addScore);
        }

        if (Life <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        //소리재생
        SoundManager.Instance.PlayBreakBlockAudio(IsBoss);

        //통로 생기면 캐릭터 속도 복구
        if (IsBoss)
        {

            if (BossBlockCheckList.Count() > 0 && BossBlockCheckList.All(x => x == null))
            {
                GameManager.Instance.Character.Speed = GameManager.Instance.Character.OriginalSpeed;

            }
        }

        //블럭 확률
        float totalWeight = 0;
        foreach (float randomRate in weightList)
        {
            totalWeight += randomRate;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0;


        for (int i = 0; i < weightList.Length; i++)
        {
            currentWeight += weightList[i];
            if (randomValue < currentWeight)
            {
                //0f,      //empty
                //2f,      //무기
                //0.4f,    //스피드업
                //0.2f,    //친구들
                //0.02f    //레벨업

                if (i == 1)         //무기
                {
                    Instantiate(GameManager.Instance.itemPrefabList[UnityEngine.Random.Range(0, GameManager.Instance.itemPrefabList.Length)], transform.position, Quaternion.identity, transform.parent)
                        .GetComponent<BulletItemBase>()
                        .Init(transform.parent);
                }
                else if (i == 2)    //스피드업
                {
                    Instantiate(GameManager.Instance.SpeedUpItem, transform.position, Quaternion.identity, transform.parent)
                        .GetComponent<BulletItemBase>()
                        .Init(transform.parent);
                }
                else if (i == 3)    //친구들
                {
                    Instantiate(GameManager.Instance.FriendsItem, transform.position, Quaternion.identity, transform.parent)
                        .GetComponent<BulletItemBase>()
                        .Init(transform.parent);
                }
                else if (i == 4)    //레벨업
                {
                }
                break;
            }
        }

        Destroy(gameObject);
    }


    bool isFreeze;
    float currentFreezeCount;

    public void Freeze(float freezeTime, float freezeRate)
    {   
        if (isFreeze)
        {
            currentFreezeCount = 0;
            return;
        }

        StartCoroutine(FreezeCoroutine(freezeTime, freezeRate));
    }


    IEnumerator FreezeCoroutine(float freezeTime, float slowRate)
    {
        isFreeze = true;
        currentFreezeCount = 0;

        iced.transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            UnityEngine.Random.Range(0f, 360f)
        );
        iced.SetActive(true);

        while (currentFreezeCount < freezeTime)
        {
            if (IsBoss)
            {
                transform.position += Vector3.right * Time.fixedDeltaTime * GameManager.Instance.Character.Speed * (slowRate/7f);
            }
            else
            {
                transform.position += Vector3.right * Time.fixedDeltaTime * GameManager.Instance.Character.Speed * slowRate;
            }
            currentFreezeCount += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isFreeze = false;
        iced.SetActive(false);

    }

    //IEnumerator FreezeCoroutine()
    //{
    //    isFreeze = true;
    //    currentFreezeCount = 0;

    //    oldParent = transform.parent;
    //    transform.SetParent(transform.parent.parent);

    //    backgroundColor.gameObject.SetActive(true);
    //    backgroundColor.color = new Color32(7, 170, 255, 255);

    //    while (currentFreezeCount < freezeTime)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        currentFreezeCount++;
    //    }

    //    isFreeze = false;

    //    backgroundColor.gameObject.SetActive(false);

    //    transform.SetParent(oldParent);

    //    //while (transform.position.x >= -50)
    //    //{
    //    //    transform.position += Vector3.left * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;
    //    //    yield return new WaitForFixedUpdate();
    //    //}

    //    //Destroy(gameObject);
    //}
}
