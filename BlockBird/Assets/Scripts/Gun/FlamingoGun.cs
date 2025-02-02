using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class FlamingoGun : GunBase
{
    private int bulletCount = 0;
    public float bulletCountDebug = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;

        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        bulletCount = 1;
        Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform);

        bulletCountDebug = bulletCount;
    }

    public override void LevelUp()
    {
        base.LevelUp();

        //bulletCountDebug = 1 + Mathf.Pow(level, 0.6f);

        if (Random.Range(0, (int)(1+(float)level/10f)) == 0)
        {
            bulletCount++;
            bulletCountDebug = bulletCount;
            Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<Bullet>().Damage += (float)level / 5f;
        }
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform);
        
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
