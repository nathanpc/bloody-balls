using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using BloodyBalls.Levels;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages the popup quiz the player will get when moving to a new level.
	/// </summary>
	public class PopupQuizManager : MonoBehaviour {
		[Header("Components")]
		[SerializeField] private InputManager inputManager;
		[SerializeField] protected RectTransform popupBox;
		[SerializeField] protected Text questionText;
		[SerializeField] protected Button[] answerButtons = new Button[4];

		[Header("Animation")]
		[SerializeField] private int _displayDuration = 10;
		[SerializeField] protected int animationDuration = 1;

		private Vector2 hiddenPosition;
		private Vector2 shownPosition;

		// Start is called before the first frame update
		void Start() {
			Hide();

			//hiddenPosition = notificationBox.position;
			//shownPosition = notificationBox.position;

			//Rect rect = notificationBox.rect;
			//shownPosition.y -= rect.height;
		}

		/// <summary>
		/// Hides the popup box.
		/// </summary>
		public void Hide() {
			//Tween.Move(notificationBox, shownPosition, hiddenPosition, animationDuration);
			inputManager.EnableInput();
			popupBox.gameObject.SetActive(false);
		}

		/// <summary>
		/// Shows the popup box.
		/// </summary>
		public void Show() {
			//Tween.Move(notificationBox, hiddenPosition, shownPosition, animationDuration);
			popupBox.gameObject.SetActive(true);
			inputManager.DisableInput();
		}

		/// <summary>
		/// Shows the popup box and sets it up with a quiz for the user.
		/// </summary>
		/// <param name="quiz"></param>
		public void Open(Quiz quiz) {
			// Setup the quiz content.
			questionText.text = quiz.Question;
			answerButtons[0].GetComponentInChildren<Text>().text = quiz.Answers[0];
			answerButtons[1].GetComponentInChildren<Text>().text = quiz.Answers[1];
			answerButtons[2].GetComponentInChildren<Text>().text = quiz.Answers[2];
			answerButtons[3].GetComponentInChildren<Text>().text = quiz.Answers[3];

			// Show up.
			Show();
		}

		/// <summary>
		/// Duration to show the notification for.
		/// </summary>
		public int Duration {
			get { return _displayDuration; }
			set { _displayDuration = value; }
		}
	}
}
