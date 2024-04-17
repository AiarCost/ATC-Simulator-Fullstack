using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    //***********Flight data about plane***********//
    //Searchable Data ONLY
    string tailNumber;


    //flight numbers can change. Can search up flights using tailNumber
    int flightNumberArrival;
    int flightNumberDeparture;
    string originAirport;
    string destinationAirport;

    //Flight scheduled arrival time & actual time
    int scheduledArrivalTime;
    int actualArrivalTime;
    int arrivalDelay;

    //flight scheduled departure time and actual time
    int scheduledDepartureTime;
    int actualDepartureTime;
    int departureDelay;

    //Flight Taxi time
    int planeTaxiTime;

    

}
