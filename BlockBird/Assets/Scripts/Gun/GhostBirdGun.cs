using System.Collections;
using UnityEngine;

public class GhostBirdGun : GunBase
{
    bool isOwner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
        StartCoroutine(FireContinuously2(bulletListPrefab[1]));


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
            float maxAttackSpeed = Calculate(GameManager.Instance.Character.Speed) - 0.2f;
            if(maxAttackSpeed < 12)
            {
                maxAttackSpeed = 12;
            }

            float randomDirection = Random.Range(0f, 90f);
            Vector3 upDirection = Quaternion.Euler(0, 0, randomDirection) * Vector2.right;
            Vector3 downDirection = Quaternion.Euler(0, 0, -randomDirection) * Vector2.right;


            //À§
            GhostBirdGunBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<GhostBirdGunBullet>();

            bullet.Delay -= (float)level/5f;

            if(bullet.Delay <= maxAttackSpeed)
            {
                bullet.Delay = maxAttackSpeed;
            }

            bullet.SetDirection(upDirection);
            bullet.Life = 100;
            bullet.Damage += (float)level;

            //¾Æ·¡
            bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<GhostBirdGunBullet>();

            bullet.Delay -= (float)level / 5f;
            if (bullet.Delay <= maxAttackSpeed)
            {
                bullet.Delay = maxAttackSpeed;
            }

            bullet.SetDirection(downDirection);
            bullet.Life = 100;
            bullet.Damage += (float)level;



            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


    IEnumerator FireContinuously2(GameObject bulletPrefab)
    {
        float originalDelay = bulletPrefab.GetComponent<Bullet>().Delay;
        while (!GameManager.Instance.Character.IsDie)
        {

            Bullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<Bullet>();


            bullet.Delay = originalDelay / Mathf.Pow(level+1, 0.5f);
            if (bullet.Delay <= 0.33f)
            {
                bullet.Delay = 0.33f;
            }

            bullet.Damage += (float)level / 5;  //0.1f;



            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }

            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

    public float Calculate(float x)
    {
        return -0.6222f * x + 7.1778f;
    }
}
