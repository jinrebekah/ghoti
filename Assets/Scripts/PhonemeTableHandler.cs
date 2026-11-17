using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PhonemeTableHandler : MonoBehaviour
{
    // public GameObject tableRowPrefab;
    
    List<List<string>> answerKey = new List<List<string>>(); // load answerKey from .csv as 2D List
    public List<string> soundsLikeOptions;
    private int numCorrectRows = 0;
    
    // keep a list of the tablerow objects
    public List<TableRow> tableRowList;

    void Awake()
    {
        tableRowList = GetComponentsInChildren<TableRow>().ToList();
    }
    void Start()
    {
        LoadAnswerKey();
        // soundsLikeOptions = answerKey.Select(row => row[1]).ToList(); // second column of answerKey
        // soundsLikeOptions.Insert(0, "");
        UpdateSoundsLikeOptions();

    }

    public void OnRowUpdate(TableRow tableRow)
    {
        List<string> entries = tableRow.entries;
        if (IsRowCorrect(entries))
        {
            tableRow.solved = true;
            tableRow.DisableInput();
            tableRow.ChangeFont();
            RemoveAnswer(entries); // remove corresponding entry from answerKey, which only tracks unused answers
            UpdateSoundsLikeOptions();
            MoveRowToTop(tableRow); // move this row to end of completed section at top
            numCorrectRows++;
        }
    }

    public void UpdateSoundsLikeOptions()
    {
        soundsLikeOptions = answerKey.Select(row => row[1]).ToList();
        if (GameManager.Instance.gameState == GameManager.GameState.Tutorial)
        {
            soundsLikeOptions = soundsLikeOptions
                .Where(option => option == "F in Fish"
                                 || option == "I in Insect"
                                 || option == "SH in SHell")
                .ToList();
        } // stupid workaround let's fix this later, only allow these three options to be displayed during tutorial
        soundsLikeOptions.Insert(0, "");
        
        foreach (TableRow tableRow in tableRowList)
        {
            if (!tableRow.solved) // so it doesn't mess with what the solved ones are displaying
            {
                tableRow.SetSoundsLikeOptions(soundsLikeOptions);   
            }
        }
    }

    public bool IsRowCorrect(List<string> entries)
    {
        bool isCorrect = answerKey.Any(answer => answer.SequenceEqual(entries));
        return isCorrect;
    }

    private void RemoveAnswer(List<string> entries)
    {
        int index = answerKey.FindIndex(answer => answer.SequenceEqual(entries));
        answerKey.RemoveAt(index);
    }

    // public void AddRow()
    // {
    //     Debug.Log("adding row");
    //     TableRow newRow = Instantiate(tableRowPrefab, transform).GetComponent<TableRow>();
    //     tableRowList.Add(newRow);
    // }

    private void MoveRowToTop(TableRow tableRow)
    {
        tableRow.GetComponent<Transform>().SetSiblingIndex(numCorrectRows+1); // +1 to account for title row
        tableRowList.Remove(tableRow);
        tableRowList.Insert(0, tableRow);
    }
    
    private void LoadAnswerKey() // copied lol, loads answerKey from answerKey.csv
    {
        TextAsset file = Resources.Load<TextAsset>("answerKey");
        string[] lines = file.text.Trim().Split('\n');
        answerKey.Clear();
        foreach (string line in lines)
        {
            answerKey.Add(new List<string>(line.Trim().Split(',')));
        }
    }
}
