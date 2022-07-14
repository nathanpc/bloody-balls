using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages the notification the player will get while playing the game.
	/// </summary>
	public class NotificationBox : MonoBehaviour {
		[Header("Components")]
		[SerializeField] private RectTransform notificationBox;
		[SerializeField] private Text textBox;

		[Header("Animation")]
		[SerializeField] private int _displayDuration = 10;
		[SerializeField] protected int animationDuration = 1;

		private Vector2 hiddenPosition;
		private Vector2 shownPosition;

		// Start is called before the first frame update
		void Start() {
			hiddenPosition = notificationBox.position;
			shownPosition = notificationBox.position;

			Rect rect = notificationBox.rect;
			hiddenPosition.y += rect.height + CameraUtils.GetScreenRect().height+2000;
			notificationBox.position = hiddenPosition;
		}

		/// <summary>
		/// Sets the text in the notification box.
		/// </summary>
		/// <param name="text">Text of the notification.</param>
		public void SetText(string text) {
			textBox.text = text;
		}

		/// <summary>
		/// Gets the text used in the notification box.
		/// </summary>
		/// <returns>Notification text.</returns>
		public string GetText() {
			return textBox.text;
		}

		/// <summary>
		/// Hides the notification box.
		/// </summary>
		public void Hide() {
			Tween.Move(notificationBox, shownPosition, hiddenPosition, animationDuration);
		}

		/// <summary>
		/// Hides the notification box.
		/// </summary>
		/// <param name="animationDuration">Duration of the animation in seconds.</param>
		public void Hide(int animationDuration) {
			Tween.Move(notificationBox, shownPosition, hiddenPosition, this.animationDuration,
				animationDuration);
		}

		/// <summary>
		/// Shows the notification box.
		/// </summary>
		public void Show() {
			Tween.Move(notificationBox, hiddenPosition, shownPosition, animationDuration);
		}

		/// <summary>
		/// Shows the notification box.
		/// </summary>
		/// <param name="shownTime">Time for the notification to be displayed for in seconds.</param>
		public void Show(int shownTime) {
			Tween.Move(notificationBox, hiddenPosition, shownPosition, animationDuration);
			Hide(shownTime);
		}

		/// <summary>
		/// Shows the notification box with a text for a pre-defined time, then hides it back again.
		/// </summary>
		/// <param name="text">Text to be shown in the notification.</param>
		public void Notify(string text) {
			SetText(text);
			Show(Duration);
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
