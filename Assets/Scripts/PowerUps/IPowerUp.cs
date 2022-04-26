using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodyBalls.PowerUps {
	/// <summary>
	/// Base implementation for all power-ups.
	/// </summary>
	public interface IPowerUp {
		/// <summary>
		/// Activates the power-up.
		/// </summary>
		public void Activate();
	}
}
