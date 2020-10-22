using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator : MonoBehaviour
{
    public int nLanes;
    public Lane lanePrefab;
    List<Lane> lstLanes;
    // Start is called before the first frame update
    void Awake()
    {
        lstLanes = new List<Lane>();
        for(int i =0; i< nLanes;i++)
        {
            Lane l = Instantiate(lanePrefab, new Vector3(i, 0, 0), Quaternion.identity);
            l.name = "Lane" + i;
            lstLanes.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
