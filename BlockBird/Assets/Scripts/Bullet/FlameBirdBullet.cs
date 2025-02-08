using System.Collections;
using UnityEngine;

public class FlameBirdBullet : Bullet
{
    private float goalPositionX = 5f; // 물결의 진폭 (높이)

    private float destroyTime;
    public float DestroyTime
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }

    bool isSpecialEffect = true;

    protected override void Start()
    {
        base.Start();

        //transform.localScale = new Vector3(Size, Size, 1);
        transform.localScale = new Vector3(0.3f, 0.3f, 1);

        goalPositionX = Random.Range(1f, 12f);


        StartCoroutine(Explosion());
    }


    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }

    protected override float SpecialEffect(BlockBase block)
    {
        if (isSpecialEffect)
        {
            transform.localScale = new Vector3(Size, Size, 1);
            isSpecialEffect = false;
        }

        return Damage;
    }

    protected override void FixedUpdate()
    {

        Life = 100;
        if (isSpecialEffect)
        {
            base.FixedUpdate();
        }


        //if (goalPositionX > transform.position.x)
        //{
        //    base.FixedUpdate();
        //}
    }
}
