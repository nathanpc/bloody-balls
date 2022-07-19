using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates the level selector menu.
/// </summary>
public class LevelSelectionGenerator : MonoBehaviour {
	[Header("UI Elements")]
	[SerializeField] protected GameObject containerSizingObject;
	[SerializeField] protected GameObject buttonContainer;
	[SerializeField] protected GameObject buttonPrefab;

	[Header("Styling")]
	[SerializeField] protected int buttonsPerRow = 4;
	[SerializeField] protected int ySpacing = 30;
	[SerializeField] protected int margins = 30;

	[Header("Disposition")]
	[SerializeField] protected int numberOfLevels = 30;

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

		// Place the buttons on screen.
		PlaceButtons();
	}

	/// <summary>
	/// Places the buttons on screen in a nice grid.
	/// </summary>
	private void PlaceButtons() {
		Vector2 rowBasePosition = buttonContainer.transform.position;
		rowBasePosition.x -= containerRect.width / 2;
		rowBasePosition.x += (buttonRect.width / 2) + (margins / 2);
		rowBasePosition.y -= buttonRect.height;

		for (int i = 0; i < numberOfLevels; i++) {
			Vector3 objectPosition = new Vector2(rowBasePosition.x + ((i % buttonsPerRow) * xDisplacement) + margins,
				rowBasePosition.y + margins - (yDisplacement * Mathf.Floor(i / buttonsPerRow)));

			Instantiate(buttonPrefab, objectPosition, buttonPrefab.transform.rotation, buttonContainer.transform);
		}
	}
}
