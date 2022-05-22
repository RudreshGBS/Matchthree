using rudscreation.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : Singleton<GridManager>
{
    public List<LevelModel> levels = new List<LevelModel>(); 
    public GameObject TilePrefab;
    public GameObject GridPrefab;
    public Transform GridParent;
    public SpriteRenderer BG;
    public SpriteRenderer EGTItem;
    public Image AcivementImage;
    //public int GridDimension = 8;
   
    public float Distance = 1.0f;
    private GameObject[,] Grid;

    public GameOverManager GameOverMenu;
    public TextMeshProUGUI MovesText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TargetScoreText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI LevelText;

    public Sprite Match4;
    public Sprite Match5;
    public Sprite LShape;

    private int _numMoves;
    private int BaseMultiplayer =10;
    private int DynamicMultiplayer =1;
    private int PowerUpMultiplayer =5;

    private List<Sprite> Sprites = new List<Sprite>();
    private Sprite blockFrame;
    private int Row = 8;
    private int Coloum = 8;
    private int StartingMoves;
    private int TargetScore;
    private TimeSpan Time;
    private TimerHandler timerHandler;
    public PowerUP CurrentpowerUP;
    bool setupCall = true;
    public Action<int> OnLevelResult; 
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
    private Dictionary<Sprite, int> spriteToIDDictionary = new Dictionary<Sprite, int>();
    protected override void Awake()
    {
        base.Awake();
        GameOverMenu.gameObject.SetActive(false);
        timerHandler = TimerHandler.Instance;
    }
    private Sprite GetPoweUpSprite(PowerUP powerUP) 
    {
        switch (powerUP)
        {
            case PowerUP.None:
                return null;
            case PowerUP.Match4:
                return Match4;
            case PowerUP.Match5:
                return Match5;
            case PowerUP.LShape:
                return LShape;
            default:
                return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelModel level = levels.FirstOrDefault(x => x.levelNo.Equals($"{GameDataStore.CurrentLevel}"));
        if (level != null)
        {
            Sprites.Clear();
            Sprites = level.symbols.ToList();
            blockFrame = level.blockFrame;
            Row = level.rows;
            Coloum = level.cols;
            StartingMoves = level.maxMoves;
            BG.sprite = level.background;
            EGTItem.sprite = level.EGTItem;
            AcivementImage.sprite = level.achievementImageForlevel;
            TargetScore = level.tagetScore;
            LevelText.text = $"Level{level.levelNo}";
            Time = new TimeSpan(0, 0, level.time);   
            if (spriteToIDDictionary != null)
            {
                spriteToIDDictionary.Clear();
            }
            for (int i = 0; i < level.symbols.Count; i++)
            {
                if (!spriteToIDDictionary.ContainsKey(level.symbols[i]))
                {
                    try
                    {
                        spriteToIDDictionary.Add(level.symbols[i], i);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }
        StartLevel();

    }
    private void Update()
    {
        if (timerHandler.totaltimeinTimespan != null)
        {
            TimerText.text = timerHandler.totaltimeinTimespan.ToString(@"mm\:ss");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameOver(true);
        }

    }

    private void StartLevel()
    {
        GridPrefab.GetComponent<SpriteRenderer>().sprite = blockFrame;
        NumMoves = StartingMoves;
        Score = 0;
        Grid = new GameObject[Coloum, Row];
        InitGrid();
        setupCall = true;
        TargetScoreText.text= TargetScore.ToString();
        TimerText.text= Time.TotalSeconds.ToString(@"hh\:mm\:ss");
        SoundManager.Instance.PlayMusic(SoundType.GamePlay);
    }

    /// <summary>
    /// Init grid
    /// </summary>

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(Coloum * Distance / 2.0f, Row * Distance / 2.0f, 0) + Vector3.right * Distance / 2;
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Coloum; column++)
            {
                GameObject newTile = Instantiate(TilePrefab);
                GameObject gridBG = Instantiate(GridPrefab);
                Tile tile = newTile.AddComponent<Tile>();
                tile.SpriteRenderer.sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Count)];
                tile.Position = new Vector2Int(column, row);
                tile.ID = spriteToIDDictionary[tile.SpriteRenderer.sprite];
                tile.powerUP = PowerUP.None;
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
        var TileScript1 = tile1.GetComponent<Tile>();
        var TileScript2 = tile2.GetComponent<Tile>();
        List<SpriteRenderer> BombMatches = new List<SpriteRenderer>();
        if (!setupCall)
        {
           
            if (TileScript1.powerUP == PowerUP.LShape || TileScript2.powerUP == PowerUP.LShape)
            {
                var MatchUpTile = (TileScript1.powerUP == PowerUP.LShape) ? TileScript1 : TileScript2;
                var powerUpTile = (TileScript1.powerUP == PowerUP.LShape) ? TileScript2 : TileScript1;
                Bomb(powerUpTile);
                NumMoves--;
                var tempPos = tile1.transform.position;
                tile1.position = tile2.position;
                tile2.position = tempPos;
                tile1.GetComponent<Tile>().powerUP = PowerUP.None;
                tile2.GetComponent<Tile>().powerUP = PowerUP.None;
                StartCoroutine(FillHoles());
            }
            if (TileScript1.powerUP == PowerUP.Match5 || TileScript2.powerUP == PowerUP.Match5)
            {
                var MatchUpTile = (TileScript1.powerUP == PowerUP.Match5) ? TileScript1 : TileScript2;
                var powerUpTile = (TileScript1.powerUP == PowerUP.Match5) ? TileScript2 : TileScript1;
                MatchFive(powerUpTile, MatchUpTile);
                NumMoves--;
                var tempPos = tile1.transform.position;
                tile1.position = tile2.position;
                tile2.position = tempPos;
                tile1.GetComponent<Tile>().powerUP = PowerUP.None;
                tile2.GetComponent<Tile>().powerUP = PowerUP.None;
                StartCoroutine(FillHoles());
                return;
            }
            if (TileScript1.powerUP == PowerUP.Match4 || TileScript2.powerUP == PowerUP.Match4)
            {
                var MatchUpTile = (TileScript1.powerUP == PowerUP.Match4) ? TileScript1 : TileScript2;
                var powerUpTile = (TileScript1.powerUP == PowerUP.Match4) ? TileScript2 : TileScript1;
                MatchFour(powerUpTile);
                NumMoves--;
                var tempPos = tile1.transform.position;
                tile1.position = tile2.position;
                tile2.position = tempPos;
                tile1.GetComponent<Tile>().powerUP = PowerUP.None;
                tile2.GetComponent<Tile>().powerUP = PowerUP.None;
                StartCoroutine(FillHoles());
                return;
            }

            
            List<SpriteRenderer> Tile1Matches = MatchCheckForPowerUps(TileScript1);
            List<SpriteRenderer> Tile2Matches = MatchCheckForPowerUps(TileScript2);
            if (Tile1Matches?.Count >= 3)
            {
                BombMatches = Tile1Matches;
                Tile tile = renderer2.GetComponent<Tile>();
                tile.powerUP = CurrentpowerUP;
                tile.SpriteRenderer.sprite = GetPoweUpSprite(tile.powerUP);
            }
            else if (Tile2Matches?.Count >= 3)
            {
                BombMatches = Tile2Matches;
                Tile tile = renderer1.GetComponent<Tile>();
                tile.powerUP = CurrentpowerUP;
                tile.SpriteRenderer.sprite = GetPoweUpSprite(tile.powerUP);
            }

        }

        bool changesOccurs = CheckMatches(BombMatches);
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
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypeMove);
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

    private List<SpriteRenderer> MatchCheckForPowerUps(Tile tile) 
    {

        List<SpriteRenderer> matchForBomb = FindRowAndColumMatchForTile(tile.Position.x, tile.Position.y, tile.SpriteRenderer.sprite);
        
         if (matchForBomb.Count >= 4)
         {
            CurrentpowerUP = PowerUP.LShape;
            return matchForBomb;
         }
        else 
        {
            List<SpriteRenderer> horizontalMatches = FindAllColumMatchForTile(tile.Position.x, tile.Position.y, tile.SpriteRenderer.sprite);
            if (horizontalMatches.Count == 4)
            {
                CurrentpowerUP = PowerUP.Match4;
                return horizontalMatches;
            }
            else if (horizontalMatches.Count > 4) 
            {
                CurrentpowerUP = PowerUP.Match5;
                return horizontalMatches;
            }
            List<SpriteRenderer> verticalMatches = FindALLRowMatchForTile(tile.Position.x, tile.Position.y, tile.SpriteRenderer.sprite);
            if (verticalMatches.Count == 4)
            {
                CurrentpowerUP = PowerUP.Match4;
                return verticalMatches;
            }
            else if (verticalMatches.Count > 4)
            {
                CurrentpowerUP = PowerUP.Match5;
                return verticalMatches;
            }
        }
        CurrentpowerUP = PowerUP.None;
        return null;
    }

    private void Bomb(Tile matchUpTile)
    {
        HashSet<SpriteRenderer> matchedTiles = new System.Collections.Generic.HashSet<SpriteRenderer>();
        List<SpriteRenderer> NineMatches = FindNineMatchForTile(matchUpTile.Position.x, matchUpTile.Position.y);
        matchedTiles.UnionWith(NineMatches);
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            if (!setupCall)
            {
                renderer.transform.GetChild(0).gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypePop);
            renderer.sprite = null;
        }
        if (!setupCall)
        {
            Score += (matchedTiles.Count * (BaseMultiplayer * PowerUpMultiplayer));
            Debug.Log($"Base Mul : {BaseMultiplayer} powerup Mul: {PowerUpMultiplayer}");
        }
    }

   

    private void MatchFive(Tile powerUpTile,Tile matchUpTile)
    {
        HashSet<SpriteRenderer> matchedTiles = new System.Collections.Generic.HashSet<SpriteRenderer>();
        List<SpriteRenderer> gridMatches = FindALLMatchForTile(matchUpTile.SpriteRenderer.sprite,powerUpTile.SpriteRenderer);
        matchedTiles.UnionWith(gridMatches);
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            if (!setupCall)
            {
                renderer.transform.GetChild(0).gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypePop);
            renderer.sprite = null;
        }
        if (!setupCall)
        {
            Score += (matchedTiles.Count * (BaseMultiplayer * PowerUpMultiplayer));
            Debug.Log($"Base Mul : {BaseMultiplayer} powerup Mul: {PowerUpMultiplayer}");
        }
    }

    private void MatchFour(Tile tile)
    {
        HashSet<SpriteRenderer> matchedTiles = new System.Collections.Generic.HashSet<SpriteRenderer>();
        List<SpriteRenderer> verticalMatches = FindALLColumnMatchForTile(tile.Position.x, tile.Position.y);
        matchedTiles.UnionWith(verticalMatches);
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            if (!setupCall)
            {
                renderer.transform.GetChild(0).gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypePop);
            renderer.sprite = null;
        }
        if (!setupCall)
        {
            Score += (matchedTiles.Count * (BaseMultiplayer * PowerUpMultiplayer));
            Debug.Log($"Base Mul : {BaseMultiplayer} powerup Mul: {PowerUpMultiplayer}");
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
    /// <summary>
    /// This will return the Tile Component
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    Tile GetTileAt(int column, int row)
    {
        if (column < 0 || column >= Coloum
             || row < 0 || row >= Row)
        {
            Debug.LogError($"issue occred column :{column} ,row: {row} ");
            return null;
        }
        GameObject tileGO = Grid[column, row];
        Tile tile = tileGO.GetComponent<Tile>();
        return tile;
    }

    bool CheckMatches(List<SpriteRenderer> t = null)
    {
        HashSet<SpriteRenderer> matchedTiles = new System.Collections.Generic.HashSet<SpriteRenderer>();
        for (int row = 0; row < Row; row++)
        {
            for (int column = 0; column < Coloum; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);
                List<SpriteRenderer> Matches = new List<SpriteRenderer>();
                if (t != null)
                {
                    Matches.AddRange(t);
                    t.Clear();
                }
                else 
                { 
                    Matches = FindRowAndColumMatchForTile(column, row, current.sprite);
                }


                if (Matches.Count >= 4)
                {
                    matchedTiles.UnionWith(Matches);
                  
                }
                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);

                if (horizontalMatches.Count >= 2)
                {

                    matchedTiles.UnionWith(horizontalMatches);
                    if (!setupCall)
                    {


                        if (horizontalMatches.Count == 3)
                        {
                            Tile tile = current.GetComponent<Tile>();
                            tile.powerUP = PowerUP.Match4;
                            tile.SpriteRenderer.sprite = Match4;
                        }
                        else if (horizontalMatches.Count == 4)
                        {
                            Tile tile = current.GetComponent<Tile>();
                            tile.powerUP = PowerUP.Match5;
                            tile.SpriteRenderer.sprite = Match5;
                        }
                        else
                        {
                            matchedTiles.Add(current);
                        }
                    }
                    else
                    {
                        matchedTiles.Add(current);
                    }
                }
                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);

                if (verticalMatches.Count >= 2 )
                {
                    matchedTiles.UnionWith(verticalMatches);
                    if (!setupCall)
                    {

                        if (verticalMatches.Count == 3)
                        {
                            Tile tile = current.GetComponent<Tile>();
                            tile.powerUP = PowerUP.Match4;
                            tile.SpriteRenderer.sprite = Match4;
                        }
                        else if (verticalMatches.Count == 4)
                        {
                            Tile tile = current.GetComponent<Tile>();
                            tile.powerUP = PowerUP.Match5;
                            tile.SpriteRenderer.sprite = Match5;
                        }
                        else
                        {
                            matchedTiles.Add(current);
                        }
                    }
                    else
                    {
                        matchedTiles.Add(current);
                    }
                }
               
            }
        }
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            if (!setupCall)
            {
                renderer.transform.GetChild(0).gameObject.SetActive(true);
            }
            SoundManager.Instance.PlaySoundOneShot(SoundType.TypePop);
            renderer.sprite = null;
        }
        if (!setupCall)
        {

            Score += (matchedTiles.Count * (BaseMultiplayer * DynamicMultiplayer));
            Debug.Log($"Base Mul : {BaseMultiplayer} Dynamic Mul: {DynamicMultiplayer}");
        }
        return matchedTiles.Count > 0;
    }
    void GameOver(bool isPassed)
    {
       // PlayerPrefs.SetInt("score", Score);
        GameOverMenu.gameObject.SetActive(true);
        GameOverMenu.OnResult(isPassed,Score);
        if (isPassed) 
        {
            if (GameDataStore.CurrentLevel == GameDataStore.LastUnloackedLevel)
            {
                GameDataStore.LastUnloackedLevel++;
                GameDataStore.CanMoveRocket = true;
            }
            else 
            {
                GameDataStore.CanMoveRocket = false;

            }
        }
        timerHandler.UpdateZone -= TimerHandler_UpdateZone;
        timerHandler.StopTimer();
        SoundManager.Instance.PlaySoundOneShot(SoundType.TypeGameOver);

    }
    private List<SpriteRenderer> FindRowAndColumMatchForTile(int column, int row, Sprite sprite)
    {
        HashSet<SpriteRenderer> result = new HashSet<SpriteRenderer>();
        List<SpriteRenderer> RowList = new List<SpriteRenderer>();
        List<SpriteRenderer> ColList = new List<SpriteRenderer>();
        for (int k = -1; k < 2; k++)
        {
            if (k != 0)
            {
                for (int i = row + k; i < Row; i++)
                {
                    SpriteRenderer nextRow = GetSpriteRendererAt(column, i);
                    if (nextRow == null || nextRow.sprite != sprite)
                    {
                        break;
                    }
                    ColList.Add(nextRow);
                }
                for (int j = column + k; j < Coloum; j++)
                {
                    SpriteRenderer nextColumn = GetSpriteRendererAt(j, row);
                    if (nextColumn == null || nextColumn.sprite != sprite)
                    {
                        break;
                    }
                    RowList.Add(nextColumn);
                }
            }
        }
        if (RowList.Count >= 2 && ColList.Count >= 2) 
        {
            result.UnionWith(ColList);
            result.UnionWith(RowList);
        }
        return result.ToList();
    }
    private List<SpriteRenderer> FindAllColumMatchForTile(int column, int row, Sprite sprite)
    {
        HashSet<SpriteRenderer> result = new HashSet<SpriteRenderer>();
        HashSet<SpriteRenderer> result1 = new HashSet<SpriteRenderer>();
        
            
        for (int i = column ; i < Coloum; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn == null || nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }

        for (int j = column; j >= 0; j--)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(j, row);
            if (nextColumn == null || nextColumn.sprite != sprite)
            {
                break;
            }
            result1.Add(nextColumn);
        }
        result.UnionWith(result1);
        return result.ToList();
    }
    private List<SpriteRenderer> FindALLRowMatchForTile(int column, int row, Sprite sprite)
    {
        HashSet<SpriteRenderer> result = new HashSet<SpriteRenderer>();
        HashSet<SpriteRenderer> result1 = new HashSet<SpriteRenderer>();


        for (int i = row ; i < Row; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(column, i);
            if (nextRow == null || nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        for (int j = row; j >= 0; j--)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(column, j);
            if (nextRow == null || nextRow.sprite != sprite)
            {
                break;
            }
            result1.Add(nextRow);
        }
        result.UnionWith(result1);
        return result.ToList();
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
    private List<SpriteRenderer> FindNineMatchForTile(int column, int row)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                SpriteRenderer nextElement = GetSpriteRendererAt(column+i, row+j);
                if (nextElement != null && nextElement.sprite != null) {
                    result.Add(nextElement);
                }
            }
        }
        return result;

    }
    private List<SpriteRenderer> FindALLMatchForTile(Sprite matchSprite, SpriteRenderer spriteRenderer)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        result.Add(spriteRenderer);
        for (int column = 0; column < Coloum; column++)
        {
            for (int row = 0; row < Row; row++)
            {
                SpriteRenderer nextElement = GetSpriteRendererAt(column, row);
                if (nextElement.sprite == matchSprite)
                {
                    result.Add(nextElement);
                }
            }
        }
        return result;
    }
    private List<SpriteRenderer> FindALLColumnMatchForTile(int column, int row)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row; i < Row; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(column, i);
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

    IEnumerator FillHolesNew()
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
                    Tile current = GetTileAt(column, row);
                    current.SpriteRenderer.sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Count)];
                    current.ID = spriteToIDDictionary[current.SpriteRenderer.sprite];
                }
            }
        }
        if (CheckMatches())
        {
            DynamicMultiplayer++;
            //StopCoroutine(FillHolesNew());
            //StartCoroutine(FillHolesNew());
        }
        else if (setupCall)
        {
            setupCall = false;
            timerHandler.StartTimer(Time);
            timerHandler.UpdateZone += TimerHandler_UpdateZone;
            timerHandler.TimerReachedToEnd += TimerHandler_TimerReachedEnd;
        }
        else if (NumMoves <= 0)
        {
            NumMoves = 0;

            GameOver(Score >= TargetScore);

        }
        else if (Score >= TargetScore)
        {
            GameOver(true);
        }
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
                    yield return new WaitForSeconds(0.01f);

                    SpriteRenderer current = GetSpriteRendererAt(column, row); 
                    SpriteRenderer next = current;
                   
                    for (int filler = row; filler < Row - 1; filler++)
                    {
                        next = GetSpriteRendererAt(column, filler + 1);
                        if (next.GetComponent<Tile>().powerUP != PowerUP.None)
                        {
                            current.GetComponent<Tile>().powerUP = next.GetComponent<Tile>().powerUP;
                            next.GetComponent<Tile>().powerUP = PowerUP.None;
                        }
                        current.sprite = next.sprite;
                        current = next;
                    }
                    next.sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Count)];
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
            timerHandler.StartTimer(Time);
            timerHandler.UpdateZone += TimerHandler_UpdateZone;
            timerHandler.TimerReachedToEnd += TimerHandler_TimerReachedEnd;
        }
        else if (NumMoves <= 0)
        {
            NumMoves = 0;
            
            GameOver(Score >= TargetScore);

        } else if (Score >= TargetScore) 
        {
            GameOver(true);
        }
    }

    private void TimerHandler_TimerReachedEnd()
    {
        timerHandler.TimerReachedToEnd -= TimerHandler_TimerReachedEnd;
        GameOver(Score >= TargetScore);
    }

    private void TimerHandler_UpdateZone(TimerColorZone zone)
    {
        switch (zone)
        {
            case TimerColorZone.Green:
                TimerText.color = Color.white;
                break;
            case TimerColorZone.Yellow:
                TimerText.color = Color.yellow;
                break;
            case TimerColorZone.Red:
                TimerText.color = Color.red;
                break;
            case TimerColorZone.Grey:
                TimerText.color = Color.grey;
                break;
            default:
                break;
        }
    }
}
