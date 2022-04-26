using UnityEngine;
using System.Collections;
using System;

namespace AppAdvisory.BallX 
{
	public class InputManager : MonoBehaviour {
		public static event Action<Vector3> OnSwipeStarted;
		public static event Action<Vector3> OnSwipe;
		public static event Action<Vector3> OnSwipeEnded;

		private Vector3 startPosition;
		void Update()
		{
			if (Input.GetMouseButtonDown (0)) {
				startPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				if (OnSwipeStarted != null)
					OnSwipeStarted (startPosition);
			} else if (Input.GetMouseButton (0)) 
			{
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				//Vector3 movement = startPosition - currentPosition;
				Vector3 movement = currentPosition - startPosition;

				if (OnSwipe != null)
					OnSwipe (movement);

			}
			else if (Input.GetMouseButtonUp(0))
			{
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				//Vector3 movement = startPosition - currentPosition;
				Vector3 movement = currentPosition - startPosition;

				if (OnSwipeEnded != null)
					OnSwipeEnded (movement);
			}
				
//			if (Input.GetKeyDown(KeyCode.Escape))
//			{
//				GameManager.Instance.Escape();
//			}

		}
	}
}

