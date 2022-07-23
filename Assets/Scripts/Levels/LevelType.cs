using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using BloodyBalls.Cells;

namespace BloodyBalls.Levels {
	/// <summary>
	/// Defines a level type base class to be used for the creation of custom
	/// level styles.
	/// </summary>
	public class LevelType : MonoBehaviour {
		[SerializeField] private string _name;

		[Header("Colorscheme")]
		[SerializeField] private Color _screenColor;
		[SerializeField] private Color _fieldColor;
		[SerializeField] private Color _textColor;

		[Header("Game Design")]
		[SerializeField] private int _levelNumber;

		[Header("Cells")]
		[SerializeField] private Cell[] _cellPrefabs;
		[SerializeField] private float[] _cellProbabilities;

		[Header("Notifications")]
		[SerializeField] private Color _notificationBoxColor;
		[SerializeField] private Color _notificationTextColor;
		[SerializeField] private List<string> _messages;

		[Header("Quiz")]
		[SerializeField] protected GameObject quizQuestionsContainer;
		protected Quiz[] quizzes;

		protected System.Random random;

		// Start is called before the first frame update
		void Start() {
			// Make sure we got messages.
			if (_messages == null)
				throw new System.Exception("At least a single message is needed for each level.");

			// Get our quizzes.
			if (quizzes == null)
				quizzes = quizQuestionsContainer.GetComponents<Quiz>();

			// Start our random number generator.
			random = new System.Random();
		}

		/// <summary>
		/// Gets a random message for this level to be displayed.
		/// </summary>
		/// <returns>Message to the player for this level.</returns>
		public string GetRandomMessage() {
			return Messages[random.Next(Messages.Count)];
		}

		/// <summary>
		/// Gets a random quiz for this level.
		/// </summary>
		/// <returns>Quiz to present to the player.</returns>
		public Quiz GetRandomQuiz() {
			return quizzes[random.Next(quizzes.Length)];
		}

		/// <summary>
		/// Name that identifies this level type.
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Background color of the game.
		/// </summary>
		public Color BackgroundColor {
			get { return _screenColor; }
			set { _screenColor = value; }
		}

		/// <summary>
		/// Background color of the play field.
		/// </summary>
		public Color FieldColor {
			get { return _fieldColor; }
			set { _fieldColor = value; }
		}

		/// <summary>
		/// Foreground color of the text in the scene.
		/// </summary>
		public Color TextColor {
			get { return _textColor; }
			set { _textColor = value; }
		}

		/// <summary>
		/// List of messages that will be displayed to the player as notifications.
		/// </summary>
		public List<string> Messages {
			get { return _messages; }
			set { _messages = value; }
		}

		/// <summary>
		/// Background color of the notification box.
		/// </summary>
		public Color NotificationBoxColor {
			get { return _notificationBoxColor; }
			set { _notificationBoxColor = value; }
		}

		/// <summary>
		/// Color of the notification box text.
		/// </summary>
		public Color NotificationTextColor {
			get { return _notificationTextColor; }
			set { _notificationTextColor = value; }
		}

		/// <summary>
		/// Cell prefabs for this level.
		/// </summary>
		public Cell[] Cells {
			get { return _cellPrefabs; }
			set { _cellPrefabs = value; }
		}

		/// <summary>
		/// What probabilities of this cell spawning?
		/// </summary>
		public float[] CellProbabilities {
			get { return _cellProbabilities; }
			set { _cellProbabilities = value; }
		}

		/// <summary>
		/// Level number that this type is going to be.
		/// </summary>
		public int LevelNumber {
			get { return _levelNumber; }
			set {
				_levelNumber = value;
			}
		}
	}
}
