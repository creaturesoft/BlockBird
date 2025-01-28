using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class PenguinGun : GunBase
{
    private int bulletCount = 0;
    public float bulletCountDebug = 0;
    public float freezeTimeDebug = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
    }

    public override void LevelUp()
    {
        base.LevelUp();
        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        bulletCountDebug = Mathf.Pow(level, 0.4f);
        bulletCount = (int)bulletCountDebug;

        //freezeTimeDebug = 10 + Mathf.Pow(level, 0.4f);
        freezeTimeDebug = 1f + level/10f;
        bulletListPrefab[0].GetComponent<PenguinGunBullet>().freezeTime = freezeTimeDebug;
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();

        float missileGap = 0.3f;
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform);
            float[] yPositions = CalculateYPositions(bulletCount, missileGap);

            // 예시로 오브젝트를 배치하려면 여기에 Instantiate 추가
            for (int j = 0; j < yPositions.Length; j++)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + yPositions[j], transform.position.z);
                Instantiate(bulletPrefab, position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform);
            }
            
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
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
