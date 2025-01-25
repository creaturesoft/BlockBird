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

    public Transform Init(Transform parent, float life, int type=0)
    {
        transform.SetParent(parent, false);
        Life = life;

        Color newColor;
        switch (type)
        {
            case 0:
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#C57200", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#6F00C5", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 10f;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#C5009A", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 20f;
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#C60000", out newColor);
                gameObject.GetComponent<SpriteRenderer>().color = newColor;
                itemDropRate = 50f;
                break;
        }


        return transform;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void TakeDamage(float damage)
    {
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
            Instantiate(GameManager.Instance.itemPrefabList[Random.Range(1, GameManager.Instance.itemPrefabList.Length)], transform.position, Quaternion.identity, transform.parent)
                .GetComponent<BulletItemBase>()
                .Init(transform.parent);
        }

        Destroy(gameObject);
    }
}
