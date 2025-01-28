using System.Linq;
using UnityEngine;

public class BulletItemBase : MonoBehaviour
{
    public GameObject[] gunPrefabList;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            if (character != null)
            {
                character.TakeBulletItem(gunPrefabList.Select(gunPrefab => gunPrefab.GetComponent<GunBase>()).ToArray());
            }

            Destroy(gameObject);
        }
    }

    public Transform Init(Transform parent)
    {
        transform.SetParent(parent, false);
        return transform;
    }
}
