using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class Plane: MonoBehaviour
{
    //***********Flight data about plane***********//
    //Searchable Data ONLY
    public string tailNumber = null;

    public enum PlaneState
    {
        arrival,
        departure,
        taxi,
        gate
    }

    public PlaneState CurrentState;
    //flight numbers can change. Can search up flights using tailNumber
    public int flightNumberArrival = 0;
    public int flightNumberDeparture = 0;
    public string originAirport = null;
    public string destinationAirport = null;

    //Flight scheduled arrival time & actual time
    public int scheduledArrivalTime = 0;
    public int actualArrivalTime = 0;
    public int arrivalDelay = 0;

    //flight scheduled departure time and actual time
    public int scheduledDepartureTime = 0;
    public int actualDepartureTime = 0;
    public int departureDelay = 0;

    private int TakeoffStage = 0;
    public bool isAtGate = false;
    public bool isTaxiForDeparture = false;
    public bool isLineUpForDeparture = false;
    private NavMeshAgent NavAgent;
    public GateController GateController;
    public PlaneManager PlaneManager;
    private float PlaneSpeed = 1f;
    public float SpeedScale = 1f;

    public Transform takeOffCheckPoint;

    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Player has gotten to the end of the runway and requires a gate to proceed to
        if(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 1f && CurrentState == PlaneState.arrival) 
        {
            NavAgent.destination = GateController.AssignGate(gameObject).position;
            NavAgent.speed = 2;
            CurrentState = PlaneState.gate;
            nextPlaneArrival = null;
        }
        if(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 5f && CurrentState == PlaneState.arrival)
        {
            //NavAgent.speed = Mathf.Lerp(50, 2, Mathf.Clamp(PlaneSpeed - Time.deltaTime * SpeedScale, 0,1));
        }
        if(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 1f && CurrentState == PlaneState.gate && destinationAirport == null)
        {
            GateController.UnassignGate(gameObject);
            Destroy(gameObject);
        }
        if(CurrentState == PlaneState.departure)
        {
            if (Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 1f && !isLineUpForDeparture)
                isLineUpForDeparture = true;

            if (isLineUpForDeparture)
            {
                NavAgent.speed = 50;
                //NavAgent.speed = Mathf.Lerp(2, 50, Mathf.Clamp(PlaneSpeed + Time.deltaTime * SpeedScale, 0, 1));
                takeOffCheckPoint = GameObject.FindGameObjectWithTag("EndTakeoff").transform;
                NavAgent.destination = takeOffCheckPoint.position;
                if (Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 1f)
                    Destroy(gameObject);
            }

        }
        if(CurrentState == PlaneState.gate && !isAtGate)
        {
            PlaneManager.DeparturePlanes.Add(gameObject);
            isAtGate = true;
        }
        if(CurrentState == PlaneState.taxi && isAtGate && NavAgent.remainingDistance<1f)
        {
            takeOffCheckPoint = GameObject.FindGameObjectWithTag("StartTakeoff").transform;
            NavAgent.destination = takeOffCheckPoint.position;

            Debug.Log(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position));
            isAtGate = false;
            isTaxiForDeparture = true;

        }
        if(CurrentState == PlaneState.taxi && isTaxiForDeparture)
        {
            Debug.Log(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position));
            if(Vector3.Distance(gameObject.transform.position, takeOffCheckPoint.position) < 1f)
            {
                Debug.Log("It is lining up");
                takeOffCheckPoint = GameObject.FindGameObjectWithTag("LineUp").transform;
                NavAgent.destination = takeOffCheckPoint.position;
                CurrentState = PlaneState.departure;
            }
        }
    }

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

                    Match flightNumInt = Regex.Match(rowData[i], @"-?\d+");
                    if (flightNumInt.Success)
                    {
                        int num = int.Parse(flightNumInt.Value);
                        flightNumberArrival = num;
                    }
                    break;
                case 3:
                    tailNumber = rowData[i];
                    break;
                case 4:
                    originAirport = rowData[i];
                    break;
                case 5:
                    rowData[i] = rowData[i].Replace(":", "");

                    Match scheduledArrInt = Regex.Match(rowData[i], @"-?\d+");
                    if (scheduledArrInt.Success)
                    {   
                        int num = int.Parse(scheduledArrInt.Value);
                        scheduledArrivalTime = num;
                    }
                    break;
                case 6:
                    rowData[i] = rowData[i].Replace(":", "");
                    Match actualArrInt = Regex.Match(rowData[i], @"-?\d+");
                    if (actualArrInt.Success)
                    {
                        int num = int.Parse(actualArrInt.Value);
                        actualArrivalTime = num;
                    }
                    break;
                case 7:
                    Match arrDelayInt = Regex.Match(rowData[i], @"-?\d+");
                    if (arrDelayInt.Success)
                    {
                        int num = int.Parse(arrDelayInt.Value);
                        arrivalDelay = num;
                    }
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

                    Match flightDepInt = Regex.Match(rowData[i], @"-?\d+");
                    if (flightDepInt.Success)
                    {
                        int num = int.Parse(flightDepInt.Value);
                        flightNumberDeparture = num;
                    }
                    break;
                case 3:
                    tailNumber = rowData[i];
                    break;
                case 4:
                    destinationAirport = rowData[i];
                    break;
                case 5:
                    rowData[i] = rowData[i].Replace(":", "");

                    Match schDepInt = Regex.Match(rowData[i], @"-?\d+");
                    if (schDepInt.Success)
                    {
                        int num = int.Parse(schDepInt.Value);
                        scheduledDepartureTime = num;
                    }
                    break;
                case 6:
                    rowData[i] = rowData[i].Replace(":", "");

                    Match actDepInt = Regex.Match(rowData[i], @"-?\d+");
                    if (actDepInt.Success)
                    {
                        int num = int.Parse(actDepInt.Value);
                        actualDepartureTime = num;
                    }
                    break;
                case 7:
                    Match depDelInt = Regex.Match(rowData[i], @"-?\d+");
                    if (depDelInt.Success)
                    {
                        int num = int.Parse(depDelInt.Value);
                        departureDelay = num;
                    }
                    break;
                default:
                    break;

            }
        }
    }



    public GameObject nextPlaneArrival = null;
    public GameObject nextPlaneDeparture = null;

}
