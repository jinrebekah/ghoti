using UnityEngine;
using UnityEngine.UI;

public class ClickableLetterHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ZoomHandler zoomHandler;

    void Start()
    {
        // automatically set zoomHandler to parent and connect its onClick function
        zoomHandler = GetComponentInParent<ZoomHandler>();
        GetComponent<Button>().onClick.AddListener(zoomHandler.OnLetterClick);
    }
    // on click
}
