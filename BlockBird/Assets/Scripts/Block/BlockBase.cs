using System.Collections;
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
        }
    }

    private float itemDropRate;

    public TextMeshPro lifeText;

    public GameObject iced;


    public Transform Init(Transform parent, float life, int type=0)
    {
        transform.SetParent(parent, false);
        Life = life;

        Color newColor;
        switch (type)
        {
            case 0:
                itemDropRate = 0f;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#C57200", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 2f;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#6F00C5", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 4f;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#C5009A", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 8f;
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#C60000", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 16f;
                break;
        }
        itemDropRate = 0;

        return transform;
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
        if (Random.Range(0f, 100f) < itemDropRate)
        {
            int itemType = Random.Range(1, GameManager.Instance.itemPrefabList.Length);
            
            //itemType = 1;

            Instantiate(GameManager.Instance.itemPrefabList[itemType], transform.position, Quaternion.identity, transform.parent)
                .GetComponent<BulletItemBase>()
                .Init(transform.parent);
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
            Random.Range(0f, 360f)
        );
        iced.SetActive(true);

        while (currentFreezeCount < freezeTime)
        {
            transform.position += Vector3.right * Time.fixedDeltaTime * GameManager.Instance.Character.Speed * slowRate;
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
