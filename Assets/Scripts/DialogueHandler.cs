using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    // this is a component of the thought bubble object itself 
    public TMP_Text dialogueText;
    public CanvasGroup uICanvasGroup;
    public float typingSpeed = 0.01f; // controls speed of typewriter effect
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public IEnumerator PlayLines(string[] lines)
    {
        Debug.Log("Playing lines");
        gameObject.SetActive(true);

        uICanvasGroup.interactable = false;
        uICanvasGroup.blocksRaycasts = false;

        foreach (string line in lines)
        {
            yield return StartCoroutine(PlayLine(line));
            // yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); 
            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);// Wait for player to click before showing next line
        }

        gameObject.SetActive(false);
        uICanvasGroup.interactable = true;
        uICanvasGroup.blocksRaycasts = true;
    }

    IEnumerator PlayLine(string line)
    {
        // plays one line with type writer effect
        dialogueText.text = "";
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
