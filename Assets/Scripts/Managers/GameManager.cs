using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using AppAdvisory.Utils;
using AppAdvisory.BallX;
using BloodyBalls.Cells;
using BloodyBalls.Levels;
using BloodyBalls.Utilities;
using BloodyBalls.UI;

namespace BloodyBalls.Managers {
	/// <summary>
	/// Main manager that literally takes care of everything in the game.
	/// </summary>
	public class GameManager : MonoBehaviour {
		[Header("Managers")]
		[SerializeField] protected LevelManager levelManager;
		[SerializeField] protected UIManager uiManager;
		[SerializeField] protected SceneManager sceneManager;
		[SerializeField] protected PopupQuizManager quizManager;

		[Header("Player")]
		[SerializeField] protected float ballSpeed = 10;
		[SerializeField] protected float ballSpawnFrequency = 0.25f;

		[Header("Game Design")]
		[SerializeField] protected int rowsToSpawn = 5;

		private bool gameStarted = false;
		private float maxCellProbability;

		[Header("Complete Mess")]

		[SerializeField]
		private int nTurnToUpgradeMaxCellCount = 1;
		[SerializeField]
		private int upgradeMaxCellCount = 2;

		[SerializeField]
		private int nTurnToUpgradeMinCellCount = 4;
		[SerializeField]
		private int upgradeMinCellCount = 1;

		private int currentMinCellCount;
		private int currentMaxCellCount;

		private int startMinCellCount = 1;
		private int startMaxCellCount = 2;

		[SerializeField]
		private Color[] cellColors;
		[SerializeField]
		private int colorStep = 10;

		[SerializeField]
		private int coinsPerVideo = 10;

		[SerializeField]
		public int numberOfPlayToShowInterstitial = 3;

		[SerializeField]
		private float brickProbability = 0.3f;

		[SerializeField]
		private float powerUpProbability = 0.1f;

		[SerializeField]
		private float emptyProbabilty = 0.2f;

		[SerializeField, Range(0, 1)]
		private float addBallProbability = 0.75f;

		private float maxSpawnProbability;

		[SerializeField] private Cell[] brickPrefabs;

		[SerializeField] private float[] brickProbabilities;

		[SerializeField] private AddBall addBallPrefab;

		[SerializeField] private AddCoin addCoinPrefab;


		[SerializeField] private AudioSource source;

		[SerializeField] private AudioClip gameOver;

		private Rect screenRect;
		//[SerializeField, Range(0f, 1f)] private float distanceBetweenCellsCoeff = 0.1f;

		public List<Transform> spawnedCells;

		private int nTurn = 0;
		private int ballToAddCount = 0;

		IEnumerator Start() {
			if (brickPrefabs.Length != brickProbabilities.Length)
				throw new System.Exception("Cell Prefabs and Probabilities don't have the same length!");

			// Sets up the game aspects.
			spawnedCells = new List<Transform>();
			uiManager.SetupUIElements();
			SetupProbabilities();
			SetupPlayer();
			StartGame();

			// Add the quiz popup to the scene.
			sceneManager.AddQuizPopup();
			yield return null;
			quizManager = FindObjectOfType<PopupQuizManager>();
		}

		/// <summary>
		/// Sets up the probabilities involved in the game.
		/// </summary>
		private void SetupProbabilities() {
			maxCellProbability = 0;
			for (int i = 0; i < brickProbabilities.Length; i++) {
				maxCellProbability += brickProbabilities[i];
			}

			maxSpawnProbability = brickProbability + powerUpProbability + emptyProbabilty;
		}

		void StartGame() {
			uiManager.SetHUDCoins(Utils.GetCoins());
			uiManager.SetHUDBestScore(Utils.GetBestScore());
			uiManager.DisplayHUD(true);


			nTurn = 1;
			currentMinCellCount = startMinCellCount;
			currentMaxCellCount = startMinCellCount;
			StartPlayer();
			NextLevel(false);

			for (int i = 0; i < rowsToSpawn; i++) {
				AdvanceGrid();
			}

			gameStarted = true;
		}

