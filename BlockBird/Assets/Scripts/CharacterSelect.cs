using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] CharactersPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(CharactersPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity).transform.localScale *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
