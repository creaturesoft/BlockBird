using System.Collections;
using UnityEngine;

public class DragonBullet : Bullet
{
    public Transform gunTransform;
    public DragonBulletFX dragonBulletFXPrefab;
    private DragonBulletFX dragonBulletFX;

    protected override void Start()
    {
        base.Start();

        dragonBulletFX = Instantiate(dragonBulletFXPrefab, transform.position, Quaternion.identity, transform.parent);
        dragonBulletFX.init(Damage, Size, Delay);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }


    public void LevelUp(float damage, float delay)
    {
        dragonBulletFX.LevelUp(damage, delay);
    }

    protected override void FixedUpdate()
    {
        if (gunTransform == null)
        {
            return;
        }

        Life = 100;
        
        transform.position = new Vector3(
            gunTransform.position.x + 1f,
            gunTransform.position.y - 0.14f,
            gunTransform.position.z);

        if (dragonBulletFX != null)
        {
            dragonBulletFX.transform.position = transform.position;
        }
    }
}
