using System.Collections;
using UnityEngine;

public class PoisonBirdGun : GunBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;
        StartCoroutine(FireContinuously(bulletListPrefab[0]));


        for (int i = 1; i < characterLevel; i++)
        {
            LevelUp();
        }
    }


    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;

            PoisonBirdGunBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<PoisonBirdGunBullet>();

            delay = bullet.Delay - (float)level/10f;
            if (delay <= 3)
            {
                delay = 3;
            }

            bullet.Damage += (float)level / 10f;  //0.1f;

            bullet.Period = 100 - (float)level/10f;
            if(bullet.Period <= 2)
            {
                bullet.Period = 2;
            }
            //bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
