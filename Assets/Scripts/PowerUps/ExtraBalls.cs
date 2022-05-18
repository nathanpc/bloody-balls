using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppAdvisory.BallX;
using BloodyBalls.Cells;
using BloodyBalls.Controls;

namespace BloodyBalls.PowerUps {
	/// <summary>
	/// Adds extra balls on demand.
	/// </summary>
	public class ExtraBalls : MonoBehaviour, IPowerUp {
		[SerializeField] protected int amount = 3;
		[SerializeField] protected Player player;

		/// <summary>
		/// Adds more balls.
		/// </summary>
		public void Activate() {
			player.AddBall();
		}
	}
}