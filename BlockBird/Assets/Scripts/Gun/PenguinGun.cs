using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class PenguinGun : GunBase
{

    private int bulletCount = 0;
    public float bulletCountDebug = 0;
    public float freezeTimeDebug = 0;
    public float lifeDebug = 0;
    public float damageDebug = 0;

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
        bulletCountDebug = Mathf.Pow(level, 0.3f);
        bulletCount = (int)bulletCountDebug;
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        float missileGap = 0.3f;
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform);
            float[] yPositions = CalculateYPositions(bulletCount, missileGap);

            // 예시로 오브젝트를 배치하려면 여기에 Instantiate 추가
            for (int j = 0; j < yPositions.Length; j++)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + yPositions[j], transform.position.z);
                PenguinGunBullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity, GetBulletsTransform())
                    .GetComponent<PenguinGunBullet>();

                bullet.Speed -= (float)j / 1.8f;
                bullet.Damage += (float)level / 150f;
                bullet.Life += (int)((float)level / 100f);
                bullet.Size += (float)level / 200f;   //0.01f;
                bullet.FreezeTime = 1f + level / 50f;
                //bullet.SlowRate = 0.2f + (float)level / 600f;
                //if (bullet.SlowRate >= 9f)
                //{
                //    bullet.SlowRate = 0.9f;
                //}
                bullet.SlowRate = 0.3f;
            }


            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

    float[] CalculateYPositions(int i, float gap)
    {
        float[] yPositions = new float[i + 1];
        int mid = i / 2; // 중앙 인덱스

        for (int j = 0; j <= i; j++)
        {
            yPositions[j] = (j - mid) * gap;
        }

        return yPositions;
    }

}
