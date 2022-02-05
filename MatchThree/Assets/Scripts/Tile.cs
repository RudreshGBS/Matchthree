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
    private void OnMouseUp()
    {
        //if (selected != null)
        //{
        //    if (selected == this)
        //        return;
        //    selected.Unselect();
        //    if (Vector2Int.Distance(selected.Position, Position) == 1)
        //    {
        //        GridManager.Instance.SwapTiles(Position, selected.Position);
        //        selected = null;
        //    }
        //    else
        //    {
        //        SoundManager.Instance.PlaySound(SoundType.TypeSelect);
        //        selected = this;
        //        Select();
        //    }
        //}
        //else
        //{
        //    SoundManager.Instance.PlaySound(SoundType.TypeSelect);
        //    selected = this;
        //    Select();
        //}
    }
    //private void OnMouseDown() 
    //{
    //    Vector2Int NewPos;
    //    if (swipeInput.swipedRight)
    //    {
    //         NewPos = Position + Vector2Int.right;

    //    }
    //    else if (swipeInput.swipedLeft)
    //    {
    //        NewPos = Position + Vector2Int.left;

    //    }
    //    else if (swipeInput.swipedUp)
    //    {
    //        NewPos = Position + Vector2Int.up;

    //    }
    //    else if (swipeInput.swipedDown)
    //    {
    //        NewPos = Position + Vector2Int.down;

    //    }
    //    else
    //    {
    //        NewPos = Position;
    //    }
    //    Debug.Log($"NewPos: {NewPos}");
    //    if (Vector2Int.Distance(NewPos, Position) == 1)
    //    {
    //        GridManager.Instance.SwapTiles(Position, NewPos);
    //    }
    //    else 
    //    {
    //        SoundManager.Instance.PlaySound(SoundType.TypeSelect);
    //    }
    //    swipeInput.ResetSwipe();

    //}
    public void SwipeTile(Vector2Int NewPos) {
        if (Vector2Int.Distance(NewPos, Position) == 1)
        {
            GridManager.Instance.SwapTiles(Position, NewPos);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundType.TypeSelect);
        }
    }
}
