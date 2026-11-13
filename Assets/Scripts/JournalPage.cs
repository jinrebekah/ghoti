using UnityEngine;

public class JournalPage : MonoBehaviour
{
    public int pageNumber;
    // extremely simple base class my pages can inherit from just for some flexibility on what's displayed in journal
    // self-explanatory: show and hide pages, children will implement their own versions of these
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
