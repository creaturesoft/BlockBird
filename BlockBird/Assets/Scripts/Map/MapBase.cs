using UnityEngine;

public class MapBase : MonoBehaviour
{
    public float BlockGap { get; set; } = 1.2f;
    
    public int Steps { get; set; } = 3;

    protected virtual void Start()
    {
    }

    void FixedUpdate()
    {
        if (GameManager.IsPaused) return;
        if (GameManager.Instance.Character != null)
        {
            transform.position += Vector3.left * Time.fixedDeltaTime * GameManager.Instance.Character.Speed;
        }
    }
}
