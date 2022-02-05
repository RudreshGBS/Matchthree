using rudscreation.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour 
{
	// If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
	public const float MAX_SWIPE_TIME = 0.5f;

	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.05f;

	public bool swipedRight = false;
	public bool swipedLeft = false;
	public bool swipedUp = false;
	public bool swipedDown = false;


	public bool debugWithArrowKeys = true;

	Vector2 startPos;
	float startTime;
    private Vector3 touchPosWorld;
	Tile currentTile = null;

	public void Update()
	{
        swipedRight = false;
        swipedLeft = false;
        swipedUp = false;
        swipedDown = false;

        if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			
			if (t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
				startTime = Time.time;

				if (Input.touchCount == 1)
				{
					touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

					Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

					//We now raycast with this information. If we have hit something we can process it.
					RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

					if (hit.collider != null)
					{
						 currentTile = hit.collider.gameObject.GetComponent<Tile>();
						//GameObject touchedObject = hit.transform.gameObject;

						//Debug.Log("Touched " + touchedObject.transform.name);
					}
				}
				
			}
			if (t.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{ // Horizontal swipe
					if (swipe.x > 0)
					{
						swipedRight = true;
						currentTile?.SwipeTile(currentTile.Position + Vector2Int.right);
					}
					else
					{
						swipedLeft = true;
						currentTile?.SwipeTile(currentTile.Position + Vector2Int.left);

					}
				}
				else
				{ // Vertical swipe
					if (swipe.y > 0)
					{
						swipedUp = true;
						currentTile?.SwipeTile(currentTile.Position + Vector2Int.up);
					}
					else
					{
						swipedDown = true;
						currentTile?.SwipeTile(currentTile.Position + Vector2Int.down);
					}
				}
				currentTile = null;
			}
		}

		if (debugWithArrowKeys)
		{
			swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
			swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
			swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
			swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
		}
	}
   
}