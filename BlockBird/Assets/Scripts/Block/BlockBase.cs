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

    public TextMeshPro lifeText;

    public Transform Init(GameObject parent, float life)
    {
        transform.SetParent(parent.transform, false);
        Life = life;
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
        Destroy(gameObject);
    }
}
