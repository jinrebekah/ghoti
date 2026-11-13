using UnityEngine;
using UnityEngine.EventSystems;


public class LetterSlotHandler : MonoBehaviour, IDropHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LetterInputHandler letterInputHandler;
    public int index;

    public bool occupied = false;
    public char letter = '*'; // begins with empty space character
    
    void Start()
    {
        // letterInputHandler = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData) {
		// Debug.Log("Dropped");
        if (eventData.pointerDrag != null) { // eventData.pointerDrag is the GameObject that was dragged in ofc
            eventData.pointerDrag.transform.position = transform.position;

            LetterHandler letterHandler = eventData.pointerDrag.GetComponent<LetterHandler>(); // letterHandler that was dragged in 
            letterHandler.inSlot = true;
            letterHandler.letterSlotHandler = this; // tell the letter which slot it's in
            occupied = true;
            letter = letterHandler.letter; // remember occupying letter
            
            letterInputHandler.UpdateLetterInput();
            
        }
    }
    
}
