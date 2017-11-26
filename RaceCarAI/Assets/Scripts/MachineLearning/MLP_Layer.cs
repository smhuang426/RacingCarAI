using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveFc
{
	None,
	RELU,
	Sigmoid,
	Tanh
}

public enum MLDirection
{
	Forward,
	Backward
}

public enum MLState
{
	ML_ERROR,
	ML_SUCCESS
}

public class MLP_Layer
{
	/*-------------------------
	Variables
	-------------------------*/
	int      numIn;
	int      numOut;

	float[,] Weight;
	float[]  bias;

	ActiveFc AF;

	float[][] layerInput;
	float[][] layerOutput;

	/*-------------------------
	Public Methods
	-------------------------*/
	public MLState Initialize ( int num_out, int num_in, ActiveFc AtFc )
	{
		if ( ( num_in <= 0 ) || ( num_out <= 0 ) )
		{
			Debug.LogError ("[MLP_Layer] number of input or output is error, numIn:" + numIn + " ,numOut:" + numOut);
			return MLState.ML_ERROR;
		}

		AF         = AtFc;

		numIn      = num_in;
		numOut     = num_out;

		Weight     = new float [numOut, numIn];
		bias       = new float [numOut];

		for (int i = 0; i < numOut; i++)
		{
			bias [i] = 0;

			for (int j = 0; j < numIn; j++)
			{
				Weight [i, j] = Random.value / numIn;
			}
		}

		Debug.Log ( "Create Layer with input:" + numIn + ", output:" + numOut );
		MLP_Print.PrintLayerParameter ( numOut, numIn, Weight, bias );
		Debug.Log ("");

		return MLState.ML_SUCCESS;
	}

