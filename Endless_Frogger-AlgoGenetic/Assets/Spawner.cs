using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<Transform> lstPath;
    public Vehicule vehiculePrefab;

    private float speed;
    private void Update()
    {
        
    }

    public void SetPath()
    {
        speed = Random.Range(0.6f, 1f);
        lstPath = new List<Transform>();

        if(GetComponent<Lane>().sens == 0)
            for (int i = 0; i < transform.childCount; i++)
                lstPath.Add(transform.GetChild(i));
        else
            for (int i = transform.childCount - 1; i >= 0; i--)
                lstPath.Add(transform.GetChild(i));

        StartCoroutine("SpawnVehicule");
    }
    IEnumerator SpawnVehicule()
    {
        yield return new WaitForSeconds(Random.Range(1f, 10f) * LaneGenerator.SpeedCoef);
        while (true)
        {
            Vehicule vehicule;
            if (GetComponent<Lane>().sens == 0)
                vehicule = Instantiate(vehiculePrefab, new Vector3(0, -1, 0), vehiculePrefab.transform.rotation);
            else
                vehicule = Instantiate(vehiculePrefab, new Vector3(0, -1, 0), vehiculePrefab.transform.rotation);
            vehicule.path = lstPath;
            vehicule.speed = speed;
            vehicule.sens = GetComponent<Lane>().sens;
            yield return new WaitForSeconds(Random.Range(4f, 20f) * LaneGenerator.SpeedCoef);
        }
    }



}
