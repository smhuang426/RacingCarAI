using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_example : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		MLP_ANN ann      = new MLP_ANN ();

		float[][] output = new float[2][];
		float[][] input  = new float[2][];
		uint[] list      = { 2, 3, 2 };
		float err        = 0;

		input[0]  = new float[]{ 1.0f, 1.0f };     //batch 1
		input[1]  = new float[]{ 0.1f, 0.1f };     //batch 2

		output[0] = new float[]{ 0.9f, 0.9f };     //batch 1 : expected output
		output[1] = new float[]{ 0.1f, 0.1f };     //batch 2 : expected output

		ann.CreateANN (list, ActiveFc.RELU, ActiveFc.Sigmoid, LossFc.MeanSquaredError);

		ann.Train (input, output, 0.1f, ref err);  // Just train once
	}
}
