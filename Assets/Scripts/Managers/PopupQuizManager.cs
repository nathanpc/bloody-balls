using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using BloodyBalls.Levels;
using UnityEngine.Events;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages the popup quiz the player will get when moving to a new level.
	/// </summary>
	public class PopupQuizManager : MonoBehaviour {
		[Header("Components")]
		[SerializeField] private InputManager _inputManager = null;
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
			if (InputManager != null)
				InputManager.EnableInput();
			popupBox.gameObject.SetActive(false);
		}

		/// <summary>
		/// Shows the popup box.
		/// </summary>
		public void Show() {
			//Tween.Move(notificationBox, hiddenPosition, shownPosition, animationDuration);
			popupBox.gameObject.SetActive(true);
			if (InputManager != null)
				InputManager.DisableInput();
		}

		/// <summary>
		/// Shows the popup box and sets it up with a quiz for the user.
		/// </summary>
		/// <param name="quiz">Quiz to be displayed.</param>
		/// <param name="rightAnswer">Action to perform when the user selected the right answer.</param>
		/// <param name="wrongAnswer">Action to perform when the user selected the wrong answer.</param>
		public void Open(Quiz quiz, UnityAction rightAnswer, UnityAction wrongAnswer) {
			// Setup the quiz content.
			questionText.text = quiz.Question;
			for (int i = 0; i < quiz.Answers.Length; i++) {
				answerButtons[i].onClick.RemoveAllListeners();
				answerButtons[i].onClick.AddListener((quiz.RightAnswerIndex == i) ? rightAnswer : wrongAnswer);
				answerButtons[i].GetComponentInChildren<Text>().text = quiz.Answers[i];
			}

			// Show up.
			Show();
		}

		/// <summary>
		/// Closes the popup box.
		/// </summary>
		public void Close() {
			Hide();
		}

		/// <summary>
		/// Duration to show the notification for.
		/// </summary>
		public int Duration {
			get { return _displayDuration; }
			set { _displayDuration = value; }
		}

		/// <summary>
		/// Input manager that controls the swipes from the user.
		/// </summary>
		public InputManager InputManager {
			get { return _inputManager; }
			set { _inputManager = value; }
		}
	}
}
