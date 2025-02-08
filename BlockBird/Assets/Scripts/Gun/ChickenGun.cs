using System.Collections;
using UnityEngine;

public class ChickenGun : GunBase
{
    public Egg eggPrefab;
    private float eggHatchDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = 10f;
        eggHatchDelay = 100f;
        StartCoroutine(FireContinuously());
    }

    public override void LevelUp()
    {
        base.LevelUp();
        //StartCoroutine(FireContinuously(bulletListPrefab[0]));
        //if (delay >= 0.3f)
        //{
        //    delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / Mathf.Pow(level, 0.7f);
        //    //delay = bulletListPrefab[0].GetComponent<Bullet>().Delay / level;
        //}
    }

    IEnumerator FireContinuously()
    {
        while (!GameManager.Instance.Character.IsDie)
        {
            //bullet.Damage = level;
            Instantiate(eggPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GameManager.Instance.bulletGameObject.transform)
                .init(eggHatchDelay, (float)(level+1) / 12f, (float)level / 100f);

            //bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
