using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perpetual rotation for virtual environments.
/// </summary>
public class Spinner : MonoBehaviour {
	[SerializeField] private bool clockwise = true;
	[SerializeField] private int degreesPerSecond = 50;

	// Update is called once per frame
	void Update() {
		transform.Rotate(0, 0, ((clockwise) ? 1 : -1) * degreesPerSecond * Time.deltaTime);
	}
}
