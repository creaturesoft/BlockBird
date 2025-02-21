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
        // 영어(a-z, A-Z), 숫자(0-9), 특수문자만 허용하는 정규식
        string filteredText = Regex.Replace(text, @"[^a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]", "");


        if (filteredText != text)
        {
            inputField.text = filteredText;
        }
    }
}
