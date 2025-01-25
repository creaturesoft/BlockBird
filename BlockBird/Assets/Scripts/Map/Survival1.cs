using System.Collections;
using UnityEngine;

public class Survival1 : MapBase
{
    public GameObject block1Prefab;

    public GameObject depthBasePrefab;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBlock());
    }

    IEnumerator SpawnBlock()
    {
        int totalDepth = 10;
        float spawnPositionX = 25f;

        while (GameManager.Instance.Character == null)
        {
            yield return new WaitForSeconds(0.2f);
        }


        //테스트
        GameObject currentDepth = null;
        float newDepthX;
        while (true)
        {

            if(currentDepth == null)
            {
                newDepthX = spawnPositionX;
            }
            else
            {
                newDepthX = currentDepth.transform.position.x + BlockGap;
            }

            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            for (int y = -Steps; y <= Steps; y++)
            {
                float type = Random.Range(0f, 100f);

                if (type <= 5f)   //5%
                {
                    Instantiate(GameManager.Instance.itemPrefabList[0], new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BulletItemBase>()
                        .Init(currentDepth.transform);
                }
                else if (type <= Mathf.Max(100 - totalDepth / 1.5f, 50)) //45% 빈공간
                {

                }
                else if (type <= Mathf.Max(100 - totalDepth / 10f, 83)) //33% 흰색
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                       .GetComponent<BlockBase>()
                       .Init(currentDepth.transform, Random.Range(totalDepth/50 < 1 ? 1 : totalDepth/50, totalDepth/2));
                }
                else if (type <= Mathf.Max(100 - totalDepth / 60f, 91)) //8% 주황색
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BlockBase>()
                        .Init(currentDepth.transform, Random.Range(totalDepth/3, totalDepth/2), 1);    
                }
                else if (type <= Mathf.Max(100 - totalDepth / 160f, 96)) //5% 보라색
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BlockBase>()
                        .Init(currentDepth.transform, Random.Range(totalDepth/2, totalDepth/1.5f), 2);
                }
                else if (type <= Mathf.Max(100 - totalDepth / 310f, 99)) //3% 자주색
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BlockBase>()
                        .Init(currentDepth.transform, Random.Range(totalDepth/1.5f, totalDepth), 3);
                }
                else if (type <= Mathf.Max(100 - totalDepth / 510f, 100)) //1% 빨간색
                {
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BlockBase>()
                        .Init(currentDepth.transform, Random.Range(totalDepth, totalDepth*1.5f), 4);
                }
            }

            totalDepth++;

            while (currentDepth.transform.position.x > spawnPositionX)
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
