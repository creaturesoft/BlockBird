using System.Linq;
using UnityEngine;

public class BulletItemBase : ItemBase
{
    public GameObject[] gunPrefabList;

    public override void TakeItem(Character character)
    {
        character.TakeBulletItem(gunPrefabList.Select(gunPrefab => gunPrefab.GetComponent<GunBase>()).ToArray());
    }
}
