
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
	public static class Utils  
	{

		private static readonly string PLAYER_PREFS_COIN = "COINS";
		private static readonly string PLAYER_PREFS_BEST_SCORE = "BEST_SCORE";
		public static void AddCoins(int add) {
			int coins = GetCoins () + add;
			PlayerPrefs.SetInt (PLAYER_PREFS_COIN, coins);
			PlayerPrefs.Save ();
		}

		public static int GetCoins() 
		{
			return PlayerPrefs.GetInt (PLAYER_PREFS_COIN, 0);
		}


		public static void SetBestScore(int score) 
		{
			int bestscore = GetBestScore();
			if (bestscore < score) {
				PlayerPrefs.SetInt (PLAYER_PREFS_BEST_SCORE, score);
				PlayerPrefs.Save ();
			}
		}

		public static int GetBestScore() 
		{
			return PlayerPrefs.GetInt (PLAYER_PREFS_BEST_SCORE, 1);
		}
	}
}