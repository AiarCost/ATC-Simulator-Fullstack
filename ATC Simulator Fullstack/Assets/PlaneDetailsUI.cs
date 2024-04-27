using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneDetailsUI : MonoBehaviour
{

    public GameObject DetailGO;
    //Searchable Data ONLY
    public TextMeshProUGUI tailNumber;


    //flight numbers can change. Can search up flights using tailNumber
    public TextMeshProUGUI flightNumberArrival;
    public TextMeshProUGUI flightNumberDeparture;
    public TextMeshProUGUI originAirport;
    public TextMeshProUGUI destinationAirport;

    //Flight scheduled arrival time & actual time
    public TextMeshProUGUI scheduledArrivalTime;
    public TextMeshProUGUI actualArrivalTime;
    public TextMeshProUGUI arrivalDelay;

    //flight scheduled departure time and actual time
    public TextMeshProUGUI scheduledDepartureTime;
    public TextMeshProUGUI actualDepartureTime;
    public TextMeshProUGUI departureDelay;

    Ray ray;
    RaycastHit hit;
    public void Start()
    {
        DetailGO.SetActive(false);
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.tag);

                if (hit.collider.CompareTag("Plane"))
                {
                    DetailGO.SetActive(true);

                    TextUpdate(hit.collider.gameObject.GetComponentInParent<Plane>()); 
                }
                else
                {
                    DetailGO.SetActive(false);
                }
            }
        }
    }

    public void TextUpdate (Plane plane)
    {
        flightNumberArrival.text = plane.flightNumberArrival.ToString ();
        flightNumberDeparture.text = plane.flightNumberDeparture.ToString ();
        originAirport.text = plane.originAirport;
        destinationAirport.text = plane.destinationAirport;
        scheduledArrivalTime.text = plane.scheduledArrivalTime.ToString ();
        actualArrivalTime.text = plane .actualArrivalTime.ToString ();
        arrivalDelay.text = plane .arrivalDelay.ToString ();
        scheduledDepartureTime.text = plane.scheduledDepartureTime .ToString ();
        actualDepartureTime.text = plane.actualDepartureTime.ToString ();
        departureDelay.text = plane .departureDelay.ToString ();

        tailNumber.text = plane.tailNumber;

    }
}
