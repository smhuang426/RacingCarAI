using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OutputDataType
{
	Left,
	Right,
	Forward,
	NotYet
}

public class InputData
{
	public float rayDist1;
	public float rayDist2;
	public float rayDist3;
	public float rayDist4;
	public float rayDist5;
}

public class OutputData
{
	public float left;
	public float right;
	public float forward;
}

public class TrainingData
{
	public InputData   inData;
	public OutputData  outData;
}

public class DataCollector : MonoBehaviour {

	/*-------------------------
	Variables
	-------------------------*/
	List<TrainingData> subTrainingData = new List<TrainingData> ();

	/*-------------------------
	Public Methods
	-------------------------*/
	public void AddNewData ( float rd1, float rd2, float rd3, float rd4, float rd5, OutputDataType type )
	{
		InputData   inData   = new InputData ();
		OutputData outData   = new OutputData ();
		TrainingData newData = new TrainingData ();

		inData.rayDist1 = rd1;
		inData.rayDist2 = rd2;
		inData.rayDist3 = rd3;
		inData.rayDist4 = rd4;
		inData.rayDist5 = rd5;

		outData.left    = 1;
		outData.right   = 1;
		outData.forward = 1;

		switch ( type )
		{
		case OutputDataType.Left:
			outData.left    = 0;
			break;
		case OutputDataType.Right:
			outData.right   = 0;
			break;
		case OutputDataType.Forward:
			outData.forward = 0;
			break;
		default:
			break;
		}

		newData.inData  = inData;
		newData.outData = outData;

		subTrainingData.Add ( newData );
	}

	public void ClearAllData ()
	{
		subTrainingData.Clear ();
	}

	public void CorrectData ( bool isGoodResult )
	{
		if( isGoodResult == false )
		{
			for ( int i = 0; i < subTrainingData.Count; i++ )
			{
				InverseResult ( subTrainingData [i] );
			}
		}
	}

	public List<TrainingData> GetSubtrainingData ()
	{
		return subTrainingData;
	}

	/*-------------------------
	Private Methods
	-------------------------*/
	private void InverseResult ( TrainingData data )
	{
		if ( data.outData.left < 0.5f )
		{
			data.outData.left = 1.0f;
		}
		else
		{
			data.outData.left = 0.0f;
		}

		if ( data.outData.right < 0.5f )
		{
			data.outData.right = 1.0f;
		}
		else
		{
			data.outData.right = 0.0f;
		}

		if ( data.outData.forward < 0.5f )
		{
			data.outData.forward = 1.0f;
		}
		else
		{
			data.outData.forward = 0.0f;
		}
	}
}
