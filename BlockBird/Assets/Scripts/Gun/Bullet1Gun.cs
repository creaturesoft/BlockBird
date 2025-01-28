using System.Collections;
using UnityEngine;

public class Bullet1Gun : GunBase
{
    public float delay;

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
        delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level + 1, 0.7f);
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;
            
            Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform);

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
