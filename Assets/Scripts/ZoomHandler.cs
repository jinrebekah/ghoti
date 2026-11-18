using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class ZoomHandler : MonoBehaviour
{
    // this will handle what happens in each zoom view -- mostly clicking all the letters lol
    public JournalHandler journalHandler;
    public PuzzleHandler puzzleHandler;

    public PenButtonHandler penButtonHandler;

    public Transform allZoomContainer;
    public Transform zooms;
    public Transform letterContainer;

    public int numLetters;
    public int numLettersClicked = 0;
    public bool allLettersClicked = false;
    public Animator animator;

	public Vector2 zoomCameraPosition; // save position of ZoomCamera 
	public Transform zoomCamera;
	public Vector2 zoomReferenceResolution; // for the pixel perfect camera
	
	public UIIcon journalIcon;
	public UIIcon penIcon;
	public PuzzleButtonHandler puzzleButton;
    
    void Start()
    {
        numLetters = letterContainer.childCount; // set numLetters
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        Debug.Log("Showing Zoom");
        // make sure everything else in AllZoomContainer is hidden
        zooms.gameObject.SetActive(true);
        foreach (Transform zoom in allZoomContainer)
        {
            zoom.gameObject.SetActive(false);
        } 
        gameObject.SetActive(true);
		// set posiiton of zoomCamera
		// pixelPerfectCamera.refResolutionX = (int)zoomReferenceResolution.x;
		// pixelPerfectCamera.refResolutionY = (int)zoomReferenceResolution.y;
		zoomCamera.GetComponent<PixelPerfectCamera>().enabled = true;
		zoomCamera.GetComponent<PixelPerfectCamera>().refResolutionX = (int)zoomReferenceResolution.x;
		zoomCamera.GetComponent<PixelPerfectCamera>().refResolutionY = (int)zoomReferenceResolution.y;
		zoomCamera.position = new Vector3(
    		zoomCameraPosition.x,
    		zoomCameraPosition.y,
    		zoomCamera.position.z
		);
		penButtonHandler.Activate(puzzleHandler, this);
    }

    public void Hide()
    {
        penButtonHandler.Deactivate();
    }

    public void OnLetterClick()
    {
		// add a little jump to journal each click to make it look like you're adding it to the journal
		journalIcon.Jump(loop: false);
        numLettersClicked++;
        Debug.Log(numLettersClicked);
        if (numLettersClicked == numLetters)
        {
            Debug.Log("All clicked");
            penIcon.Jump(loop: true); // wiggle penButton
            allLettersClicked = true;
        } 
    }

    public IEnumerator SketchAppear()
    {
	    // ugh this is so bad and spread across so many different things
	    // but anyway at this point the puzzle is safely in the journal so we can change the color
	    puzzleButton.PuzzleDiscovered();
        animator.Play("SketchAppear", 0, 0f);
		yield return new WaitForSeconds(2.5f);
		journalIcon.Jump(loop: true); // wiggle journalButton
    }
}
