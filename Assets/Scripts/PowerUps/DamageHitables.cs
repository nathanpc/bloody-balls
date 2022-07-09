using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppAdvisory.BallX;
using BloodyBalls.Cells;

namespace BloodyBalls.PowerUps {
	/// <summary>
	/// Inflicts damage to specific hitable objects on screen.
	/// </summary>
	public class DamageHitables : PowerUp {
		[SerializeField] protected string tagName = "Hitable";
		[SerializeField] protected int damage = 5;
		[SerializeField] private List<Cell> afflictedCells = null;
		protected List<string> afflictedCellNames = null;

		// Start is called before the first frame update
		void Start() {
			// Making sure...
			if (afflictedCells == null)
				afflictedCells = new List<Cell>();

			// Get the afflicted cell names.
			afflictedCellNames = new List<string>();
			foreach (Cell cell in afflictedCells) {
				afflictedCellNames.Add(cell.cellName);
			}
		}

		/// <summary>
		/// Damages all hitable objects of the specified types.
		/// </summary>
		override public void Activate() {
			// Go through objects with the right tag.
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tagName)) {
				// Get the cell component if it exists.
				Cell cell = obj.GetComponent<Cell>();
				if (cell == null)
					continue;

				foreach (string name in afflictedCellNames) {
					if (cell.cellName == name) {
						// Inflict damage.
						for (int i = 0; i < damage; i++) {
							// Make sure the object hasn't been destroyed already.
							if (obj == null)
								goto nextobj;

							cell.BallHit(null);
						}

						goto nextobj;
					}
				}

nextobj:
				continue;
			}

			base.Activate();
		}
	}
}