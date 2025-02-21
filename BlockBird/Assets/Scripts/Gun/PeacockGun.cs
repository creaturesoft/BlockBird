using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class PeacockGun : GunBase
{
    private int bulletCount = 0;
    public float bulletCountDebug = 0;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
        bulletCount = 1;


        for (int i = 1; i < characterLevel; i++)
        {
            LevelUp();
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();

        bulletCountDebug = 1 + Mathf.Pow(level, 0.6f);
        bulletCount = (int)bulletCountDebug;
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GetBulletsTransform())
                .GetComponent<Bullet>();

            bullet.Damage += (float)level;
            bullet.Life += (int)((float)level / 50f);

            for (int i = 1; i <= bulletCount/2; i++)
            {
                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, i * 3), GetBulletsTransform())
                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 20f;
                bullet.Life += (int)((float)level / 80f);

                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -i * 3), GetBulletsTransform())
                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 20f;
                bullet.Life += (int)((float)level / 80f);
            }

            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
