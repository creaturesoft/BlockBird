using System.Collections;
using UnityEngine;

public class Survival1 : MapBase
{
    public GameObject block1Prefab;
    public GameObject bullet2ItemPrefab;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBlock());
    }

    IEnumerator SpawnBlock()
    {
        int totalDepth = 16;
        int delay = 50;

        while (GameManager.Instance.Character == null)
        {
            yield return new WaitForSeconds(0.2f);
        }


        for (int x = 0; x < 1; x++)
        {
            for (int y = -Steps; y <= Steps; y++)
            {
                Instantiate(bullet2ItemPrefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
                    .GetComponent<BulletItemBase>()
                    .Init(transform);
            }

            totalDepth++;

            while (GameManager.IsPaused) yield return null;
            yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        }
        

        //[1][1][1][1][1][1][1]
        //[1][1][1][1][1][1][1]
        //[1][1][1][1][1][1][1]
        //[1][1][1][1][1][1][1]
        //[1][1][1][1][1][1][1]
        //[1][1][1][1][1][1][1]

        for (int x = 0; x < 6; x++)
        {
            for (int y = -Steps; y <= Steps; y++)
            {
                Instantiate(block1Prefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
                    .GetComponent<BlockBase>()
                    .Init(transform, 12f);
            }

            totalDepth++;

            while (GameManager.IsPaused) yield return null;
            yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        }


        //[2][2][2][2][2][2][2]
        //[2][2][2][2][2][2][2]
        //[2][2][2][2][2][2][2]
        //[2][2][2][2][2][2][2]
        //[2][2][2][2][2][2][2]
        //[2][2][2][2][2][2][2]

        for (int x = 0; x < 6; x++)
        {
            for (int y = -Steps; y <= Steps; y++)
            {
                Instantiate(block1Prefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
                    .GetComponent<BlockBase>()
                    .Init(transform, 2f);
            }

            totalDepth++;

            while (GameManager.IsPaused) yield return null;
            yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        }

    }
}
