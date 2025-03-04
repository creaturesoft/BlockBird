using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingoGun : GunBase
{
    private List<FlamingoGunBullet> bulletList;
    private int bulletCount = 0;
    public float bulletCountDebug = 0;
    bool isExpand = true;
    float lastRadius = 1.3f;

    private void OnDestroy()
    {
        foreach (FlamingoGunBullet bullet in bulletList)
        {
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;

        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        bulletList = new List<FlamingoGunBullet>();

        //GameObject flamingoGunBullet = Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GetBulletsTransform());

        //FlamingoGunBullet bullet = flamingoGunBullet.GetComponent<FlamingoGunBullet>();

        //bulletList.Add(bullet);
        //float currentAngle = (2 * Mathf.PI / characterLevel) * bulletCount; // 각도 간격에 따라 초기화

        //bullet.currentAngle = currentAngle;
        //bullet.centerObject = transform;



        for (int i = 0; i < characterLevel; i++)
        {
            LevelUp();
        }
        StartCoroutine(FireContinuously());
    }

    public override void LevelUp()
    {
        base.LevelUp();


        //bulletCountDebug = 1 + Mathf.Pow(level, 0.6f);

        if (bulletList.Count > 0)
        {
            lastRadius = bulletList[0].radius;
        }

        foreach (FlamingoGunBullet bullet in bulletList)
        {
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
        }
        bulletList.Clear();


        bulletCount = 0;
        bulletCountDebug = 0;

        for (int i = 0; i < level; i++)
        {
            if (bulletCount < 30)
            {
                bulletCount++;
                bulletCountDebug = bulletCount;

                GameObject flamingoGunBullet = Instantiate(bulletListPrefab[0], transform.position, Quaternion.identity, GetBulletsTransform());


                FlamingoGunBullet bullet = flamingoGunBullet.GetComponent<FlamingoGunBullet>();
                bullet.radius = lastRadius;

                bulletList.Add(bullet);
                float currentAngle = (2 * Mathf.PI / (level > 30 ? 30 : level)) * bulletCount; // 각도 간격에 따라 초기화

                bullet.currentAngle = currentAngle;
                bullet.centerObject = transform;

            }
        }


        foreach (FlamingoGunBullet oldBulllet in bulletList)
        {
            oldBulllet.Damage = (float)level / 16f;
        }


    }

    IEnumerator FireContinuously()
    {

        while (!GameManager.Instance.Character.IsDie)
        {
            if (lastRadius > 3f)
            {
                isExpand = false;
            }
            else if (lastRadius < 0.5f)
            {
                isExpand = true;
            }

            if (isExpand)
            {
                lastRadius += 0.02f;
            }
            else
            {
                lastRadius -= 0.02f;
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i] != null)
                {
                    bulletList[i].radius = lastRadius;
                }
            }

            if (lastRadius < 1.3f)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForSeconds(0.02f);
            }
        }

        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] != null)
            {
                Destroy(bulletList[i].gameObject);
            }
        }
    }


}
