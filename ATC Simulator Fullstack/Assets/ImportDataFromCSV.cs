using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Windows;

public class ImportDataFromCSV : MonoBehaviour
{

    public GameObject PlanePrefab;

    public ImportPlaneData PlaneData;

    public Dictionary<string, GameObject> AirplaneList;


    // Start is called before the first frame update
    void Start()
    {
        AirplaneList = new Dictionary<string, GameObject>();
        string path = Application.dataPath;

        string DatabaseCollector = "\\Database\\ConsoleApp1\\bin\\Debug\\net6.0\\ConsoleApp1.exe";

        string[] ParsedString = path.Split(char.Parse("/"));
        string NewPath = "";

        if (Application.isEditor)
        {
            for (int x = 0; x < ParsedString.Length - 2; x++)
            {
                if (x == 0)
                    NewPath += ParsedString[x];
                else
                    NewPath += "\\" + ParsedString[x];

            }
        }
        else
        {
            for (int x = 0; x < ParsedString.Length - 1; x++)
            {
                if (x == 0)
                    NewPath += ParsedString[x];
                else
                    NewPath += "\\" + ParsedString[x];
            }
        }



        string DataFolderForFiles = Application.dataPath;

        UnityEngine.Debug.Log("NewPath Location is: " + NewPath + DatabaseCollector);
        
        Process process = Process.Start(NewPath + DatabaseCollector);
        process.WaitForExit();
        process.Dispose();


        PlaneData.ImportData();
    }


}
