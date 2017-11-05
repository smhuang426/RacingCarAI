using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public CarManager carManager;

	private List<State> states          = new List<State> ();
	private bool        isStateHasStart = false;
	private int         stateIdx        = 0;

	// Use this for initialization
	void Start () {
		states.Add (new GameStartState (this));
		states.Add (new GameAiUpdateState (this));
		states.Add (new GameEndState (this));

		stateIdx        = 0;
		isStateHasStart = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isStateHasStart == false)
		{
			isStateHasStart = true;

		    states [stateIdx].StartState ();
		}

		if (states [stateIdx].UpdateState () != true)
		{
			states [stateIdx].EndState ();
			isStateHasStart = false;

			//Search next state
			stateIdx++;
			if (stateIdx >= states.Count)
			{
				stateIdx = 0;
			}
		}
	}

	public void EnableCarManager (bool Setting)
	{
		carManager.enabled = Setting;
	}

	public bool IsEpisodeOver ()
	{
		if (carManager.enabled == false)
		{
			Debug.LogError ("CarManager is not enabled");
			return false;
		}

		return carManager.GetIsEpisodeOver ();
	}
}
