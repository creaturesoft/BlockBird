using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RealtimeAnimation : MonoBehaviour
{
    public Animation animationComponent;
    public string animationName;
    private float lastTime;

    void Start()
    {
    }
    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastTime;
        if (animationComponent[animationName].enabled)
        {
            animationComponent[animationName].time += deltaTime; // Realtime ������Ʈ
            animationComponent.Sample(); // �ִϸ��̼� ������ ������Ʈ
        }
        lastTime = Time.realtimeSinceStartup;
    }
}
