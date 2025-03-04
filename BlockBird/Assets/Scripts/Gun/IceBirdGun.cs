using System.Collections;
using UnityEngine;

public class IceBirdGun : GunBase
{

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
        //if (delay >= 0.5f)
        //{
        //    delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level, 0.7f);
        //    //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / level;
        //}
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            IceBirdGunBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<IceBirdGunBullet>();

            bullet.Damage += (float)level / 12f;  //0.1f;
            bullet.Life += (int)((float)level / 30f);
            //bullet.SlowRate = 0.2f + (float)level / 400f;
            //if(bullet.SlowRate >= 0.9f)
            //{
            //    bullet.SlowRate = 0.9f;
            //}
            bullet.SlowRate = 0.3f;

            bullet.Delay = bullet.Delay / Mathf.Pow(level + 1, 0.7f);

            if (bullet.Delay <= 0.4f)
            {
                bullet.Delay = 0.4f;
            }


            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }

            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
