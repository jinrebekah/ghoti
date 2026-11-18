using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class TableRow : MonoBehaviour
{
    public int rowIndex;
    public bool solved = false;
    public PhonemeTableHandler phonemeTableHandler;
    public List<string> entries = new List<string>() { "", "", "" }; // keep track of row entries with a simple List

    public TMP_InputField spellingInputField;
    public TMP_Dropdown soundsLikeDropdown;
    public TMP_InputField asInInputField;

    public GameObject arrow;
    public Button deleteRowButton;
    
    // public TMP_FontAsset draftFont;
    public TMP_FontAsset correctFont;
    
    
    void Start()
    {
        phonemeTableHandler = GetComponentInParent<PhonemeTableHandler>();
        // SetSoundsLikeOptions(phonemeTableHandler.soundsLikeOptions);
        
        spellingInputField.onValueChanged.AddListener(delegate { UpdateEntries(); }); // delegate thing ignores the argument
        soundsLikeDropdown.onValueChanged.AddListener(delegate { UpdateEntries(); });
        asInInputField.onValueChanged.AddListener(delegate { UpdateEntries(); });
        
        spellingInputField.onValidateInput += ValidateSpellingInput;
        asInInputField.onValidateInput += ValidateAsInInput;
    }
    
    char ValidateSpellingInput(string text, int charIndex, char addedChar)
    {
        if (!char.IsLetter(addedChar))
        {
            return '\0';
        }
        return char.ToUpper(addedChar);
    }
    
    char ValidateAsInInput(string text, int charIndex, char addedChar)
    {
        if (!char.IsLetter(addedChar))
        {
            return '\0';
        }
        return char.ToLower(addedChar);
    }
    
    // stupid but this whole section is just the delete button

    public void OnPointerEnterDeleteButton()
    {
        // Debug.Log("Pointer Enter Delete Button");
        ShowDeleteButton();
    }
    
    public void OnPointerExitDeleteButton()
    {
        // Debug.Log("Pointer Exit Delete Button");
        HideDeleteButton();
    }
    
    public void HideDeleteButton()
    {
        deleteRowButton.GetComponentInChildren<TMP_Text>().enabled = false;
        deleteRowButton.interactable = false;
    }

    public void ShowDeleteButton()
    {
        deleteRowButton.GetComponentInChildren<TMP_Text>().enabled = true;
        deleteRowButton.interactable = true;
    }
    
    public void DeleteRow()
    {
        phonemeTableHandler.tableRowList.Remove(this);
        phonemeTableHandler.numRows--;
        Destroy(gameObject);
    }
    
    //--------------------------------------------------------------------------------
    
    public void UpdateEntries()
    {
        // change input field to capitalized input
        // string upper = value.ToUpper();
        
        // update entries list
        entries[0] = spellingInputField.text;
        entries[1] = soundsLikeDropdown.options[soundsLikeDropdown.value].text;
        entries[2] = asInInputField.text;
        
        Debug.Log($"Row updated! Current entries: {string.Join(", ", entries)}");
        phonemeTableHandler.OnRowUpdate(this);
    }
    
    public void SetSoundsLikeOptions(List<string> options)
    {
        string currentSelection = soundsLikeDropdown.options[soundsLikeDropdown.value].text;
        soundsLikeDropdown.ClearOptions();
        soundsLikeDropdown.AddOptions(options);
        int index = options.IndexOf(currentSelection);
        soundsLikeDropdown.value = index >= 0 ? index : 0; // ugh just so it doesn't erase what's currenlty in there
        soundsLikeDropdown.RefreshShownValue();
    }

    public void DisableInput()
    {
        spellingInputField.interactable = false;
        soundsLikeDropdown.interactable = false;
        asInInputField.interactable = false;
        deleteRowButton.gameObject.SetActive(false); // also remove deleteButton
        arrow.SetActive(false);
    }

    public void ChangeFont()
    {
        spellingInputField.fontAsset = correctFont;
        soundsLikeDropdown.captionText.font = correctFont;
        asInInputField.fontAsset = correctFont;
    }
}