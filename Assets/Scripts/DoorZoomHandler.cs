using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorZoomHandler : MonoBehaviour
{
    public Transform allZoomContainer;
    public Transform zooms;
    
    public Vector2 zoomCameraPosition; // save position of ZoomCamera 
    public Transform zoomCamera;
    public Vector2 zoomReferenceResolution; // for the pixel perfect camera
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Show()
    {
        Debug.Log("Showing Door Zoom");
        // make sure everything else in AllZoomContainer is hidden
        zooms.gameObject.SetActive(true);
        foreach (Transform zoom in allZoomContainer)
        {
            zoom.gameObject.SetActive(false);
        } 
        gameObject.SetActive(true);
        // set posiiton of zoomCamera
        zoomCamera.GetComponent<PixelPerfectCamera>().refResolutionX = (int)zoomReferenceResolution.x;
        zoomCamera.GetComponent<PixelPerfectCamera>().refResolutionY = (int)zoomReferenceResolution.y;
            
        zoomCamera.position = new Vector3(
            zoomCameraPosition.x,
            zoomCameraPosition.y,
            zoomCamera.position.z
        );
    }
}
