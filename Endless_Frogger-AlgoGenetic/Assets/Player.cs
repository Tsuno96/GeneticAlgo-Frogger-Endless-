using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public class Player : MonoBehaviour
{
    public int currentLane;
    public int currentCube;

    public Cube[] neighbors;
    public int[] inputs;
    public int steps;

    public Matrix<float> inputLayers;

    public bool isDead;
    List<Lane> lstLanes;

    NeuralNetwork neuralNetwork;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        lstLanes = GameObject.FindGameObjectWithTag("LaneGenerator").GetComponent<LaneGenerator>().lstLanes;
        //GetNeighbors();
        StartCoroutine("MakeDecision");

        neuralNetwork = gameObject.AddComponent<NeuralNetwork>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MakeDecision()
    {
        yield return new WaitForSeconds(2f);

        while (currentLane != 10 && steps != 20 && !isDead)
        {
            GetNeighbors();

            Matrix<float> outputs = neuralNetwork.RunNetwork(inputs);
            //Debug.Log(outputs);

            int move = MaxOutputs(outputs);
            ChooseMove(move);

            CheckDeath();

            steps++;

            yield return new WaitForSeconds(1f);
        }
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

        SetInputs();
    }

    void SetInputs()
    {
        inputs = new int[5];

        if (neighbors[0] == null)
            inputs[0] = 1;
        else
        {
            if (neighbors[0].empty == -1)
                inputs[0] = 0;
            else
                inputs[0] = 1;
        }

        if (neighbors[1] == null)
            inputs[1] = 1;
        else
        {
            if (neighbors[1].empty == -1)
                inputs[1] = 0;
            else
                inputs[1] = 1;
        }

        if (neighbors[2] == null)
            inputs[2] = 1;
        else
        {
            if (neighbors[2].empty == -1 || neighbors[2].empty == 1)
                inputs[2] = 0;
            else
                inputs[2] = 1;
        }

        if (neighbors[3] == null)
            inputs[3] = 1;
        else
        {
            if (neighbors[3].empty == -1)
                inputs[3] = 0;
            else
                inputs[3] = 1;
        }

        if (neighbors[4] == null)
            inputs[4] = 1;
        else
        {
            if (neighbors[4].empty == -1 || neighbors[4].empty == 0)
                inputs[4] = 0;
            else
                inputs[4] = 1;
        }
    }

    void ChooseMove(int index)
    {
        switch(index)
        {
            case 0: MoveLeft();
                break;
            case 1: MoveRight();
                break;
            case 3: MoveForward();
                break;
            default: break;
        }
    }
    int MaxOutputs(Matrix<float> outputs)
    {
        float max = -2;
        int index = -1;
        for (int i = 0; i < outputs.ColumnCount; i++)
            if (outputs[0, i] > max)
            {
                index = i;
                max = outputs[0, i];
            }
        return index;
    }
    void MoveForward()
    {
        transform.position += new Vector3(1,0,0);
        currentLane++;
    }
    void MoveLeft()
    {
        transform.position += new Vector3(0, 0, 1);
        currentCube--;
    }
    void MoveRight()
    {

            transform.position += new Vector3(0, 0, -1);
            currentCube++;
    }

    void CheckDeath()
    {
        if (currentCube < 0 || currentCube > 9)
        {
            Debug.Log("Dead");
            isDead = true;
            GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Dead");
        isDead = true;
        GetComponent<Renderer>().enabled = false;
    }
}
