using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Managers;

namespace BloodyBalls.UI {
	/// <summary>
	/// Ensures that a level selection button has everything it needs to work.
	/// </summary>
	public class LevelSelectionButton : MonoBehaviour {
		[Header("UI Elements")]
		[SerializeField] private SceneManager _sceneManager;
		[SerializeField] private Text label;

		[Header("Game Design")]
		[SerializeField] private int _levelNumber;

		// Start is called before the first frame update
		void Start() {

		}

		/// <summary>
		/// Takes the user to the level.
		/// </summary>
		public void GoToLevel() {
			SceneManager.SwitchToPlayArea();
		}

		/// <summary>
		/// Scene manager game object responsible for taking us to places.
		/// </summary>
		public SceneManager SceneManager {
			get { return _sceneManager; }
			set { _sceneManager = value; }
		}

		/// <summary>
		/// Number of the level this button will take us to.
		/// </summary>
		public int LevelNumber {
			get { return _levelNumber; }
			set {
				_levelNumber = value;
				label.text = value.ToString();
			}
		}
	}
}
