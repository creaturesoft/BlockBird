using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public List<Character> CharactersPrefabs { get; set; }

    public GameObject[] AllCharactersPrefabs;

    public static int SelectedCharacter;
    GameObject selectedPlayer;
    public StartButton startbutton;

    public Slider hpSlider;
    public Slider speedSlider;
    public Slider attckSpeedSlider;
    public Slider jumpSlider;

    public void Init()
    {
        CharactersPrefabs = new List<Character>();
        CharactersPrefabs.Add(AllCharactersPrefabs[0].GetComponent<Character>());
        CharactersPrefabs.Add(AllCharactersPrefabs[1].GetComponent<Character>());

        foreach (BirdData bird in PersistentObject.Instance.UserData.birdList)
        {
            AllCharactersPrefabs.Where(x => x.name == bird.name).ToList().ForEach(x => {

                Character character = x.GetComponent<Character>();
                character.CharacterName = bird.name;
                character.Level = bird.pullLevel + bird.expLevel;
                character.PullLevel = bird.pullLevel;
                character.ExpLevel = bird.expLevel;
                character.Exp = bird.exp;
                CharactersPrefabs.Add(character);
            });
        }

        if (GameManager.IsRestart)
        {
            SetCurrentCharacter(SelectedCharacter);
            startbutton.OnStart();
            GameManager.IsRestart = false;
        }
        else
        {
            SelectedCharacter = 2;
            SetCurrentCharacter(SelectedCharacter);
        }
    }


    void SetCurrentCharacter(int select)
    {
        if(selectedPlayer != null)
        {
            Destroy(selectedPlayer);
        }

        Character character = Instantiate(CharactersPrefabs[select], new Vector3(0, 0, 0), Quaternion.identity).Init(CharactersPrefabs[select]);
        selectedPlayer = character.gameObject;

        if (select == 0 || select == 1)
        {
            GameManager.Instance.levelText.text = "";
            GameManager.Instance.expText.text = "";
        }
        else
        {
            string expPercent = (character.Exp / character.GetExpForLevel() * 100).ToString("F2");
            GameManager.Instance.levelText.text = "Lv " + character.Level;
            GameManager.Instance.expText.text = expPercent + "%";
        }

        character.transform.localScale *= 2;
        character.transform.position = new Vector3(0, 0.8f, 0);

        GameManager.Instance.Character = character;

        hpSlider.value = (float)character.MaxHp / 8.0f;
        speedSlider.value = character.Speed / 10.0f;
        attckSpeedSlider.value = character.AttackSpeed / 10.0f;
        jumpSlider.value = character.JumpForce / selectedPlayer.GetComponent<Rigidbody2D>().mass / 10.0f;

    }

    public void SetRandomCharacter()
    {
        SetCurrentCharacter(Random.Range(2, CharactersPrefabs.Count));
    }

    public void NextCharacter()
    {

        SelectedCharacter++;
        if(CharactersPrefabs.Count <= SelectedCharacter)
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
            SelectedCharacter = CharactersPrefabs.Count - 1;
        }
        SetCurrentCharacter(SelectedCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
