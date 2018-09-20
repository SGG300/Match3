using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform tileContainers;
    public Transform cellContainers;
    public GameObject tilePrefab;
    public GameObject cellPrefab;
    public GameObject pointTextPrefab;

    public List<Tile> tilePool = new List<Tile>();
    public Transform pointTextContainer;
    public List<GameObject> pointTextPool = new List<GameObject>();

    public Transform gridCellsContainer;
    public List<List<Cell>> grid = new List<List<Cell>>();

    public static int numRows = 8;
    public static int numCols = 8;
    public int rangeTileType = 0;

    public Cell selectedCellA;
    public Cell selectedCellB;

    private int points = 0;
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            pointsText.text = "Points: \n" + points;
        }

    }
    public Text pointsText;

    private int moves = 0;
    public int Moves
    {
        get
        {
            return moves;
        }
        set
        {
            moves = value;
            movesText.text = "Moves: \n" + moves;
        }
    }
    public Text movesText;

    public Animation popUpRandomizeText;

    private bool inputAllowed = true;


    private Vector2 lastMousePosition;


    public void Start()
    {
        instance = this;
        rangeTileType = Enum.GetValues(typeof(TileTypeEnum)).Length;
        StartCoroutine(InitGameObjects());
        Points = 0;
        Moves = 0;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0 && selectedCellA != null && inputAllowed)
        {
            if (Input.GetTouch(0).deltaPosition.x > 1)
            {
                if (selectedCellA.colId < (numCols - 1))
                {
                    SelectCell(grid[selectedCellA.rowId][selectedCellA.colId + 1]);
                }
            }

            if (Input.GetTouch(0).deltaPosition.x < -1)
            {
                if (selectedCellA.colId > 0)
                {
                    SelectCell(grid[selectedCellA.rowId][selectedCellA.colId - 1]);
                }
            }

            if (Input.GetTouch(0).deltaPosition.y > 1)
            {
                if (selectedCellA.rowId > 0)
                {
                    SelectCell(grid[selectedCellA.rowId - 1][selectedCellA.colId]);
                }
            }

            if (Input.GetTouch(0).deltaPosition.y < -1)
            {
                if (selectedCellA.rowId < (numRows - 1))
                {
                    SelectCell(grid[selectedCellA.rowId + 1][selectedCellA.colId]);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && selectedCellA != null && inputAllowed)
        {
            if ((Input.mousePosition.x - lastMousePosition.x) > 10)
            {
                if (selectedCellA.colId < (numCols - 1))
                {
                    SelectCell(grid[selectedCellA.rowId][selectedCellA.colId + 1]);
                }
            }

            if ((Input.mousePosition.x - lastMousePosition.x) < -10)
            {
                if (selectedCellA.colId > 0)
                {
                    SelectCell(grid[selectedCellA.rowId][selectedCellA.colId - 1]);
                }
            }

            if ((Input.mousePosition.y - lastMousePosition.y) > 10)
            {
                if (selectedCellA.rowId > 0)
                {
                    SelectCell(grid[selectedCellA.rowId - 1][selectedCellA.colId]);
                }
            }

            if ((Input.mousePosition.y - lastMousePosition.y) < -10)
            {
                if (selectedCellA.rowId < (numRows - 1))
                {
                    SelectCell(grid[selectedCellA.rowId + 1][selectedCellA.colId]);
                }
            }
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public IEnumerator InitGameObjects()
    {
        List<Transform> gridCellPositions = new List<Transform>();
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                gridCellPositions.Add(Instantiate(cellPrefab, cellContainers).transform);
            }
        }
        yield return new WaitForEndOfFrame();

        int index = 0;
        for (int i = 0; i < numRows; i++)
        {
            grid.Add(new List<Cell>());
            for (int j = 0; j < numCols; j++)
            {
                Cell cell = gridCellPositions[index].gameObject.GetComponent<Cell>();
                cell.rowId = i;
                cell.colId = j;
                cell.tile = GetNewTile();
                cell.tile.free = false;
                cell.tile.TyleType = (TileTypeEnum)UnityEngine.Random.Range(0, rangeTileType);
                cell.InstanceTileOnSite();
                grid[i].Add(cell);
                index++;
            }
        }

        CheckSolvedRowAndColsAndChange();
        RandomizeUntilCanBeSolved(false);
    }

    public void RandomizeCells()
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                grid[i][j].tile.TyleType = (TileTypeEnum)UnityEngine.Random.Range(0, rangeTileType);
            }
        }
    }

    public void CheckSolvedRowAndColsAndChange()
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                while (grid[i][j].CheckIfSolved() == true)
                {
                    grid[i][j].tile.TyleType = (TileTypeEnum)UnityEngine.Random.Range(0, rangeTileType);
                }
            }
        }
    }


    public void SelectCell(Cell selectedCell)
    {
        if (inputAllowed == false)
            return;
        if (selectedCellA == null)
        {
            selectedCellA = selectedCell;
            selectedCellA.outline.enabled = true;
        }
        else
        {
            if (CheckAdjacency(selectedCellA, selectedCell))
            {
                inputAllowed = false;
                Tile auxTile = selectedCell.tile;
                selectedCell.tile = selectedCellA.tile;
                selectedCellA.tile = auxTile;
                selectedCellB = selectedCell;
                selectedCellB.outline.enabled = true;
                Invoke("CheckAfterMove", 1.0f);
            }
            else
            {
                selectedCellA.outline.enabled = false;
                selectedCellA = null;
            }
        }
    }

    public void CheckAfterMove()
    {
        if (selectedCellA.CheckIfSolved() == false && selectedCellB.CheckIfSolved() == false)
        {
            Tile auxTile = selectedCellB.tile;
            selectedCellB.tile = selectedCellA.tile;
            selectedCellA.tile = auxTile;
            inputAllowed = true;
        }
        else
        {
            Moves++;
            List<Cell> cellsToSolveA = selectedCellA.GetAllSolvedCells();
            List<Cell> cellsToSolveB = selectedCellB.GetAllSolvedCells();
            foreach (Cell cell in cellsToSolveA)
            {
                if (cell.tile != null)
                {
                    InstantiatePointText(cell.transform.position);
                    cell.tile.free = true;
                    cell.tile.gameObject.SetActive(false);
                    cell.tile = null;
                    Points++;
                }
            }
            foreach (Cell cell in cellsToSolveB)
            {
                if (cell.tile != null)
                {
                    InstantiatePointText(cell.transform.position);
                    cell.tile.free = true;
                    cell.tile.gameObject.SetActive(false);
                    cell.tile = null;
                    Points++;
                }

            }
            MakePiecesFall();
        }
        selectedCellA.outline.enabled = false;
        selectedCellB.outline.enabled = false;
        selectedCellA = null;
        selectedCellB = null;
    }

    public void CheckAfterFall()
    {
        List<Cell> cellsToSolve = new List<Cell>();
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                List<Cell> cellsToSolveToAdd = grid[i][j].GetAllSolvedCells();
                foreach (Cell cell in cellsToSolveToAdd)
                {
                    cellsToSolve.Add(cell);
                }
            }
        }
        foreach (Cell cell in cellsToSolve)
        {
            if (cell.tile != null)
            {
                InstantiatePointText(cell.transform.position);
                cell.tile.free = true;
                cell.tile.gameObject.SetActive(false);
                cell.tile = null;
                Points++;
            }

        }
        if (cellsToSolve.Count > 0)
        {
            MakePiecesFall();
        }
        else
        {
            RandomizeUntilCanBeSolved(true);
            inputAllowed = true;
        }
    }

    public void RandomizeUntilCanBeSolved(bool showText)
    {
        if(showText && !CheckIfThereIsMoreSolutions())
        {
            popUpRandomizeText.Play();
        }
        
        while (!CheckIfThereIsMoreSolutions())
        {
            RandomizeCells();
            CheckSolvedRowAndColsAndChange();
        }
    }

    public bool CheckIfThereIsMoreSolutions()
    {
        bool canBeSolved = false;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (grid[i][j].CheckIfCanBeSolved())
                {
                    canBeSolved = true;
                }
            }
        }
        return canBeSolved;
    }

    public void MakePiecesFall()
    {
        for (int i = 0; i < numCols; i++)
        {
            int piecesFalling = 0;
            for (int j = numRows - 1; j >= 0; j--)
            {
                if (grid[j][i].tile == null)
                {
                    bool filled = false;
                    for (int t = j - 1; t >= 0; t--)
                    {
                        if (grid[t][i].tile != null)
                        {
                            grid[j][i].tile = grid[t][i].tile;
                            grid[t][i].tile = null;
                            filled = true;
                            break;
                        }
                    }

                    if (!filled)
                    {
                        grid[j][i].tile = GetNewTile();
                        grid[j][i].tile.free = false;
                        grid[j][i].tile.TyleType = (TileTypeEnum)UnityEngine.Random.Range(0, rangeTileType);
                        grid[j][i].tile.transform.position = grid[0][i].transform.position + new Vector3(0.0f, grid[j][i].tile.GetComponent<SpriteRenderer>().bounds.size.y, 0.0f) + new Vector3(0.0f, grid[j][i].tile.GetComponent<SpriteRenderer>().bounds.size.y, 0.0f) * piecesFalling;
                        piecesFalling++;
                    }
                }
            }
        }
        Invoke("CheckAfterFall", 1.0f);
    }

    public bool CheckAdjacency(Cell cellA, Cell cellB)
    {
        if (Math.Abs(cellA.rowId - cellB.rowId) < 2 && Math.Abs(cellA.colId - cellB.colId) < 2 && (cellA.rowId == cellB.rowId || cellA.colId == cellB.colId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Tile GetNewTile()
    {
        foreach (Tile tile in tilePool)
        {
            if (tile.free == true)
            {
                tile.gameObject.SetActive(true);
                return tile;
            }

        }
        
        GameObject GameObjectTile = GameObject.Instantiate(tilePrefab, tileContainers);
        tilePool.Add(GameObjectTile.GetComponent<Tile>());

        return tilePool[tilePool.Count - 1];
    }

    public void InstantiatePointText(Vector3 position)
    {
        bool instantiated = false;
        foreach(GameObject go in pointTextPool)
        {
            if(go.activeSelf == false)
            {
                instantiated = true;
                go.transform.position = position;
                go.SetActive(true);
            }
        }

        if(!instantiated)
        {
            pointTextPool.Add(Instantiate(pointTextPrefab, position, Quaternion.identity, pointTextContainer));
        }
    }

}
