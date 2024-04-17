using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ImportDataFromCSV : MonoBehaviour
{

    public GameObject PlanePrefab;

    public TextAsset ArrivalCSV;
    public TextAsset DepartureCSV;

    public Dictionary<string, GameObject> AirplaneList;

    // Start is called before the first frame update
    void Start()
    {
        AirplaneList = new Dictionary<string, GameObject>();
        //string path = Application.dataPath + ""
        Resources.Load("ConsoleApp1.exe");

       Process process = Process.Start("C:\\Users\\Arian\\OneDrive\\Desktop\\Projects\\Visual Studios Projects\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\net6.0\\ConsoleApp1.exe");
       process.WaitForExit();
       process.Close();
    }


}
