using System.Collections;
using UnityEngine;

public class DragonBullet : Bullet
{

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
        Life = 100;
        
        transform.position = new Vector3(
            GameManager.Instance.Character.transform.position.x + 1f,
            GameManager.Instance.Character.transform.position.y - 0.14f,
            GameManager.Instance.Character.transform.position.z);

        if (dragonBulletFX != null)
        {
            dragonBulletFX.transform.position = transform.position;
        }
    }
}
