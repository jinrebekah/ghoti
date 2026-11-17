using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    // this is a component of the thought bubble object itself 
    public TMP_Text dialogueText;
    // public CanvasGroup uICanvasGroup;
    public float typingSpeed = 0.01f; // controls speed of typewriter effect
    private bool clicked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            clicked = true;
    }

    public IEnumerator PlayLines(string[] lines)
    {
        gameObject.SetActive(true);

        foreach (string line in lines)
        {
            clicked = false;
            yield return PlayLine(line);

            clicked = false;
            yield return new WaitUntil(() => clicked);
        }

        gameObject.SetActive(false);
    }

    // IEnumerator PlayLine(string line)
    // {
    //     dialogueText.text = "";
    //
    //     foreach (char letter in line)
    //     {
    //         if (clicked)
    //         {
    //             dialogueText.text = line;
    //             yield break;
    //         }
    //
    //         dialogueText.text += letter;
    //         yield return new WaitForSeconds(typingSpeed);
    //     }
    // }
    
    IEnumerator PlayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= line.Length; i++)
        {
            if (clicked)
            {
                dialogueText.maxVisibleCharacters = line.Length;
                yield break;
            }

            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
