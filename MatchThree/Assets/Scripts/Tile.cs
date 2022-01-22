using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private static Tile selected;
    private SpriteRenderer tileRenderer;
    public Vector2Int Position;

    private void Start() 
    {
        tileRenderer = GetComponent<SpriteRenderer>();
    }

    public void Select() 
    {
        tileRenderer.color = Color.grey;
    }

    public void Unselect() 
    {
        tileRenderer.color = Color.white;
    }

    private void OnMouseDown() 
    {
        if (selected != null)
        {
            if (selected == this)
                return;
            selected.Unselect();
            if (Vector2Int.Distance(selected.Position, Position) == 1)
            {
                GridManager.Instance.SwapTiles(Position, selected.Position);
                selected = null;
            }
            else
            {
                selected = this;
                Select();
            }
        }
        else
        {
            selected = this;
            Select();
        }
    }
   
}
