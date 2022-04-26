
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AppAdvisory.BallX
{
	public class AddBall : MonoBehaviour
	{
		[SerializeField] private Transform border;
		public Action<AddBall> OnCollision;
		private Vector3 startScale;
		private float count =0;
		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.CompareTag (Constants.BALL_TAG)) 
			{
				if (OnCollision != null)
					OnCollision (this);

				Destroy (gameObject);
			} else if (other.CompareTag (Constants.FLOOR_TAG)) {
				gameObject.SetActive (false);
			}
		}

		void Start() {
			count = UnityEngine.Random.Range (0, Mathf.PI);
			startScale = border.localScale;
		}

		void Update() {
			count += Time.deltaTime;
			border.localScale = startScale * (1 + 0.1f * Mathf.Sin (count * 10));
		}
	}
}