using UnityEngine;
using System.Collections;

public class AimSight : MonoBehaviour {

	public Vector3 aimSight;
	public Vector3 hipFire;

	float aimSpeed = 10;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1))
		{
			transform.localPosition = Vector3.Slerp(transform.localPosition, aimSight, aimSpeed * Time.deltaTime);
		}
		else if (transform.localPosition != hipFire){
			transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, aimSpeed * Time.deltaTime);
		}
	}
}
