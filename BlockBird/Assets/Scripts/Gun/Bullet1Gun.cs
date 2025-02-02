using System.Collections;
using UnityEngine;

public class Bullet1Gun : GunBase
{
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
        if (delay >= 0.3f)
        {
            delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level, 0.7f);
            //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / level;
        }
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;
            
            Bullet bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .GetComponent<Bullet>();

            bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
