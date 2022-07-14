using UnityEngine;
using System.Collections;
using System;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages the controlling inputs that the game receives.
	/// </summary>
	public class InputManager : MonoBehaviour {
		public bool acceptingInput = true;

		public static event Action<Vector3> OnSwipeStarted;
		public static event Action<Vector3> OnSwipe;
		public static event Action<Vector3> OnSwipeEnded;

		private Vector3 startPosition;

		void Update() {
			// Don't do anything if we are not allowed to respond to input events.
			if (!acceptingInput)
				return;
			
			// Interpret the various inputs that we get.
			if (Input.GetMouseButtonDown(0)) {
				startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				if (OnSwipeStarted != null)
					OnSwipeStarted(startPosition);
			} else if (Input.GetMouseButton(0)) {
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 movement = currentPosition - startPosition;

				if (OnSwipe != null)
					OnSwipe(movement);
			} else if (Input.GetMouseButtonUp(0)) {
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 movement = currentPosition - startPosition;

				if (OnSwipeEnded != null)
					OnSwipeEnded(movement);
			}
		}

		/// <summary>
		/// Enables this manager to accept controllling inputs.
		/// </summary>
		public void EnableInput() {
			acceptingInput = true;
		}

		/// <summary>
		/// Disables this manager to accept controllling inputs.
		/// </summary>
		public void DisableInput() {
			acceptingInput = false;
		}

		/// <summary>
		/// Is this manager still accepting inputs?
		/// </summary>
		/// <returns>Is this manager still accepting inputs?</returns>
		public bool IsAcceptingInput() {
			return acceptingInput;
		}
	}
}

