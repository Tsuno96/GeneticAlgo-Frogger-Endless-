using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentLane;
    public int currentCube;

    public Cube[] neighbors;

    List<Lane> lstLanes;
    // Start is called before the first frame update
    void Start()
    {
        lstLanes = GameObject.FindGameObjectWithTag("LaneGenerator").GetComponent<LaneGenerator>().lstLanes;
        GetNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNeighbors()
    {
        neighbors = new Cube[5];

        if (currentCube > 0)
            neighbors[0] = lstLanes[currentLane].transform.GetChild(currentCube - 1).gameObject.GetComponent<Cube>();

        if (currentCube < lstLanes[currentLane].transform.childCount - 1)
            neighbors[1] = lstLanes[currentLane].transform.GetChild(currentCube + 1).gameObject.GetComponent<Cube>();

        if (currentCube > 0)
            neighbors[2] = lstLanes[currentLane + 1].transform.GetChild(currentCube - 1).gameObject.GetComponent<Cube>();

        neighbors[3] = lstLanes[currentLane + 1].transform.GetChild(currentCube).gameObject.GetComponent<Cube>();

        if (currentCube < lstLanes[currentLane + 1].transform.childCount - 1)
            neighbors[4] = lstLanes[currentLane + 1].transform.GetChild(currentCube + 1).gameObject.GetComponent<Cube>();
    }
}
