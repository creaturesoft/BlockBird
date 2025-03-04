using System.Collections;
using UnityEngine;

public class RandomRate
{
    public string name;
    public float weight;
    public float addWeight;
    public float initWeight;
}

public class Survival1 : MapBase
{
    public BlockBase block1Prefab;

    public DepthBase depthBasePrefab;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBlock());
    }


    IEnumerator SpawnBlock()
    {
        //GameManager.Instance.stage = 100;
        //float hard = Random.Range(1f, 3.5f);
        //float hard = Random.Range(10f, 10f);
        float hard = Random.Range(1f, 50f - GameManager.Instance.Stage < 10f ? 10f : 50 - GameManager.Instance.Stage);

        GameManager.Instance.GoalScore = 100 * GameManager.Instance.Stage * 1;
        //float lifeWeight = 10f + GameManager.Instance.stage / 10f;
        float lifeWeight = 10f + (float)GameManager.Instance.Stage / 5f * hard;
        //float addLifeWeight = 0.01f + (float)GameManager.Instance.stage / 20000f;
        float addLifeWeight = 0.01f + (float)GameManager.Instance.Stage / 10000f * hard;

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
        DepthBase currentDepth = null;
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
                //lifeWeight += 0.005f;
                lifeWeight += addLifeWeight;

                totalBlock++;

                //가중치 증가
                for (int i = 0; i < weightList.Length; i++)
                {
                    weightList[i].weight += weightList[i].addWeight;
                }

                //if (totalBlock > 500f)
                if(weightList[weightList.Length - 6].weight * 1f < weightList[weightList.Length - 5].weight)
                {
                    if(GameManager.Instance.Score > GameManager.Instance.GoalScore)
                    {
                        break;
                    }


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
                               .Init(currentDepth.transform, Random.Range(lifeWeight / 50 < 1 ? 1 : lifeWeight / 50f, lifeWeight / 4f));

                        }
                        else if(i == 2) //주황색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 4f, lifeWeight / 3f), 1);
                        }
                        else if (i == 3) //보라색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 3f, lifeWeight / 2f), 2);
                        }
                        else if (i == 4) //자주색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .Init(currentDepth.transform, Random.Range(lifeWeight / 2f, lifeWeight / 1f), 3);
                        }
                        else if (i == 5) //빨간색
                        {
                            Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                .Init(currentDepth.transform, Random.Range(lifeWeight, lifeWeight * 1.5f), 4);
                        }

                        break;
                    }
                }


            }


            if (GameManager.Instance.Score > GameManager.Instance.GoalScore)
            {
                break;
            }
        }


        CurrentLifeWeight = (int)(lifeWeight / hard);


        for (int i = 0; i < 15; i++)
        {
            newDepthX = currentDepth.transform.position.x + BlockGap;
            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            while (currentDepth.transform.position.x > spawnPositionX)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        int bossLine = 2;
        BlockBase[][] bossBlock = new BlockBase[bossLine][];
        for (int i = 0; i < bossLine; i++)
        {
            bossBlock[i] = new BlockBase[Steps * 2 + 1];
        }


        for (int i = 0; i < bossLine; i++)
        {
            newDepthX = currentDepth.transform.position.x + BlockGap;
            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            for (int y = -Steps; y <= Steps; y++)
            {
                if (y == -Steps)   //맨아래
                {
                    bossBlock[i][y+Steps] =
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .Init(currentDepth.transform, Random.Range(lifeWeight * 4f, lifeWeight * 7f), 5, true);
                }
                else if (y == Steps)         //맨위
                {

                    bossBlock[i][y+Steps] =
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .Init(currentDepth.transform, Random.Range(lifeWeight * 3f, lifeWeight * 6f), 5, true);
                }
                else
                {

                    bossBlock[i][y+Steps] =
                    Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                        .Init(currentDepth.transform, Random.Range(lifeWeight * 1f, lifeWeight * 4f), 5, true);
                }

            }

            while (currentDepth.transform.position.x > spawnPositionX)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }


        for (int i = 0; i < 3; i++)
        {
            newDepthX = currentDepth.transform.position.x + BlockGap;
            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            while (currentDepth.transform.position.x > spawnPositionX)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        //성공 영역
        newDepthX = currentDepth.transform.position.x + BlockGap;
        currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);
        currentDepth.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        currentDepth.transform.localScale = new Vector3(5, 20, 1);
        //currentDepth.AddComponent<SpriteRenderer>().color = new Color(100,100,100);

        GameManager.Instance.Character.Speed = GameManager.Instance.Character.IsMeleeAttack ? 2.0f : 2.5f;

        for (int i = 0; i < bossBlock[0].Length; i++)
        {
            bossBlock[0][i].BossBlockCheckList.Add(bossBlock[1][i]);
            bossBlock[1][i].BossBlockCheckList.Add(bossBlock[0][i]);
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
