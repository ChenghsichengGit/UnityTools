using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Text nameLabel;
    [SerializeField] private Text textLabel;
    [SerializeField] private string excleSheetName;
    [SerializeField] private float textSpeed;

    private int index;
    private bool textEnd = true;
    private bool skipText;

    List<string> textList = new List<string>();
    List<string> nameList = new List<string>();

    private void OnEnable()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextText();
        }
    }

    void StartDialogue()
    {
        GetTextFormFile();
        NextText();
    }

    void NextText()
    {
        if (index >= textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }

        if (!textEnd)
        {
            skipText = true;
            return;
        }

        StartCoroutine(SetText());
    }

    void GetTextFormFile()
    {
        index = 0;
        GetExcelDate();
    }

    void GetExcelDate()
    {
        var excelRow = ExcelReader.ReadExcel(excleSheetName);

        textList.Clear();
        nameList.Clear();


        for (int i = 1; i <= excelRow.Count - 1; i++)
        {
            nameList.Add(excelRow[i][0].ToString());
        }

        for (int i = 1; i <= excelRow.Count - 1; i++)
        {
            textList.Add(excelRow[i][1].ToString());
        }

    }

    IEnumerator SetText()
    {
        textEnd = false;
        textLabel.text = "";
        nameLabel.text = nameList[index];

        int letter = 0;

        while (letter < textList[index].Length - 1 && !skipText)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }

        textLabel.text = textList[index];
        index++;
        skipText = false;
        textEnd = true;
        yield return 0;
    }
}
