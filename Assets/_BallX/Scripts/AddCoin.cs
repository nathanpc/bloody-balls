
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
	public class AddCoin : MonoBehaviour
	{

		public Action<AddCoin> OnCollision;
		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.CompareTag (Constants.BALL_TAG)) {
				if (OnCollision != null)
					OnCollision (this);

				Destroy (gameObject);
			}
		}
			

	}
}