using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleButtonHandler : MonoBehaviour
{
    public bool clicked = false;
    public bool isPuzzle = true;
    public PuzzleHandler puzzleHandler; // associated puzzleHandler
    public ZoomHandler zoomHandler; // associated zoom
    public Animator buttonAnimator;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        if (isPuzzle)
        {
            button.onClick.AddListener(zoomHandler.Show);            
        }
        
    }

    public void OnClick()
    {
        clicked = true;
    }
    public IEnumerator WaitForClick()
    {
        clicked = false;
        yield return new WaitUntil(() => clicked);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        buttonAnimator.Play("Appear", 0, 0.0f);
    }
}
