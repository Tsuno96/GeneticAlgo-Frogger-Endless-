using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

public class Player : MonoBehaviour, System.IComparable<Player>
{
    public int currentLane;
    public int currentCube;

    public Cube[] neighbors;
    public int[] inputs;

    int steps;

    public Matrix<float> inputLayers;

    public bool isDead;
    List<Lane> lstLanes;

    public NeuralNetwork neuralNetwork;


    // Start is called before the first frame update
    void Start()
    {
        steps = 0;
        isDead = false;
        lstLanes = GameObject.FindGameObjectWithTag("LaneGenerator").GetComponent<LaneGenerator>().lstLanes;

        //GetNeighbors();
        
        StartCoroutine("MakeDecision");

    }

    public NeuralNetwork SetRandomNN()
    {
        neuralNetwork = gameObject.AddComponent<NeuralNetwork>();
        neuralNetwork.Initialise(2, 4);

        return neuralNetwork;
    }

    public void SetNN(NeuralNetwork nn)
    {
        neuralNetwork = nn;
    }

    IEnumerator MakeDecision()
    {
        
        yield return new WaitForSeconds(2f * LaneGenerator.SpeedCoef);

        while (currentLane < lstLanes.Count - 1 && steps != PlayerManager.instance.maxSteps && !isDead)
        {
            GetNeighbors();

            Matrix<float> outputs = neuralNetwork.RunNetwork(inputs);
            //Debug.Log(outputs);

            int move = MaxOutputs(outputs);
            ChooseMove(move);

            CheckDeath();

            steps++;

            if (steps == PlayerManager.instance.maxSteps)
                PlayerManager.instance.nDeadPlayers++;

            yield return new WaitForSeconds(0.5f * LaneGenerator.SpeedCoef);
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
            Dead();
        }

        if(currentLane ==0 && steps >= (PlayerManager.instance.maxSteps *0.33))
        {
            Dead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Dead();
    }

    public int CompareTo(Player other)
    {
        if (other == null) return 0;

        return other.currentLane.CompareTo(currentLane);
    }


    public void ResetPlayer()
    {
        int z = UnityEngine.Random.Range(0, -10); ;
        transform.position = new Vector3(0, 1, z);
        currentCube = -z;
        currentLane = 0;
        isDead = false;
        steps = 0;
        gameObject.SetActive(true);
        StartCoroutine("MakeDecision");
    }

    void Dead()
    {
        isDead = true;
        PlayerManager.instance.nDeadPlayers++;
        gameObject.SetActive(false);
    }


}
