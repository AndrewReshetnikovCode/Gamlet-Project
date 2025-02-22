using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public float interval = 1f;
    public GameObject mazeMinoutaur;
    public GameObject cube;
    public int mazeLength = 5;
    public int branchFactor = 12;
    public int branchLength = 7;
    public List<List<Vector2Int>> branches = new List<List<Vector2Int>>();
    public Vector2Int startPoint;
    public int mazeScaleFactor = 1;

    private bool isDeadEnd;

    private List<Vector2Int> path = new List<Vector2Int>();
    private List<MazeCell> maze = new List<MazeCell>();


    [ContextMenu("Generate Maze")]
    public List<MazeCell> GenerateMaze()
    {
        startPoint = new Vector2Int(0, 0);

        path.Add(startPoint);
        AddMazeCell(new Vector2Int(0, 0), Directions.None);
        MazeMinotaur(startPoint);
        Vector2Int current = new Vector2Int();

        while (path.Count <= mazeLength)
        {
            if (isDeadEnd)
            {
                break;
            }

            current = path[path.Count - 1];
            path = GenerateNextMazePoint(current, path);
            //yield return null;
        }
        Debug.Log("Основной путь завершен.");

        for (int i = 0; i < branchFactor; i++)
        {
            isDeadEnd = false;
            GenerateBranch();
        }
        Debug.Log("Лабиринт завершен.");
        return maze;
    }


    private void GenerateBranch()
    {
        var r = path[UnityEngine.Random.Range(0, path.Count)];
        branches.Add(new List<Vector2Int> { new Vector2Int(r.x, r.y) });

        var b = branches[branches.Count - 1];
        Vector2Int current = new Vector2Int();

        current = b[b.Count - 1];
        b = GenerateNextMazePoint(current, b); //#TODO 

        while (b.Count <= branchLength)
        {
            if (isDeadEnd)
            {
                break;
            }
            current = b[b.Count - 1];
            b = GenerateNextMazePoint(current, b);
            //yield return null;
        }
        Debug.Log("Ветка завершена.");
    }

    public List<Vector2Int> GenerateNextMazePoint(Vector2Int current, List<Vector2Int> newPath)
    {
        List<Vector2Int> freeCells = new List<Vector2Int>()
        {
            new Vector2Int(current.x, current.y + 1),
            new Vector2Int(current.x, current.y - 1),
            new Vector2Int(current.x + 1, current.y),
            new Vector2Int(current.x - 1, current.y)
        };

        freeCells.RemoveAll(cell => maze.Any(pos => pos.Position == cell));
        

        if (freeCells.Count != 0)
        {
            Vector2Int next = freeCells[UnityEngine.Random.Range(0, freeCells.Count)];

            AddMazeCell(next, Directions.None);

            int index = maze.FindIndex(pos => pos.Position == next);
            int index1 = maze.FindIndex(pos => pos.Position == current);
            switch (next)
            {
                case Vector2Int v when v == new Vector2Int(current.x, current.y + 1):
                    maze[index].AddPassage(Directions.Down);
                    maze[index1].AddPassage(Directions.Up);
                    break;

                case Vector2Int v when v == new Vector2Int(current.x, current.y - 1):
                    maze[index].AddPassage(Directions.Up);
                    maze[index1].AddPassage(Directions.Down);
                    break;
                case Vector2Int v when v == new Vector2Int(current.x + 1, current.y):
                    maze[index].AddPassage(Directions.Left);
                    maze[index1].AddPassage(Directions.Right);
                    break;
                case Vector2Int v when v == new Vector2Int(current.x - 1, current.y):
                    maze[index].AddPassage(Directions.Right);
                    maze[index1].AddPassage(Directions.Left);
                    break;
                default:
                    Debug.Log("Вектор не соответствует ни одному из заданных значений.");
                    break;
            }
            newPath.Add(next);

            MazeMinotaur(next);
        }
        else
        {
            Debug.Log("Опа. Тупичок.");
            isDeadEnd = true;
        }

        return newPath;
    }

    private void MazeMinotaur(Vector2Int cell)
    {
        //mazeMinoutaur.transform.position = new Vector3(cell.x * mazeScaleFactor, 0, cell.y * mazeScaleFactor);
        //Instantiate(cube, new Vector3(cell.x * mazeScaleFactor, 0, cell.y * mazeScaleFactor), Quaternion.Euler(0, 0, 0));
    }


    private void AddMazeCell(Vector2Int position, Directions openPassages)
    {
        MazeCell newCell = new MazeCell(position, openPassages);
        maze.Add(newCell);
        Debug.Log($"Ячейка добавлена: {newCell.Position} с проходами: {newCell.OpenPassages}");
    }
}

[System.Flags]
public enum Directions
{
    None = 0,      // Нет открытых проходов
    Up = 1 << 0,   // Проход вверх (0001)
    Down = 1 << 1, // Проход вниз (0010)
    Left = 1 << 2, // Проход влево (0100)
    Right = 1 << 3 // Проход вправо (1000)
}

public class MazeCell
{
    public Vector2Int Position { get; set; }   // Координаты ячейки
    public Directions OpenPassages { get; set; } // Открытые проходы в четырех направлениях

    // Конструктор
    public MazeCell(Vector2Int position, Directions openPassages = Directions.None)
    {
        Position = position;
        OpenPassages = openPassages;
    }

    // Метод для добавления прохода
    public void AddPassage(Directions direction)
    {
        OpenPassages |= direction; // Установка прохода с помощью побитового OR
    }

    // Метод для удаления прохода
    public void RemovePassage(Directions direction)
    {
        OpenPassages &= ~direction; // Удаление прохода с помощью побитового AND
    }

    // Метод для проверки наличия прохода
    public bool HasPassage(Directions direction)
    {
        return (OpenPassages & direction) != Directions.None;
    }
}

//public class GridInfo
//{
//    List<List<MazeCell>> _Grid;

//    public MazeCell GetCellAtPosition(Vector2Int position)
//    {
//        return _Grid[position.x][position.y];
//    }

//    public void SetCellInPosition(MazeCell mazeCell, Vector2Int position)
//    {
//        _Grid[position.x][position.y] = mazeCell;
//    }

//    public void Init(Vector2Int size, Vector2Int position)
//    {
//        _Grid = new();
//        for (int x = 0; x < size.x; x++)
//        {
//            List<MazeCell> xCollumn = new List<MazeCell>();
//            for (int y = 0; y < size.y; y++)
//            {
//                List<MazeCell> yCollumn = new List<MazeCell>();
//                MazeCell mazeCell = new MazeCell(new Vector2Int(x, y));
//            }
//        }
//    }
//}
