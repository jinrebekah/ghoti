using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static so everyone can see it 
    
    public GameState gameState;
    
    public IntroManager introManager;
    public TutorialManager tutorialManager;
    
    public Transform iconCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        
        // prepare starting state -- disable/enable everything, etc.
        // UpdateGameState(GameState.Intro); // set state to intro, starts playing intro
        // UpdateGameState(GameState.Tutorial); // for testing tutorial
        UpdateGameState(GameState.Playing); // for testing playing
        
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
        }
    }

    IEnumerator Playing()
    {
        // make sure UI icons work
        SetGroupInteractable(iconCanvas, true);
        // make all puzzle buttons appear and interatable now
        
        // yield return new WaitUntil(() => puzzleManager.AllPuzzlesCompleted());
        // UpdateGameState(GameState.Ending);
        yield break;
    }
    
    public void SetGroupInteractable(Transform transform, bool interactable)
    {
        foreach (Selectable selectable in transform.GetComponentsInChildren<Selectable>(true))
        {
            selectable.interactable = interactable;
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
