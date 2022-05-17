using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BloodyBalls.Utilities {
	/// <summary>
	/// A set of utilities that deal with the camera.
	/// </summary>
	public static class CameraUtils {
		/// <summary>
		/// Gets the rectangle that defines the extents of the screen.
		/// </summary>
		/// <returns>Extents of the screen.</returns>
		public static Rect GetScreenRect() {
			Camera cam = Camera.main;
			float height = 2f * cam.orthographicSize;
			float width = height * cam.aspect;

			return new Rect(cam.transform.position.x - width / 2,
							cam.transform.position.y - height / 2,
							width,
							height);
		}
	}
}