		private void AdvanceGrid() {
			CreateLine();
			MoveGrid();
			CreateSuperCells();
			UpgradeDifficulty();
		}

		private void CreateLine() {
			float random;
			float probability;
			for (int x = 0; x < uiManager.CellColumns; x++) {
				probability = brickProbability;
				random = Random.Range(0, maxSpawnProbability);
				if (random < probability) {
					CreateBrick(x, 0);
					continue;
				}
				probability += powerUpProbability;
				if (random < probability) {
					CreatePowerUp(x, 0);
					continue;
				}
			}
		}

		private void MoveGrid() {
			Vector3 endPosition;
			foreach (Transform gridCell in spawnedCells) {
				endPosition = gridCell.position - Vector3.up * uiManager.CellStepX;
				//gridCell.DOMove(gridCell.position, endPosition, 0.5f);
				gridCell.position = endPosition;

				Cell cell = gridCell.GetComponent<Cell>();
				if (cell != null) {
					cell.MoveToNextLine();
				}
			}
		}

		/// <summary>
		/// Starts a new turn and increases the difficulty.
		/// </summary>
		/// <param name="showNotification">Show a litle notification related to this level?</param>
		public void NextTurn(bool showNotification) {
			if (gameStarted && (spawnedCells.Count == 0)) {
				if (PlayerPrefs.GetInt("LastFinishedLevel", 0) < levelManager.CurrentLevelType.LevelNumber)
					PlayerPrefs.SetInt("LastFinishedLevel", levelManager.CurrentLevelType.LevelNumber);
				sceneManager.SwitchToFinishedLevel();
			}

			// Do visual stuff.
			uiManager.SetHUDCurrentScore(nTurn);
			StartCoroutine(NextTurnCoroutine());
		}

		/// <summary>
		/// Goes to the next "level" in the game.
		/// </summary>
		/// <param name="clearGrid">Clear the cell grid?</param>
		public void NextLevel(bool clearGrid) {
			// Setup a new level.
			levelManager.GoToNextLevel();
			uiManager.ApplySkin(levelManager.CurrentLevelType);
			brickPrefabs = levelManager.CurrentLevelType.Cells;
			brickProbabilities = levelManager.CurrentLevelType.CellProbabilities;
			SetupProbabilities();

			// Should we destroy everything?
			if (clearGrid) {
				for (int i = (spawnedCells.Count - 1); i > 0; i--) {
					// Destroy a simple cell.
					Cell cell = spawnedCells[i].GetComponent<Cell>();
					if (cell != null) {
						cell.Kill();
						continue;
					}

					// Destroy a "power-up".
					Destroy(spawnedCells[i].gameObject);
					spawnedCells.RemoveAt(i);
				}
			}

			// Go to the next turn and display a little message.
			// TODO: Go to "next turn scene".

			uiManager.DisplayHUD(true);
			NextTurn(true);
		}

		/// <summary>
		/// Creates a super cell whenever there are 3 cells of the same type together.
		/// </summary>
		private void CreateSuperCells() {
#if SUPERCELLS
			// Go through the spawned cells.
			for (int i = 0; i < spawnedCells.Count; i++) {
				// Filter out only proper cells.
				Cell masterCell = spawnedCells[i].GetComponent<Cell>();
				if (masterCell == null)
					continue;
				if ((i + 1) >= spawnedCells.Count)
					break;
				Cell adjCell = spawnedCells[i + 1].GetComponent<Cell>();
				if (adjCell == null)
					continue;

				// Stop looking whenever we go past the first line.
				if (masterCell.gridY > 1)
					break;

				// Check if we have an adjacent cell.
				if ((masterCell.cellName == adjCell.cellName) && (masterCell.gridX == (adjCell.gridX - 1))) {
					// Found an adjacent cell, look for a bottom one.
					for (int j = i + 2; j < spawnedCells.Count; j++) {
						// Get adjacent cell.
						Cell bottomCell = spawnedCells[j].GetComponent<Cell>();
						if (bottomCell == null)
							continue;

						// Only look for things in the second line and below the found ones.
						if ((bottomCell.gridY == 1) || ((bottomCell.gridX != masterCell.gridX) && (bottomCell.gridX != adjCell.gridX)))
							continue;

						// Check if we have a bottom cell.
						if (bottomCell.cellName == masterCell.cellName) {
							Debug.Log("CREATE A MASTER CELL! " + masterCell.cellName);
							Debug.Log("(" + masterCell.gridX + ", " + masterCell.gridY + ") (" +
								adjCell.gridX + ", " + adjCell.gridY + ") (" + bottomCell.gridX +
								", " + bottomCell.gridY + ")");

							Debug.DrawLine(masterCell.transform.position, adjCell.transform.position, Color.red);
							Debug.DrawLine(masterCell.transform.position, bottomCell.transform.position, Color.red);
							Debug.DrawLine(bottomCell.transform.position, adjCell.transform.position, Color.red);

							masterCell.Color = Color.black;
							adjCell.Color = Color.black;
							bottomCell.Color = Color.black;
						}

						// Stop looking whenever we go past the second line.
						if (bottomCell.gridY > 2)
							break;
					}
				}
			}
#endif
		}

