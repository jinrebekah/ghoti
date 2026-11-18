using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleButtonHandler : MonoBehaviour
{
    public bool puzzleDiscovered = false;
    public Image image;
    
    public bool clicked = false;
    public bool isPuzzle = true;
    // public PuzzleHandler puzzleHandler; // associated puzzleHandler
    public ZoomHandler zoomHandler; // associated zoom
    public Animator buttonAnimator;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
        Button button = GetComponent<Button>();
        if (isPuzzle)
        {
            button.onClick.AddListener(zoomHandler.Show);            
        }
        
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        buttonAnimator.Play("Appear", 0, 0.0f);
    }

    public void PuzzleDiscovered()
    {
        Debug.Log("Puzzle discovered");
        buttonAnimator.gameObject.SetActive(false);
        image.color = new Color32(195, 35, 84, 255);
        buttonAnimator.gameObject.SetActive(true); // lol stupid workaround
    }
}
