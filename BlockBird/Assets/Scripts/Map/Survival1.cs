using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRate
{
    public float weight;
    public float addWeight;
    public float initWeight;
}

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
        float lifeWeight = 10f;
        float spawnPositionX = 25f;

        int totalBlock = 0;

        while (GameManager.Instance.Character == null)
        {
            yield return new WaitForSeconds(0.2f);
        }



        RandomRate[] weightList = { 
              new RandomRate { weight = 1000, addWeight = 0f } //빈칸
            , new RandomRate { weight = 0, addWeight = 1f }  //흰색
            , new RandomRate { weight = 0, addWeight = 1f / 10f }   //주황색
            , new RandomRate { weight = 0, addWeight = 1f / 20f }    //보라색
            , new RandomRate { weight = 0, addWeight = 1f / 40f }     //자주색
            , new RandomRate { weight = 0, addWeight = 1f / 80f }       //빨간색
        };

        //초기화
        for (int i = 0; i < weightList.Length; i++)
        {
            weightList[i].initWeight = weightList[i].weight;
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
                lifeWeight += 0.005f;
                totalBlock++;

                //가중치 증가
                for (int i = 0; i < weightList.Length; i++)
                {
                    weightList[i].weight += weightList[i].addWeight;
                }

                //if (totalBlock > 500f)
                if(weightList[weightList.Length - 6].weight * 1f < weightList[weightList.Length - 5].weight)
                {
                    lifeWeight *= 2.0f;
                    //GameManager.Instance.Character.Speed *= 1.1f;
                    //totalBlock = 0;
                    //Debug.Log("init @@@@@@@@@@@@@@@@@@@@@@@@");
                    //Debug.Log("init @@@@@@@@@@@@@@@@@@@@@@@@");
                    //Debug.Log("init @@@@@@@@@@@@@@@@@@@@@@@@");

                    for (int i = 0; i < weightList.Length; i++)
                    {
                        weightList[i].weight = weightList[i].initWeight;
                    }
                }

                if (totalBlock % 50 == 0)
                {
                    //Debug.Log("totalBlock : " + totalBlock + " / lifeWeight : " + lifeWeight);

                    string weight = "";
                    for (int i = 0; i < weightList.Length; i++)
                    {
                        weight += weightList[i].weight + "/";
                    }

                    float remainWeight = weightList[weightList.Length - 6].weight * 1f - weightList[weightList.Length - 5].weight;
                    //Debug.Log("weight : " + weight + "("+ remainWeight.ToString() + ")");
                }


                while (currentDepth.transform.position.x > spawnPositionX)
                {
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }

                //아이템 확률
                float type = Random.Range(0f, 100f);
                if (type <= 5f)   //5%
                {
                    Instantiate(GameManager.Instance.itemPrefabList[0], new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .GetComponent<BulletItemBase>()
                        .Init(currentDepth.transform);

                    continue;
                }


                //블럭 확률
                float totalWeight = 0;
                foreach (RandomRate randomRate in weightList)
                {
                    totalWeight += randomRate.weight;
                }

                float randomValue = Random.Range(0, totalWeight);
                float currentWeight = 0;


                for(int i=0; i < weightList.Length; i++)
                {
                    currentWeight += weightList[i].weight;
                    if (randomValue < currentWeight)
                    {
                        if (i == 1) //흰색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                               .GetComponent<BlockBase>()
                               .Init(currentDepth.transform, Random.Range(lifeWeight / 50 < 1 ? 1 : lifeWeight / 50f, lifeWeight / 4f));

                        }
                        else if(i == 2) //주황색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .GetComponent<BlockBase>()
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 4f, lifeWeight / 3f), 1);
                        }
                        else if (i == 3) //보라색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .GetComponent<BlockBase>()
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 3f, lifeWeight / 2f), 2);
                        }
                        else if (i == 4) //자주색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .GetComponent<BlockBase>()
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 2f, lifeWeight / 1f), 3);
                        }
                        else if (i == 5) //빨간색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .GetComponent<BlockBase>()
                                .Init(currentDepth.transform, Random.Range(lifeWeight, lifeWeight * 1.5f), 4);
                        }

                        break;
                    }
                }




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
