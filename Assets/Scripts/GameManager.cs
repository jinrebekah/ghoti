using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static so everyone can see it 
    
    public GameState gameState;
    
    public IntroManager introManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        
        // prepare starting state -- disable/enable everything, etc.
        UpdateGameState(GameState.Intro); // set state to intro, starts playing intro
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
            case GameState.Playing:
                Debug.Log("Playing I guess");
                break;
        }
    }

    public enum GameState // my list of possible states
    {
        Intro,
        Playing,
        Ending
    }
        
}
