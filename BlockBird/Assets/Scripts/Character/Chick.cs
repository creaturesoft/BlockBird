using UnityEngine;

public class Chick : Character
{
    public void init(float chickDamage, float chickLife, float destroyTime, bool isFriend = false)
    {
        this.isFriend = isFriend;
        this.Level = 1;

        Destroy(gameObject, destroyTime);
        foreach (GameObject gunPrefab in initGunPrefabList)
        {
            ChickGun gunBasePrefab = gunPrefab.GetComponent<ChickGun>();
            Instantiate(gunBasePrefab, transform).init(chickDamage, chickLife, this);
            //useGunList.Add(Instantiate(gunBasePrefab, transform).init(chickDamage));
            //useGunPrefabList.Add(gunBasePrefab);
        }
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
    }
}
