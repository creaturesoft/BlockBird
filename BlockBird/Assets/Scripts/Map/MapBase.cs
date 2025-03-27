using UnityEngine;

public class MapBase : MonoBehaviour
{
    public float BlockGap { get; set; } = 1.15f;
    
    public int Steps { get; set; } = 3;

    protected virtual void Start()
    {
    }

    public static int CurrentLifeWeight { get; set; }
}
