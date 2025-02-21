using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ChickenGun : GunBase
{
    public Egg eggPrefab;
    private float eggHatchDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = 10f;
        StartCoroutine(FireContinuously());

        for (int i = 1; i < characterLevel; i++)
        {
            LevelUp();
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    IEnumerator FireContinuously()
    {
        bool isFriend = (character == null) ? false : character.isFriend;

        while (!GameManager.Instance.Character.IsDie)
        {
            eggHatchDelay = 50f - level / 10f;
            if (eggHatchDelay < 10)
            {
                eggHatchDelay = 10;
            }

            //bullet.Damage = level;
            Instantiate(eggPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform())
                .init(eggHatchDelay, (float)(level+1) / 12f, (float)level / 100f, isFriend);

            //bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;
            yield return new WaitForSeconds(delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
