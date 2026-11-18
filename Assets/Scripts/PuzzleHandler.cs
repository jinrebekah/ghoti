using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Collections.Generic;

public class PuzzleHandler : JournalPage
{
    // stuff visible in inspector -- I want to have the puzzle build itself from these few things
    public bool puzzleSolved = false;
    public bool typeSolved = false;
    public bool letterSolved = false;
    
    // public Image puzzleImage; // apparently just setting these types of objects manually in inspector is easier and more robust to changes
    // public Sprite puzzleSprite; // what image to display (e.g. drawing of a fish)
    public string englishAnswer; // the correct answer, spelled in regular English (e.g. fish)
    public string ghotiAnswer; // the correct answer, spelled in ghoti (e.g. ghoti)

    public GameObject typeSlotPrefab;
    public Transform typeSlotContainer;
    
    public GameObject letterSlotPrefab;
    public Transform letterSlotContainer;
    
    public GameObject letterPrefab;
    public Transform letterContainer;
    public List<LetterHandler> letterHandlers = new List<LetterHandler>();

	public TMP_Text soundsLikeText;

    public RectTransform inputField;

    public LetterInputHandler letterInputHandler;
    public TypeInputHandler typeInputHandler;

	public Animator animator;
    public bool hasBeenOpened = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Create the puzzle
        // puzzleImage.sprite = puzzleSprite;
        GenerateTypeSlots();
        GenerateLetterSlots();
        GenerateLetterBank();
    }

    // public void CreatePuzzle()
    // {
    //     
    // }

    public bool IsTypeCorrect(string text)
    {
        return text.ToUpper() == englishAnswer.ToUpper();
    }

    public bool IsLetterCorrect(string text)
    {
        return text.ToUpper() == ghotiAnswer.ToUpper();
    }

    public void OnTypeSolved()
    {
        Debug.Log("Type solved!");
        typeSolved = true;
        // visual stuff - for now, remove underlines
        foreach (Transform typeSlot in typeSlotContainer)
        {
            typeSlot.GetComponentInChildren<Image>().enabled = false; 
        }
        CheckPuzzleSolved();

    }

    public void OnLetterSolved()
    {
        Debug.Log("Letter solved!");
        letterSolved = true;
        
        // disable both transparent and underline images in letter slots
        foreach (Transform letterSlot in letterSlotContainer)
        {
            foreach (Image image in letterSlot.GetComponentsInChildren<Image>())
            {
                image.enabled = false;
            }
        }
        // make letters not draggable
        foreach (LetterHandler letterHandler in letterHandlers)
        {
            letterHandler.draggable = false;
        }
        CheckPuzzleSolved();
    }

    void GenerateTypeSlots()
    {
        ClearChildren(typeSlotContainer); // this doesn't really work so whatever lol
        
        int numSlots = englishAnswer.Length;
        for (int i=0; i<numSlots; i++)
        {
            Instantiate(typeSlotPrefab, typeSlotContainer);
        }
        float inputWidth = 100*numSlots + 10*(numSlots-1); // hardcoded for now lol
        inputField.sizeDelta = new Vector2(inputWidth, inputField.sizeDelta.y);  // adjust width of input field to accomodate # of type slots
        inputField.GetComponent<TMP_InputField>().characterLimit = numSlots;
        
        typeInputHandler.MakeSlotTexts();
    }

    void GenerateLetterSlots()
    {
        // Debug.Log(letterSlotContainer.childCount);
        ClearChildren(letterSlotContainer);
        // Debug.Log(letterSlotContainer.childCount);
        
        int numLetters = ghotiAnswer.Length;
        for (int i = 0; i < numLetters; i++)
        {
            GameObject newLetterSlot = Instantiate(letterSlotPrefab, letterSlotContainer);
            LetterSlotHandler newLetterSlotHandler = newLetterSlot.GetComponent<LetterSlotHandler>();
            newLetterSlotHandler.index = i;
            newLetterSlotHandler.letterInputHandler = letterInputHandler; 
            
            letterInputHandler.letterSlotHandlers.Add(newLetterSlotHandler); // letterInputHandler keeps track of letterSlots
        }
    }

    void GenerateLetterBank()
    {
        ClearChildren(letterContainer);
        
        int numLetters = ghotiAnswer.Length;
        int idx = 0;
        int seed = 12346;
        System.Random rng = new System.Random(seed); // seed it so it's not diff every time lol
        foreach (int i in Enumerable.Range(0, numLetters).OrderBy(x => rng.Next())) // shuffles letter order
        {
            char letter = ghotiAnswer[i];
            GameObject newLetter = Instantiate(letterPrefab, letterContainer);
            LetterHandler newLetterHandler = newLetter.GetComponent<LetterHandler>();
            newLetterHandler.SetLetter(char.ToUpper(letter));
            newLetterHandler.index = idx; // set index here bc of start issue
            idx++;
            
            letterHandlers.Add(newLetterHandler); // also keep track of the LetterHandlers in a separate array, since the parent of each letter changes by design
        }
    }
    
    void ClearChildren(Transform container) // need this to clear the existing objects from puzzle template
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    void CheckPuzzleSolved()
    {
        if (!puzzleSolved && typeSolved && letterSolved)
        {
            Debug.Log("Puzzle solved!");
            puzzleSolved = true;
            GameManager.Instance.puzzlesSolved++;
        }
    }
    
	// visual stuff I guess
    // public void Show() // overloads the method in JournalPage
    // {
    //     if (!hasBeenOpened)
    //     {
    //         // PlayDrawPuzzleImage();
    //         hasBeenOpened = true;
    //         // deactivate the Journal Background lol
    //         
    //     }
    //     gameObject.SetActive(true);
    // }
    
	// public void PlayDrawPuzzleImage()
	// {
 //    	animator.Play("DrawPuzzleImage", 0, 0f);
	// }
}
