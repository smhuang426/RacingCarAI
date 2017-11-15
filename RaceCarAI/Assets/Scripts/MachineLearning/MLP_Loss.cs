using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LossFc
{
	MeanSquaredError
}

public class MLP_Loss
{
	/*-------------------------
	Variables
	-------------------------*/
	LossFc   LF;

	/*-------------------------
	Public Methods
	-------------------------*/
	public MLState Initialize ( LossFc LsFc )
	{
		LF = LsFc;

		return MLState.ML_SUCCESS;
	}

	public MLState Evaluate( float[][] yHead, float[][] yExp, ref float evl, ref float[][] output )
	{
		if ( yHead.GetLength (0) != yExp.GetLength (0) )
		{
			Debug.LogError ("[Evaluate] Size of both array are not the same");
			return MLState.ML_ERROR;
		}

		float batchSize = yHead.GetLength (0);
		float tmpEvl    = 0;

		for ( int i = 0; i < batchSize; i++ )
		{
			switch (LF)
			{
			case LossFc.MeanSquaredError:
				if ( MeanSquaredError ( yHead [i], yExp [i], ref tmpEvl ) == MLState.ML_ERROR )
				{
					return MLState.ML_ERROR;
				}

				if ( MeanSquaredErrorBack ( yHead [i] , yExp [i], ref output[i] ) == MLState.ML_ERROR )
				{
					return MLState.ML_ERROR;
				}
				break;
			default:
				break;
			}

			evl += tmpEvl;
		}

		evl = evl / batchSize;

		return MLState.ML_SUCCESS; 
	}

	/*-------------------------
	Loss Function
	-------------------------*/
	private MLState MeanSquaredErrorBack ( float[] yHead, float[] yExp, ref float[] error )
	{
		if ( yHead.Length != yExp.Length )
		{
			Debug.LogError ("[OutputBackward] Size of both array are not the same");
			return MLState.ML_ERROR;
		}

		for ( int i = 0; i < yHead.Length; i++ )
		{
			error [i] = 0;
		}

		switch ( LF )
		{
		case LossFc.MeanSquaredError:

			for ( int i = 0; i < yHead.Length; i++ )
			{
				error [i] = yHead [i] - yExp [i];
			}

			break;
		default:
			break;
		}

		return MLState.ML_SUCCESS;
	}

	private MLState MeanSquaredError ( float[] yHead, float[] yExp, ref float evl )
	{
		if ( yHead.Length != yExp.Length )
		{
			Debug.LogError ("[MeanSquaredError] Size of both array are not the same");
			return MLState.ML_ERROR;
		}

		evl = 0;

		for ( int i = 0; i < yHead.Length; i++ )
		{
			evl += ( 0.5f * ( yHead[i] - yExp[i] ) * ( yHead[i] - yExp[i] ) );
		}

		return MLState.ML_SUCCESS;
	}
}
