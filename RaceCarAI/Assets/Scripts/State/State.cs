using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State : Object {

	//public string stateName;

	public virtual void StartState ()
	{
	}

	public virtual bool UpdateState ()
	{
		return false;
	}

	public virtual void EndState()
	{
	}
}
