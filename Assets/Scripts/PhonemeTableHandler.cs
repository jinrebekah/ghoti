using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PhonemeTableHandler : MonoBehaviour
{
    public GameObject tableRowPrefab;
    
    List<List<string>> answerKey = new List<List<string>>(); // load answerKey from .csv as 2D List
    private List<List<string>> completeAnswerKey;
    public List<string> soundsLikeOptions;
    public int numCorrectRows = 0;
    private int maxRows = 10; // for playing stage
    
    // keep a list of the tablerow objects
    public List<TableRow> tableRowList;
    public int numRows;

    void Awake()
    {
        tableRowList = GetComponentsInChildren<TableRow>().ToList();
        numRows = tableRowList.Count;
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
            GameManager.Instance.rowsSolved++;
        }
    }

    public void UpdateSoundsLikeOptions()
    {
        soundsLikeOptions = answerKey.Select(row => row[1]).ToList();
		List<string> allowedLetters;

		switch (GameManager.Instance.gameState)
		{
		case GameManager.GameState.Tutorial:
    		allowedLetters = new List<string> {"GH", "O", "TI"};
    		break;
		case GameManager.GameState.Playing:
			allowedLetters = new List<string> {"LL", "DD", "WR", "S", "Y", "OA"};
    		break;
		case GameManager.GameState.Ending:
			allowedLetters = new List<string> {"KN", "OA", "EA"};
    		break;
		default:
            allowedLetters = new List<string>();
    		break;
		}
		soundsLikeOptions = answerKey
    		.Where(row => allowedLetters.Contains(row[0]))
    		.Select(row => row[1])
    		.ToList();
		// soundsLikeOptions = soundsLikeOptions.Where(option => allowedOptions.Contains(option)).ToList();
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

    public void AddRow()
    {
        Debug.Log("adding row");
        if (numRows < maxRows)
        {
            TableRow newRow = Instantiate(tableRowPrefab, transform).GetComponent<TableRow>();
            tableRowList.Add(newRow);
            UpdateSoundsLikeOptions();
            numRows++;
        }
    }

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
