using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : State {

	private GameManager gameManager;

	public GameEndState (GameManager manager)
	{
		gameManager = manager;
	}

	/// <summary>
	/// Starts the state.
	/// </summary>
	public override void StartState ()
	{
	}

	/// <summary>
	/// Updates the state.
	/// </summary>
	/// <returns><c>true</c>, if state was updated, <c>false</c> otherwise.</returns>
	public override bool UpdateState ()
	{
		return false;
	}

	/// <summary>
	/// Ends the state.
	/// </summary>
	public override void EndState ()
	{
		gameManager.EnableCarManager (false);
	}
}
