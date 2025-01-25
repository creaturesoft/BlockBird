using System.Collections;
using UnityEngine;

public class Bullet2Gun : GunBase
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
    }

    public override void LevelUp()
    {
        base.LevelUp();
        StartCoroutine(FireContinuously(bulletListPrefab[0]));
    }

    IEnumerator FireContinuously(GameObject bulletPrefab)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        while (!GameManager.Instance.Character.IsDie)
        {
            Instantiate(bulletPrefab, transform);
            yield return new WaitForSeconds(bullet.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
