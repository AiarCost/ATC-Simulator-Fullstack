using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class PlaneManager : MonoBehaviour
{
    public Dictionary<string, GameObject> PlaneList = new Dictionary<string, GameObject>();
    public List<GameObject> DeparturePlanes = null;
    public GameObject NextPlaneArrivalGO = null;
    public GameObject NextPlaneDepartureGO = null;

    public TextMeshProUGUI ArrivalDelayTimer;
    public TextMeshProUGUI DepartureDelayTimer;
    bool isNOTDelayTimeUpdated = true;

    public TimeCounter timeCounter;
    public Transform Test;
    public Transform ArrivalStartPos;
    public Transform DepartureStartPos;
    public Transform TakeoffStartPos;
    public Transform TakeoffEndPos;
    public GateController controller;


    public void Update()
    {
        if (isNOTDelayTimeUpdated)
        {
            DelayTimeUpdate();
            isNOTDelayTimeUpdated = false;
        }
        if(timeCounter.Timer >= NextPlaneArrivalGO.GetComponent<Plane>().actualArrivalTime - 5)
        {
            NextPlaneArrivalGO.SetActive(true);
            NextPlaneArrivalGO.transform.position = ArrivalStartPos.position;
            NextPlaneArrivalGO.GetComponent<Plane>().CurrentState = Plane.PlaneState.arrival;
            NextPlaneArrivalGO.GetComponent<Plane>().GateController = controller;
            NextPlaneArrivalGO.GetComponent<Plane>().PlaneManager = this;

            NextPlaneArrivalGO.GetComponent<NavMeshAgent>().destination = Test.position;
            NextPlaneArrivalGO.GetComponent<Plane>().takeOffCheckPoint = Test;
            NextPlaneArrivalGO = NextPlaneArrivalGO.GetComponent<Plane>().nextPlaneArrival;
        }
        if(NextPlaneDepartureGO != null && timeCounter.Timer >= NextPlaneDepartureGO.GetComponent<Plane>().actualDepartureTime - 5)
        {
            Debug.Log(NextPlaneDepartureGO.name);
            NextPlaneDepartureGO.GetComponent<Plane>().CurrentState = Plane.PlaneState.taxi;
            NextPlaneDepartureGO.GetComponent<NavMeshAgent>().destination = DepartureStartPos.position;
            NextPlaneDepartureGO.GetComponent<Plane>().GateController.UnassignGate(NextPlaneDepartureGO);
            NextPlaneDepartureGO = NextPlaneDepartureGO.GetComponent<Plane>().nextPlaneDeparture;

            
        }

    }


    public void ArrivalSorting()
    {
        GameObject headPlane = null;

        foreach(GameObject PlaneInfo in PlaneList.Values)
        {
            if (headPlane == null)
            {
                headPlane = PlaneInfo;
                continue;
            }

            Plane plane = PlaneInfo.GetComponent<Plane>();
            if (plane.actualArrivalTime != 0)
            {
                bool sorted = false;
                Plane nextPlane = headPlane.GetComponent<Plane>();
                Plane prevPlane = null;
                
                while (!sorted)
                {
                    if(nextPlane != null)
                    {
                        if (plane.actualArrivalTime > nextPlane.actualArrivalTime)
                        {
                            prevPlane = nextPlane;
                            if(nextPlane.nextPlaneArrival != null)
                                nextPlane = nextPlane.nextPlaneArrival.GetComponent<Plane>();
                            else
                            {
                                prevPlane.nextPlaneArrival = plane.gameObject;
                                plane.nextPlaneArrival = null;
                                sorted = true;
                            }
                        }
                        else if(plane.actualArrivalTime < nextPlane.actualArrivalTime)
                        {
                            plane.nextPlaneArrival = nextPlane.gameObject;
                            if (prevPlane != null)
                                prevPlane.nextPlaneArrival = plane.gameObject;
                            else
                                headPlane = plane.gameObject;
                            sorted = true;
                        }
                        else
                        {
                            plane.actualArrivalTime++;
                        }
                    }
                    else
                    {
                        prevPlane.nextPlaneArrival = plane.gameObject;
                        plane.nextPlaneArrival = null;
                        sorted = true;
                    }
                }
            }
        }
        NextPlaneArrivalGO = headPlane;
        Debug.Log("NextArrival is: " +  NextPlaneArrivalGO.GetComponent<Plane>().actualArrivalTime +" with plane: " + NextPlaneArrivalGO.name);
    }


    public void DepartureSorting()
    {
        GameObject headPlane = null;

        foreach (GameObject PlaneInfo in PlaneList.Values)
        {
            if (PlaneInfo.GetComponent<Plane>().actualArrivalTime > PlaneInfo.GetComponent<Plane>().actualDepartureTime)
                continue;

            if (headPlane == null)
            {
                headPlane = PlaneInfo;
                continue;
            }

            Plane plane = PlaneInfo.GetComponent<Plane>();
            if (plane.actualDepartureTime != 0 && plane.flightNumberArrival !=0)
            {
                bool sorted = false;
                Plane nextPlane = headPlane.GetComponent<Plane>();
                Plane prevPlane = null;

                while (!sorted)
                {
                    if (nextPlane != null)
                    {
                        if (plane.actualDepartureTime > nextPlane.actualDepartureTime)
                        {
                            prevPlane = nextPlane;
                            if (nextPlane.nextPlaneDeparture != null)
                                nextPlane = nextPlane.nextPlaneDeparture.GetComponent<Plane>();
                            else
                            {
                                prevPlane.nextPlaneDeparture = plane.gameObject;
                                plane.nextPlaneDeparture = null;
                                sorted = true;
                            }
                        }
                        else if (plane.actualDepartureTime < nextPlane.actualDepartureTime)
                        {
                            plane.nextPlaneDeparture = nextPlane.gameObject;
                            if (prevPlane != null)
                                prevPlane.nextPlaneDeparture = plane.gameObject;
                            else
                                headPlane = plane.gameObject;
                            sorted = true;
                        }
                        else
                        {
                            plane.actualDepartureTime++;
                        }
                    }
                    else
                    {
                        prevPlane.nextPlaneDeparture = plane.gameObject;
                        plane.nextPlaneDeparture = null;
                        sorted = true;
                    }
                }
            }
            else
            {
                
            }
        }
        NextPlaneDepartureGO = headPlane;
        Debug.Log("NextArrival is: " + NextPlaneDepartureGO.GetComponent<Plane>().actualDepartureTime + " with plane: " + NextPlaneDepartureGO.name);
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

             int num = GO.GetComponent<Plane>().arrivalDelay;
             ArrivalDelay += num;
        }

        ArrivalDelay /= values.Count;

        //departure delay
        foreach(GameObject GO in values)
        {

             int num = GO.GetComponent<Plane>().departureDelay;
             DepartureDelay += num;

        }

        DepartureDelay /= values.Count;


        ArrivalDelayTimer.text = ArrivalDelay.ToString();
        DepartureDelayTimer.text = DepartureDelay.ToString();

        Debug.Log("ArrivalDelay: "+ ArrivalDelay + "\nDepartureDelay: " +DepartureDelay);
    }

    public void TimerCountStart()
    {
        int hours;
        int minutes;
        string startingTime = NextPlaneArrivalGO.GetComponent<Plane>().actualArrivalTime.ToString();
        if (startingTime.Length == 4)
        {
             hours = int.Parse(startingTime.Substring(0, 2));
             minutes = int.Parse(startingTime.Substring(2, 4));

        }
        else
        {
             hours = int.Parse(startingTime.Substring(0, 1));
             minutes = int.Parse(startingTime.Substring(1, 3));
        }

        timeCounter.elapsedTime = (60*hours) + minutes -30;
    }
}