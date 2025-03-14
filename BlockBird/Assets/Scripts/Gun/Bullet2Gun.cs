using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class Bullet2Gun : GunBase
{
    private int bulletCount = 0;
    public float bulletCountDebug = 0;

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
        bulletCountDebug = Mathf.Pow(level, 0.5f);
        bulletCount = (int)bulletCountDebug;
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        float missileGap = 0.25f;
        Bullet bullet = null;
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform);
            float[] yPositions = CalculateYPositions(bulletCount, missileGap);

            // 예시로 오브젝트를 배치하려면 여기에 Instantiate 추가
            for (int j = 0; j < yPositions.Length; j++)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + yPositions[j], transform.position.z);
                bullet = Instantiate(bulletPrefab, position, Quaternion.identity, GetBulletsTransform())
                                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 7f;
                bullet.Life += (int)((float)level / 40f);
            }


            if (Time.time - GameManager.attackSoundLastPlayTime >= GameManager.attackSoundPlayCooldown && defaultAudio != null && SoundManager.Instance.isSFXOn)
            {
                defaultAudio?.PlayOneShot(defaultAudio.clip);
                GameManager.attackSoundLastPlayTime = Time.time;
            }
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);

#if UNITY_EDITOR
            //Debug.Log("level: " + level + " / dps: " + yPositions.Length * bullet.Damage * 1 / (delay / GameManager.Instance.Character.AttackSpeed));
#endif
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


    // Update is called once per frame
    void Update()
    {

    }
}
