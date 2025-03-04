using System.Collections;
using UnityEngine;

public class FireRingGun : GunBase
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
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;
            
            FireRingBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<FireRingBullet>();

            bullet.Damage += (float)level / 1f;  //0.1f;
            //bullet.Period -= (float)level / 100f;

            //bullet.Delay -= (float)level / 10f;

            //if (bullet.Delay <= 10f)
            //{
            //    bullet.Delay = 10f;
            //}

            //bullet.Delay = 50f;


            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
