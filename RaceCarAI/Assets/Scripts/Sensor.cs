using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

	Ray ray1;
	Ray ray2;
	Ray ray3;
	Ray ray4;
	Ray ray5;

	RaycastHit hit;

	public float maxDistance1 = 10;
	public float maxDistance2 = 6;
	public float maxDistance3 = 4;

	public float ray1_dis = 0;
	public float ray2_dis = 0;
	public float ray3_dis = 0;
	public float ray4_dis = 0;
	public float ray5_dis = 0;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ray1.origin = ray2.origin = ray3.origin = ray4.origin = ray5.origin = transform.position;

		ray1.direction = -transform.forward;

		//-30 degree
		ray2.direction = -transform.forward * 0.866f + transform.right * 0.5f;

		//30 degree
		ray3.direction = -transform.forward * 0.866f - transform.right * 0.5f;

		//-60 degree
		ray4.direction = -transform.forward * 0.5f + transform.right * 0.866f;

		//60 degree
		ray5.direction = -transform.forward * 0.5f - transform.right * 0.866f;

		if (Physics.Raycast (ray1, out hit, maxDistance1, LayerMask.GetMask ("Wall")))
		{
			ray1_dis = hit.distance;
		}
		else
		{
			ray1_dis = maxDistance1;
		}

		if (Physics.Raycast (ray2, out hit, maxDistance2, LayerMask.GetMask ("Wall")))
		{
			ray2_dis = hit.distance;
		}
		else
		{
			ray2_dis = maxDistance2;
		}

		if (Physics.Raycast (ray3, out hit, maxDistance2, LayerMask.GetMask ("Wall")))
		{
			ray3_dis = hit.distance;
		}
		else
		{
			ray3_dis = maxDistance2;
		}

		if (Physics.Raycast (ray4, out hit, maxDistance3, LayerMask.GetMask ("Wall")))
		{
			ray4_dis = hit.distance;
		}
		else
		{
			ray4_dis = maxDistance3;
		}

		if (Physics.Raycast (ray5, out hit, maxDistance3, LayerMask.GetMask ("Wall")))
		{
			ray5_dis = hit.distance;
		}
		else
		{
			ray5_dis = maxDistance3;
		}

		Debug.DrawRay (transform.position, maxDistance1 * ray1.direction);
		Debug.DrawRay (transform.position, maxDistance2 * ray2.direction, Color.blue);
		Debug.DrawRay (transform.position, maxDistance2 * ray3.direction, Color.blue);
		Debug.DrawRay (transform.position, maxDistance3 * ray4.direction, Color.green);
		Debug.DrawRay (transform.position, maxDistance3 * ray5.direction, Color.green);
	}
}
