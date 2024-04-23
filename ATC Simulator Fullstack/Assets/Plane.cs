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
    int flightNumberDeparture;
    public string originAirport;
    string destinationAirport;

    //Flight scheduled arrival time & actual time
    public string scheduledArrivalTime;
    public string actualArrivalTime;
    public string arrivalDelay;

    //flight scheduled departure time and actual time
    int scheduledDepartureTime;
    int actualDepartureTime;
    string departureDelay;

    
    public void PlaneImport(string[] rowData)
    {
        for(int i = 0; i < 6; i++)
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
                    scheduledArrivalTime = rowData[i];
                    break;
                case 6:
                    Debug.Log(rowData[i]);
                    actualArrivalTime = rowData[i];
                    break;
                case 7:
                    arrivalDelay = rowData[i];
                    break;
                default:
                    break;

            }
        }
    }
}
