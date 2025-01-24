using UnityEngine;

public class BulletItemBase : MonoBehaviour
{
    public GameObject bulletPrefab;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            if (character != null)
            {
                character.TakeBulletItem(bulletPrefab);
            }

            Destroy(gameObject);
        }
    }

    public Transform Init(GameObject parent)
    {
        transform.SetParent(parent.transform, false);
        return transform;
    }
}
