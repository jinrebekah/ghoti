using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public DialogueHandler dialogueHandler;
    public PuzzleButtonHandler fishPuzzleButton;
    public ZoomHandler fishZoom;
    public CanvasGroup fishLetterContainer;
    public Animator fishSketchImageAnimator;
    
    public UIIcon penIcon;
    public UIIcon journalIcon;
    public UIIcon leftArrowIcon;
    public Button penButton;
    public Button journalButton;
    public Button leftArrowButton;
	public JournalHandler journal;
    public Button journalCloseButton;
    
    public PuzzleHandler fishPuzzle;
    public PhonemeTableHandler phonemeTable;
    
    void Awake()
    {
        penButton = penIcon.GetComponent<Button>();
        journalButton = journalIcon.GetComponent<Button>();
        leftArrowButton = leftArrowIcon.GetComponent<Button>();
    }

    public IEnumerator Tutorial()
    {
        // yield return ZoomSequence();
        // yield return JournalSequence();
        yield return PhonemeTableSequence();
    }

    public IEnumerator ZoomSequence()
    {
        Debug.Log("Beginning Tutorial");
        journalIcon.Jump();
        // journalButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f);
        // journalButton.transform.DOPunchPosition(new Vector3(0, 1.5f, 0), 2.0f).SetLoops(-1, LoopType.Restart);
        // journalButton.transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 2.0f).SetLoops(-1, LoopType.Restart);
        // journalButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 2f).SetLoops(-1, LoopType.Restart);
        yield return dialogueHandler.PlayLines(new[]
        {
            "Well, time to sketch now I suppose.",
            "Let's start with the fish in the pond."
        });
        fishPuzzleButton.Show();
        yield return WaitForClick(fishPuzzleButton.GetComponent<Button>());
        
        yield return new WaitForSeconds(1f);
        yield return dialogueHandler.PlayLines(new[]
        {
            "Wow, what a beautiful fish!",
            "Wait, waht are those weird bits of paper floating around?",
            "How are they not soggy if they're in the pond?",
            "Must be something about reusable assets...",
            "Anyway, I suppose I'll pick them up for now."
        });
        fishLetterContainer.interactable = true;
        yield return new WaitUntil(() => fishZoom.allLettersClicked);
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "Cool, I've collected all the letters.",
            "Now I'll sketch the fish!"
        });
        penButton.interactable = true;
		journal.Initialize(); // have to intialize here so dumb
        yield return WaitForClick(penButton);
        
        // yield return WaitForAnimation(fishSketchImageAnimator, "Idle"); oooh i don't want to use this bc pen button itself is playing the animation
        yield return new WaitForSeconds(2.5f); // let's just wait a second instead lol
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "Great, let's check it out in the journal."
        }); 
        journalButton.interactable = true;
        yield return WaitForClick(journalButton);
        Debug.Log("First part over, should be in journal now.");
        yield break;
    }

    IEnumerator JournalSequence()
    {
        // journal opened to fish puzzle
        journalCloseButton.interactable = false;
        journalButton.interactable = false; // make sure player can't exit out lol
        yield return dialogueHandler.PlayLines(new[]
        {
            "Great, now my sketch is in my journal!",
            "And those weird letters I found earlier.",
            "So, what did I just draw?"
        });
        // player fills in type part of puzzle
        fishPuzzle.typeSlotContainer.GetComponent<CanvasGroup>().DOFade(1f, 1f).WaitForCompletion();
        yield return new WaitUntil(() => fishPuzzle.typeSolved);
        yield return dialogueHandler.PlayLines(new[]
        {
            "Yup, I drew a fish.",
            "But what to do about these weird letters I found?",
            "Maybe if I rearrange them a certain way..."
        }); 
        // queue the drag and drop
        fishPuzzle.letterSlotContainer.GetComponent<CanvasGroup>().DOFade(1f, 1f).WaitForCompletion();
        yield return new WaitUntil(() => fishPuzzle.letterSolved);
        yield return dialogueHandler.PlayLines(new[]
        {
            "Oh, oh, I know!",
            "When I rearrange the letters this way..",
            "GHOTI sounds like fish!",
        }); 
        yield return fishPuzzle.soundsLikeText.DOFade(1f, 1f).WaitForCompletion();
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "Maybe I should write down why this makes sense..."
        }); 
        // make left arrow appear/jump, go to phoneme table
        leftArrowIcon.Jump(loop: true);
        yield return WaitForClick(leftArrowButton);
		leftArrowIcon.StopAnimation();
        // guide to phoneme table part
    }

    IEnumerator PhonemeTableSequence()
    {
        // yield return dialogueHandler.PlayLines(new[]
        // {
        //     "I think the letters correspond to certain sounds...",
        //     "I'll write down what I learn here.",
        //     "So why does GHOTI sound like fish?",
        // }); 
        //
        Debug.Log(phonemeTable.tableRowList.Count);
        foreach (TableRow tableRow in phonemeTable.tableRowList)
        {
            tableRow.GetComponent<CanvasGroup>().interactable = false; // make all tableRows
        } 
        
        yield return dialogueHandler.PlayLines(new[] { "Well, the spelling GH..."});
        phonemeTable.tableRowList[0].spellingInputField.text = "GH";
        yield return dialogueHandler.PlayLines(new[] { "Sounds like the F in Fish..."});
        phonemeTable.tableRowList[0].soundsLikeDropdown.value = 1;
        yield return dialogueHandler.PlayLines(new[] { "As in the word tough!"});
        phonemeTable.tableRowList[0].asInInputField.text = "tough";
        
        yield return new WaitUntil(() => phonemeTable.tableRowList[0].solved); // first row solved
        yield return dialogueHandler.PlayLines(new[]
        {
            "First row solved!",
            "And the spelling O...",
            "Sounds like the I in Insect...",
            "As in the word women!",
            "Let me write that down."
        }); 
        yield return new WaitUntil(() => phonemeTable.tableRowList[1].solved); // first row solved
        yield return dialogueHandler.PlayLines(new[]
        {
            "Lastly, the spelling TI...",
            "Sounds like the SH in SHell...",
            "As in the word shell!",
        }); 
        yield return new WaitUntil(() => phonemeTable.tableRowList[2].solved); // first row solved
        
        yield return dialogueHandler.PlayLines(new[]
        {
            "I'll keep on looking around the garden.",
            "Maybe this will help me understand what the secret message says...",
        }); 
        journalCloseButton.interactable = true;
        yield return WaitForClick(journalCloseButton);
        foreach (TableRow tableRow in phonemeTable.tableRowList)
        {
            if (!tableRow.solved)
            {
                tableRow.GetComponent<CanvasGroup>().interactable = true; // make empty tableRows interactable again                
            }
        }
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing); // move onto next state
        Debug.Log("Tutorial over");
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
