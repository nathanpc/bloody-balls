using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;

namespace BloodyBalls.Levels {
	/// <summary>
	/// Abstracts away a quiz question and answer.
	/// </summary>
	public class Quiz : MonoBehaviour {
		[SerializeField] private string _question;
		[SerializeField, Range(0, 3)] private int _rightAnswerIndex;
		[SerializeField] private string[] _answers = new string[4];

		/// <summary>
		/// Question to be asked to the player.
		/// </summary>
		public string Question {
			get { return _question; }
			set { _question = value; }
		}

		/// <summary>
		/// Which of the answers is the right one?
		/// </summary>
		public int RightAnswerIndex {
			get { return _rightAnswerIndex; }
			set { _rightAnswerIndex = value; }
		}

		/// <summary>
		/// Answers for the player to guess.
		/// </summary>
		public string[] Answers {
			get { return _answers; }
			set { _answers = value; }
		}
	}
}
