using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AppAdvisory.BallX 
{
	public class Player : MonoBehaviour {
		[SerializeField]
		private Ball ballPrefab;

		[SerializeField]
		private TextMesh nRemainingBalls;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private float offsetRotation = 10f;

		[SerializeField]
		private float numberOfDots = 10;
		[SerializeField]
		private GameObject dotPrefab;

		private List<Transform> trajectoryDots;
		private List<Ball> balls;
		private int stopedBallsCount = 0;

		private float threshold = 0.1f;

		public Action TurnEnded;

		public Rect ScreenRect {
			get;
			set;

		}

		public float Speed {
			get;
			set;
		}

		public float SpawnFrequency {
			get;
			set;
		}

		public float BallScale {
			get;
			set;
		}

		public bool AllBallsStoped {
			get 
			{
				return stopedBallsCount == balls.Count;
			}
		}

		private void SubscribeToInputManager() {
			InputManager.OnSwipeStarted += StartFiring;
			InputManager.OnSwipe += Fire;
			InputManager.OnSwipeEnded += EndFiring;
		}

		private void UnsubscribeToInputManager() {
			InputManager.OnSwipeStarted -= StartFiring;
			InputManager.OnSwipe -= Fire;
			InputManager.OnSwipeEnded -= EndFiring;
		}

		public void SetUpBalls() 
		{
			balls = new List<Ball> ();
			AddBall ();
		}


		public void AddBall() 
		{
			Ball ball = (Ball)Instantiate (ballPrefab);
			ball.transform.position = transform.position;
			ball.transform.localScale *= BallScale;
			ball.gameObject.SetActive (false);
			ball.HitFloor += OnBallHitWall;
			balls.Add (ball);
		}

		private void OnBallHitWall(Ball ball) 
		{
			ball.gameObject.SetActive (false);

			if (stopedBallsCount == 0) {
				transform.position = new Vector3 (ball.transform.position.x, transform.position.y);
				DisplayPlayer (true);
			}

			ball.transform.position = transform.position;
			stopedBallsCount++;;

			if (AllBallsStoped)
				EndTurn ();

		}

		public void StartTurn() 
		{
			SubscribeToInputManager ();

			DisplayNRemainingBalls (true);
			SetNRemainingBalls (balls.Count);

			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, ScreenRect.xMin + transform.localScale.x, ScreenRect.xMax - transform.localScale.x), transform.position.y, 0);
		}

		private void EndTurn() 
		{
			if (TurnEnded != null)
				TurnEnded ();

		}

		public void SetUpTrajectoryDots() {
			trajectoryDots = new List<Transform> ();
			for (int i = 0; i < numberOfDots; i++) {
				GameObject dot = Instantiate (dotPrefab);
				dot.transform.localScale *= BallScale;
				dot.transform.position = Vector3.zero;
				dot.SetActive (false);
				trajectoryDots.Add (dot.transform);
			}
		}


		private void StartFiring(Vector3 startPos) 
		{
			foreach (Transform dot in trajectoryDots) {
				dot.position = transform.position;
				dot.gameObject.SetActive (true);
			}
		}

		private void Fire(Vector3 movement) 
		{
			if (movement == Vector3.zero)
				return;
			SetTrajectoryPoints (transform.position, movement);
		}

		private void EndFiring(Vector3 movement) 
		{
			foreach (Transform dot in trajectoryDots) {
				dot.gameObject.SetActive (false);
			}
				
			if (Vector3.Angle (movement, Vector3.up) > 90 - offsetRotation)
				return;

			Vector3 leftBottom = new Vector3 (ScreenRect.xMin, transform.position.y, 0);

			if (Vector3.Distance (transform.position, leftBottom) < transform.localScale.x + threshold && Vector3.Dot (movement, Vector3.right) < 0)
				return;

			Vector3 rightBottom = new Vector3 (ScreenRect.xMax, transform.position.y, 0);

			if (Vector3.Distance (transform.position, rightBottom) < transform.localScale.x + threshold && Vector3.Dot (movement, Vector3.right) > 0)
				return;

			UnsubscribeToInputManager ();

			DisplayPlayer (false);
			stopedBallsCount = 0;
			StartCoroutine (SpawnBallsCoroutine (movement));
		}

		private IEnumerator SpawnBallsCoroutine(Vector3 movement) {



			Vector3 startPosition = transform.position;
			for(int i = 0; i < balls.Count; i++) 
			{
				Ball ball = balls [i];
				ball.transform.position = startPosition + movement * 0.2f * 0;

				ball.gameObject.SetActive (true);
				ball.Direction = movement.normalized;
				ball.Speed = Speed;

				SetNRemainingBalls (balls.Count - 1 - i);

				yield return new WaitForSeconds (SpawnFrequency);
			}
			yield return new WaitForSeconds (2*SpawnFrequency);
			DisplayNRemainingBalls (false);
		}

		void SetNRemainingBalls(int count) 
		{
			nRemainingBalls.text = "x" + count.ToString ();
		}

		void DisplayNRemainingBalls(bool isShown) 
		{
			nRemainingBalls.gameObject.SetActive (isShown);
		}

		void DisplayPlayer(bool isShown) 
		{
			spriteRenderer.enabled = isShown;
		}

		void SetTrajectoryPoints(Vector3 posStart, Vector2 direction)
		{
			float velocity = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y));
			float angle = Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x));
			float fTime = 0;

			fTime += 0.1f;
			foreach (Transform dot in trajectoryDots)
			{
				float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
				float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad);
				Vector3 pos = new Vector3(posStart.x + dx, posStart.y + dy, 0);
				dot.position = pos;
				dot.gameObject.SetActive (true);
				dot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x));
				fTime += 0.1f;
			}

		}

	}
}