using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    private float cellSize = 0;
    private float zPos = 7;
    private float xPos = 0;
    [SerializeField] private FloatVariable cellOffset; // must be a floatVariable shared with the player movement
    private Dictionary<Vector3, CellType> cellProperties = new Dictionary<Vector3, CellType>();

    private List<Cell> activeCellList = new List<Cell>();

    // start Position and player spawn
    private Vector3 spawnCell;
    [SerializeField] private LocalEvent spawnPlayer;
    [SerializeField] private LocalEvent spawnTwinCells;
    [SerializeField] private LocalEvent setCameraPosition;
    [SerializeField] private BoundsEvent onLevelSetBounds;

    //soundEffects
    private AudioSource aSource;

    //teleportation
    private Cell[] twinCells = new Cell[2];

    //cell animation
    [SerializeField] private int cellInitialPos;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
        aSource.pitch = 0.5f;
    }

    public void BuildLevel(LevelLayout levelData)
    {
        activeCellList.Clear();
        zPos = 7;
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < levelData.level.rows.Length; i++)
        {
            ArrayLayout.RowData bigRow = levelData.level.rows[i];
            for (int j = 0; j < bigRow.row.Length; j++)
            {
                Vector3 newPos = new Vector3(xPos, cellInitialPos, zPos); // minus 1 because cell goes upwards when spawned
                xPos += cellOffset.Value;
                if (bigRow.row[j] == CellType.N) continue;
                cellProperties.Add(newPos, bigRow.row[j]);
            }
            zPos -= cellOffset.Value;
            xPos = 0;
        }
        StartCoroutine(SpawnCellsAndPlayer(levelData.spawnCellProperty));
        onLevelSetBounds.Raise(LevelBounds());
        setCameraPosition.Raise(GetLevelCenter());
    }

    private IEnumerator SpawnCellsAndPlayer(CellProperty spawnProperty)
    {
        foreach (KeyValuePair<Vector3, CellType> cellProp in cellProperties)
        {
            Cell cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();
            activeCellList.Add(cell);
            if (cellSize == 0) cellSize = cell.GetComponent<Renderer>().bounds.size.y;
            switch (cellProp.Value)
            {
                case CellType.W:
                    cell.SetProperty(CellProperty.Free);
                    break;
                case CellType.B:
                    cell.SetProperty(CellProperty.Slide);
                    break;
                case CellType.P:
                    spawnCell = cellProp.Key;
                    cell.SetProperty(spawnProperty);
                    break;
                case CellType.G:
                    cell.SetProperty(CellProperty.Goal);
                    break;
                case CellType.S:
                    cell.SetProperty(CellProperty.Static);
                    break;
                case CellType.Bo:
                    cell.SetProperty(CellProperty.Bounce);
                    break;
                case CellType.T:
                    cell.SetProperty(CellProperty.Teleport);
                    if (twinCells[0] == null)
                    {
                        twinCells[0] = cell;
                    } else
                    {
                        twinCells[1] = cell;
                        cell.SetTwinCells(twinCells[0]);
                    }
                    break;
                default:
                    break;
            }
            AnimateCell(cell);
            cell.transform.position = cellProp.Key;
            yield return new WaitForSeconds(0.05f);
        }
        if (twinCells[0] != null)
        {
            for (int i = 0; i < 2; i++)
            {
                spawnTwinCells.Raise(new Vector3(twinCells[i].transform.position.x, 0, twinCells[i].transform.position.z));
                twinCells[i] = null;
            }
        }
        cellProperties.Clear();
        aSource.pitch = 0.5f;
        spawnPlayer.Raise(TopOfCellPosition(spawnCell));
    }

    private void AnimateCell(Cell cell)
    {
        cell.transform.localScale = Vector3.one / 10;
        cell.transform.DOMoveY(0, 0.25f).SetEase(Ease.OutBounce);
        cell.transform.DOScale(new Vector3(2, 1, 2), 0.5f).SetEase(Ease.OutBounce);
        aSource.Play();
        aSource.pitch += 0.03f;
    }
    
    private IEnumerator MakeCellDance()
    {
        foreach (var cell in activeCellList)
        {
            if (cell.property == CellProperty.Goal || cell == null) continue;
            LeanTween.moveY(cell.gameObject, cell.transform.position.y + 2, 1f).setEasePunch().setLoopCount(-1);
            yield return new WaitForSeconds(0.05f);
        }
        activeCellList.Clear();
    }

    public void ReachedGoalAnimation()
    {
        if (activeCellList.Count <= 0) return;
        StartCoroutine(MakeCellDance());
    }

    public void ClearActiveCellList()
    {
        activeCellList.Clear();
    }

    private Vector3 TopOfCellPosition(Vector3 cellPosition)
    {
        Vector3 pos = new Vector3(
            cellPosition.x,
            cellSize,
            cellPosition.z
            );
        return pos;
    }

    private Bounds LevelBounds()
    {
        var bounds = new Bounds();
        foreach (var cellPosition in cellProperties.Keys)
        {
            bounds.Encapsulate(cellPosition);
        }
        return bounds;
    }

    private Vector3 GetLevelCenter()
    {
        var bounds = new Bounds();
        foreach (var cellPosition in cellProperties.Keys)
        {
            bounds.Encapsulate(cellPosition);
        }
        return bounds.center;
    }
}
