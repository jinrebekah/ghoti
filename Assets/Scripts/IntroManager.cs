using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public DialogueHandler dialogueHandler;

    public GameObject letterObject;
    public Animator doorAnimator;
    public Animator letterAnimator;
    public PuzzleButtonHandler doorButton;
    
    void Start()
    {
        
    }

    public IEnumerator Intro()
    {
        Debug.Log("Beginning Intro");
        // yield return OpeningSequence();
        yield return DoorSequence();
        yield return TutorialSequence();
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing); // move onto next state
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
        doorButton.Show();

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

    IEnumerator DoorSequence()
    {
        doorButton.Show();
        // PlayAnimation
        
        // wait until player clicks door button
        yield return doorButton.WaitForClick();
        // yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        // yield return new WaitForSeconds(1f);
        // yield return WaitForAnimation(doorAnimator, "HandleJiggle");
        // yield return new WaitForSeconds(2f);
        // yield return WaitForAnimation(doorAnimator, "SlotOpening", "LookAround");
        // yield return new WaitForSeconds(3f);
        letterObject.SetActive(true);
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
        // make fake letter button clickable
        letterObject.GetComponent<Button>().interactable = true;
    }
    

    IEnumerator WaitForAnimation(Animator animator, string animationName, string finishedAnimationName="Closed")
    {
        animator.Play(animationName, 0, 0f);
        yield return null; // give an extra frame to update animation state to closed
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(finishedAnimationName)); // wait for animation to finish
    }

    IEnumerator TutorialSequence()
    {
        // first ghoti puzzle button appears
        // "What is that by the water?"
        // etc.
        yield break;
    }
}
