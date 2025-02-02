using System.Collections;
using UnityEngine;

public class GhostBirdGun : GunBase
{

    public int destroyRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyRate = 0;
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay;
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
    }

    public override void LevelUp()
    {
        base.LevelUp();

        destroyRate = (int)Mathf.Pow(level, 0.5f);
        if (delay >= 0.3f)
        {
            delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level, 0.7f);
        }


    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {

            GhostBirdGunBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<GhostBirdGunBullet>();

            
            bullet.DestroyRate -= destroyRate;  //0.1f;
            //bullet.Damage += (float)level / 50f;  //0.1f;
            bullet.Life = 2 + (int)((float)level / 50f);

            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
