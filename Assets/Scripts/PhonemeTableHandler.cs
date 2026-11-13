using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PhonemeTableHandler : MonoBehaviour
{
    public GameObject tableRowPrefab;
    
    List<List<string>> answerKey = new List<List<string>>(); // load answerKey from .csv as 2D List
    public List<string> soundsLikeOptions;
    private int numCorrectRows = 0;
    
    // keep a list of the tablerow objects

    void Awake()
    {
        LoadAnswerKey();
        soundsLikeOptions = answerKey.Select(row => row[1]).ToList(); // second column of answerKey
    }
    
    void Start()
    {
        
    }

    public void OnRowUpdate(TableRow tableRow)
    {
        List<string> entries = tableRow.entries;
        if (IsRowCorrect(entries))
        {
            tableRow.DisableInput();
            tableRow.ChangeFont();
            RemoveAnswer(entries); // remove corresponding entry from answerKey, which only tracks unused answers
            MoveRowToTop(tableRow); // move this row to end of completed section at top
            numCorrectRows++;
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

    public void AddRow()
    {
        Debug.Log("adding row");
        Instantiate(tableRowPrefab, GetComponent<Transform>());
    }

    private void MoveRowToTop(TableRow tableRow)
    {
        tableRow.GetComponent<Transform>().SetSiblingIndex(numCorrectRows+1); // +1 to account for title row
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
