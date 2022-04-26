
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

namespace AppAdvisory.Utils
{
	public static class SimpleTween 
	{

		public static void DOMove(this Transform transform, Vector3 startPosition, Vector3 endposition, float time = 1f, float delay = 0f) 
		{
			MonoBehaviour script = transform.GetComponent<MonoBehaviour> ();
			script.StartCoroutine (DOMoveCoroutine(transform, startPosition, endposition, time, delay));
		}

		public static IEnumerator DOMoveCoroutine(Transform transform, Vector3 startPosition, Vector3 endPosition, float time = 1f, float delay = 1f) {
			yield return new WaitForSeconds (delay);

			transform.position = startPosition;

			float count = 0;
			while (count < time) {
				count += Time.deltaTime;

				transform.position = Vector3.Lerp (startPosition, endPosition, count / time);
				yield return null;
			}
			transform.position = endPosition;


		}
	
	}
}