using UnityEngine;

public class PenButtonHandler : UIIcon
{
    // basically like for the most part I want this to be displayed, but do nothing when clicked
    // it's only in a Zoomed view, once all the letters have been clicked, that it's capable of pulling up puzzle
    
    // actually all I really need to know for now is which zoom is active

    public GameObject zooms;
    public PuzzleHandler puzzleHandler;
    public JournalHandler journalHandler;
    
    public ZoomHandler zoomHandler;
	public UIIcon journalIcon;
    
    public void Activate(PuzzleHandler setPuzzleHandler, ZoomHandler setZoomHandler)
    {
        puzzleHandler = setPuzzleHandler;
        zoomHandler = setZoomHandler;
    }
    
    public void Deactivate()
    {
        puzzleHandler = null;
        zoomHandler = null;
    }

    public void OnClick()
    {
		StopAnimation();
        if (zooms.activeSelf && zoomHandler.allLettersClicked)
        {
            Debug.Log("Pen clicked");
			StartCoroutine(zoomHandler.SketchAppear());
			journalHandler.DisplayPage(puzzleHandler, false);
            Deactivate();
        }
        
    }

	


}
