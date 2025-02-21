using System.Linq;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            if (character != null)
            {
                TakeItem(character);
            }

            Destroy(gameObject);
        }
    }

    public Transform Init(Transform parent)
    {
        transform.SetParent(parent, false);
        return transform;
    }

    public abstract void TakeItem(Character character);
}
