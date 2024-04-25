using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane: MonoBehaviour
{
    //***********Flight data about plane***********//
    //Searchable Data ONLY
    public string tailNumber;


    //flight numbers can change. Can search up flights using tailNumber
    public string flightNumberArrival;
    public string flightNumberDeparture;
    public string originAirport;
    public string destinationAirport;

    //Flight scheduled arrival time & actual time
    public string scheduledArrivalTime;
    public string actualArrivalTime;
    public string arrivalDelay;

    //flight scheduled departure time and actual time
    public string scheduledDepartureTime;
    public string actualDepartureTime;
    public string departureDelay;

    
    public void PlaneArrivalImport(string[] rowData)
    {

        for(int i = 0; i < 8; i++)
        {

              switch (i)
            {
                case 0:
                case 1:
                    break;
                case 2:

                    flightNumberArrival = rowData[i];
                    break;
                case 3:
                    tailNumber = rowData[i];
                    break;
                case 4:
                    originAirport = rowData[i];
                    break;
                case 5:
                    scheduledArrivalTime = rowData[i].Replace(":", "");
                    break;
                case 6:
                    actualArrivalTime = rowData[i].Replace(":", "");
                    break;
                case 7:
                    arrivalDelay = rowData[i];
                    break;
                default:
                    break;

            }
        }
    }

    public void PlaneDepartureImport(string[] rowData)
    {

        for (int i = 0; i < 8; i++)
        {

            switch (i)
            {
                case 0:
                case 1:
                    break;
                case 2:

                    flightNumberDeparture = rowData[i];
                    break;
                case 3:
                    tailNumber = rowData[i];
                    break;
                case 4:
                    destinationAirport = rowData[i];
                    break;
                case 5:
                    scheduledDepartureTime = rowData[i].Replace(":", "");
                    break;
                case 6:
                    actualDepartureTime = rowData[i].Replace(":", "");
                    break;
                case 7:
                    departureDelay = rowData[i];
                    break;
                default:
                    break;

            }
        }
    }


    public void Update()
    {
        
    }
}
