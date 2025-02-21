using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class GhostBirdGunBullet : Bullet
{
    public NoColliderBulletFX effectPrefab;

    //private float amplitude = 0.03f; // 물결의 진폭 (높이)
    private int destroyRate = 1;
    public int DestroyRate
    {
        get { return destroyRate; }
        set { destroyRate = value; }
    }


    private float rotationSpeed = 50f; // 초당 회전 속도

    private Vector3 direction;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    protected override float SpecialEffect(BlockBase block)
    {
        Instantiate(effectPrefab, transform.position, Quaternion.identity, transform.parent).init(Size);

        if(block.IsBoss)
        {
            return block.Life / 2;
        }
        else
        {
            return block.Life;
        }

        //if (destroyRate < 2)
        //{
        //    destroyRate = 2;
        //}

        //if (Random.Range(0, destroyRate) == 0)
        //{
        //    Instantiate(effectPrefab, transform.position, Quaternion.identity, transform.parent).init(Size);
        //    return block.Life/2;
        //}
        //else
        //{
        //    return 1;
        //}
    }


    protected override void FixedUpdate()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Speed * Time.fixedDeltaTime);

        rb.MovePosition(rb.position
            + (Vector2)((transform.parent.right + direction) * Speed * Time.fixedDeltaTime));
    }

    //protected override void FixedUpdate()
    //{

    //    // 시간에 따라 x축 물결 이동
    //    float wave = Mathf.Sin((Time.time - startTime) * Speed + Time.time) * amplitude;

    //    transform.Rotate(0f, 0f, rotationSpeed * Speed * Time.fixedDeltaTime);

    //    rb.MovePosition(rb.position
    //        + (Vector2)(transform.parent.right * Speed * Time.fixedDeltaTime)
    //        + new Vector2(0f, wave));
    //}
}
