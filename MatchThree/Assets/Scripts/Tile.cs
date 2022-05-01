using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUP
{
    None,
    Match4,
    Match5,
    LShape
}

public class Tile : MonoBehaviour
{
    public PowerUP powerUP; 
    public int ID { get; set; }
    public SpriteRenderer SpriteRenderer { 
        get 
        {
            return sprite;
        }
        set
        {
            sprite = value;
        }
    }
    private SpriteRenderer sprite;
    private SpriteRenderer tileRenderer;
    public Vector2Int Position;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
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
    
    public void SwipeTile(Vector2Int NewPos) {
        if (Vector2Int.Distance(NewPos, Position) == 1)
        {
            GridManager.Instance.SwapTiles(Position, NewPos);
        }
        else
        {
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypeSelect);
        }
    }
}
