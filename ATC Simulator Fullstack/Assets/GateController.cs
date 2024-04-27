using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    public GameObject[] Gates;
    public bool isPlanesWaiting = false;
    Queue<GameObject> GatesQueue = new Queue<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        //Gates = gameObject.GetComponentsInChildren<GameObject>();
    }

    public Transform AssignGate(GameObject Plane)
    {
        foreach (GameObject GateGO in Gates)
        {
            Gate gate = GateGO.GetComponent<Gate>(); 
            if (gate.isAvailable)
            {
                gate.PlaneTag = Plane.GetComponent<Plane>().tailNumber;
                gate.isAvailable = false;
                return GateGO.transform;
            } 
            
        }
        GatesQueue.Enqueue(Plane);
        isPlanesWaiting = true;
        return null;
    }

    public void UnassignGate(GameObject Plane)
    {
        foreach(GameObject GateGO in Gates)
        {
            Gate gate = GateGO.GetComponent<Gate>();
            if(gate.PlaneTag == Plane.GetComponent<Plane>().tailNumber)
            {
                gate.PlaneTag = "";
                gate.isAvailable = true;
                if(GatesQueue.Count > 0)
                {
                    GameObject NextPlane = GatesQueue.Dequeue();
                    AssignGate(NextPlane);
                }
            }
        }
    }
}
