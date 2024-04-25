using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class PlaneManager : MonoBehaviour
{
    public Dictionary<string, GameObject> PlaneList = new Dictionary<string, GameObject>();

    public TextMeshProUGUI ArrivalDelayTimer;
    public TextMeshProUGUI DepartureDelayTimer;
    bool isNOTDelayTimeUpdated = true;

    public int MilitaryTimeMinutes = 0;
    public int MilitaryTimeHours = 0;


    public void Update()
    {
        if(MilitaryTimeMinutes >= 60)
        {
            MilitaryTimeMinutes = 0;
            MilitaryTimeHours++;
        }

        MilitaryTimeMinutes++;

        if (isNOTDelayTimeUpdated)
        {
            DelayTimeUpdate();
            isNOTDelayTimeUpdated = false;
        }


    }


    public void DelayTimeUpdate()
    {
        int ArrivalDelay = 0;
        int DepartureDelay = 0;
        ICollection<GameObject> values = PlaneList.Values;

        Debug.Log(values.Count);

        //Arrival Delay Check
        foreach(GameObject GO in values)
        {
            Match match = Regex.Match(GO.GetComponent<Plane>().arrivalDelay, @"-?\d+");
            if (match.Success)
            {
                int num = int.Parse(match.Value);
                ArrivalDelay += num;

            }
            else
                Debug.Log("There is an error with a value for the avg arrival delay!");

        }

        ArrivalDelay /= values.Count;

        //departure delay
        foreach(GameObject GO in values)
        {
            Match match = Regex.Match(GO.GetComponent<Plane>().departureDelay, @"-?\d+");
            if (match.Success)
            {
                int num = int.Parse(match.Value);
                DepartureDelay += num;
            }
            else
                Debug.Log("There is an error with a value for the avg departure delay!");
        }

        DepartureDelay /= values.Count;


        ArrivalDelayTimer.text = ArrivalDelay.ToString();
        DepartureDelayTimer.text = DepartureDelay.ToString();

        Debug.Log("ArrivalDelay: "+ ArrivalDelay + "\nDepartureDelay: " +DepartureDelay);
    }
}
