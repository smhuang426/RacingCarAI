using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAiUpdateState : State {

	private GameManager gameManager;

	public GameAiUpdateState (GameManager manager)
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
	/// Ends of the state.
	/// </summary>
	public override void EndState ()
	{
	}
}
