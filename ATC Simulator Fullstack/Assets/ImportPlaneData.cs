using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImportPlaneData : MonoBehaviour
{
    public GameObject PlanePrefab;
    public PlaneData PlaneData;

    public PlaneManager PlaneManager;

    TextAsset ArrivalTextImport;
    TextAsset DepartureTextImport;
    string[] ArrivalRows;
    string[] DepartureRows;


    public void ImportData()
    {
        //AssetDatabase.Refresh();

        ArrivalTextImport = Resources.Load<TextAsset>("Detailed_Statistics_Arrivals");


        ArrivalRows = ArrivalTextImport.text.Split("\n");

        for( int i = 0; i < ArrivalRows.Length; i++)
        {
            string[] row = ArrivalRows[i].Split(",");

            if (row.Length < 8)
                continue;

            //If plane is already in system, continue onwards to next row
            if (PlaneManager.PlaneList.ContainsKey(row[3]))
                continue;

            //Create new plane and add new data
            GameObject NewPlane = Instantiate(PlanePrefab);
            NewPlane.name = "PlaneGO" + i.ToString();
            NewPlane.GetComponent<Plane>().PlaneArrivalImport(row);
            NewPlane.SetActive(false);

            if (PlaneManager.PlaneList.ContainsKey(row[3]))
                Destroy(NewPlane);

            else
                PlaneManager.PlaneList.Add(row[3], NewPlane);
        }

        //Departure Data Import
        DepartureTextImport = Resources.Load<TextAsset>("Detailed_Statistics_Departures");

        DepartureRows = DepartureTextImport.text.Split("\n");

        for (int i = 0; i < DepartureRows.Length; i++)
        {
            string[] row = DepartureRows[i].Split(",");
            if (row.Length < 8)
                continue;


            //If plane is already in system, import data already
            if (PlaneManager.PlaneList.ContainsKey(row[3]))
            {
                PlaneManager.PlaneList.TryGetValue(row[3], out GameObject Plane);
                Plane.GetComponent<Plane>().PlaneDepartureImport(row);

            }
            else
            {
                //If no plane is in system yet, create new one
                GameObject NewPlane = Instantiate(PlanePrefab);
                NewPlane.name = "PlaneGO" + i.ToString();
                NewPlane.GetComponent<Plane>().PlaneDepartureImport(row);
                PlaneManager.PlaneList.Add(row[3], NewPlane);
                NewPlane.SetActive(false);
            }
        }


        PlaneManager.ArrivalSorting();
        PlaneManager.DepartureSorting();
        //PlaneManager.TimerCountStart();

    }

}
