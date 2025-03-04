using System;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GameObject[] bulletListPrefab;
    
    public AudioSource defaultAudio;

    public int level;
    public int characterLevel;
    public bool isLastGun;
    public Type prefabType;
    protected float delay;
    
    public Character character;

    public GunBase init(int characterLevel)
    {
        isLastGun = true;
        prefabType = this.GetType();
        this.characterLevel = characterLevel;
        return this;
    }

    public GunBase init(Character character)
    {
        init(character.ExpLevel + character.PullLevel);
        this.character = character;
        return this;
    }

    public virtual void LevelUp()
    {
        level++;
    }

    public Transform GetBulletsTransform()
    {
        if(character != null && character.isFriend)
        {
            return GameManager.Instance.friendBulletGameObject.transform;
        }
        return GameManager.Instance.bulletGameObject.transform;
    }
}
