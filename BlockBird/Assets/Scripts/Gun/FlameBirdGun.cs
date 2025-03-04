using System.Collections;
using UnityEngine;

public class FlameBirdGun : GunBase
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

    public override void LevelUp()
    {
        base.LevelUp();
        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        //if (delay >= 0.3f)
        //{
        //    delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level+1, 0.7f);
        //    //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / level;
        //}
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;

            FlameBirdBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<FlameBirdBullet>();

            bullet.Damage += (float)level / 6f;  //0.1f;
            bullet.Size += (float)level / 80f;  //0.1f;
            if(bullet.Size > 2)
            {
                bullet.Size = 2;
            }

            bullet.DestroyTime = 10f + (float)level / 50f;  //0.5f;
            if(bullet.DestroyTime > 50f)
            {
                bullet.DestroyTime = 50f;
            }

            bullet.Delay -= (float)level / 30f;  //0.5f;
            if(bullet.Delay < 2f)
            {
                bullet.Delay = 2f;
            }

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
