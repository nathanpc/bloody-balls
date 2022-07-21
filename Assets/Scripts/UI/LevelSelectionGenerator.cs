using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BloodyBalls.Levels;
using BloodyBalls.Managers;

namespace BloodyBalls.UI {
	/// <summary>
	/// Generates the level selector menu.
	/// </summary>
	public class LevelSelectionGenerator : MonoBehaviour {
		[Header("UI Elements")]
		[SerializeField] protected SceneManager sceneManager;
		[SerializeField] protected GameObject containerSizingObject;
		[SerializeField] protected GameObject buttonContainer;
		[SerializeField] protected GameObject buttonPrefab;

		[Header("Styling")]
		[SerializeField] protected GameObject levelTypeContainer;
		[SerializeField] protected int buttonsPerRow = 4;
		[SerializeField] protected int ySpacing = 30;
		[SerializeField] protected int margins = 30;

		[Header("Disposition")]
		[SerializeField] protected int numberOfLevels = 30;

		protected List<LevelType> levelTypes;
		private Rect containerRect;
		private Rect buttonRect;
		private float xDisplacement;
		private float yDisplacement;

		// Start is called before the first frame update
		void Start() {
			containerRect = containerSizingObject.GetComponent<RectTransform>().rect;
			buttonRect = buttonPrefab.GetComponent<RectTransform>().rect;

			// Compute object displacements.
			xDisplacement = ((containerRect.width -
				((buttonRect.width * buttonsPerRow) + (margins * 2))) / buttonsPerRow) +
				buttonRect.width;
			yDisplacement = ySpacing + buttonRect.height;

			// Set the new scrolled content area height.
			containerRect.height = (margins * 2) + (yDisplacement * Mathf.Ceil(numberOfLevels / buttonsPerRow));
			buttonContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
				RectTransform.Axis.Vertical, containerRect.height);

			// Populate the level type array.
			levelTypes = new List<LevelType>();
			foreach (LevelType levelType in levelTypeContainer.GetComponentsInChildren<LevelType>()) {
				levelTypes.Add(levelType);
			}

			// Place the buttons on screen.
			PlaceButtons();
		}

		/// <summary>
		/// Places the buttons on screen in a nice grid.
		/// </summary>
		private void PlaceButtons() {
			// Sets the base position to start calculating each subsequent row/column.
			Vector2 rowBasePosition = buttonContainer.transform.position;
			rowBasePosition.x -= containerRect.width / 2;
			rowBasePosition.x += (buttonRect.width / 2) + (margins / 2);
			rowBasePosition.y -= buttonRect.height;

			// Go through creating the buttons.
			for (int i = 0; i < numberOfLevels; i++) {
				// Determine the position of the button in the grid.
				Vector3 objectPosition = new Vector2(rowBasePosition.x + ((i % buttonsPerRow) * xDisplacement) + margins,
					rowBasePosition.y + margins - (yDisplacement * Mathf.Floor(i / buttonsPerRow)));

				// Create the button and set it up.
				GameObject buttonObj = Instantiate(buttonPrefab, objectPosition, buttonPrefab.transform.rotation,
					buttonContainer.transform);
				LevelSelectionButton button = buttonObj.GetComponent<LevelSelectionButton>();
				button.SceneManager = sceneManager;
				button.Button = buttonObj.GetComponent<Button>();
				button.LevelNumber = i + 1;
				button.LevelType = GetLevelType(i);
			}
		}

		/// <summary>
		/// Gets the appropriate level type given a level number.
		/// </summary>
		/// <param name="levelNumber">Number of the level to take into consideration.</param>
		/// <returns>Level type related to the level number.</returns>
		private LevelType GetLevelType(int levelNumber) {
			int decision = levelNumber / 10;
			if (decision >= levelTypes.Count)
				decision /= 10;

			return levelTypes[decision];
		}
	}
}