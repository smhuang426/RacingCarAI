using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectorManager : MonoBehaviour {

	/*-------------------------
	MonoBehaviour life cycle
	-------------------------*/
	// Use this for initialization
	void Start ()
	{
		collectors = GetComponentsInChildren<DataCollector> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	/*-------------------------
	Variables
	-------------------------*/
	DataCollector[]    collectors;

	List<TrainingData> mainTrainingData = new List<TrainingData> ();

	/*-------------------------
	Public Methods
	-------------------------*/


}
