using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] CharactersPrefabs;
    public static int SelectedCharacter;
    GameObject selectedPlayer;
    public StartButton startbutton;

    public Slider hpSlider;
    public Slider speedSlider;
    public Slider attckSpeedSlider;
    public Slider jumpSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.IsRestart)
        {
            SetCurrentCharacter(SelectedCharacter);
            startbutton.OnStart();
            GameManager.IsRestart = false;
        }
        else
        {
            SelectedCharacter = 1;
            SetCurrentCharacter(SelectedCharacter);
        }
    }

    void SetCurrentCharacter(int select)
    {
        if(selectedPlayer != null)
        {
            Destroy(selectedPlayer);
        }

        selectedPlayer = Instantiate(CharactersPrefabs[select], new Vector3(0, 0, 0), Quaternion.identity);
        selectedPlayer.transform.localScale *= 2;
        selectedPlayer.transform.position = new Vector3(0, 0.5f, 0);

        Character character = selectedPlayer.GetComponent<Character>();
        GameManager.Instance.Character = character;

        hpSlider.value = (float)character.MaxHp / 5;
        speedSlider.value = character.Speed / 5;
        attckSpeedSlider.value = character.AttackSpeed / 10;
        jumpSlider.value = character.JumpForce / selectedPlayer.GetComponent<Rigidbody2D>().mass / 10;

    }

    public void SetRandomCharacter()
    {
        SetCurrentCharacter(Random.Range(1, CharactersPrefabs.Length));
    }

    public void NextCharacter()
    {

        SelectedCharacter++;
        if(CharactersPrefabs.Length <= SelectedCharacter)
        {
            SelectedCharacter = 0;
        }
        SetCurrentCharacter(SelectedCharacter);
    }

    public void PrevCharacter()
    {
        SelectedCharacter--;
        if (SelectedCharacter < 0)
        {
            SelectedCharacter = CharactersPrefabs.Length - 1;
        }
        SetCurrentCharacter(SelectedCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
