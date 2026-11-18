using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    private string messageTranslation = "friend or foe?";
    public bool solved = false;
    public bool selected = false;
    public string selection;
    public GameObject circleParent;
    public HoverImage[] circles;

    public GameObject zooms;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(checkAnswer);
        inputField.onValidateInput += ValidateInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (!char.IsLetter(addedChar) && addedChar != ' ' && addedChar != '?')
        {
            return '\0';
        }
        return char.ToLower(addedChar);
    }

    private void checkAnswer(string input)
    {
        if (input == messageTranslation)
        {
            // answer correct
            Debug.Log("Correct Answer");
            solved = true;
            inputField.interactable = false;
        }
    }

    public void ActivateCircles()
    {
        circleParent.SetActive(true);
    }

    public void onCircleClick(HoverImage selectedCircle)
    {
        selection = selectedCircle.answer;
        Debug.Log("Set answer" + selection);
        foreach (HoverImage circle in circles)
        {
            circle.selected = true;
        }
        selected = true;
    }

    public void CloseMessage()
    {
        gameObject.SetActive(false);
    }
}


