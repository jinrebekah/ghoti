using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EndingManager : MonoBehaviour
{
    public DialogueHandler dialogueHandler;
    public DialogueHandler outroDialogueHandler;
    
    public MessageHandler message;
    public Button messageButton;
    public TMP_InputField messageInputField;
    public CanvasGroup messageInputFieldCanvasGroup;
    public UIIcon messageIcon;
    public Image outline;
    public JournalHandler journal;
    public PhonemeTableHandler phonemeTable;
    public Button doorButton;
    public Animator doorAnimator;
    public Button hiddenDoorButton;
    public CanvasGroup outroCanvasGroup;
    
    
    
    void Awake()
    {
    }

    public IEnumerator Ending()
    {
        // some state stuff I guess ugh
        // yield return MessageSequence(); // first translate letter
        // yield return new WaitUntil(() => phonemeTable.numCorrectRows >= 12);
        Debug.Log("Phoneme table all correct");
        yield return DoorSequence();
        // then finish filling in table
        yield return Outro();
    }

    public IEnumerator MessageSequence()
    {
        Debug.Log("Beginning ending");
        yield return dialogueHandler.PlayLines(new[]
        {
            "I should be able to understand what the letter says now...",
        });
        messageIcon.Jump(loop: true);
        yield return WaitForClick(messageButton);
        messageIcon.StopAnimation();
        yield return messageInputFieldCanvasGroup.DOFade(1f, 2f).WaitForCompletion();
        // now letter should contain a new input field
        yield return dialogueHandler.PlayLines(new[]
        {
            "What does it say? I'll write it on the line.",
        });
        yield return new WaitUntil(() => message.solved);
        outline.DOFade(0f, 1f);
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "It says friend or foe!",
            "The creature wants to know if I'm friendly.",
            "I'll circle my choice..."
        });
        message.ActivateCircles();
        yield return new WaitUntil(() => message.selected);
        if (message.selection == "friend")
        {
            yield return dialogueHandler.PlayLines(new[]
            {
                "I'd like to be its friend!",
            });
        }
        else
        {
            yield return dialogueHandler.PlayLines(new[]
            {
                "I don't trust it...",
            });
        }
        yield return dialogueHandler.PlayLines(new[]
        {
            "I should give the letter back to the creature.",
            "But first, I've learned a few new things I should write in my table..."
        });
        message.CloseMessage();
        journal.DisplayPage(0);
        journal.OpenJournal();
        // journalIcon.Jump(loop: true);
    }

    IEnumerator DoorSequence()
    {
        yield return dialogueHandler.PlayLines(new[]
        {
            "Great, my table's all complete!",
            "Now to give the letter back through the door."
        });
        yield return WaitForClick(doorButton);
        // now in Zoom
        // prob start playing animation automatically
        yield return WaitForAnimation(doorAnimator, "SlotOpening", "LookAround");
        messageIcon.Jump();
        yield return new WaitForSeconds(2f);
        messageIcon.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        yield return WaitForAnimation(doorAnimator, "SlotClosed", "Closed");
        if (message.selection == "friend")
        {
            Debug.Log("I chose friend");
            yield return new WaitForSeconds(2f);
            doorAnimator.Play("Open", 0, 0f);
            yield return WaitForClick(hiddenDoorButton);
            Debug.Log("Hidden door button clicked");
        }
        else
        {
            Debug.Log("I chose foe");
            yield return dialogueHandler.PlayLines(new[]
            {
                "Oh no, the door didn't open :( so sad"
            });
        }
    }
    public IEnumerator Outro()
    {
        outroCanvasGroup.gameObject.SetActive(true);
        Debug.Log("Starting post ending");
        outroCanvasGroup.alpha = 0f;
        yield return outroCanvasGroup.DOFade(1f, 5f).WaitForCompletion();
        yield return outroDialogueHandler.PlayLines(new[]
        {
            "Thanks for playing! -Rebekah (WRUHBBEKUH)"
        });
    }
    
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