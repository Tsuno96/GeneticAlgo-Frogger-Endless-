using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;

public class NeuralNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 5);

    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();

    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 4);

    public List<Matrix<float>> weights = new List<Matrix<float>>();

    public List<float> biases = new List<float>();

    //public float fitness;

    public void Initialise(int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();

        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {

            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);

            hiddenLayers.Add(f);

            biases.Add(Random.Range(-1f, 1f));

            //WEIGHTS
            if (i == 0)
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(5, hiddenNeuronCount);
                weights.Add(inputToH1);
            }

            Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            weights.Add(HiddenToHidden);

        }

        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 4);
        weights.Add(OutputWeight);
        biases.Add(Random.Range(-1f, 1f));

        RandomiseWeights();

    }
    private void RandomiseWeights()
    {

        for (int i = 0; i < weights.Count; i++)
        {

            for (int x = 0; x < weights[i].RowCount; x++)
            {

                for (int y = 0; y < weights[i].ColumnCount; y++)
                {

                    weights[i][x, y] = Random.Range(-1f, 1f);

                }

            }

        }

    }

    public Matrix<float> RunNetwork(int[] inputs)
    {

        for(int i = 0;i<inputs.Length;i++)
            inputLayer[0, i] = inputs[i];

        inputLayer = inputLayer.PointwiseTanh();
        //Debug.Log(inputLayer);
        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();

        return outputLayer;
    }

    private float Sigmoid(float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }

}
