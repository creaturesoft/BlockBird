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

        if (dragonBulletFX == null)
        {
            dragonBulletFX = Instantiate(dragonBulletFXPrefab, transform.position, Quaternion.identity, transform.parent);
            dragonBulletFX.init(Damage, Size, Delay);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }


    public void LevelUp(float damage, float delay)
    {
        if (dragonBulletFX == null)
        {
            dragonBulletFX = Instantiate(dragonBulletFXPrefab, transform.position, Quaternion.identity, transform.parent);
            dragonBulletFX.init(damage, Size, delay);
        }
        else
        {
            dragonBulletFX.LevelUp(damage, delay);
        }
    }

    protected override void FixedUpdate()
    {
        if (gunTransform == null)
        {
            return;
        }

        Life = 100;
        
        transform.position = new Vector3(
            gunTransform.transform.position.x + 1f,
            gunTransform.transform.position.y - 0.14f,
            gunTransform.transform.position.z);

        if (dragonBulletFX != null)
        {
            dragonBulletFX.transform.position = transform.position;
        }
    }

    private void OnDestroy()
    {
        if (dragonBulletFX != null)
        {
            Destroy(dragonBulletFX.gameObject);
        }
    }
}
