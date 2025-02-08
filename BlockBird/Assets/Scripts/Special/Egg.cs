using System.Collections;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public Chick chickPrefab;
    private float hatchDelay;
    private float chickDamage;
    private float chickLife;
    private float delay;

    public void init(float hatchDelay, float chickDamage, float chickLife)
    {
        this.hatchDelay = hatchDelay;
        this.chickDamage = chickDamage;
        this.chickLife = chickLife;
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


        Instantiate(chickPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
            .init(chickDamage, chickLife, 20f);
        Destroy(gameObject);
    }

}
