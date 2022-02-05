using rudscreation.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public GameObject GridPrefab;
    public Transform GridParent;
    //public int GridDimension = 8;
    public int Row = 8;
    public int Coloum = 8;
    public float Distance = 1.0f;
    private GameObject[,] Grid;

    public GameObject GameOverMenu;
    public TextMeshProUGUI MovesText;
    public TextMeshProUGUI ScoreText;

    public int StartingMoves = 50;
    private int _numMoves;
    private int BaseMultiplayer =10;
    private int DynamicMultiplayer =1;
    bool setupCall = true;
    public int NumMoves
    {
        get
        {
            return _numMoves;
        }

        set
        {
            _numMoves = value;
            MovesText.text = _numMoves.ToString();
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            ScoreText.text = _score.ToString();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Score = 0;
        NumMoves = StartingMoves;
        GameOverMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[Coloum, Row];
        InitGrid();
        setupCall = true;
    }
    /// <summary>
    /// Init grid
    /// </summary>

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(Coloum * Distance / 2.0f, Row * Distance / 2.0f, 0) + Vector3.right*Distance/2;
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Coloum; column++)
            {
                GameObject newTile = Instantiate(TilePrefab);
                GameObject gridBG = Instantiate(GridPrefab);
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);
                gridBG.transform.parent = GridParent;
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                gridBG.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                Grid[column, row] = newTile;
            }
        }
        StartCoroutine(FillHoles());
    }
    /// <summary>
    /// this function will swap the the tiles 
    /// </summary>
    /// <param name="tile1Position"></param>
    /// <param name="tile2Position"></param>
    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {


        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();
   
        Hashtable paramHashtable = new Hashtable();
        paramHashtable.Add("value1", renderer1);
        paramHashtable.Add("value2", renderer2);
        paramHashtable.Add("value3", tile1.transform);
        paramHashtable.Add("value4", tile2.transform);
        iTween.MoveTo(tile1, iTween.Hash(
            "position", tile2.transform.position,
            "islocal", true,
            "easetype", iTween.EaseType.linear,
            "time", 0.2f
            )); ;
        iTween.MoveTo(tile2, iTween.Hash(
            "position", tile1.transform.position,
            "islocal", true,
            "easetype", iTween.EaseType.linear,
            "time", 0.2f,
            "oncompletetarget", gameObject,
            "oncomplete", "SwapPostAnimation",
            "oncompleteparams", paramHashtable
            ));
        //SwapPostAnimation(renderer1, renderer2);
    }

    private void SwapPostAnimation(Hashtable paramHashtable)
    {
        var renderer1 = (SpriteRenderer)paramHashtable["value1"];
        var renderer2 = (SpriteRenderer)paramHashtable["value2"];
        var tile1 = (Transform)paramHashtable["value3"];
        var tile2 = (Transform)paramHashtable["value4"];
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
        DynamicMultiplayer = 1;
        bool changesOccurs = CheckMatches();
        if (!changesOccurs)
        {
            iTween.MoveTo(tile1.gameObject, iTween.Hash(
            "position", tile2.transform.position,
            "islocal", true,
            "easetype", iTween.EaseType.linear,
            "time", 0.1f
            ));
            iTween.MoveTo(tile2.gameObject, iTween.Hash(
            "position", tile1.transform.position,
            "islocal", true,
            "easetype", iTween.EaseType.linear,
            "time", 0.1f));
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
            SoundManager.Instance.PlaySound(SoundType.TypeMove);
        }
        else
        {
            NumMoves--;
            var tempPos = tile1.transform.position;
            tile1.position = tile2.position;
            tile2.position = tempPos;
            StartCoroutine(FillHoles());
            //do
            //{
            //FillHoles();
            Debug.Log("Fill the holes");
            //} while (CheckMatches());
        }
        
    }

    /// <summary>
    /// this will retrn sprit rander 
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= Coloum
             || row < 0 || row >= Row) 
        {
            Debug.LogError($"issue occred column :{column} ,row: {row} ");
            return null;
        }
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    bool CheckMatches()
    {
        HashSet<SpriteRenderer> matchedTiles = new System.Collections.Generic.HashSet<SpriteRenderer>();
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Coloum; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current); 
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles)
        {
            if (!setupCall)
            {
                renderer.transform.GetChild(0).gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySound(SoundType.TypePop);
            renderer.sprite = null;
        }
        if (!setupCall)
        {

            Score += (matchedTiles.Count * (BaseMultiplayer * DynamicMultiplayer));
            Debug.Log($"Base Mul : {BaseMultiplayer} Dynamic Mul: {DynamicMultiplayer}");
        }
        return matchedTiles.Count > 0;
    }
    void GameOver()
    {
        PlayerPrefs.SetInt("score", Score);
        GameOverMenu.SetActive(true);
        SoundManager.Instance.PlaySound(SoundType.TypeGameOver);
    }
    private List<SpriteRenderer> FindRowMatchForTile(int column, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < Row; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(column, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    private List<SpriteRenderer> FindColumnMatchForTile(int column, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = column + 1; i < Coloum; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    IEnumerator FillHoles()
    {
        if (setupCall) 
        { 
            yield return new WaitForSeconds(0f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        for (int column = 0; column < Coloum; column++)
        {
            yield return new WaitForSeconds(0.01f);

            for (int row = 0; row < Row; row++)
            {
                yield return new WaitForSeconds(0.01f);

                while (GetSpriteRendererAt(column, row).sprite == null)
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, row);
                    SpriteRenderer next = current;
                    for (int filler = row; filler < Row - 1; filler++)
                    {
                        next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                        current = next;
                    }
                    next.sprite = Sprites[Random.Range(0, Sprites.Count)];
                }
            }
        }
        if (CheckMatches())
        {
            DynamicMultiplayer++;
            StopCoroutine(FillHoles());
            StartCoroutine(FillHoles());
        }
        else if (setupCall) { 
        setupCall = false;
        }
        else if (NumMoves <= 0)
        {
            NumMoves = 0;
            GameOver();
        }
    }
}
