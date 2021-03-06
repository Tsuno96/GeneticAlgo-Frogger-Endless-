﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicule : MonoBehaviour
{
    public List<Transform> path;

    public float speed;

    public int target = 0;

    public int sens;
    // Start is called before the first frame update

    private void Start()
    {
        target = 0;
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (target < path.Count)
            {
                transform.position = path[target].position + Vector3.up;
                path[target].gameObject.GetComponent<Cube>().empty = sens;
                if(target > 0)
                    path[target - 1].gameObject.GetComponent<Cube>().empty = -1;
                target++;
            }
            else
                Destroy(gameObject);

            yield return new WaitForSeconds(speed * LaneGenerator.SpeedCoef);
        }
    }
    
}
