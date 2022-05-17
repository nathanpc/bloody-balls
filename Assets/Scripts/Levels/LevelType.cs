﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;

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

		[Header("Notifications")]
		[SerializeField] private List<string> _messages;

		protected System.Random random;

		// Start is called before the first frame update
		void Start() {
			// Make sure we got messages.
			if (_messages == null)
				throw new System.Exception("At least a single message is needed for each level.");

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
		/// List of messages that will be displayed to the player as notifications.
		/// </summary>
		public List<string> Messages {
			get { return _messages; }
			set { _messages = value; }
		}
	}
}