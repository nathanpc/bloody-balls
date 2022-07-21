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
		[SerializeField] private int _turnsBeforeQuizNag = 4;

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
		/// Goes to the next level. WARNING: Level skin must be applied separately.
		/// </summary>
		public void GoToNextLevel() {
			CurrentLevelType = GetNextLevel();
		}

		/// <summary>
		/// Gets the next level type for the game.
		/// </summary>
		/// <returns>Level type object.</returns>
		protected LevelType GetNextLevel() {
			return GameObject.Find("Level Type").GetComponent<LevelType>();
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
		/// Number of turns before a level switch happens.
		/// </summary>
		public int TurnsBeforeLevelSwitch {
			get { return _turnsBeforeLevelSwitch; }
			set { _turnsBeforeLevelSwitch = value; }
		}

		/// <summary>
		/// Number of turns before a quiz nag.
		/// </summary>
		public int TurnsBeforeQuizNag {
			get { return _turnsBeforeQuizNag; }
			set { _turnsBeforeQuizNag = value; }
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