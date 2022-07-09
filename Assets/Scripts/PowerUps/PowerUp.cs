using BloodyBalls.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppAdvisory.BallX;

namespace BloodyBalls.PowerUps {
	/// <summary>
	/// Base implementation for all power-ups.
	/// </summary>
	public abstract class PowerUp : MonoBehaviour {
		[SerializeField] protected int points = 1;
		[SerializeField] protected UIManager uiManager;

		/// <summary>
		/// Activates the power-up.
		/// </summary>
		virtual public void Activate() {
			Utils.RemoveCoins(points);
			uiManager.SetHUDCoins(Utils.GetCoins());
		}
	}
}
