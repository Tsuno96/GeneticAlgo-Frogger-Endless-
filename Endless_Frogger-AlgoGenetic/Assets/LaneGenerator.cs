using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator : MonoBehaviour
{
    public int nLanes;
    public Lane lanePrefab;
    public List<Lane> lstLanes;

    public static float SpeedCoef;
    [Range(0.01f, 1f)]
    public float pubSpeedCoef;

    public Vehicule vehiculePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        lstLanes = new List<Lane>();
        Lane start = Instantiate(lanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        start.name = "Lane0";
        //start.GetComponent<Spawner>().enabled = false;
        lstLanes.Add(start);

        for (int i =1; i<= nLanes;i++)
        {
            Lane l = Instantiate(lanePrefab, new Vector3(i, 0, 0), Quaternion.identity);
            l.InitLane(vehiculePrefab);
            l.name = "Lane" + i;
            lstLanes.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCoef = pubSpeedCoef;
    }
}
