using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LetterHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // [SerializeField] private Canvas canvas;
    public Canvas canvas;
    public TMP_Text tmpText;
    
    public char letter; // the letter to be displayed
    public int index; // current order index in letterContainer
    public bool draggable = true;
    
    public bool inSlot = false;
    public LetterSlotHandler letterSlotHandler;// also store which slot it's in

    private Transform letterContainer; // save the original letterContainer parent
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition; // keep track of this so the letter can snap back into place when released
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        letterContainer = transform.parent;
        // index = transform.GetSiblingIndex();
        
        // GetComponentInChildren<TMP_Text>().text = letter.ToString();
        // TMP_Text tmpText = GetComponentInChildren<TMP_Text>();
        // Debug.Log(tmpText == null);
        // tmpText.text = letter.ToString();
        
        // Debug.Log(GetComponent<TMP_Text>());
        // Debug.Log(GetComponentInChildren<TMP_Text>());
        // Debug.Log("Letter " + GetComponent<TMP_Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Debug.Log("Clicked: " + gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;

        if (inSlot)
        {
            inSlot = false;    
            letterSlotHandler.occupied = false;
            letterSlotHandler.letter = '*'; // 'empty space' character
            letterSlotHandler = null; // reset the slot it was in
        }
        
        canvasGroup.blocksRaycasts = false; // so Raycast isn't absorbed by the letter, but picked up by the slot

        // remove this letter from the LayoutGroup (make its parent the parent of the layout container)
        transform.SetParent(letterContainer.parent.transform);
    }

    public void OnDrag(PointerEventData eventData) { // triggers every frame the mouse moves
        if (!draggable) return; 
        
        // rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // actually move the letter
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // Debug.Log("Ended drag: " + gameObject.name + " | ID: " + GetInstanceID());
        if (!draggable) return; 
        
        canvasGroup.blocksRaycasts = true;
        // put back in layout group (letter bank) if it's not dropped in slot
        if (!inSlot)
        {
            Debug.Log("Letter thinks it isn't in a slot");
            transform.SetParent(letterContainer.transform);
            transform.SetSiblingIndex(index);
            inSlot = false;
        } else {
            // otherwise it was dropped in a slot I guess
            index = 0;

        }
        ResetIndices();
    }
    
    public void ResetIndices() {
        // reset all the index variables in letterContainer
        foreach (Transform draggableLetter in letterContainer) {
            draggableLetter.GetComponent<LetterHandler>().index = draggableLetter.GetSiblingIndex();    
        }
    }
    
    public void SetLetter(char setLetter)
    {
        letter = setLetter;
        tmpText.text = setLetter.ToString();
    }
}
