using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public int sens;
    void Start()
    {
        sens = Random.Range(0, 2);
    }
}
