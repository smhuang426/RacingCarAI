using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartState : State {

	private GameManager gameManager;

	public GameStartState (GameManager manager)
	{
		gameManager = manager;
	}

	/// <summary>
	/// Starts the state.
	/// </summary>
	public override void StartState ()
	{
		gameManager.EnableCarManager (true);
	}

	/// <summary>
	/// Updates the state.
	/// </summary>
	/// <returns><c>true</c>, if state was updated, <c>false</c> otherwise.</returns>
	public override bool UpdateState ()
	{
		return !gameManager.IsEpisodeOver ();
	}

	/// <summary>
	/// Ends the state.
	/// </summary>
	public override void EndState ()
	{
	}
}
