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
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            float maxAttackSpeed = Calculate(GameManager.Instance.Character.Speed);

            float randomDirection = Random.Range(0f, 90f);
            Vector3 upDirection = Quaternion.Euler(0, 0, randomDirection) * Vector2.right;
            Vector3 downDirection = Quaternion.Euler(0, 0, -randomDirection) * Vector2.right;


            //À§
            GhostBirdGunBullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<GhostBirdGunBullet>();

            bullet.Delay -= (float)level/10f;

            if(bullet.Delay <= maxAttackSpeed)
            {
                bullet.Delay = maxAttackSpeed;
            }

            bullet.SetDirection(upDirection);

            //¾Æ·¡
            bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<GhostBirdGunBullet>();

            bullet.Delay -= (float)level / 10f;
            if (bullet.Delay <= maxAttackSpeed)
            {
                bullet.Delay = maxAttackSpeed;
            }

            bullet.SetDirection(downDirection);

            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


    IEnumerator FireContinuously2(GameObject bulletPrefab)
    {
        float originalDelay = bulletPrefab.GetComponent<Bullet>().Delay;
        while (!GameManager.Instance.Character.IsDie)
        {

            Bullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<Bullet>();


            bullet.Delay = originalDelay / Mathf.Pow(level+1, 0.5f);
            if (bullet.Delay <= 0.35f)
            {
                bullet.Delay = 0.35f;
            }

            bullet.Damage += (float)level / 10;  //0.1f;


            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

    public float Calculate(float x)
    {
        return -0.6222f * x + 7.1778f;
    }
}
