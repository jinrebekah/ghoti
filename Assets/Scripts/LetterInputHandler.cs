using UnityEngine;
using System.Collections.Generic;

public class LetterInputHandler : MonoBehaviour
{
    // intended purpose is to mirror TypeInputHandler
    // LetterHandler does dragging/dropping
    // this keeps track of what letters are in the slots
    public PuzzleHandler puzzleHandler;
    public string letterInput;
    public List<LetterSlotHandler> letterSlotHandlers = new List<LetterSlotHandler>(); // store letterSlotHandlers in ordered array
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateLetterInput()
    {
        // loop through LetterSlotHandlers to construct current letterInput
        letterInput = "";
        foreach (LetterSlotHandler letterSlotHandler in letterSlotHandlers)
        {
            char slotLetter = letterSlotHandler.letter;
            // Debug.Log(letterSlotHandler);
            // Debug.Log(slotLetter);
            letterInput += slotLetter;
        }
        // Debug.Log("Current letterInput: " + letterInput);
        
        if (puzzleHandler.IsLetterCorrect(letterInput))
        {
            puzzleHandler.OnLetterSolved();
        }
    }
}
