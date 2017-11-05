using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour {

	private CarOperator[] cars;
	private bool isEpisodeOver = false;
	// Use this for initialization
	void Awake ()
	{
		isEpisodeOver = false;

		cars = GetComponentsInChildren<CarOperator> ();
	}

	void OnEnable ()
	{
		isEpisodeOver = false;

		SetCarsToStartPoint ();

		Invoke ("SetCarsDriving", 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isEpisodeOver)
		{
			return;
		}

		for (int i = 0; i < cars.Length; i++)
		{
			if (cars [i].HasCollide () == false)
			{
				return;
			}
		}

		//All of cars are collided.
		isEpisodeOver = true;
	}

	/// <summary>
	/// Gets variable for episode being over or not.
	/// </summary>
	/// <returns><c>true</c>, if is episode over was gotten, <c>false</c> otherwise.</returns>
	public bool GetIsEpisodeOver()
	{
		return isEpisodeOver;
	}

	/// <summary>
	/// Sets cars driving.
	/// </summary>
	public void SetCarsDriving()
	{
		for (int i = 0; i < cars.Length; i++)
		{
			cars [i].SetDriving (true);
		}
	}

	/// <summary>
	/// Sets cars to start point.
	/// </summary>
	public void SetCarsToStartPoint()
	{
		for (int i = 0; i < cars.Length; i++)
		{
			cars [i].BackToSpwanPosition ();
		}
	}
}
