using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public int rowId;
    public int colId;
    public Tile tile;
    public Outline outline;

    public const float timeToSwap = 1.0f;
    public float progressSwapping = 0.0f;

    public Cell(Transform paramCellPos, int paramRowId, int paramColId, Tile paramTile)
    {
        rowId = paramRowId;
        colId = paramColId;
        tile = paramTile;
    }

    public void Update()
    {
        if (tile != null)
        {
            if (tile.gameObject.transform.position != gameObject.transform.position)
            {
                tile.gameObject.transform.position = Vector3.Lerp(tile.gameObject.transform.position, gameObject.transform.position, (Time.deltaTime / timeToSwap) + progressSwapping);
                progressSwapping += Time.deltaTime / timeToSwap;
            }
            else
            {
                progressSwapping = 0.0f;
            }
        }

    }

    public void InstanceTileOnSite()
    {
        tile.gameObject.transform.position = gameObject.transform.position;
    }

    public bool CheckIfSolved()
    {
        for (int i = -1; i < 2; i++)
        {
            int r0 = rowId + i - 1;
            int r1 = rowId + i;
            int r2 = rowId + i + 1;
            if (r0 < 0 || r1 < 0 || r2 < 0 || r0 >= GameManager.numRows || r1 >= GameManager.numRows || r2 >= GameManager.numRows)
                continue;
            if (GameManager.instance.grid[r0][colId].tile.TyleType == GameManager.instance.grid[r1][colId].tile.TyleType && GameManager.instance.grid[r1][colId].tile.TyleType == GameManager.instance.grid[r2][colId].tile.TyleType)
            {
                return true;
            }
        }

        for (int i = -1; i < 2; i++)
        {
            int c0 = colId + i - 1;
            int c1 = colId + i;
            int c2 = colId + i + 1;
            if (c0 < 0 || c1 < 0 || c2 < 0 || c0 >= GameManager.numCols || c1 >= GameManager.numCols || c2 >= GameManager.numCols)
                continue;
            if (GameManager.instance.grid[rowId][c0].tile.TyleType == GameManager.instance.grid[rowId][c1].tile.TyleType && GameManager.instance.grid[rowId][c1].tile.TyleType == GameManager.instance.grid[rowId][c2].tile.TyleType)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckIfCanBeSolved()
    {
        if (colId > 0)
        {
            for (int i = -1; i < 2; i++)
            {
                int r0 = rowId + i - 1;
                int r1 = rowId + i;
                int r2 = rowId + i + 1;
                int c0 = 0;
                int c1 = 0;
                int c2 = 0;
                switch (i)
                {
                    case -1:
                        c0 = -1;
                        break;
                    case 0:
                        c1 = -1;
                        break;
                    case 1:
                        c2 = -1;
                        break;
                }
                if (r0 < 0 || r1 < 0 || r2 < 0 || r0 >= GameManager.numRows || r1 >= GameManager.numRows || r2 >= GameManager.numRows)
                    continue;
                if (GameManager.instance.grid[r0][colId + c0].tile.TyleType == GameManager.instance.grid[r1][colId + c1].tile.TyleType && GameManager.instance.grid[r1][colId + c1].tile.TyleType == GameManager.instance.grid[r2][colId + c2].tile.TyleType)
                {
                    return true;
                }
            }
        }

        if (colId < GameManager.numCols - 1)
        {
            for (int i = -1; i < 2; i++)
            {
                int r0 = rowId + i - 1;
                int r1 = rowId + i;
                int r2 = rowId + i + 1;
                int c0 = 0;
                int c1 = 0;
                int c2 = 0;
                switch (i)
                {
                    case -1:
                        c0 = 1;
                        break;
                    case 0:
                        c1 = 1;
                        break;
                    case 1:
                        c2 = 1;
                        break;
                }
                if (r0 < 0 || r1 < 0 || r2 < 0 || r0 >= GameManager.numRows || r1 >= GameManager.numRows || r2 >= GameManager.numRows)
                    continue;
                if (GameManager.instance.grid[r0][colId + c0].tile.TyleType == GameManager.instance.grid[r1][colId + c1].tile.TyleType && GameManager.instance.grid[r1][colId + c1].tile.TyleType == GameManager.instance.grid[r2][colId + c2].tile.TyleType)
                {
                    return true;
                }
            }
        }

        if (rowId > 0)
        {
            for (int i = -1; i < 2; i++)
            {
                int c0 = colId + i - 1;
                int c1 = colId + i;
                int c2 = colId + i + 1;
                int r0 = 0;
                int r1 = 0;
                int r2 = 0;
                switch (i)
                {
                    case -1:
                        r0 = -1;
                        break;
                    case 0:
                        r1 = -1;
                        break;
                    case 1:
                        r2 = -1;
                        break;
                }
                if (c0 < 0 || c1 < 0 || c2 < 0 || c0 >= GameManager.numCols || c1 >= GameManager.numCols || c2 >= GameManager.numCols)
                    continue;
                if (GameManager.instance.grid[rowId + r0][c0].tile.TyleType == GameManager.instance.grid[rowId + r1][c1].tile.TyleType && GameManager.instance.grid[rowId + r1][c1].tile.TyleType == GameManager.instance.grid[rowId + r2][c2].tile.TyleType)
                {
                    return true;
                }
            }
        }

        if (rowId < GameManager.numRows - 1)
        {
            for (int i = -1; i < 2; i++)
            {
                int c0 = colId + i - 1;
                int c1 = colId + i;
                int c2 = colId + i + 1;
                int r0 = 0;
                int r1 = 0;
                int r2 = 0;
                switch (i)
                {
                    case -1:
                        r0 = 1;
                        break;
                    case 0:
                        r1 = 1;
                        break;
                    case 1:
                        r2 = 1;
                        break;
                }
                if (c0 < 0 || c1 < 0 || c2 < 0 || c0 >= GameManager.numCols || c1 >= GameManager.numCols || c2 >= GameManager.numCols)
                    continue;
                if (GameManager.instance.grid[rowId + r0][c0].tile.TyleType == GameManager.instance.grid[rowId + r1][c1].tile.TyleType && GameManager.instance.grid[rowId + r1][c1].tile.TyleType == GameManager.instance.grid[rowId + r2][c2].tile.TyleType)
                {
                    return true;
                }
            }
        }



        return false;
    }

    public List<Cell> GetAllSolvedCells()
    {
        List<Cell> solvedCells = new List<Cell>();

        for (int i = -1; i < 2; i++)
        {
            int c0 = colId + i - 1;
            int c1 = colId + i;
            int c2 = colId + i + 1;
            if (c0 < 0 || c1 < 0 || c2 < 0 || c0 >= GameManager.numCols || c1 >= GameManager.numCols || c2 >= GameManager.numCols)
                continue;
            if (GameManager.instance.grid[rowId][c0].tile.TyleType == GameManager.instance.grid[rowId][c1].tile.TyleType && GameManager.instance.grid[rowId][c1].tile.TyleType == GameManager.instance.grid[rowId][c2].tile.TyleType)
            {
                solvedCells.Add(GameManager.instance.grid[rowId][c0]);
                solvedCells.Add(GameManager.instance.grid[rowId][c1]);
                solvedCells.Add(GameManager.instance.grid[rowId][c2]);
            }
        }

        for (int i = -1; i < 2; i++)
        {
            int r0 = rowId + i - 1;
            int r1 = rowId + i;
            int r2 = rowId + i + 1;
            if (r0 < 0 || r1 < 0 || r2 < 0 || r0 >= GameManager.numRows || r1 >= GameManager.numRows || r2 >= GameManager.numRows)
                continue;
            if (GameManager.instance.grid[r0][colId].tile.TyleType == GameManager.instance.grid[r1][colId].tile.TyleType && GameManager.instance.grid[r1][colId].tile.TyleType == GameManager.instance.grid[r2][colId].tile.TyleType)
            {
                solvedCells.Add(GameManager.instance.grid[r0][colId]);
                solvedCells.Add(GameManager.instance.grid[r1][colId]);
                solvedCells.Add(GameManager.instance.grid[r2][colId]);
            }
        }



        return solvedCells;
    }

    public void SelectCell()
    {
        GameManager.instance.SelectCell(this);
    }

}
