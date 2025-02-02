using System.Collections;
using UnityEngine;

public class PenguinGunBullet : Bullet
{

    public PenguinGunBulletFX bulletFXPrefab;

    private Vector3 fixedWorldPosition;
    private float freezeTime = 1;
    public float FreezeTime
    {
        get { return freezeTime; }
        set { freezeTime = value; }
    }

    private float slowRate = 1;
    public float SlowRate
    {
        get { return slowRate; }
        set { slowRate = value; }
    }

    public float rotationSpeed = 100f; // 초당 회전 속도

    protected override float SpecialEffect(BlockBase block)
    {
        Instantiate(bulletFXPrefab, transform.position, Quaternion.identity, transform.parent).init(Damage, Size, FreezeTime, SlowRate);
        //block.Freeze(FreezeTime, SlowRate);
        return 0;
    }

    protected override void FixedUpdate()
    {
        //주위를 돔
        transform.Rotate(0f, 0f, rotationSpeed * Speed * Time.fixedDeltaTime);

        // Kinematic Rigidbody를 이동
        rb.MovePosition(rb.position + (Vector2)(transform.parent.right * Speed * Time.fixedDeltaTime));
    }
}
