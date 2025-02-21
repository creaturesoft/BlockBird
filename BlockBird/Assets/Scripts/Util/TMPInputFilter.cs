using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class TMPInputFilter : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.onValueChanged.AddListener(ValidateInput);
    }

    void ValidateInput(string text)
    {
        // ����(a-z, A-Z), ����(0-9), Ư�����ڸ� ����ϴ� ���Խ�
        string filteredText = Regex.Replace(text, @"[^a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]", "");


        if (filteredText != text)
        {
            inputField.text = filteredText;
        }
    }
}
