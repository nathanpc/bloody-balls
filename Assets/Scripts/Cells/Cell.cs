using UnityEngine;
using System.Collections;
using AppAdvisory.Utils;
using AppAdvisory.BallX;

namespace BloodyBalls.Cells {
	public class Cell : MonoBehaviour, IHitableByBall {

		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private TextMesh number;

		private Vector3 startScale;

		public event OnDestroyedEventHandler OnDestroyedByBall;

		public string cellName = "Unknown";

		public int gridX = 0;
		public int gridY = 0;

		public int _count;

		public int Count {
			get {
				return _count;
				//return int.Parse(number.text);
			}
			set {
				_count = value;
				number.text = _count.ToString();
			}
		}

		public Color Color {
			get {
				return spriteRenderer.color;
			}
			set {
				spriteRenderer.color = value;
			}
		}

		public void MoveToNextLine() {
			gridY++;
		}

		public void SetCount(int count) {

			number.text = count.ToString();
		}

		private int colorStep;
		private Color[] colors;
		public void SetColors(Color[] colors, int colorStep) {
			this.colors = colors;
			this.colorStep = colorStep;
			Color = GetColorFromCount(_count);
		}

		private Color GetColorFromCount(int count) {
			Color color;
			int max;
			for (int i = 0; i < colors.Length - 1; i++) {
				max = (i + 1) * colorStep;
				if (count < max) {
					color = Color.Lerp(colors[i], colors[i + 1], (float)count / colorStep);
					return color;
				}
			}
			color = colors[colors.Length - 1];
			return color;
		}

		private void Awake() {
			startScale = spriteRenderer.transform.localScale;

		}

		public IEnumerator DOPunchScaleCoroutine(float amplitude, float time = 1f) {
			Vector3 midScale = startScale * (1 - amplitude);

			float count = 0;
			float firstDuration = time / 2;

			while (count < firstDuration) {
				count += Time.deltaTime;

				spriteRenderer.transform.localScale = Vector3.Lerp(startScale, midScale, count / firstDuration);
				yield return null;
			}

			count = 0;

			while (count < firstDuration) {
				count += Time.deltaTime;

				spriteRenderer.transform.localScale = Vector3.Lerp(midScale, startScale, count / firstDuration);
				yield return null;
			}

			spriteRenderer.transform.localScale = startScale;
		}

		/// <summary>
		/// Handles being hit by the ball.
		/// </summary>
		/// <param name="ball">Ball that hit us.</param>
		public void BallHit(Ball ball) {
			// Make visual and internal changes.
			_count--;
			number.text = _count.ToString();
			Color = GetColorFromCount(_count);
			StartCoroutine(DOPunchScaleCoroutine(0.1f, 0.1f));

			// We dead?
			if (Count <= 0)
				Kill();
		}

		/// <summary>
		/// Handles the destruction of the cell.
		/// </summary>
		public void Kill() {
			// Trigger the destruction event.
			if (OnDestroyedByBall != null)
				OnDestroyedByBall(this);

			// Actually destroy the cell.
			Destroy(gameObject);
		}
	}
}

