using System.Collections;
using UnityEngine;

public class Bullet1Gun : GunBase
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
            
            Bullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<Bullet>();


            bullet.Damage += (float)level / 8f;  //0.1f;

            bullet.Delay = bullet.Delay / Mathf.Pow(level + 1, 0.7f);

            if (bullet.Delay <= 0.3f)
            {
                bullet.Delay = 0.3f;
            }


            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;

            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);

#if UNITY_EDITOR
            //Debug.Log("level: " + level + " / dps: " + bullet.Damage * 1 / (bullet.Delay / GameManager.Instance.Character.AttackSpeed));
#endif
        }
    }


}
