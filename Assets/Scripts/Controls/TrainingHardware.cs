using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Utilities;
using UnityEngine.Networking;
using Defective.JSON;

public class TrainingHardware : MonoBehaviour {
	[Header("API")]
	[SerializeField] private string endpoint = "http://localhost:5050";

	[Header("UI Elements")]
	[SerializeField] private RectTransform dialog;
	[SerializeField] private Text text;

	[Header("Animation")]
	public int animationDuration = 1;

	[Header("Hardware Control")]
	public int sessionDuration = 30;
	public Color idleColor = new Color(100, 100, 255);
	public Color activeColor = new Color(255, 0, 0);
	public Color endColor = new Color(0, 255, 0);

	private Vector2 hiddenPosition;
	private Vector2 shownPosition;

	public delegate void CallbackPrototype();
	public CallbackPrototype callback;

	// Start is called before the first frame update
	void Start() {
		hiddenPosition = dialog.position;
		shownPosition = dialog.position;

		Rect rect = dialog.rect;
		hiddenPosition.y += rect.height + CameraUtils.GetScreenRect().height + 2000;
		dialog.position = hiddenPosition;
	}

	public void SetText(string message) {
		text.text = message;
	}

	public IEnumerator DoSession() {
		Show();
		SetText("Setting up a new training session...");
		yield return StartCoroutine(SetupSession());
		callback();
	}

	private IEnumerator SetupSession() {
		WWWForm form = new WWWForm();
		form.AddField("idle_color", ((int)idleColor.r) + "," + ((int)idleColor.g) + "," + ((int)idleColor.b));
		form.AddField("act_color", ((int)activeColor.r) + "," + ((int)activeColor.g) + "," + ((int)activeColor.b));
		form.AddField("end_color", ((int)endColor.r) + "," + ((int)endColor.g) + "," + ((int)endColor.b));
		form.AddField("duration", sessionDuration);

		using (UnityWebRequest www = UnityWebRequest.Post(endpoint + "/setup", form)) {
			yield return www.SendWebRequest();

			// For development purposes, let's just ignore any "No connection" issues.
			if (www.result != UnityWebRequest.Result.Success) {
				Debug.Log(www.error);
				Debug.Log("For development purposes, let's just ignore any \"No connection\" issues.");
				yield return new WaitForSeconds(1);
			}

			yield return StartCoroutine(StartSession());
		}
	}

	private IEnumerator StartSession() {
		SetText("Waiting for training session to end...");

		using (UnityWebRequest www = UnityWebRequest.Post(endpoint + "/start", new WWWForm())) {
			yield return www.SendWebRequest();

			// For development purposes, let's just ignore any "No connection" issues.
			if (www.result != UnityWebRequest.Result.Success) {
				Debug.Log(www.error);
				Debug.Log("For development purposes, let's just ignore any \"No connection\" issues.");
				yield return new WaitForSeconds(1);
			}

			yield return StartCoroutine(CheckSessionEnded());
		}
	}

	private IEnumerator CheckSessionEnded() {
		bool sessionFinished = false;
		float avgResponseTime = 0;
		int mockupIteration = 0;

		while (!sessionFinished) {
			using (UnityWebRequest www = UnityWebRequest.Get(endpoint + "/session_info")) {
				yield return www.SendWebRequest();
				string response;

				// For development purposes, let's just ignore any "No connection" issues.
				if (www.result != UnityWebRequest.Result.Success) {
					Debug.Log(www.error);
					response = "{\"finished\": " + ((mockupIteration > 9) ? "true" : "false") + ",\"avgresponse\": 0.5}";
					SetText("Waiting for training session to end... " + mockupIteration);
					mockupIteration++;
				} else {
					response = www.downloadHandler.text;
				}


				JSONObject json = new JSONObject(response);
				sessionFinished = json["finished"].boolValue;
				avgResponseTime = json["avgresponse"].floatValue;
			}

			yield return new WaitForSeconds(1);
		}

		SetText("Good job! Your average response time was " + avgResponseTime + "s!");
		Hide(3);
		yield return new WaitForSeconds(2);
	}

	protected void Hide(int delay) {
		Tween.Move(dialog, shownPosition, hiddenPosition, animationDuration, delay);
	}

	protected void Show() {
		Tween.Move(dialog, hiddenPosition, shownPosition, animationDuration);
	}
}
