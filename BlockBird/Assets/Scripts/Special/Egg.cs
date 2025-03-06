using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Egg : Bullet
{
    public Chick chickPrefab;
    private float hatchDelay;
    private float chickDamage;
    private float chickLife;
    //private float delay;
    private bool isFriend;

    public BulletFX bulletFXPrefab;

    protected override float SpecialEffect(BlockBase block)
    {
        if (block.IsBoss)
        {
            Damage *= 1.8f;
        }
        Instantiate(bulletFXPrefab, transform.position, Quaternion.identity, transform.parent).init(Damage, Size);
        return 0;
    }

    public void init(float hatchDelay, float chickDamage, float chickLife, bool isFriend = false)
    {
        this.hatchDelay = hatchDelay;
        this.chickDamage = chickDamage;
        this.chickLife = chickLife;
        this.isFriend = isFriend;
        StartCoroutine(Hatch());
    }

    IEnumerator Hatch()
    {
        float elapsedTime = 0f;
        float hatchPositionX = Random.Range(-2f, 6f);
        Vector3 startPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + hatchPositionX, transform.position.y, transform.position.z);
        while (elapsedTime < hatchDelay)
        {
            float time = elapsedTime / hatchDelay;
            transform.position = Vector3.Lerp(startPosition, destination, time);

            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }


        Instantiate(chickPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
            .init(chickDamage, chickLife, 25f, isFriend);
        Destroy(gameObject);
    }

    public Transform GetBulletsTransform()
    {
        if (isFriend)
        {
            return GameManager.Instance.friendBulletGameObject.transform;
        }
        return GameManager.Instance.bulletGameObject.transform;
    }
}
