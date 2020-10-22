using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicule : MonoBehaviour
{
    public List<Transform> path;

    public float speed;

    public int target = 0;
    // Start is called before the first frame update

    private void Start()
    {
        target = 0;
        StartCoroutine("Move");
    }
    void Update()
    {

    }

    IEnumerator Move()
    {
        while (true)
        {
            if (target < path.Count)
            {
                transform.position = path[target].position + Vector3.up;
                target++;
            }
            else
                Destroy(gameObject);

            yield return new WaitForSeconds(speed);
        }
    }
    
}
