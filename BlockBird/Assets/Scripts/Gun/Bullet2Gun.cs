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
        while (!GameManager.Instance.Character.IsDie)
        {
            //Instantiate(bulletPrefab, transform);
            float[] yPositions = CalculateYPositions(bulletCount, missileGap);

            // 예시로 오브젝트를 배치하려면 여기에 Instantiate 추가
            for (int j = 0; j < yPositions.Length; j++)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + yPositions[j], transform.position.z);
                Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                                    .GetComponent<Bullet>();
                bullet.Damage += (float)level / 20f;
                bullet.Life += (int)((float)level / 50f);
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


    // Update is called once per frame
    void Update()
    {

    }
}
