using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImportPlaneData : MonoBehaviour
{
    public GameObject PlanePrefab;
    public PlaneData PlaneData;

    TextAsset ArrivalTextImport;
    string[] ArrivalRows;

    public void ImportData()
    {
        AssetDatabase.Refresh();

        ArrivalTextImport = Resources.Load<TextAsset>("Detailed_Statistics_Arrivals");


        ArrivalRows = ArrivalTextImport.text.Split("\n");

        for( int i = 0; i < ArrivalRows.Length; i++)
        {
            string[] row = ArrivalRows[i].Split(",");

            Debug.Log(row.Length);

            GameObject NewPlane = Instantiate(PlanePrefab);
            NewPlane.GetComponent<Plane>().PlaneImport(row);
        }

        Debug.Log(ArrivalRows);
    }

}
