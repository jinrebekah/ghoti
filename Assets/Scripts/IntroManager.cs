using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class IntroManager : MonoBehaviour
{
    public DialogueHandler dialogueHandler;
    public DialogueHandler preIntroDialogueHandler;

    public GameObject letterImage;
    public Button letterButton;
    public Button letterCloseButton;
    public Button realLetterButton;
    public Animator doorAnimator;
    public Animator letterAnimator;
    public PuzzleButtonHandler doorPuzzleButton;
    public CanvasGroup preIntroCanvasGroup;
    
    void Start()
    {
        
    }
    
    public IEnumerator Intro()
    {
        Debug.Log("Beginning Intro");
        // set intro state: turn off UI, etc.
        
        yield return PreIntro();
        // yield return OpeningSequence();
        // yield return DoorLetterSequence();
        realLetterButton.gameObject.SetActive(true); // just moved this to here lol
        GameManager.Instance.UpdateGameState(GameManager.GameState.Tutorial); // move onto next state
        yield break;
    }

    public IEnumerator PreIntro()
    {
        Debug.Log("Starting preintro");
        yield return preIntroDialogueHandler.PlayLines(new[]
        {
            "Welcome to ghoti garden.",
            "I'm the groundskeeper.",
            "So you're an artist, huh?",
            "I hope you'll find everything to your liking.",
            "Or find it...",
            "Everything, everywhere, all at once.",
            "Actually, I have no idea why I said that. That has nothing to do with anything here.",
            "Anyway, please do enjoy the garden...",
            "Just keep out of the door in the back. I haven't been able to get it open.",
            "I hope you find what you're looking for."
        });
        // fade out canvas
        preIntroCanvasGroup.DOFade(0f, 5f);
        yield break;
    }
    
    IEnumerator OpeningSequence()
    {
        Debug.Log("Beginning Opening Sequence");
        // play opening dialogue (prev dialogue should include like a "idk what's behind that door, but it's locked")
        // "Feel free to sketch whatever, but don't go behind the door. I haven't been able to get it open"
        yield return dialogueHandler.PlayLines(new[]
        {
            "Wow, what a beautiful day to sketch!",
            "Here's the other thing I have to say!"
        });
        // door handle jiggles a few times
        yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "Wait, did the door handle just move?"
        });
        // door handle jiggles another few times
        yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "I thought no one else lived here...",
            "I should go check it out."
        });
        
        // make door sparkle button appear
        doorPuzzleButton.Show();

        // animation of door slot opening, revealing mysterious eyes (no zoom in, done in aseprite)
        // "Wait, who is that? I thought the groundskeeper said no one lived here..." "Hello?"
        // animation of said mysterious eyes looking side to side (done in aseprite) 
        // animation of letter appearing and falling to floor (done in unity)
        // <ah I should just move the UI letter icon's transform around, don't need to make a separate object>
        // "What is that...a letter?"
        // (letter wiggles, prompting player to click on the interactable letter object)
        // pull up special UI with letter background displaying the message (put this in UI Canvas next to journal)
        // <this should also eventually include the logic for translating the letter but don't worry about that yet>
        // "What the hell does this say?"
        // "Guess I'll just put it in my bag for now."
        // when player clicks close button, Letter icon transform moved to main UI
    }

    IEnumerator DoorLetterSequence()
    {
        doorPuzzleButton.Show();
        // PlayAnimation
        
        // wait until player clicks door button
        yield return WaitForClick(doorPuzzleButton.GetComponent<Button>());
        
        yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        yield return new WaitForSeconds(1f);
        yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        yield return new WaitForSeconds(2f);
        yield return WaitForAnimation(doorAnimator, "SlotOpening", "LookAround");
        yield return new WaitForSeconds(3f);
        letterImage.SetActive(true);
        yield return WaitForAnimation(letterAnimator, "LetterFall", "Idle");
        Debug.Log("Letter done falling");
        yield return new WaitForSeconds(1f);
        Debug.Log("Supposedly playing slot closed");
        yield return WaitForAnimation(doorAnimator, "SlotClosed", "Closed");
        yield return dialogueHandler.PlayLines(new[]
        {
            "What is that? A letter?",
            "I should pick it up."
        });
        // make fake letter button appear
        letterButton.gameObject.SetActive(true);
        // wait until player clicks (fake) letter button
        yield return WaitForClick(letterButton);
        yield return new WaitForSeconds(1f);
        yield return dialogueHandler.PlayLines(new[]
        {
            "Woahhh no way lmao what does that even say",
            "Strange pen",
            "Anyway time to get back to sketching lol"
        });
        yield return WaitForClick(letterCloseButton);
        
        // remove the fake letter object, make real letter icon appear in the corner now
        letterImage.SetActive(false);
        letterButton.gameObject.SetActive(false);
        Debug.Log("Intro now finished!");
    }
    
    // helpers
    IEnumerator WaitForAnimation(Animator animator, string animationName, string finishedAnimationName="Closed")
    {
        animator.Play(animationName, 0, 0f);
        yield return null; // give an extra frame to update animation state to closed
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(finishedAnimationName)); // wait for animation to finish
    }
    IEnumerator WaitForClick(Button button) // should work with any button
    {
        bool clicked = false;
        UnityEngine.Events.UnityAction listener = () => clicked = true;
        button.onClick.AddListener(listener);
        yield return new WaitUntil(() => clicked);
        button.onClick.RemoveListener(listener); // just names the listening func so i can remove it individually
    }
}
