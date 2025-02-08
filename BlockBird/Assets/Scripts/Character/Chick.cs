using UnityEngine;

public class Chick : Character
{
    public void init(float chickDamage, float chickLife, float destroyTime)
    {
        Destroy(gameObject, destroyTime);
        foreach (GameObject gunPrefab in initGunPrefabList)
        {
            ChickGun gunBasePrefab = gunPrefab.GetComponent<ChickGun>();
            Instantiate(gunBasePrefab, transform).init(chickDamage, chickLife);
            //useGunList.Add(Instantiate(gunBasePrefab, transform).init(chickDamage));
            //useGunPrefabList.Add(gunBasePrefab);
        }
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
    }
}
