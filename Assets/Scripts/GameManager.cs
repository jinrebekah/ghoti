using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static so everyone can see it 
    
    public GameState gameState;
    
    public IntroManager introManager;
    public TutorialManager tutorialManager;
    public EndingManager endingManager;
    
    public Transform iconCanvas;
    public JournalHandler journalHandler;
    public DialogueHandler dialogueHandler;
    
    public int puzzlesSolved;
    private int numPuzzles = 5;
    public int rowsSolved;
    private int numRowsPlaying = 3+6; // actually this is the number of rows in intro/playing section, stupid but whatever
    private int numRowsEnding = 3; // numRows in ending section
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        
        // prepare starting state -- disable/enable everything, etc.
        // UpdateGameState(GameState.Intro); // set state to intro, starts playing intro
        // UpdateGameState(GameState.Tutorial); // for testing tutorial
        // UpdateGameState(GameState.Playing); // for testing playing
        UpdateGameState(GameState.Ending);
        journalHandler.Initialize();
    }

    void Start()
    {
        // prep for the preintro blank background with text
        // 
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.Intro:
                StartCoroutine(introManager.Intro()); // play the intro
                break;
            case GameState.Tutorial:
                Debug.Log("Tutorial I guess");
                StartCoroutine(tutorialManager.Tutorial()); // play the tutorial
                break;
            case GameState.Playing:
                Debug.Log("Playing I guess");
                StartCoroutine(Playing());
                break;
            case GameState.Ending:
                Debug.Log("Ending I guess");
                StartCoroutine(endingManager.Ending());
                break;
        }
    }
    
    IEnumerator Playing()
    {
        // make sure UI icons work
        SetGroupActive(iconCanvas, true);
        SetGroupInteractable(iconCanvas, true);
        // make all puzzle buttons appear and interactable now


        puzzlesSolved = numPuzzles; /// temp temp temp
        rowsSolved = numRowsPlaying;
        yield return new WaitUntil(() => (puzzlesSolved >= numPuzzles));
        if (rowsSolved < numRowsPlaying)
        {
            yield return dialogueHandler.PlayLines(new[]
            {
                "I think I've discovered everything from the garden.",
                "I should finish filling out my table."
            });
        }
        UpdateGameState(GameState.Ending);
        yield break;
    }
    
    public void SetGroupInteractable(Transform transform, bool interactable) // these two are copied
    {
        foreach (Selectable selectable in transform.GetComponentsInChildren<Selectable>(true))
        {
            selectable.interactable = interactable;
        }
    }
    public void SetGroupActive(Transform transform, bool active) // for all children, and including itself
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(active);
        }
    }
    
    public enum GameState // my list of possible states
    {
        Intro,
        Tutorial,
        Playing,
        Ending
    }
        
}
