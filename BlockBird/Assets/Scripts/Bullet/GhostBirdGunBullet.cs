using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class GhostBirdGunBullet : Bullet
{
    public NoColliderBulletFX effectPrefab;


    BlockBase minBlock = null;

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
        if (minBlock == block)
        {
            Life = 0;
            Instantiate(effectPrefab, transform.position, Quaternion.identity, transform.parent).init(Size);


            //if (block.Life <= 1)
            //{
            //    return 1;
            //}
            //else
            //{
            //    return block.Life - 1;
            //}

            if (block.IsBoss)
            {
                return Damage > block.Life / 2 ? Damage : block.Life / 2;
            }
            else
            {

                if (block.Life <= 1)
                {
                    return 1;
                }
                else
                {
                    return block.Life - 1;
                }
            }

            //if (block.IsBoss)
            //{
            //    if (block.Life <= 1)
            //    {
            //        return 1;
            //    }
            //    else
            //    {
            //        return block.Life - 1;
            //    }
            //}
            //else
            //{
            //    return block.Life;
            //}

        }
        else
        {
            return 0;
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

    float elapseTime = 0;

    protected override void FixedUpdate()
    {
        elapseTime += Time.fixedDeltaTime;
        if (elapseTime > 0.8f)
        {
            elapseTime = 0;

            GameObject map = null;
            foreach (Transform child in GameManager.Instance.map.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    map = child.gameObject;
                    break;
                }
            }

            float minLife = int.MaxValue;
            foreach (Transform depth in map.transform)
            {
                if (depth.position.x > GameManager.Instance.Character.transform.position.x
                    && depth.position.x < 20)
                {
                    foreach (Transform block in depth.transform)
                    {
                        if (block.gameObject.layer == GameManager.Instance.blockLayer)
                        {
                            BlockBase checkBlock = block.gameObject.GetComponent<BlockBase>();
                            if (minLife > checkBlock.Life)
                            {
                                minLife = checkBlock.Life;
                                minBlock = checkBlock;
                            }
                        }
                    }
                }
            }
        }

        transform.Rotate(0f, 0f, rotationSpeed * Speed * Time.fixedDeltaTime);

        if (minBlock == null)
        {
            rb.MovePosition(rb.position
                + (Vector2)((transform.parent.right + direction) * Speed * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, minBlock.transform.position, 5 * Speed * Time.fixedDeltaTime));
            //rb.MovePosition(rb.position + (Vector2)(minBlock.transform.position.normalized * Speed * Time.fixedDeltaTime));
        }
    }


}
