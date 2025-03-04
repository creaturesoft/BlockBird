using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class ChickGun : GunBase
{
    float chickDamage;
    float chickLife;

    static float attackSoundLastPlayTime = 0f;
    static float attackSoundPlayCooldown = 0.1f; // 0.1초 쿨타임 (필요에 따라 조절)

    public void init(float chickDamage, float chickLife, Character character)
    {
        base.init(character);
        this.chickDamage = chickDamage;
        this.chickLife = chickLife;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
    }

    public override void LevelUp()
    {
        base.LevelUp();
        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        //if (delay >= 0.3f)
        //{
        //    delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level, 0.7f);
        //    //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / level;
        //}
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;
            
            Bullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<Bullet>();

            bullet.Damage = chickDamage;
            bullet.Life = (int)chickLife;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;


            //defaultAudio?.PlayOneShot(defaultAudio.clip);

            if (Time.time - attackSoundLastPlayTime >= attackSoundPlayCooldown)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                attackSoundLastPlayTime = Time.time;
            }

            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
