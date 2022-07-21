using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Managers;
using BloodyBalls.Levels;

namespace BloodyBalls.UI {
	/// <summary>
	/// Ensures that a level selection button has everything it needs to work.
	/// </summary>
	public class LevelSelectionButton : MonoBehaviour {
		[Header("UI Elements")]
		[SerializeField] private SceneManager _sceneManager;
		[SerializeField] private Text label;
		[SerializeField] private Button _button;
		[SerializeField] private Image lockImage;

		[Header("Game Design")]
		[SerializeField] private int _levelNumber;
		[SerializeField] private LevelType _levelType;
		[SerializeField] private bool _locked;

		// Start is called before the first frame update
		void Start() {
		}

		/// <summary>
		/// Takes the user to the level.
		/// </summary>
		public void GoToLevel() {
			// Are we allowed to go?
			if (Locked)
				return;

			// Setup the level.
			LevelType.LevelNumber = LevelNumber;
			LevelType.gameObject.transform.parent = null;
			LevelType.gameObject.name = "Level Type";
			DontDestroyOnLoad(LevelType.gameObject);

			// Switch scenes.
			SceneManager.SwitchToPlayArea();
		}

		/// <summary>
		/// Applies level type specific characteristics to the button.
		/// </summary>
		protected void ApplyLevelType() {
			Button.image.color = LevelType.FieldColor;
			label.color = LevelType.TextColor;
		}

		/// <summary>
		/// Scene manager game object responsible for taking us to places.
		/// </summary>
		public SceneManager SceneManager {
			get { return _sceneManager; }
			set { _sceneManager = value; }
		}

		/// <summary>
		/// Button that's actually associated with this script.
		/// </summary>
		public Button Button {
			get { return _button; }
			set { _button = value; }
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

		/// <summary>
		/// Type of level that this button relates to.
		/// </summary>
		public LevelType LevelType {
			get { return _levelType; }
			set {
				_levelType = value;
				ApplyLevelType();
			}
		}

		/// <summary>
		/// Is this level locked up?
		/// </summary>
		public bool Locked {
			get { return _locked; }
			set {
				_locked = value;
				lockImage.enabled = value;
			}
		}
	}
}
