using UnityEngine;
using System.Collections;
using AppAdvisory.Utils;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#pragma warning disable 0162 // code unreached.
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used

#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif

#if VSRATE
using AppAdvisory.VSRATE;
#endif

namespace AppAdvisory.BallX 
{
	public class GameManager : MonoBehaviour 
	{
		private static readonly string VerySimpleAdsURL = "http://u3d.as/oWD";

		private static readonly string VerySimpleRateURL = "http://u3d.as/Dt2";

		[SerializeField] private float speed = 10;

		[SerializeField] private float spawnFrequency = 0.25f;

		[SerializeField] private int numberOfRow = 8;

		[SerializeField] private int numberOfColumn = 7;

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

		[SerializeField, Range(0,1)]
		private float addBallProbability = 0.75f;

		private float maxSpawnProbability;

		[SerializeField] private Cell[] brickPrefabs;

		[SerializeField] private float[] brickProbabilities;

		[SerializeField] private AddBall addBallPrefab;

		[SerializeField] private AddCoin addCoinPrefab;

		[SerializeField]
		private BoxCollider2D leftWall;

		[SerializeField]
		private BoxCollider2D rightWall;

		[SerializeField]
		private BoxCollider2D bottomWall;

		[SerializeField]
		private BoxCollider2D topWall;


		[SerializeField]
		private Transform background;


		[SerializeField] private UIManager uiManager;

		[SerializeField] private Player player;


		[SerializeField] private AudioSource source;

		[SerializeField] private AudioClip gameOver;

		private Rect screenRect;
		//[SerializeField, Range(0f, 1f)] private float distanceBetweenCellsCoeff = 0.1f;
	
		private float stepX;
		private float maxCellProbability;
		private List<Transform> cells;
		private Transform gridContainer;
		private Vector3 bottomLimit;

		private int nTurn = 0;
		private int ballToAddCount = 0;

		void Start () 
		{
			if (brickPrefabs.Length != brickProbabilities.Length) 
			{
				throw new System.Exception ("Cell Prefabs and Probabilities don't have the same length !");
			}

			SubscribeToUIManager ();

			SetUpScreen ();

			SetUpGrid ();

			SetUpLevelBounds ();

			SetUpPlayer ();
		}

		#region UI
		void SubscribeToUIManager() 
		{
			uiManager.PlayButtonClicked += OnPlayButtonClicked;
			uiManager.WatchAdButtonClicked += OnWatchAdButtonClicked;
			uiManager.RateButtonClicked += OnRateButtonClicked;
			uiManager.MainMenuButtonClicked += OnMainMenuButtonClicked;
			uiManager.ReplayButtonClicked += OnReplayButtonClicked;
		}


		void OnPlayButtonClicked() {
			StartGame ();
		}

