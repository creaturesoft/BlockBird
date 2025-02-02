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
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<Bullet>();

            bullet.Damage += (float)level / 25f;
            bullet.Life += (int)((float)level / 100f);

            for (int i = 1; i <= bulletCount/2; i++)
            {
                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, i * 3), GameManager.Instance.bulletGameObject.transform)
                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 25f;
                bullet.Life += (int)((float)level / 100f);

                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -i * 3), GameManager.Instance.bulletGameObject.transform)
                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 25f;
                bullet.Life += (int)((float)level / 100f);
            }

            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
