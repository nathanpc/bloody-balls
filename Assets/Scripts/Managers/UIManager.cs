﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;

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

		private float _cellStepX;
		private Vector3 _playFieldBottom;


		[Header("Complete Mess")]
		[SerializeField]
		private RectTransform titlecard;
		[SerializeField]
		private RectTransform gameOver;

		[SerializeField]
		private RectTransform hud;

		[SerializeField]
		private Text gameOverCurrentScore;
		[SerializeField]
		private Text gameOverBestScore;

		[SerializeField]
		private Text hudCurrentScore;
		[SerializeField]
		private Text hudBestScore;
		[SerializeField]
		private Text hudCoins;

		public Action PlayButtonClicked;
		public Action ReplayButtonClicked;
		public Action MainMenuButtonClicked;

		void Start() {
			SetupUIElements();

			DisplayTitlecard(true);
			DisplayGameOver(false);
			DisplayHUD(false);
		}

		/// <summary>
		/// Sets up all of the UI elements.
		/// </summary>
		private void SetupUIElements() {
			SetupCellGrid();
			SetupPlayFieldBounds();
		}

		/// <summary>
		/// Sets up the cells grid.
		/// </summary>
		void SetupCellGrid() {
			Rect screenRect = CameraUtils.GetScreenRect();

			// Calculate the step size between cell columns.
			CellStepX = Mathf.Min(screenRect.width, screenRect.height) / (CellColumns + 2);

			// Calculate where the play field bottom should be.
			PlayFieldBottom = new Vector3(0, GridContainer.position.y - (CellColumns + 1.5f) * CellStepX, 0);
		}

		/// <summary>
		/// Sets up the play field bounds.
		/// </summary>
		void SetupPlayFieldBounds() {
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

			// Finish setting up the side walls.
			leftWall.transform.position = new Vector3(-fieldBackground.localScale.x / 2 + startOffset, 0, 0);
			rightWall.transform.position = new Vector3(fieldBackground.localScale.x / 2 - startOffset, 0, 0);

			// Position the grid container.
			GridContainer.position = new Vector3(-fieldBackground.localScale.x / 2 + startOffset,
				screenRect.yMax - startOffset - topBorderHeight);
		}

		public void DisplayTitlecard(bool isShown) {
			titlecard.gameObject.SetActive(isShown);
		}

		public void DisplayGameOver(bool isShown) {
			gameOver.gameObject.SetActive(isShown);
		}

		public void DisplayHUD(bool isShown) {
			hud.gameObject.SetActive(isShown);
		}

		public void SetGameOverBestScore(int score) {
			gameOverBestScore.text = "BEST " + score.ToString();
		}

		public void SetGameOverCurrentScore(int score) {
			gameOverCurrentScore.text = score.ToString();
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

		public void OnPlayButton() {
			if (PlayButtonClicked != null)
				PlayButtonClicked();

			DisplayTitlecard(false);
		}

		public void OnReplayButton() {
			if (ReplayButtonClicked != null)
				ReplayButtonClicked();
		}

		public void OnMainMenuButton() {
			if (MainMenuButtonClicked != null)
				MainMenuButtonClicked();
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
	}
}