	public MLState BatchBackward ( float[][] backErr, ref float[,] deltaW, ref float[] deltaB, ref float[][] prev_output )
	{
		if ( layerInput == null )
		{
			Debug.LogError ("[BatchBackward] layerInput is null");
			return MLState.ML_ERROR;
		}

		int batchSize       = backErr.GetLength (0);
		float [,] tmpDeltaW = new float[numOut, numIn];
		float [] tmpDeltaB  = new float[numOut];

		FillFloatArrayToZero (ref deltaW);
		FillFloatArrayToZero (ref deltaB);

		for ( int i = 0; i < batchSize ; i++ )
		{
			prev_output[i] = new float[numIn];

			MLP_Print.PrintArray (backErr[i], "backErr_batch"+i);

			if ( Backward ( layerOutput[i], backErr [i], layerInput [i], (float)batchSize, ref tmpDeltaW, ref tmpDeltaB, ref prev_output [i] ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			MLP_Print.PrintArray (layerInput[i], "layerInput_bach"+i);
			MLP_Print.PrintArray (prev_output[i], "prev_output");
			MLP_Print.PrintLayerParameter (numOut, numIn, tmpDeltaW, tmpDeltaB);

			FloatArraySum ( tmpDeltaB, deltaB, ref deltaB );
			FloatArraySum ( tmpDeltaW, deltaW, ref deltaW );
		}

		return MLState.ML_SUCCESS;
	}

	public MLState BatchForward ( float[][] input, ref float[][] output )
	{
		int batchSize = input.GetLength (0);

		layerInput = (float[][])input.Clone ();

		for ( int i = 0; i < batchSize ; i++ )
		{
			output [i] = new float[numOut];

			MLP_Print.PrintArray ( input[i], "layer_input_batch" + i );

			if ( Forward (input [i], ref output [i]) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			MLP_Print.PrintArray ( output[i], "layer_output_batch" + i );
		}

		layerOutput = (float[][])output.Clone ();

		return MLState.ML_SUCCESS;
	}

	public MLState Forward ( float[] input, ref float[] output )
	{
		if ( input.Length != numIn )
		{
			Debug.LogError ("[Forward] number of float[] input(" + input.Length + ") is not equal to numIn(" + numIn + ")");
			return MLState.ML_ERROR;
		}

		FillFloatArrayToZero (ref output);

		for (int i = 0; i < numOut; i++)
		{
			for (int j = 0; j < numIn; j++)
			{
				output [i] += Weight [i, j] * input [j];
			}

			output [i] += bias [i];
		}

		switch ( AF )
		{
		case ActiveFc.RELU:
			RELU ( output, ref output );
			break;
		case ActiveFc.Sigmoid:
			Sigmoid ( output, ref output );
			break;
		case ActiveFc.Tanh:
			Tanh ( output, ref output );
			break;
		default:
			break;
		}

		return MLState.ML_SUCCESS;
	}

	public int GetNumInput ()
	{
		return numIn;
	}

	public int GetNumOutput ()
	{
		return numOut;
	}

	public MLState Update ( float[,] deltaW,  float[] deltaB , float lrnRate )
	{
		for ( int i = 0; i < numOut; i++ )
		{
			for ( int j = 0; j < numIn; j++ )
			{
				Weight [i, j] = Weight [i, j] - lrnRate * deltaW [i, j];
			}

			bias [i] = bias [i] - lrnRate * bias [i];
		}

		return MLState.ML_SUCCESS;
	}

	/*-------------------------
	Private Methods
	-------------------------*/
	private MLState Backward ( float[] layerOut, float[] backErr, float[] input, float batchSize, ref float[,] deltaW, ref float[] deltaB, ref float[] prev_output )
	{
		if ( backErr.Length != numOut && layerOut.Length != numOut )
		{
			Debug.LogError ("[Backward] number of float[] backErr(" + backErr.Length + ") is not equal to numOut(" + numOut + ")");
			return MLState.ML_ERROR;
		}

		switch ( AF )
		{
		case ActiveFc.RELU:
			RELU_derivative ( backErr, ref backErr );
			break;
		case ActiveFc.Sigmoid:
			Sigmoid_derivative ( layerOut, backErr, ref backErr );
			break;
		case ActiveFc.Tanh:
			Tanh_derivative ( layerOut, backErr, ref backErr );
			break;
		default:
			break;
		}

		MLP_Print.PrintArray (backErr, "a");

		for (int i = 0; i < numOut; i++)
		{
			for (int j = 0; j < numIn; j++)
			{
				deltaW [i, j] = backErr [i] * input [j] / batchSize;
			}

			deltaB [i] = backErr [i] / batchSize;
		}

		FillFloatArrayToZero (ref prev_output);

		for (int i = 0; i < numIn; i++)
		{
			for (int j = 0; j < numOut; j++)
			{
				prev_output [i] += Weight [j, i] * backErr [j];
			}
		}

		return MLState.ML_SUCCESS;
	}

	private MLState FloatArraySum( float[] arr1, float[] arr2, ref float[] outArr )
	{
		if ( arr1.Length != arr2.Length )
		{
			Debug.LogError ("[FloatArraySum] Size of both array are not the same");
			return MLState.ML_ERROR;
		}

		for ( int i = 0; i < arr1.Length; i++ )
		{
			outArr [i] = arr1 [i] + arr2 [i];
		}

		return MLState.ML_SUCCESS;
	}

	private MLState FloatArraySum( float[,] arr1, float[,] arr2, ref float[,] outArr )
	{
		if ( arr1.GetLength(0) != arr2.GetLength(0) && arr1.GetLength(1) != arr2.GetLength(1) )
		{
			Debug.LogError ("[FloatArraySum] Size of both array are not the same");
			return MLState.ML_ERROR;
		}

		for ( int i = 0; i < arr1.GetLength(0); i++ )
		{
			for (int j = 0; j < arr1.GetLength (1); j++)
			{
				outArr [i, j] = arr1 [i, j] + arr2 [i, j];
			}
		}

		return MLState.ML_SUCCESS;
	}

	private void FillFloatArrayToZero( ref float[] arr )
	{
		for ( int i = 0; i < arr.Length; i++ )
		{
			arr [i] = 0;
		}
	}

	private void FillFloatArrayToZero( ref float[,] arr )
	{
		for ( int i = 0; i < arr.GetLength(0); i++ )
		{
			for (int j = 0; j < arr.GetLength (1); j++)
			{
				arr [i, j] = 0;
			}
		}
	}

	/*-------------------------
	Activation Function
	-------------------------*/
	private void RELU ( float[] input, ref float[] output )
	{
		for (int i = 0; i < input.Length; i++)
		{
			if (input [i] < 0)
			{
				output [i] = 0;
			}
			else
			{
				output [i] = input [i];
			}
		}
	}

	private void RELU_derivative ( float[] input, ref float[] output )
	{
		for (int i = 0; i < input.Length; i++)
		{
			if (input [i] < 0)
			{
				output [i] = 0;
			}
			else
			{
				output [i] = input [i];
			}
		}	
	}

	private void Sigmoid ( float[] input, ref float[] output )
	{
		for ( int i = 0; i < input.Length; i++ )
		{
			output [i] = 1.0f / ( 1.0f + Mathf.Exp ( -input [i] ) );
		}
	}

	private void Sigmoid_derivative ( float[] layerOut, float[] backErr, ref float[] output )
	{
		for ( int i = 0; i < backErr.Length; i++ )
		{
			output [i] = backErr [i] * ( layerOut[i] * ( 1.0f - layerOut[i] ) );
		}
	}

	private void Tanh ( float[] input, ref float[] output )
	{
		for ( int i = 0; i < input.Length; i++ )
		{
			output [i] = 2.0f / ( 1.0f + Mathf.Exp ( -2.0f * input [i] ) ) - 1;
		}
	}

	private void Tanh_derivative ( float[] layerOut, float[] backErr, ref float[] output )
	{
		for ( int i = 0; i < backErr.Length; i++ )
		{
			output [i] = backErr [i] * ( 1.0f - layerOut[i] * layerOut[i] );
		}
	}
}
