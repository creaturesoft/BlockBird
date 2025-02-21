using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FriendsItem : ItemBase
{

    public override void TakeItem(Character character)
    {
        character.TakeFriendsItem();
    }

}
