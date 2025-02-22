using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    public int minRoomSize = 5;
    public int maxRoomSize = 15;
    public int roomCount = 10;

    [Header("Corridor Settings")]
    public int corridorWidth = 2;

    [Header("Prefabs")]
    public RoomGenerator roomGenerator;
    public MazeGenerator mazeGenerator;

    private List<MazeCell> maze = new List<MazeCell>();

    private List<Room> rooms = new List<Room>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        maze = mazeGenerator.GenerateMaze();
        foreach (var room in maze)
        {
            var roomSizeX = Random.Range(1, 5);
            var roomSizeY = Random.Range(1, 5);
            roomGenerator.GenerateRoom(
                roomSizeX,
                roomSizeY,
                new Vector3(room.Position.x * mazeGenerator.mazeScaleFactor, 0, room.Position.y * mazeGenerator.mazeScaleFactor), room.OpenPassages);

            Room roomInfo = new Room();
            roomInfo.Length = roomSizeX;
            roomInfo.Width = roomSizeY;

            rooms.Add(roomInfo);
        }
        foreach (var room in maze)
        {
            
            if (room.OpenPassages.HasFlag(Directions.Up))
            {
                var room2 = maze.FindIndex(pos => (pos.Position.y == (room.Position.y + 1) && pos.Position.x == room.Position.x));
            roomGenerator.GenerateCorridor(
                room.Position * mazeGenerator.mazeScaleFactor,
                maze[room2].Position * mazeGenerator.mazeScaleFactor, 
                new Vector2(rooms[maze.IndexOf(room)].Width, rooms[maze.IndexOf(room)].Length),
                new Vector2(rooms[room2].Width, rooms[room2].Length),
                true,
                mazeGenerator.mazeScaleFactor
                );
            }
            if (room.OpenPassages.HasFlag(Directions.Right))
            {
                var room2 = maze.FindIndex(pos => (pos.Position.y == room.Position.y && pos.Position.x == (room.Position.x + 1)));
                roomGenerator.GenerateCorridor(
                    room.Position * mazeGenerator.mazeScaleFactor,
                    maze[room2].Position * mazeGenerator.mazeScaleFactor,
                    new Vector2(rooms[maze.IndexOf(room)].Width, rooms[maze.IndexOf(room)].Length),
                    new Vector2(rooms[room2].Width, rooms[room2].Length),
                    false,
                    mazeGenerator.mazeScaleFactor
                    );
            }

        }


        for (int i = 0; i < maze.Count; i++)
        {
            MazeCell cell = maze[i];
            Debug.Log($"Ячейка в позиции: {cell.Position}, Открытые проходы: {cell.OpenPassages}");
        }

    }



    void CreateCorridorBetweenRooms(Room roomA, Room roomB)
    {

    }

    void CreateDoorway(Room room, Vector3 corridorPosition)
    {
        // Определим ближайшую стену, к которой прилегает коридор
        Transform closestWall = null;
        float minDistance = float.MaxValue;

        // Получаем все стены комнаты
        //foreach (Transform wall in room.RoomObject.transform)
        //{
        //    if (wall.CompareTag("Wall")) // Убедитесь, что стена имеет тег "Wall"
        //    {
        //        float distance = Vector3.Distance(wall.position, corridorPosition);
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            closestWall = wall;
        //        }
        //    }
        //}

        if (closestWall == null)
        {
            Debug.LogError("Не найдена ближайшая стена для создания отверстия!");
            return;
        }

        // Создаем отверстие в ближайшей стене
        CreateHoleInWall(closestWall, corridorPosition, corridorWidth);
    }

    void CreateHoleInWall(Transform wall, Vector3 corridorPosition, float holeWidth)
    {
        Vector3 wallScale = wall.localScale;

        // Рассчитываем позиции для новой стены слева и справа от отверстия
        Vector3 leftWallPosition = wall.position + (Vector3.left * holeWidth / 2);
        Vector3 rightWallPosition = wall.position + (Vector3.right * holeWidth / 2);

        // Уменьшаем размер оригинальной стены по ширине для создания отверстия
        float newWallWidth = (wallScale.x - holeWidth) / 2;

        if (newWallWidth <= 0)
        {
            // Если новая ширина стенки отрицательная или нулевая, просто удаляем стену
            Destroy(wall.gameObject);
            return;
        }

        // Создаем левую часть стены
        //GameObject leftWall = Instantiate(wallPrefab, leftWallPosition, wall.rotation, wall.parent);
        //leftWall.transform.localScale = new Vector3(newWallWidth, wallScale.y, wallScale.z);

        //// Создаем правую часть стены
        //GameObject rightWall = Instantiate(wallPrefab, rightWallPosition, wall.rotation, wall.parent);
        //rightWall.transform.localScale = new Vector3(newWallWidth, wallScale.y, wallScale.z);

        //// Удаляем оригинальную стену
        //Destroy(wall.gameObject);
    }

}

public class Room
{
    public float Width { get; set; }
    public float Length { get; set; }
    //public int RoomID { get; private set; }
}