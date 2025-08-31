using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.IO;
using System.Data;

public static class ExcelReader
{
    // private void Start()
    // {
    //     string excelPath = Path.Combine(Application.dataPath, "ExcelTest.xlsx");
    //     string sheetName = "Sheet1";

    //     var excelRow = ReadExcel(excelPath, sheetName);

    //     for (int i = 1; i < excelRow.Count; i++)
    //     {
    //         Debug.Log(excelRow[i][0]);
    //     }
    // }
    public static DataRowCollection ReadExcel(string excelSheet)
    {
        string excelPath = Path.Combine(Application.dataPath, "ExcelTest.xlsx");

        using (FileStream fs = File.Open(excelPath, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);

            var result = excelDataReader.AsDataSet();

            return result.Tables[excelSheet].Rows;
        }
    }
}
