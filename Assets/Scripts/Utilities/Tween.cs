using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodyBalls.Utilities {
	/// <summary>
	/// A simple class that enables us to move things fluently on screen.
	/// </summary>
	public static class Tween {
		/// <summary>
		/// Enables asynchronous movement on screen.
		/// </summary>
		/// <param name="transform">Object to be moved.</param>
		/// <param name="startPosition">Start position of the object.</param>
		/// <param name="endPosition">End position of the object.</param>
		/// <param name="time">Time the animation will take in seconds.</param>
		/// <param name="delay">Delay to start the animation in seconds.</param>
		public static void Move(this Transform transform, Vector3 startPosition, Vector3 endposition, float time = 1f, float delay = 0f) {
			// Get ourselves and start the movement asynchronously.
			MonoBehaviour script = transform.GetComponent<MonoBehaviour>();
			script.StartCoroutine(MoveCoroutine(transform, startPosition, endposition, time, delay));
		}

		/// <summary>
		/// Co-routine that enables asynchronous movement on screen.
		/// </summary>
		/// <param name="transform">Object to be moved.</param>
		/// <param name="startPosition">Start position of the object.</param>
		/// <param name="endPosition">End position of the object.</param>
		/// <param name="time">Time the animation will take in seconds.</param>
		/// <param name="delay">Delay to start the animation in seconds.</param>
		public static IEnumerator MoveCoroutine(Transform transform, Vector3 startPosition, Vector3 endPosition, float time = 1f, float delay = 1f) {
			// Wait for a bit.
			yield return new WaitForSeconds(delay);

			// Make sure the object is at the start position.
			transform.position = startPosition;

			// Move around.
			float count = 0;
			while (count < time) {
				count += Time.deltaTime;
				transform.position = Vector3.Lerp(startPosition, endPosition, count / time);
				yield return null;
			}

			// Finish up the movement.
			transform.position = endPosition;
		}

	}
}
