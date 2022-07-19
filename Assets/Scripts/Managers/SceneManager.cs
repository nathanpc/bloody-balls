using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Changes the scene to another one and maybe bring some stuff over with us.
	/// </summary>
	public class SceneManager : MonoBehaviour {
		[Header("Scene Names")]
		[SerializeField] protected string mainMenu = "Main Menu";
		[SerializeField] protected string levelMenu = "Level Menu";
		[SerializeField] protected string playArea = "Play Area";
		[SerializeField] protected string finishedLevel = "Finished Level";
		[SerializeField] protected string gameOver = "Game Over";

		/// <summary>
		/// Completely switch to the next scene.
		/// </summary>
		/// <param name="sceneName">Name of the scene to change to.</param>
		public void SwitchScene(string sceneName) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}

		/// <summary>
		/// Switches to the main menu scene.
		/// </summary>
		public void SwitchToMainMenu() {
			SwitchScene(mainMenu);
		}

		/// <summary>
		/// Switches to the level menu scene.
		/// </summary>
		public void SwitchToLevelMenu() {
			SwitchScene(levelMenu);
		}

		/// <summary>
		/// Switches to the play area scene.
		/// </summary>
		public void SwitchToPlayArea() {
			SwitchScene(playArea);
		}

		/// <summary>
		/// Switches to the finished level scene.
		/// </summary>
		public void SwitchToFinishedLevel() {
			SwitchScene(finishedLevel);
		}

		/// <summary>
		/// Switches to the game over scene.
		/// </summary>
		public void SwitchToGameOver() {
			SwitchScene(gameOver);
		}
	}
}