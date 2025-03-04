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

            bullet.Delay -= (float)level/5f;
            if (bullet.Delay <= 2)
            {
                bullet.Delay = 2;
            }

            bullet.Damage += (float)level / 1.5f;  //0.1f;

            bullet.Period = 500 - (float)level/10f;
            if(bullet.Period <= 100)
            {
                bullet.Period = 100;
            }
            //bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


}
