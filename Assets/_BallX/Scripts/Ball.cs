using UnityEngine;
using System.Collections;
using System;

namespace AppAdvisory.BallX 
{
	public class Ball : MonoBehaviour {


		[SerializeField] private AudioSource source;
		[SerializeField] private AudioClip hitBrick;
		[SerializeField] private AudioClip hitPickable;

		public Action<Ball> HitFloor;

		public float Speed {
			get;
			set;
		}

		public Vector3 Direction {
			get;
			set;
		}

		private float timeToConsiderBeingStuck = 15f;
		private float timer = 0;

		private Vector3 baseOffsetDirection = Vector3.down * 0.1f;

		void FixedUpdate() {
			transform.position += Direction * Time.deltaTime * Speed;

			timer += Time.deltaTime;



			RaycastHit2D hit = Physics2D.Raycast (transform.position, Direction, Speed * Time.deltaTime * 1.2f, ~(1 << 8));
			if (!hit)
				return;

			if (hit.collider.CompareTag (Constants.HITABLE_TAG)) {
				//IHitableByBall hitable = hit.collider.GetComponent<IHitableByBall> ();
				Cell hitable = hit.collider.GetComponent<Cell>();

				hitable.BallHit (this);
				source.PlayOneShot (hitBrick);

				timer = 0;
			}

			if (hit.collider.CompareTag (Constants.FLOOR_TAG))
				return;

			if (hit.collider.CompareTag (Constants.PICKABLE_TAG)) {
				source.PlayOneShot (hitPickable);
				timer = 0;
				return;
			}

			Vector3 offsetDirection = Vector3.zero;
			if (timer >= timeToConsiderBeingStuck) {
				timer = 0;
				offsetDirection = baseOffsetDirection;
			}

			Direction = Vector3.Reflect (Direction, hit.normal) + offsetDirection;

		}

		void OnEnable() 
		{
			timer = 0;
		}

		void OnDisable() 
		{
			timer = 0;
		}

		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.CompareTag (Constants.FLOOR_TAG)) 
			{
				if (HitFloor != null)
					HitFloor (this);
			}
		}
			
	}
}
