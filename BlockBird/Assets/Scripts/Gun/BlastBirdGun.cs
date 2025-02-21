using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlastBirdGun : GunBase
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
        //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level + 1, 0.7f);

        
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;

            BlastBirdGunBullet blastBirdGunBullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .GetComponent<BlastBirdGunBullet>();

            blastBirdGunBullet.Damage += (float)level/3f;  //0.1f;
            blastBirdGunBullet.Size += (float)level/100f;   //0.01f;
            blastBirdGunBullet.Life += (int)(level/30f);

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
