using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using BloodyBalls.Levels;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages the levels and completely transforms the playground.
	/// </summary>
	public class LevelManager : MonoBehaviour {
		[Header("Game Design")]
		[SerializeField] private int _turnsBeforeLevelSwitch = 10;

		[Header("Controlled Objects")]
		[SerializeField] private GameObject screenBackground;

		[Header("Misc")]
		[SerializeField] protected int randomSeed = 123456789;

		protected List<LevelType> levelTypes;
		protected System.Random random;
		private LevelType _currentLevelType;

		// Start is called before the first frame update
		void Start() {
			// Populate the level types list.
			levelTypes = new List<LevelType>();
			levelTypes.AddRange(GetComponents<LevelType>());

			// Start our random number generator.
			random = new System.Random(randomSeed);
		}

		/// <summary>
		/// Goes to the next level and sets everything up.
		/// </summary>
		public void GoToNextLevel() {
			CurrentLevelType = GetNextLevel();
			ApplyLevelStyling(CurrentLevelType);
		}

		/// <summary>
		/// Gets the next level type for the game.
		/// </summary>
		/// <returns>Level type object.</returns>
		protected LevelType GetNextLevel() {
			return GetRandomLevel();
		}

		/// <summary>
		/// Gets a random level type object.
		/// </summary>
		/// <returns>Level type object.</returns>
		protected LevelType GetRandomLevel() {
			// Just making sure...
			if (levelTypes.Count == 0)
				throw new System.Exception("At least one level type must be supplied to the level manager.");

			// Get the next level randomly.
			return levelTypes[random.Next(levelTypes.Count)];
		}

		/// <summary>
		/// Sets everything up according to the level settings.
		/// </summary>
		/// <param name="level">Level to be used as the base.</param>
		protected void ApplyLevelStyling(LevelType level) {

		}

		/// <summary>
		/// Number of turns before a level switch happens.
		/// </summary>
		public int TurnsBeforeLevelSwitch {
			get { return _turnsBeforeLevelSwitch; }
			set { _turnsBeforeLevelSwitch = value; }
		}

		/// <summary>
		/// Current level type.
		/// </summary>
		public LevelType CurrentLevelType {
			get { return _currentLevelType; }
			set { _currentLevelType = value; }
		}
	}
}