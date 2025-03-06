using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossHourglass : MapBase
{
    public BlockBase block1Prefab;
    public DepthBase depthBasePrefab;

    private List<BlockBase> blockList = new List<BlockBase>();

    IEnumerator CheckBlockCount()
    {
        while (true)
        {
            int nullCount = 0;
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i] == null)
                {
                    nullCount++;
                }
            }

            if (nullCount >= blockList.Count / 10)
            {
                break;
            }

            yield return new WaitForSeconds(2f);
        }

        GameManager.Instance.Character.Speed = 6f;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBlock());
    }



    IEnumerator SpawnBlock()
    {
        float lifeWeight = GameManager.Instance.Stage * Random.Range(1f, 20f - GameManager.Instance.Stage < 2.5f ? 2.5f : 20f - GameManager.Instance.Stage);
        if (lifeWeight < 1)
        {
            lifeWeight = 1;
        }

        while (GameManager.Instance.Character == null)
        {
            yield return new WaitForSeconds(0.2f);
        }



        float spawnPositionX = 25f;
        DepthBase currentDepth = null;
        float newDepthX;
        int totalDepth = -1;
        int blockPosition = -Steps;
        bool isUpFlow = false;
        bool isSetSpeed = false;
        int stepBlockCount = 12;

        float itemDropRate = (float)GameManager.Instance.Stage / 3f > 30f ? 30f : (float)GameManager.Instance.Stage / 3f;
        float bulletDropRate = (float)GameManager.Instance.Stage / 100f > 0.5f ? 0.5f : (float)GameManager.Instance.Stage / 100f;

        //StartCoroutine(CheckBlockCount());
        while (true)
        {
            totalDepth++;
            if (currentDepth == null)
            {
                newDepthX = spawnPositionX;
            }
            else
            {
                newDepthX = currentDepth.transform.position.x + BlockGap;
            }

            currentDepth = Instantiate(depthBasePrefab, new Vector3(newDepthX, 0, 0), Quaternion.identity, transform);

            int randomeWeak = Random.Range(-Steps, Steps + 1);
            for (int y = -Steps; y <= Steps; y++)
            {
                while (currentDepth.transform.position.x > spawnPositionX)
                {
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }

                //아이템 확률
                if (totalDepth < 50)
                {
                    float type = Random.Range(0f, 100f);

                    if (type <= itemDropRate)   //20%
                    {
                        Instantiate(GameManager.Instance.itemPrefabList[0], new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                            .GetComponent<BulletItemBase>()
                            .Init(currentDepth.transform);
                    }
                    else if (type <= itemDropRate + bulletDropRate)
                    {
                        Instantiate(GameManager.Instance.itemPrefabList[Random.Range(1, GameManager.Instance.itemPrefabList.Length)], new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                            .GetComponent<BulletItemBase>()
                            .Init(currentDepth.transform);
                    }

                    continue;
                }
                else
                {
                    if (!isSetSpeed)
                    {
                        isSetSpeed = true;
                        GameManager.Instance.Character.Speed = GameManager.Instance.Character.IsMeleeAttack ? 2f : 2.5f;
                    }
                }


                for(int i=0; i< stepBlockCount; i++)
                {
                    if (totalDepth == 50 + (i))
                    {
                        if (y == -1 || y == -2)
                        {
                            blockList.Add(
                                Instantiate(block1Prefab, new Vector3(0, BlockGap * y, 0), Quaternion.identity)
                                    .Init(currentDepth.transform, Random.Range(lifeWeight * 4f, lifeWeight * 5f), 5, true));
                        }
                    }
                }


                if (totalDepth == 50 + stepBlockCount || totalDepth == 56 + stepBlockCount)
                {
                    if (y == -3 || y == 3)
                    {
                        MakeBlock(y, currentDepth, lifeWeight);
                    }
                }
                else if (totalDepth == 51 + stepBlockCount || totalDepth == 55 + stepBlockCount)
                {
                    if (y == -3 || y == -2 || y == 2 || y == 3)
                    {
                        MakeBlock(y, currentDepth, lifeWeight);
                    }
                }
                else if (totalDepth == 52 + stepBlockCount || totalDepth == 54 + stepBlockCount)
                {
                    if (y == -3 || y == -2 || y == -1 || y == 1 || y == 2 || y == 3)
                    {
                        MakeBlock(y, currentDepth, lifeWeight);
                    }
                }
                else if (totalDepth == 53 + stepBlockCount)
                {
                    if (y == -3 || y == -2 || y == -1 || y == 0 || y == 1 || y == 2 || y == 3)
                    {
                        MakeBlock(y, currentDepth, lifeWeight);
                    }
                }
            }


            if (totalDepth > 56 + stepBlockCount)
            {
                break;
            }
        }


        //빈공간
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

    }

    void MakeBlock(int step, DepthBase currentDepth, float lifeWeight)
    {
        if (step == -3 || step == 3)
        {
            blockList.Add(
                Instantiate(block1Prefab, new Vector3(0, BlockGap * step, 0), Quaternion.identity)
                .Init(currentDepth.transform, Random.Range(lifeWeight * 2.5f, lifeWeight * 3.5f), 1, true));
        }
        else if (step == -2 || step == 2)
        {
            blockList.Add(
            Instantiate(block1Prefab, new Vector3(0, BlockGap * step, 0), Quaternion.identity)
                .Init(currentDepth.transform, Random.Range(lifeWeight * 3f, lifeWeight * 4f), 2, true));
        }
        else if (step == -1 || step == 1)
        {
            blockList.Add(
            Instantiate(block1Prefab, new Vector3(0, BlockGap * step, 0), Quaternion.identity)
                .Init(currentDepth.transform, Random.Range(lifeWeight * 3.5f, lifeWeight * 4.5f), 3, true));
        }
        else
        {
            blockList.Add(
            Instantiate(block1Prefab, new Vector3(0, BlockGap * step, 0), Quaternion.identity)
                .Init(currentDepth.transform, Random.Range(lifeWeight * 4f, lifeWeight * 5f), 5, true));
        }
    }
}
