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
	private MLState Backward ( float[] output, float[] input, ref float[,] deltaW, ref float[] deltaB, ref float[] prev_output )
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
				deltaW [i, j] = output [i] * input [j];
			}

			deltaB [i] = output [i];
		}

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
