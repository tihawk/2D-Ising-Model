using UnityEngine;
using System.Collections.Generic;

public class SpinTileRendererController : MonoBehaviour {

	public static SpinTileRendererController Instance;

	public Sprite spinTileSprite;
	public Sprite spinUpSprite;
	public Sprite spinDownSprite;
//	// For ordering the tiles in a grid:
//	int y = -1;
//	int x = 0;

	int gridSize;
	//int n;
	Vector2 currentSpin = new Vector2 ();
	Dictionary<Vector2, int> spinData;

	Dictionary<Vector2, GameObject> spinTileGameObjectMap;
	Dictionary<Vector2, GameObject> spinOrientationGameObjectMap;
	Dictionary<Vector2, SpriteRenderer> spinOrientationSpriteRendererMap;

	// Use this for initialization
	void Start () {

		Instance = this;

		spinTileGameObjectMap = new Dictionary<Vector2, GameObject> ();
		spinOrientationGameObjectMap = new Dictionary<Vector2, GameObject> ();
		spinOrientationSpriteRendererMap = new Dictionary<Vector2, SpriteRenderer> ();

		InitializeEverything ();
	
	}

	// Update is called once per frame
	void LateUpdate () {

		spinData = IsingModelController.Instance.isingModel.spins;

		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				currentSpin.x = x;
				currentSpin.y = y;

				if (spinData[currentSpin] == 1)
					spinOrientationSpriteRendererMap[currentSpin].sprite = spinUpSprite;

				else if (spinData[currentSpin] == -1)
					spinOrientationSpriteRendererMap[currentSpin].sprite = spinDownSprite;
			}
		}
	}

//	public void DestroyChildren()
//	{
//		for (int i = 0; i < transform.childCount; i++)
//		{
//			Destroy (transform.GetChild (i).gameObject);
//		}
//
//		spinTileGameObjectMap.Clear ();
//		spinOrientationGameObjectMap.Clear ();
//		spinOrientationSpriteRendererMap.Clear ();
//
//		InitializeEverything ();
//	}

	void InitializeEverything()
	{
		
		spinData = IsingModelController.Instance.isingModel.spins;
		//n = spinData.Count;
		gridSize = IsingModelController.gridSize;

		// Create a GameObject for each spinTile and assign them
		// both to a dictionary entry, thus giving the sprite the
		// coordinates of the Nth spinTile:
		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				GameObject spinTile_GO = new GameObject ();
				GameObject spinOrientation_GO = new GameObject ();

				currentSpin.x = x;
				currentSpin.y = y;

				spinTileGameObjectMap.Add (currentSpin, spinTile_GO);
				spinOrientationGameObjectMap.Add (currentSpin, spinOrientation_GO);


				//			// Orders the tiles in a grid, for easier viewing. Doesn't
				//			// mean that it's 2D!
				//			if (i == -y*100)
				//			{
				//				y--;
				//				x = 0;
				//
				//			}
				//			x++;

				spinTile_GO.name = "spinTile_" + x + "_" + y;
				spinTile_GO.transform.position = new Vector3 (x, y, 0);
				spinTile_GO.transform.SetParent (this.transform, true);

				spinOrientation_GO.name = "spinOrient_" + x + "_" + y;
				spinOrientation_GO.transform.position = new Vector3 (x, y, 0);
				spinOrientation_GO.transform.SetParent (this.transform, true);


				// Add a sprite renderer to the GameObject
				SpriteRenderer spinTile_sr = spinTile_GO.AddComponent<SpriteRenderer> ();
				spinTile_sr.sprite = spinTileSprite;
				spinTile_sr.sortingLayerName = "spinTile";
				SpriteRenderer spintOrientation_sr = spinOrientation_GO.AddComponent<SpriteRenderer> ();
				spintOrientation_sr.sortingLayerName = "spinOrient";
				spinOrientationSpriteRendererMap.Add (currentSpin, spintOrientation_sr);

				if (spinData[currentSpin] == 1)
					spinOrientationSpriteRendererMap[currentSpin].sprite = spinUpSprite;

				else if (spinData[currentSpin] == -1)
					spinOrientationSpriteRendererMap[currentSpin].sprite = spinDownSprite;
			}
		}

	}

}
