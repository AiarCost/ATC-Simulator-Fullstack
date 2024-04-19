using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Windows;

public class ImportDataFromCSV : MonoBehaviour
{

    public GameObject PlanePrefab;

    public Dictionary<string, GameObject> AirplaneList;


    // Start is called before the first frame update
    void Start()
    {
        AirplaneList = new Dictionary<string, GameObject>();
        string path = Application.dataPath;
        UnityEngine.Debug.Log(path);

        string DatabaseCollector = "\\Database\\ConsoleApp1\\bin\\Debug\\net6.0\\ConsoleApp1.exe";

        string[] ParsedString = path.Split(char.Parse("/"));
        string NewPath = "";

        UnityEngine.Debug.Log(Application.isEditor);

        if (Application.isEditor)
        {
            for (int x = 0; x < ParsedString.Length - 2; x++)
            {
                if (x == 0)
                    NewPath += ParsedString[x];
                else
                    NewPath += "\\" + ParsedString[x];

                UnityEngine.Debug.Log(NewPath);

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


        UnityEngine.Debug.Log(NewPath + DatabaseCollector);

        string DataFolderForFiles = Application.dataPath;
        UnityEngine.Debug.Log(DataFolderForFiles);

        File.Delete(DataFolderForFiles + "Detailed_Statistics_Arrivals.csv");
        File.Delete(DataFolderForFiles + "Detailed_Statistics_Departures.csv");

        Process process = Process.Start(NewPath + DatabaseCollector);
        process.WaitForExit();
        process.Dispose();
    }


}