		private IEnumerator NextTurnCoroutine() {
			if (CheckLoose()) {
				ballToAddCount = 0;
				source.PlayOneShot(gameOver);
				yield return new WaitForSeconds(0.5f);
				GameOver();
			} else {
				yield return new WaitForSeconds(0.5f);
				for (int i = 0; i < ballToAddCount; i++) {
					uiManager.Player.AddBall();
				}
				ballToAddCount = 0;

				//player.transform.position = new Vector3 (screenRect.xMax, player.transform.position.y, 0);	
				uiManager.Player.StartTurn();
			}
		}

		private void GameOver() {
			ShowAds();

			for (int i = spawnedCells.Count - 1; i > -1; i--) {
				Destroy(spawnedCells[i].gameObject);
				spawnedCells.RemoveAt(i);
			}

			//uiManager.Player.Hide();

			Utils.SetBestScore(nTurn);
			int bestScore = Utils.GetBestScore();

			sceneManager.SwitchToGameOver();

			/*
			uiManager.DisplayHUD(false);
			uiManager.SetGameOverBestScore(bestScore);
			uiManager.SetGameOverCurrentScore(nTurn);
			uiManager.DisplayGameOver(true);
			*/
		}

		private void UpgradeDifficulty() {
			if (nTurn % nTurnToUpgradeMinCellCount == 0) {
				currentMinCellCount += upgradeMinCellCount * levelManager.CurrentLevelType.LevelNumber;
			}

			if (nTurn % nTurnToUpgradeMaxCellCount == 0) {
				currentMaxCellCount += upgradeMaxCellCount * levelManager.CurrentLevelType.LevelNumber;
			}
		}

		private bool CheckLoose() {
			if (spawnedCells.Count == 0)
				return false;

			for (int i = 0; i < spawnedCells.Count; i++) {
				if (spawnedCells[i].CompareTag(Constants.PICKABLE_TAG))
					continue;

				Vector3 startPosition = spawnedCells[i].position;
				int layerMask = ~((1 << 9) | (1 << 10));
				RaycastHit2D hit = Physics2D.Raycast(startPosition, -Vector3.up, uiManager.CellStepX * 2.25f, layerMask);
				if (!hit)
					return false;

				return hit.collider.CompareTag(Constants.FLOOR_TAG);
			}

			return false;
		}

		/// <summary>
		/// Event that's fired whenever a turn has ended.
		/// </summary>
		private void OnTurnEnded() {
			// Increase the turn counter.
			nTurn++;

			/*
			// Check if it's time to go to the next level or do a little quiz.
			if ((nTurn % levelManager.TurnsBeforeLevelSwitch) == 0) {
				uiManager.DisplayHUD(false);
				uiManager.DisplayNextLevel(true);
				return;
			} else if ((nTurn % levelManager.TurnsBeforeQuizNag) == 0) {
				quizManager.Open(levelManager.CurrentLevelType.GetRandomQuiz());
				return;
			}
			*/

			// Just go to the next turn.
			NextTurn(false);
		}

