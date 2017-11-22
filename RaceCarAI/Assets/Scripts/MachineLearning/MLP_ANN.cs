using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLP_ANN
{
	List<MLP_Layer> ANN_List;
	MLP_Loss        LossFunction;

	public MLState CreateANN ( uint[] MLP_List, ActiveFc hiddenLayer, ActiveFc outputLayer, LossFc LossFunc )
	{
		if ( MLP_List.Length < 2 )
		{
			Debug.LogError ("[CreateANN] Need two layer at least");
			return MLState.ML_ERROR;
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
}