		void OnWatchAdButtonClicked() {
			#if APPADVISORY_ADS
			AdsManager.instance.ShowRewardedVideo((bool isSuccess) => {
				if(isSuccess)
					Utils.AddCoins(coinsPerVideo);
			});
			#else
			Debug.LogWarning("To show video ads, please have a look at Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
			#endif
		}

		void OnRateButtonClicked() 
		{
			#if VSRATE
			RateUsManager.ShowRateUsWindows();

			#else
			Debug.LogWarning("To rate the game, please have a look at Very Simple Rate on the Asset Store, or go to this link: " + VerySimpleRateURL);
			#endif
		}

		void OnShopButtonClicked() 
		{

		}

		void OnReplayButtonClicked()
		{
			uiManager.DisplayGameOver (false);
			StartGame ();
		}

		void OnMainMenuButtonClicked()
		{
			uiManager.DisplayGameOver (false);
			uiManager.DisplayTitlecard (true);
		}

		#endregion

		void StartGame() 
		{
			uiManager.SetHUDCoins (Utils.GetCoins ());
			uiManager.SetHUDBestScore (Utils.GetBestScore ());
			uiManager.DisplayHUD (true);


			nTurn = 1;
			currentMinCellCount = startMinCellCount;
			currentMaxCellCount = startMinCellCount;
			StartPlayer ();
			NextTurn ();
		}

		void SetUpGrid() {
			//stepX = screenRect.width / ((numberOfColumn+2) + (numberOfColumn + 4) * distanceBetweenCellsCoeff);
			//float startOffset = (stepX / 2) * (1 + distanceBetweenCellsCoeff);

			stepX = Mathf.Min (screenRect.width, screenRect.height) / (numberOfColumn + 2);
			//stepX = screenRect.width / (numberOfColumn+2);
			float startOffset = stepX / 2;

			gridContainer = new GameObject ("Grid").transform;
			cells = new List<Transform> ();

			for (int i = 0; i < brickProbabilities.Length; i++) 
			{
				maxCellProbability += brickProbabilities[i];
			}

			maxSpawnProbability = brickProbability + powerUpProbability + emptyProbabilty;
			bottomLimit = new Vector3 (0, gridContainer.position.y -(numberOfColumn+1.5f) * stepX, 0);
		} 

		private void CreateLine() 
		{
			float random;
			float probability;
			for (int x = 0; x < numberOfColumn; x++) 
			{
				probability = brickProbability;
				random = Random.Range (0, maxSpawnProbability);
				if (random < probability) 
				{
					CreateBrick (x, 0);
					continue;
				}
				probability += powerUpProbability;
				if (random < probability) {
					CreatePowerUp (x, 0);
					continue;
				}
			}
		}

		private void MoveGrid() {
			Vector3 endPosition;
			foreach (Transform gridCell in cells) 
			{
				endPosition = gridCell.position - Vector3.up * stepX;
				gridCell.DOMove (gridCell.position, endPosition, 0.5f);
			}
		}

		private void NextTurn() 
		{
			uiManager.SetHUDCurrentScore (nTurn);
			StartCoroutine (NextTurnCoroutine ());
		}

		private IEnumerator NextTurnCoroutine() 
		{
			CreateLine ();
			MoveGrid ();
			UpgradeDifficulty ();
		

			if (CheckLoose ()) 
			{
				ballToAddCount = 0;
				source.PlayOneShot (gameOver);
				yield return new WaitForSeconds (0.5f);
				GameOver ();
			} else 
			{
				yield return new WaitForSeconds (0.5f);
				for (int i = 0; i < ballToAddCount; i++) 
				{
					player.AddBall ();
				}
				ballToAddCount = 0;

				//player.transform.position = new Vector3 (screenRect.xMax, player.transform.position.y, 0);	
				player.StartTurn ();
					
			}

		}

		private void GameOver() 
		{
			ShowAds ();

			for (int i = cells.Count - 1; i > -1; i--) 
			{
				Destroy (cells [i].gameObject);
				cells.RemoveAt (i);
			}

			DisplayPlayer (false);

			Utils.SetBestScore (nTurn);
			int bestScore = Utils.GetBestScore ();

			uiManager.DisplayHUD (false);
			uiManager.SetGameOverBestScore (bestScore);
			uiManager.SetGameOverCurrentScore (nTurn);
			uiManager.DisplayGameOver (true);
		}

		private void DisplayPlayer(bool isShown) {
			player.gameObject.SetActive (isShown);
		}

		private void UpgradeDifficulty() 
		{
			if (nTurn % nTurnToUpgradeMinCellCount == 0) 
			{
				currentMinCellCount += upgradeMinCellCount;
			}

			if (nTurn % nTurnToUpgradeMaxCellCount == 0) 
			{
				currentMaxCellCount += upgradeMaxCellCount;
			}
		}

		private bool CheckLoose() 
		{
			if (cells.Count == 0)
				return false;

			for (int i = 0; i < cells.Count; i++) 
			{
				if(cells[i].CompareTag(Constants.PICKABLE_TAG))
					continue;

				Vector3 startPosition = cells [i].position;
			

				int layerMask = ~((1 << 9)| (1 << 10));
				RaycastHit2D hit = Physics2D.Raycast (startPosition, -Vector3.up, stepX*2.25f, layerMask);
				if (!hit)
					return false;
				
				return hit.collider.CompareTag (Constants.FLOOR_TAG);
			}
			return false;
		}

		private void OnTurnEnded() {
			nTurn++;
			NextTurn ();
		}

		private void CreatePowerUp(int x , int y) {
			float random = Random.value;
			Transform powerupTransform;

			if (random < addBallProbability) {
				AddBall addBall = Instantiate (addBallPrefab);
				addBall.OnCollision += AddBall_OnCollision;
				powerupTransform = addBall.transform;

			} else {
				AddCoin addCoin = Instantiate (addCoinPrefab);
				addCoin.OnCollision += AddCoin_OnCollision;
				powerupTransform = addCoin.transform;
			}

			powerupTransform.SetParent (gridContainer);
			powerupTransform.localPosition = GetPositionFromModel (x, y);
			powerupTransform.localScale *= stepX;
			cells.Add (powerupTransform);
		}

		private void AddBall_OnCollision (AddBall sender) {
			cells.Remove (sender.transform);
			ballToAddCount++;
		}

		private void AddCoin_OnCollision(AddCoin sender) {
			cells.Remove (sender.transform);
			Utils.AddCoins (1);
			uiManager.SetHUDCoins (Utils.GetCoins ());
		}

		private void CreateBrick(int x, int y) {
			float random = Random.Range (0f, maxCellProbability);
			float probability = 0;
			Cell cell = null;

			for(int i = 0; i < brickPrefabs.Length; i++) 
			{
				probability += brickProbabilities[i];

				if(random < probability) {
					cell = Instantiate(brickPrefabs[i]);
					break;
				}
			}
			cell.transform.SetParent (gridContainer);
			cell.gameObject.name += "_" + x.ToString ();
			cell.transform.localScale *= stepX;
			cell.transform.localPosition = GetPositionFromModel (x, y);

			cell.OnDestroyedByBall += Cell_OnDestroyedByBall;

			cells.Add (cell.transform);
			int count = Random.Range (currentMinCellCount, currentMaxCellCount + 1);
			//cell.Count = count;
			cell._count = count;

			cell.SetCount (count);
			cell.SetColors (cellColors, colorStep);
		}

		void Cell_OnDestroyedByBall (IHitableByBall sender)
		{
			MonoBehaviour mono = (MonoBehaviour)sender;
			cells.Remove (mono.transform);
		}

		Vector3 GetPositionFromModel(int x, int y) {
			//Vector3 position = new Vector3 (stepX + x * (stepX + distanceBetweenCellsCoeff), -y * (stepX + distanceBetweenCellsCoeff), 0);
			Vector3 position = new Vector3 (stepX + x * stepX, -y * stepX, 0);
			return position;

		}

		void SetUpPlayer()
		{
			player.transform.localScale *= stepX;
			player.Speed = speed;
			player.SpawnFrequency = spawnFrequency;
			player.BallScale = stepX;
			player.ScreenRect = screenRect;

			player.SetUpTrajectoryDots ();

			player.TurnEnded += OnTurnEnded;

			//StartPlayer ();
		}

		void StartPlayer() {
			player.transform.position = bottomLimit;
			player.SetUpBalls ();

			DisplayPlayer (true);
		}

		void SetUpScreen() 
		{
			screenRect = CameraTools.GetScreenRect ();
		}

		void SetUpLevelBounds() 
		{
			Camera cam = Camera.main;
			float height = 2f * cam.orthographicSize;
			float width = height * cam.aspect;

			float gridHeight = (numberOfRow+1) * stepX;
			float remainingSpace = height - gridHeight;
			float topBorderHeight = remainingSpace * 0.5f;
			float bottomBorderHeight = remainingSpace * 0.5f;

			float startOffset = stepX / 2;
			gridContainer.position = new Vector3(screenRect.xMin + startOffset, screenRect.yMax - startOffset - topBorderHeight);

			bottomLimit = new Vector3 (0, screenRect.yMin + bottomBorderHeight, 0);


			Vector2 boxWidth = new Vector2 (screenRect.width + 1f, 0.1f);
			Vector2 boxHeight = new Vector2 (0.1f, screenRect.height + 1f);

			topWall.transform.position = new Vector3(0, screenRect.yMax - topBorderHeight, 0);
			topWall.size = boxWidth;

			bottomWall.transform.position = bottomLimit - 0.125f * stepX * Vector3.up;
			bottomWall.size = boxWidth;

			leftWall.transform.position = new Vector3 (screenRect.xMin, 0, 0);

			leftWall.size = boxHeight;

			rightWall.transform.position = new Vector3 (screenRect.xMax, 0, 0);
			rightWall.size = boxHeight; 


			//background.localScale = new Vector3 (screenRect.width + 0.6f, gridHeight + 0.6f, 0);
			background.localScale = new Vector3 (Mathf.Min(screenRect.width, screenRect.height) + 0.6f, gridHeight + 0.6f, 0);


			background.transform.position = (topWall.transform.position + bottomWall.transform.position) / 2;

			leftWall.transform.position = new Vector3 (-background.localScale.x / 2 + startOffset, 0, 0);
			rightWall.transform.position = new Vector3 (background.localScale.x / 2 - startOffset, 0, 0);

			gridContainer.position = new Vector3(- background.localScale.x / 2 + startOffset, screenRect.yMax - startOffset - topBorderHeight);

		}

		/// <summary>
		/// If using Very Simple Ads by App Advisory, show an interstitial if number of play > numberOfPlayToShowInterstitial: http://u3d.as/oWD
		/// </summary>
		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
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
			if(count >= numberOfPlayToShowInterstitial)
			{
			Debug.LogWarning("To show ads, please have a look at Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
			}
			else
			{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#endif
		}

	}

}