		private void CreatePowerUp(int x, int y) {
			float random = Random.value;
			Transform powerupTransform;

			if (random < addBallProbability) {
				AddBall addBall = Instantiate(addBallPrefab);
				addBall.OnCollision += AddBall_OnCollision;
				powerupTransform = addBall.transform;

			} else {
				AddCoin addCoin = Instantiate(addCoinPrefab);
				addCoin.OnCollision += AddCoin_OnCollision;
				powerupTransform = addCoin.transform;
			}

			powerupTransform.SetParent(uiManager.GridContainer);
			powerupTransform.localPosition = GetPositionFromModel(x, y);
			powerupTransform.localScale *= uiManager.CellStepX;
			//spawnedCells.Add(powerupTransform);
			spawnedCells.Insert(0, powerupTransform);
		}

		private void AddBall_OnCollision(AddBall sender) {
			spawnedCells.Remove(sender.transform);
			ballToAddCount++;

			// Show quiz.
			quizManager.Open(levelManager.CurrentLevelType.GetRandomQuiz(),
				() => {
					Debug.Log("Right Answer!");
					quizManager.Close();
				}, () => {
					Debug.Log("Wrong Answer!");
					quizManager.Close();
				});
		}

		private void AddCoin_OnCollision(AddCoin sender) {
			spawnedCells.Remove(sender.transform);
			Utils.AddCoins(1);
			uiManager.SetHUDCoins(Utils.GetCoins());
		}

		private void CreateBrick(int x, int y) {
			float random = Random.Range(0f, maxCellProbability);
			float probability = 0;
			Cell cell = null;

			for (int i = 0; i < brickPrefabs.Length; i++) {
				probability += brickProbabilities[i];

				if (random < probability) {
					cell = Instantiate(brickPrefabs[i]);
					break;
				}
			}
			cell.transform.SetParent(uiManager.GridContainer);
			cell.gameObject.name += "_" + x.ToString();
			cell.transform.localScale *= uiManager.CellStepX;
			cell.transform.localPosition = GetPositionFromModel(x, y);
			cell.gridX = x;
			cell.gridY = y;

			cell.OnDestroyedByBall += Cell_OnDestroyedByBall;

			spawnedCells.Add(cell.transform);
			//spawnedCells.Insert(0, cell.transform);
			int count = Random.Range(currentMinCellCount, currentMaxCellCount + 1);
			//cell.Count = count;
			cell._count = count;

			cell.SetCount(count);
			cell.SetColors(cellColors, colorStep);
		}

		void Cell_OnDestroyedByBall(IHitableByBall sender) {
			MonoBehaviour mono = (MonoBehaviour)sender;
			spawnedCells.Remove(mono.transform);
		}

		Vector3 GetPositionFromModel(int x, int y) {
			//Vector3 position = new Vector3 (stepX + x * (stepX + distanceBetweenCellsCoeff), -y * (stepX + distanceBetweenCellsCoeff), 0);
			Vector3 position = new Vector3(uiManager.CellStepX + x * uiManager.CellStepX, -y * uiManager.CellStepX, 0);
			return position;

		}
		
		/// <summary>
		/// Sets up the player's parameters.
		/// </summary>
		void SetupPlayer() {
			uiManager.Player.Speed = ballSpeed;
			uiManager.Player.SpawnFrequency = ballSpawnFrequency;
			uiManager.Player.TurnEnded += OnTurnEnded;
		}

		/// <summary>
		/// Starts up a new player session.
		/// </summary>
		void StartPlayer() {
			uiManager.Player.SetupBalls();
			uiManager.Player.Show();
		}

		/// <summary>
		/// If using Very Simple Ads by App Advisory, show an interstitial if number of play > numberOfPlayToShowInterstitial: http://u3d.as/oWD
		/// </summary>
		public void ShowAds() {
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT", 0);
			count++;

#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{

				if(AdsManager.instance.IsReadyInterstitial())
				{
					PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
					AdsManager.instance.ShowInterstitial();
				}
			}
			else
			{
				PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
#else
			if (count >= numberOfPlayToShowInterstitial) {
				PlayerPrefs.SetInt("GAMEOVER_COUNT", 0);
			} else {
				PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
#endif
		}

	}

}