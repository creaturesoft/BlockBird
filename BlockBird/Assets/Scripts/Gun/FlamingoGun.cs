using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingoGun : GunBase
{
    private List<GameObject> bulletList;
    private int bulletCount = 0;
    public float bulletCountDebug = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;

        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        bulletCount = 1;
        bulletList = new List<GameObject>();

        GameObject flamingoGunBullet = Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GetBulletsTransform());
        bulletList.Add(flamingoGunBullet);
        float currentAngle = (2 * Mathf.PI / characterLevel) * bulletCount; // 각도 간격에 따라 초기화

        FlamingoGunBullet bullet = flamingoGunBullet.GetComponent<FlamingoGunBullet>();
        bullet.currentAngle = currentAngle;
        bullet.centerObject = transform;

        bulletCountDebug = bulletCount;


        for (int i = 1; i < characterLevel; i++)
        {
            LevelUp();
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();

        //bulletCountDebug = 1 + Mathf.Pow(level, 0.6f);

        if (level < 20)
        {
            bulletCount++;
            bulletCountDebug = bulletCount;

            GameObject flamingoGunBullet = Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GetBulletsTransform());
            bulletList.Add(flamingoGunBullet);
            float currentAngle = (2 * Mathf.PI / characterLevel) * bulletCount; // 각도 간격에 따라 초기화
            
            FlamingoGunBullet bullet = flamingoGunBullet.GetComponent<FlamingoGunBullet>();
            bullet.currentAngle = currentAngle;
            bullet.centerObject = transform;

            foreach (GameObject oldBulllet in bulletList)
            {
                oldBulllet.GetComponent<Bullet>().Damage = (float)level / 10f;
            }
        }
        else
        {
            foreach (GameObject bullet in bulletList)
            {
                bullet.GetComponent<Bullet>().Damage = (float)level / 10f;
            }
        }
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform.position, Quaternion.identity, GetBulletsTransform());
        
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
