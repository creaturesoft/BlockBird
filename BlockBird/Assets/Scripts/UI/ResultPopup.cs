using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnRestart()
    {
        GameManager.IsRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClaim()
    {
    }
}
