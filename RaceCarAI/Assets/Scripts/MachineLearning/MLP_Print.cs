using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLP_Print
{
	static private bool isDebug = false;

	public static void PrintLayerParameter ( int numOut, int numIn, float[,] Weight, float[] bias )
	{
		if (isDebug != true)
		{
			return;
		}

		object biasArray   = "bias:\n";
		object WeightArray = "Weight:\n";

		for (int i = 0; i < numOut; i++)
		{
			biasArray += bias [i].ToString ();

			if ( i != numOut - 1 )
			{
				biasArray += ", ";
			}

			for (int j = 0; j < numIn; j++)
			{
				WeightArray += Weight [i, j].ToString ();

				if ( j != numIn - 1 )
				{
					WeightArray += "\t,";
				}
			}

			WeightArray += "\n";
		}

		Debug.Log ( WeightArray );
		Debug.Log ( biasArray );
	}

	public static void PrintArray ( float[] array, string name )
	{
		if (isDebug != true)
		{
			return;
		}

		int len      = array.Length;
		object Array =  name + ":\n";

		for ( int i = 0; i < len; i ++ )
		{
			Array += array [i].ToString ();

			if ( i != len - 1 )
			{
				Array += ", ";
			}
		}

		Debug.Log ( Array );
	}
}
