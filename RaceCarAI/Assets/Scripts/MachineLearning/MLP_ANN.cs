using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLP_ANN
{
	/*-------------------------
	Variables
	-------------------------*/
	List<MLP_Layer> ANN_List = new List<MLP_Layer> ();
	MLP_Loss        LossFunction;

	/*-------------------------
	Public Methods
	-------------------------*/
	public MLState CreateANN ( uint[] MLP_List, ActiveFc hiddenLayer, ActiveFc outputLayer, LossFc LossFunc )
	{
		if ( MLP_List.Length < 2 )
		{
			Debug.LogError ("[CreateANN] Need two layer at least");
			return MLState.ML_ERROR;
		}

		for ( int i = 0; i < MLP_List.Length - 1; i++ )
		{
			if ( MLP_List[i] == 0 )
			{
				Debug.LogError ("[CreateANN] Number of neurons can not be zero");
				return MLState.ML_ERROR;
			}
		}

		for ( int i = 1; i < MLP_List.Length; i++ )
		{
			if ( ( MLP_List.Length - 1 ) == i )
			{
				//Output layer
				MLP_Layer layer = new MLP_Layer ();
				layer.Initialize ( (int)MLP_List [i], (int)MLP_List [i - 1], outputLayer );
				ANN_List.Add (layer);
			}
			else
			{
				//Hidden layer
				MLP_Layer layer = new MLP_Layer ();
				layer.Initialize ( (int)MLP_List [i], (int)MLP_List [i - 1], hiddenLayer );
				ANN_List.Add (layer);
			}
		}

		LossFunction = new MLP_Loss ();
		LossFunction.Initialize ( LossFunc );

		return MLState.ML_SUCCESS;
	}

	public MLState Test ( float[][] test_input, float[][] test_output, ref float errorRate )
	{
		if ( test_input.GetLength(0) != test_output.GetLength(0) )
		{
			Debug.LogError ("[Test] test_input's batch size is different from test_output's");
			return MLState.ML_ERROR;
		}

		int batchSize                = test_input.GetLength (0);
		int numLayer                 = ANN_List.Count;
		float[][] tmp_input          = (float[][])test_input.Clone ();
		float[][] back_err           = new float[ batchSize ][];
		float[][] layerOutput        = new float[batchSize][];

		errorRate = 0;

		for ( int layer = 0; layer < numLayer - 1; layer++ )
		{
			if ( ANN_List[layer].BatchForward ( tmp_input, ref layerOutput ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			tmp_input = (float[][])layerOutput.Clone ();
		}

		LossFunction.Evaluate( layerOutput, test_output, ref errorRate, ref back_err );

		return MLState.ML_SUCCESS;
	}

	public MLState Train ( float[][] train_input, float[][] train_output, float lrnRate, ref float errorRate )
	{
		if ( train_input.GetLength(0) != train_output.GetLength(0) )
		{
			Debug.LogError ("[Train] train_input's batch size is different from train_output's");
			return MLState.ML_ERROR;
		}

		int batchSize                = train_input.GetLength (0);
		int numLayer                 = ANN_List.Count;
		List<float[][]> layerOutputs = new List<float[][]> ();
		float[][] tmp_input          = (float[][])train_input.Clone ();
		float[][] back_err           = new float[ batchSize ][];

		errorRate = 0;

		for ( int layer = 0; layer < numLayer - 1; layer++ )
		{
			float[][] layerOutput = new float[batchSize][];

			if ( ANN_List[layer].BatchForward ( tmp_input, ref layerOutput ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			tmp_input = (float[][])layerOutput.Clone ();
			layerOutputs.Add ( layerOutput );
		}

		LossFunction.Evaluate( layerOutputs[ numLayer - 1 ], train_output, ref errorRate, ref back_err );

		for ( int layer = numLayer - 1; layer < 0; layer-- )
		{
			int numOut = ANN_List [layer].GetNumOutput ();
			int numIn  = ANN_List [layer].GetNumInput ();

			float[,] deltaW = new float [numOut, numIn];
			float[] deltaB  = new float [numOut];

			float[][] prev_output = new float[batchSize][];

			if ( ANN_List [layer].BatchBackward ( back_err, ref deltaW, ref deltaB, ref prev_output ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			back_err = (float[][])prev_output.Clone ();

			if( ANN_List [layer].Update( deltaW, deltaB, lrnRate ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}
		}

		return MLState.ML_SUCCESS;
	}

	public MLState Run ( float[] input, ref float[] output )
	{
		if ( ANN_List.Count == 0 )
		{
			Debug.LogError ("[Run] ANN_List is zero");
			return MLState.ML_ERROR;
		}

		if ( input.Length != ANN_List[0].GetNumInput() )
		{
			Debug.LogError ("[Run] Len of input is not equal to ANN_List[0].GetNumInput() ");
			return MLState.ML_ERROR;
		}

		if ( output.Length != ANN_List[ANN_List.Count - 1].GetNumOutput() )
		{
			Debug.LogError ("[Run] Len of output is not equal to ANN_List[ANN_List.Count-1].GetNumOutput() ");
			return MLState.ML_ERROR;
		}

		float[] tmp_output = new float[0];
		output             = (float[])input.Clone ();

		for ( int i = 0; i < ANN_List.Count; i++ )
		{
			Array.Resize ( ref tmp_output, ANN_List[i].GetNumOutput() );

			if ( ANN_List [i].Forward ( output, ref tmp_output ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			output = (float[])tmp_output.Clone ();
		}

		return MLState.ML_SUCCESS;
	}
}
