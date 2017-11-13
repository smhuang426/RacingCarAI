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

		return MLState.ML_SUCCESS;
	}

	/*-------------------------
	Private Methods
	-------------------------*/
	private MLState BatchBackward ( float[][] output, float[][] input, ref float[,] deltaW, ref float[] deltaB, ref float[][] prev_output )
	{
		int batchSize       = output.GetLength (0);
		float [,] tmpDeltaW = new float[numOut, numIn];
		float [] tmpDeltaB  = new float[numOut];

		FillFloatArrayToZero (ref deltaW);
		FillFloatArrayToZero (ref deltaB);

		for ( int i = 0; i < batchSize ; i++ )
		{
			if ( Backward ( output [i], input [i], (float)batchSize, ref tmpDeltaW, ref tmpDeltaB, ref prev_output [i] ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			FloatArraySum ( tmpDeltaB, deltaB, ref deltaB );
			FloatArraySum ( tmpDeltaW, deltaW, ref deltaW );
		}

		return MLState.ML_SUCCESS;
	}

	private MLState BatchForward ( float[][] input, ref float[][] output )
	{
		int batchSize = input.GetLength (0);

		for ( int i = 0; i < batchSize ; i++ )
		{
			if ( Forward (input [i], ref output [i]) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}
		}

		return MLState.ML_SUCCESS;
	}

	private MLState Backward ( float[] output, float[] input, float batchSize, ref float[,] deltaW, ref float[] deltaB, ref float[] prev_output )
	{
		if ( output.Length != numOut )
		{
			Debug.LogError ("[Backward] number of float[] output(" + output.Length + ") is not equal to numOut(" + numOut + ")");
			return MLState.ML_ERROR;
		}

		switch ( AF )
		{
		case ActiveFc.RELU:
			RELU ( output, ref output, MLDirection.Backward );
			break;
		case ActiveFc.Sigmoid:
			Sigmoid ( output, ref output, MLDirection.Backward );
			break;
		case ActiveFc.Tanh:
			Tanh ( output, ref output, MLDirection.Backward );
			break;
		default:
			break;
		}

		for (int i = 0; i < numOut; i++)
		{
			for (int j = 0; j < numIn; j++)
			{
				deltaW [i, j] = output [i] * input [j] / batchSize;
			}

			deltaB [i] = output [i] / batchSize;
		}

		FillFloatArrayToZero (ref prev_output);

		for (int i = 0; i < numIn; i++)
		{
			for (int j = 0; j < numOut; j++)
			{
				prev_output [i] += Weight [i, j] * output [j];
			}
		}

		return MLState.ML_SUCCESS;
	}

	private MLState Forward ( float[] input, ref float[] output )
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
			RELU ( output, ref output, MLDirection.Forward );
			break;
		case ActiveFc.Sigmoid:
			Sigmoid ( output, ref output, MLDirection.Forward );
			break;
		case ActiveFc.Tanh:
			Tanh ( output, ref output, MLDirection.Forward );
			break;
		default:
			break;
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
	private void RELU ( float[] input, ref float[] output, MLDirection dir )
	{
		if (dir == MLDirection.Forward)
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
		else
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
	}

	private void Sigmoid ( float[] input, ref float[] output, MLDirection dir )
	{
		if (dir == MLDirection.Forward)
		{
			for ( int i = 0; i < input.Length; i++ )
			{
				output [i] = 1.0f / ( 1.0f + Mathf.Exp ( -input [i] ) );
			}
		}
		else
		{
			for ( int i = 0; i < input.Length; i++ )
			{
				output [i] = input [i] * ( 1.0f - input[i] );
			}
		}
	}

	private void Tanh ( float[] input, ref float[] output, MLDirection dir )
	{
		if (dir == MLDirection.Forward)
		{
			for ( int i = 0; i < input.Length; i++ )
			{
				output [i] = 2.0f / ( 1.0f + Mathf.Exp ( -2.0f * input [i] ) ) - 1;
			}
		}
		else
		{
			for ( int i = 0; i < input.Length; i++ )
			{
				output [i] = 1.0f - input[i] * input[i];
			}
		}
	}
}
