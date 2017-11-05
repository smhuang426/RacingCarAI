using UnityEngine;
using System.Collections;

public class CarOperator : MonoBehaviour {

	public float speed       = 1;
	public float rotateSpeed = 1;
	public bool  isManual    = true;

	private bool       canDrive     = false;
	private bool       hasCollide   = false;
	private Rigidbody  carRB;
	private Vector3    rotateVector = new Vector3(0,1,0);
	private Vector3    spawnPos;
	private Quaternion spawnQuat;

	// Use this for initialization
	void Start ()
	{
		spawnPos  = transform.position;
		spawnQuat = transform.rotation;
		carRB     = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!canDrive)
		{
			return;
		}

		Drive ();

		if (isManual)
		{
			if (Input.GetKey (KeyCode.LeftArrow))
			{
				TurnLeft ();
			}
			else if (Input.GetKey (KeyCode.RightArrow))
			{
				TurnRight ();
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if ((1 << collision.gameObject.layer) == LayerMask.GetMask ("Wall"))
		{
			SetDriving (false);
		}
	}

	/// <summary>
	/// Backs to spwan position.
	/// </summary>
	public void BackToSpwanPosition()
	{
		transform.position = spawnPos;
		transform.rotation = spawnQuat;
	}

	/// <summary>
	/// Drive car or not.
	/// </summary>
	/// <param name="setting">If set to <c>true</c> setting.</param>
	public void SetDriving (bool setting)
	{
		canDrive          = setting;
		hasCollide        = !setting;
		carRB.isKinematic = !setting;
		GetComponentInChildren<Collider> ().enabled = setting;
	}

	/// <summary>
	/// Go stright.
	/// </summary>
	public void GoStright()
	{
		//Do nothing
	}

	/// <summary>
	/// Determines whether this instance has collide.
	/// </summary>
	/// <returns><c>true</c> if this instance has collide; otherwise, <c>false</c>.</returns>
	public bool HasCollide()
	{
		return hasCollide;
	}

	/// <summary>
	/// Turn left.
	/// </summary>
	public void TurnLeft()
	{
		transform.Rotate ( - rotateSpeed * rotateVector);
	}

	/// <summary>
	/// Turn right.
	/// </summary>
	public void TurnRight()
	{
		transform.Rotate ( rotateSpeed * rotateVector);
	}

	/// <summary>
	/// Drive this instance.
	/// </summary>
	private void Drive()
	{
		carRB.velocity = -speed * transform.forward;
	}
}
