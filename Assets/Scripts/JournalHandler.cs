using UnityEngine;
using System.Collections.Generic;

public class JournalHandler : MonoBehaviour
{
	public GameObject IconCanvas;
    public UIIcon journalIcon;
    
    // want this to do stuff like buttons, turning pages, etc.
    private List<JournalPage> pages = new List<JournalPage>();
    private JournalPage currentPage;
    private int currentPageNumber = 0;
    public JournalPage phonemeTablePage;


    void Awake()
    {
        
    }

	public void Initialize()
	{
        Debug.Log("Adding phoneme table page");
        AddPage(phonemeTablePage); // just make the phoneme table page 0 for now
        DisplayPage(phonemeTablePage, openJournal: false);
	}
    
    public void CloseJournal()
    {
        gameObject.SetActive(false);
		IconCanvas.SetActive(true); // show icons
    }
    
    public void OpenJournal()
    {
        journalIcon.StopAnimation();
        gameObject.SetActive(true);
		IconCanvas.SetActive(false); // hide icons
    }

    public void AddPage(JournalPage page)
    {
        // adds a new page to the end
		Debug.Log("Adding page");
		Debug.Log(pages.Count);
        page.pageNumber = pages.Count; // assign page number
        pages.Add(page);
    }

    public void DisplayNextPage()
    {
        if (currentPageNumber >= pages.Count-1) // if last page
        {
            return;
        }
        currentPageNumber++;
        DisplayPage(currentPageNumber);
    }
    
    public void DisplayPreviousPage()
    {
        if (currentPageNumber == 0) // if first page
        {
            return;
        }
        currentPageNumber--;
        DisplayPage(currentPageNumber);
    }

    public void DisplayPage(JournalPage page, bool openJournal = true)
    {
		if (openJournal) // this gives me the option to open the journal afterwards
		{
			OpenJournal();
		}
       
     
        if (!IsInPages(page))
        {
            AddPage(page);
        }

        if (currentPage != null)
        {
            currentPage.Hide();            
        }
        page.Show();
        currentPage = page;
        currentPageNumber = pages.IndexOf(currentPage);
    }
    
    public void DisplayPage(int pageNumber) // pageNumber version
    {
        if (pageNumber < 0 || pageNumber >= pages.Count)
        {
            return;
        }
        DisplayPage(pages[pageNumber]);
    }

    private bool IsInPages(JournalPage page)
    {
        return pages.Contains(page);
    }
}
