using UnityEngine;
using TMPro;

public class TypeInputHandler : MonoBehaviour
{
    public PuzzleHandler puzzleHandler;
    
    public Transform typeSlotContainer;
    private TMP_InputField inputField;
    private TMP_Text[] slotTexts; // store slotText in ordered array

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnTextChanged);
        // grab the answer from the puzzle
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeSlotTexts()
    {
        //slottexts is too long
        // Debug.Log(typeSlotContainer.childCount);
        slotTexts = new TMP_Text[typeSlotContainer.childCount];
        for (int i=0; i<slotTexts.Length; i++) {
            slotTexts[i] = typeSlotContainer.GetChild(i).GetComponentInChildren<TMP_Text>(); // searches for TMP_Text component in children of typeslot
        }

        // Debug.Log(slotTexts.Length);
    }

    void OnTextChanged(string text)
    {
        UpdateSlotTexts(text);
        if (puzzleHandler.IsTypeCorrect(text))
        {
            inputField.interactable = false; // no longer accepts input when solved
            puzzleHandler.OnTypeSolved();
        }
    }

    void UpdateSlotTexts(string text)
    {
        // update text displayed in typeSlots according to input
        // Debug.Log("Update slots with" + text);
        for (int i = 0; i < inputField.characterLimit; i++)
        {
            if (i < text.Length)
            {
                // Debug.Log("update slot " + i);
                slotTexts[i].text = text[i].ToString(); // obv update the letter in this slotText
                Debug.Log(slotTexts[i].text);
            }
            else
            {
                slotTexts[i].text = "";
            }
        }
    }

}
