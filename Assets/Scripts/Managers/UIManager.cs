using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using BloodyBalls.Controls;
using BloodyBalls.Levels;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Manages UI stuff.
	/// </summary>
	public class UIManager : MonoBehaviour {
		[Header("General Stuff")]
		[SerializeField] protected Transform screenBackground;

		[Header("Play Field")]
		[SerializeField] protected Transform fieldBackground;
		[SerializeField] private Transform _gridContainer;
		[SerializeField] protected BoxCollider2D leftWall;
		[SerializeField] protected BoxCollider2D rightWall;
		[SerializeField] protected BoxCollider2D bottomWall;
		[SerializeField] protected BoxCollider2D topWall;

		[Header("Cells Grid")]
		[SerializeField] private int _numberOfCellRows = 8;
		[SerializeField] private int _numberOfCellColumns = 7;

		[Header("Player")]
		[SerializeField] private Player _playerObject;

		private float _cellStepX;
		private Vector3 _playFieldBottom;


		[Header("Complete Mess")]
		[SerializeField]
		private RectTransform hud;

		[SerializeField]
		private Text hudCurrentScore;
		[SerializeField]
		private Text hudBestScore;
		[SerializeField]
		private Text hudCoins;

		void Start() {
			SetupUIElements();
			//DisplayHUD(false);
		}

		/// <summary>
		/// Sets up all of the UI elements.
		/// </summary>
		public void SetupUIElements() {
			SetupCellGrid();
			SetupPlayFieldBounds();
			SetupPlayerPosition();
		}

		/// <summary>
		/// Sets up the cells grid.
		/// </summary>
		private void SetupCellGrid() {
			Rect screenRect = CameraUtils.GetScreenRect();

			// Calculate the step size between cell columns.
			CellStepX = Mathf.Min(screenRect.width, screenRect.height) / (CellColumns + 2);

			// Calculate where the play field bottom should be.
			PlayFieldBottom = new Vector3(0, GridContainer.position.y - (CellColumns + 1.5f) * CellStepX, 0);
		}

		/// <summary>
		/// Sets up the play field bounds.
		/// </summary>
		private void SetupPlayFieldBounds() {
			// Get some camera/screen dimensions.
			Camera cam = Camera.main;
			float height = 2f * cam.orthographicSize;
			float width = height * cam.aspect;
			Rect screenRect = CameraUtils.GetScreenRect();

			// Get some specific shared dimensions.
			float gridHeight = (CellRows + 1) * CellStepX;
			float remainingSpace = height - gridHeight;
			float topBorderHeight = remainingSpace * 0.5f;
			float bottomBorderHeight = remainingSpace * 0.5f;

			// Setup grid container and some limits.
			float startOffset = CellStepX / 2;
			GridContainer.position = new Vector3(screenRect.xMin + startOffset, screenRect.yMax - startOffset - topBorderHeight);
			PlayFieldBottom = new Vector3(0, screenRect.yMin + bottomBorderHeight, 0);

			// Setup walls.
			Vector2 boxWidth = new Vector2(screenRect.width + 1f, 0.1f);
			Vector2 boxHeight = new Vector2(0.1f, screenRect.height + 1f);
			topWall.transform.position = new Vector3(0, screenRect.yMax - topBorderHeight, 0);
			topWall.size = boxWidth;
			bottomWall.transform.position = PlayFieldBottom - 0.125f * CellStepX * Vector3.up;
			bottomWall.size = boxWidth;
			leftWall.transform.position = new Vector3(screenRect.xMin, 0, 0);
			leftWall.size = boxHeight;
			rightWall.transform.position = new Vector3(screenRect.xMax, 0, 0);
			rightWall.size = boxHeight;

			// Setup the field background.
			fieldBackground.localScale = new Vector3(Mathf.Min(screenRect.width, screenRect.height) + 0.6f, gridHeight + 0.6f, 0);
			fieldBackground.transform.position = (topWall.transform.position + bottomWall.transform.position) / 2;

			// Setup the screen background.
			screenBackground.localScale = new Vector2(screenRect.width + 3, screenRect.height + 3);
			screenBackground.transform.position = (topWall.transform.position + bottomWall.transform.position) / 2;

			// Finish setting up the side walls.
			leftWall.transform.position = new Vector3(-fieldBackground.localScale.x / 2 + startOffset, 0, 0);
			rightWall.transform.position = new Vector3(fieldBackground.localScale.x / 2 - startOffset, 0, 0);

			// Position the grid container.
			GridContainer.position = new Vector3(-fieldBackground.localScale.x / 2 + startOffset,
				screenRect.yMax - startOffset - topBorderHeight);
		}

		/// <summary>
		/// Sets up the player-related objects.
		/// </summary>
		private void SetupPlayerPosition() {
			// Setup various position attributes.
			Player.transform.localScale *= CellStepX;
			Player.BallScale = CellStepX;
			Player.ScreenRect = CameraUtils.GetScreenRect();
			Player.transform.position = PlayFieldBottom;

			// Create the trajectory dots for the player to orient themselves.
			Player.SetUpTrajectoryDots();
		}
		
		/// <summary>
		/// Applies a skin to the game based on a level.
		/// </summary>
		/// <param name="level">Level type to get the skin from.</param>
		public void ApplySkin(LevelType level) {
			// Backgrounds.
			screenBackground.GetComponent<SpriteRenderer>().color = level.BackgroundColor;
			fieldBackground.GetComponent<SpriteRenderer>().color = level.FieldColor;
		}

		public void DisplayHUD(bool isShown) {
			hud.gameObject.SetActive(isShown);
		}

		public void SetHUDBestScore(int score) {
			hudBestScore.text = score.ToString();
		}

		public void SetHUDCurrentScore(int score) {
			hudCurrentScore.text = score.ToString();
		}

		public void SetHUDCoins(int coins) {
			hudCoins.text = coins.ToString();
		}


		/// <summary>
		/// Container that will hold the grid of cells for us.
		/// </summary>
		public Transform GridContainer {
			get { return _gridContainer; }
			set { _gridContainer = value; }
		}

		/// <summary>
		/// Step size between cell columns.
		/// </summary>
		public float CellStepX {
			get { return _cellStepX; }
			set { _cellStepX = value; }
		}

		/// <summary>
		/// Where the play field bottom should be.
		/// </summary>
		public Vector3 PlayFieldBottom {
			get { return _playFieldBottom; }
			set { _playFieldBottom = value; }
		}

		/// <summary>
		/// Number of cell rows in the grid.
		/// </summary>
		public int CellRows {
			get { return _numberOfCellRows; }
			set { _numberOfCellRows = value; }
		}

		/// <summary>
		/// Number of cell columns in the grid.
		/// </summary>
		public int CellColumns {
			get { return _numberOfCellColumns; }
			set { _numberOfCellColumns = value; }
		}

		/// <summary>
		/// Player object.
		/// </summary>
		public Player Player {
			get { return _playerObject; }
			set { _playerObject = value; }
		}
	}
}