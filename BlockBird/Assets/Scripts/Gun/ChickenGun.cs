using System.Collections;
using System.Drawing;
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
            eggHatchDelay = 40f - (float)level / 10f;
            if (eggHatchDelay < 20)
            {
                eggHatchDelay = 20;
            }

            //bullet.Damage = level;
            Egg egg = Instantiate(eggPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, GetBulletsTransform());
            egg.init(eggHatchDelay, (float)(level+1) / 22f, (float)level / 100f, isFriend);
            
            egg.Damage += (float)level * 1.2f;
            egg.Size += (float)level / 100f;
            if(egg.Size > 1f)
            {
                egg.Size = 1f;
            }

            egg.Delay -= level / 100f;

            if (egg.Delay <= 3f)
            {
                egg.Delay = 3f;
            }

            //bullet.Damage += (float)level/10f;  //0.1f;

            //delay = bullet.Delay / (level/2) / GameManager.Instance.Character.AttackSpeed;


            //if (GameManager.Instance.Character.useGunList[0] == this)
            //{
            //    defaultAudio?.PlayOneShot(defaultAudio.clip);
            //}
            yield return new WaitForSeconds(egg.Delay / GameManager.Instance.Character.AttackSpeed);
        }
    }

}
