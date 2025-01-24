using System.Collections;
using UnityEngine;

public class Survival1 : MapBase
{
    public GameObject block1Prefab;
    public GameObject bullet2ItemPrefab;

    public GameObject depthBasePrefab;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBlock());
    }

    IEnumerator SpawnBlock()
    {
        int totalDepth = 10;

        while (GameManager.Instance.Character == null)
        {
            yield return new WaitForSeconds(0.2f);
        }


        //Å×½ºÆ®
        GameObject currentDepth = null;
        float newDepthX;
        while (true)
        {

            if(currentDepth == null)
            {
                newDepthX = 25f;
            }
            else
            {
                newDepthX = currentDepth.transform.position.x + BlockGap;
            }

            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            for (int y = -Steps; y <= Steps; y++)
            {
                int type = Random.Range(0, 100);

                if (type < 5)
                {
                    Instantiate(bullet2ItemPrefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BulletItemBase>()
                        .Init(currentDepth);
                }
                else if (type < 60)
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                       .GetComponent<BlockBase>()
                       .Init(currentDepth, Random.Range(1, totalDepth / 2));
                }
                else
                {

                }
            }

            totalDepth++;

            while(currentDepth.transform.position.x > 25f)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }


        //for (int x = 0; x < 1; x++)
        //{
        //    for (int y = -Steps; y <= Steps; y++)
        //    {
        //        Instantiate(bullet2ItemPrefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
        //            .GetComponent<BulletItemBase>()
        //            .Init(transform);
        //    }

        //    totalDepth++;

        //    yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        //}
        

        ////[1][1][1][1][1][1][1]
        ////[1][1][1][1][1][1][1]
        ////[1][1][1][1][1][1][1]
        ////[1][1][1][1][1][1][1]
        ////[1][1][1][1][1][1][1]
        ////[1][1][1][1][1][1][1]

        //for (int x = 0; x < 6; x++)
        //{
        //    for (int y = -Steps; y <= Steps; y++)
        //    {
        //        Instantiate(block1Prefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
        //            .GetComponent<BlockBase>()
        //            .Init(transform, 12f);
        //    }

        //    totalDepth++;

        //    yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        //}


        ////[2][2][2][2][2][2][2]
        ////[2][2][2][2][2][2][2]
        ////[2][2][2][2][2][2][2]
        ////[2][2][2][2][2][2][2]
        ////[2][2][2][2][2][2][2]
        ////[2][2][2][2][2][2][2]

        //for (int x = 0; x < 6; x++)
        //{
        //    for (int y = -Steps; y <= Steps; y++)
        //    {
        //        Instantiate(block1Prefab, new Vector3(BlockGap * totalDepth, BlockGap * y, 0), Quaternion.identity)
        //            .GetComponent<BlockBase>()
        //            .Init(transform, 2f);
        //    }

        //    totalDepth++;

        //    yield return new WaitForSeconds((delay * Time.fixedDeltaTime) / GameManager.Instance.Character.Speed);
        //}

    }
}
