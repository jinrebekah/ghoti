using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MessageHandler message;
    public string answer;
    public Image image;
    public bool selected = false;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected)
            image.DOFade(1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
            image.DOFade(0f, 0.2f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!selected)
        {
            selected = true;
            message.onCircleClick(this);
        }
    }
}