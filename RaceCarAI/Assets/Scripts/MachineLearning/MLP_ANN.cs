using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLP_ANN
{
	/*-------------------------
	Variables
	-------------------------*/
	List<MLP_Layer> ANN_List;
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

		for ( int i = 1; i < MLP_List.Length - 1; i++ )
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
		return MLState.ML_SUCCESS;
	}

	public MLState Train ( float[][] train_input, float[][] train_output, float lrnRate )
	{
		return MLState.ML_SUCCESS;
	}

	public MLState Run ( float[] train_input, ref float[] train_output )
	{
		if ( ANN_List.Count == 0 )
		{
			Debug.LogError ("[Run] ANN_List is zero");
			return MLState.ML_ERROR;
		}

		if ( train_input.Length != ANN_List[0].GetNumInput() )
		{
			Debug.LogError ("[Run] Len of train_input is not equal to ANN_List[0].GetNumInput() ");
			return MLState.ML_ERROR;
		}

		if ( train_output.Length != ANN_List[ANN_List.Count - 1].GetNumOutput() )
		{
			Debug.LogError ("[Run] Len of train_output is not equal to ANN_List[ANN_List.Count-1].GetNumOutput() ");
			return MLState.ML_ERROR;
		}

		float[] tmp_output = new float[0];
		train_output       = (float[])train_input.Clone (); 

		for ( int i = 0; i < ANN_List.Count; i++ )
		{
			Array.Resize ( ref tmp_output, ANN_List[i].GetNumOutput() );

			if ( ANN_List [i].Forward ( train_output, ref tmp_output ) == MLState.ML_ERROR )
			{
				return MLState.ML_ERROR;
			}

			train_output = (float[])tmp_output.Clone ();
		}

		return MLState.ML_SUCCESS;
	}
}
