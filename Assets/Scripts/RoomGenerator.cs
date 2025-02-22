using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;
    public void GenerateRoom(float roomWidth, float roomLength, Vector3 roomPos, Directions openPassages)
    {
        // Генерация пола
        GameObject floor = Instantiate(floorPrefab, roomPos, Quaternion.identity);
        floor.transform.localScale = new Vector3(roomWidth, 1, roomLength);

        // Генерация потолка
        GameObject ceiling = Instantiate(ceilingPrefab, roomPos + new Vector3(0, 3, 0), Quaternion.Euler(180, 0, 0)); // Поворачиваем потолок вниз
        ceiling.transform.localScale = new Vector3(roomWidth, 1, roomLength);

        // Генерация стен
        if (!openPassages.HasFlag(Directions.Up))
        {
            // Задняя стена
            GameObject backWall = Instantiate(wallPrefab, roomPos + new Vector3(0, 1.5f, roomLength * 5), Quaternion.Euler(-90, 0, 0));
            backWall.transform.localScale = new Vector3(roomWidth, 3, 0.3f);
        }
        if (!openPassages.HasFlag(Directions.Down))
        {
            // Передняя стена
            GameObject frontWall = Instantiate(wallPrefab, roomPos + new Vector3(0, 1.5f, -(roomLength * 5)), Quaternion.Euler(90, 0, 0));
            frontWall.transform.localScale = new Vector3(roomWidth, 3, 0.3f);
        }
        if (!openPassages.HasFlag(Directions.Right))
        {
            // Левая стена
            GameObject leftWall = Instantiate(wallPrefab, roomPos + new Vector3(roomWidth * 5, 1.5f, 0), Quaternion.Euler(0, 0, 90));
            leftWall.transform.localScale = new Vector3(0.3f, 3, roomLength);
        }
        if (!openPassages.HasFlag(Directions.Left))
        {
            // Правая стена
            GameObject rightWall = Instantiate(wallPrefab, roomPos + new Vector3(-(roomWidth * 5), 1.5f, 0), Quaternion.Euler(0, 0, -90));
            rightWall.transform.localScale = new Vector3(0.3f, 3, roomLength);
        }

        if (openPassages.HasFlag(Directions.Up))
        {
            GameObject wall = Instantiate(wallPrefab, roomPos + new Vector3(- 5 * (roomWidth + 1) / 2, 1.5f, roomLength * 10 / 2), Quaternion.Euler(-90, 0, 0));
            wall.transform.localScale = new Vector3(((roomWidth * 10 / 2) - 5) / 10, 3, 0.3f);

            GameObject wall1 = Instantiate(wallPrefab, roomPos + new Vector3(5 * (roomWidth + 1) / 2, 1.5f, roomLength * 10 / 2), Quaternion.Euler(-90, 0, 0));
            wall1.transform.localScale = new Vector3(((roomWidth * 10 / 2) - 5) / 10, 3, 0.3f);
        }

        if (openPassages.HasFlag(Directions.Down))
        {
            GameObject wall = Instantiate(wallPrefab, roomPos + new Vector3(-5 * (roomWidth + 1) / 2, 1.5f, - roomLength * 10 / 2), Quaternion.Euler(90, 0, 0));
            wall.transform.localScale = new Vector3(((roomWidth * 10 / 2) - 5) / 10, 3, 0.3f);

            GameObject wall1 = Instantiate(wallPrefab, roomPos + new Vector3(5 * (roomWidth + 1) / 2, 1.5f, - roomLength * 10 / 2), Quaternion.Euler(90, 0, 0));
            wall1.transform.localScale = new Vector3(((roomWidth * 10 / 2) - 5) / 10, 3, 0.3f);
        }

        if (openPassages.HasFlag(Directions.Right))
        {
            GameObject wall = Instantiate(wallPrefab, roomPos + new Vector3(roomWidth * 10 / 2, 1.5f, - 5 * (roomLength + 1) / 2), Quaternion.Euler(0, 0, 90));
            wall.transform.localScale = new Vector3(0.3f, 3, ((roomLength * 10 / 2) - 5) / 10);

            GameObject wall1 = Instantiate(wallPrefab, roomPos + new Vector3(roomWidth * 10 / 2, 1.5f, 5 * (roomLength + 1) / 2), Quaternion.Euler(0, 0, 90));
            wall1.transform.localScale = new Vector3(0.3f, 3, ((roomLength * 10 / 2) - 5) / 10);
        }

        if (openPassages.HasFlag(Directions.Left))
        {
            GameObject wall = Instantiate(wallPrefab, roomPos + new Vector3(-roomWidth * 10 / 2, 1.5f, -5 * (roomLength + 1) / 2), Quaternion.Euler(0, 0, -90));
            wall.transform.localScale = new Vector3(0.3f, 3, ((roomLength * 10 / 2) - 5) / 10);

            GameObject wall1 = Instantiate(wallPrefab, roomPos + new Vector3(-roomWidth * 10 / 2, 1.5f, 5 * (roomLength + 1) / 2), Quaternion.Euler(0, 0, -90));
            wall1.transform.localScale = new Vector3(0.3f, 3, ((roomLength * 10 / 2) - 5) / 10);
        }

    }



    public void GenerateCorridor(Vector2 roomPos1, Vector2 roomPos2, Vector2 size1, Vector2 size2, bool isUpCorridor, int mazeScaleFactor)
    {
        float halfLength1 = 0;
        float halfLength2 = 0;
        float corridorScale;
        Vector2 edgePoint1 = new Vector2();
        Vector2 edgePoint2 = new Vector2();
        Vector2 corridorCenter;

        if (isUpCorridor)
        {
            halfLength1 = (size1.x / 2) * 10;
            halfLength2 = (size2.x / 2) * 10;
            edgePoint1 = new Vector2(roomPos1.x, roomPos1.y + (size1.x / 2) * 10);
            edgePoint2 = new Vector2(roomPos2.x, roomPos2.y - (size2.x / 2) * 10);
            corridorScale = (mazeScaleFactor - (halfLength1 + halfLength2)) / 10;
            corridorCenter = (edgePoint1 + edgePoint2) / 2;
            GameObject upCorridorFloor = Instantiate(floorPrefab, new Vector3(corridorCenter.x, 0, corridorCenter.y), Quaternion.identity);
            upCorridorFloor.transform.localScale = new Vector3(1, 1, corridorScale);

            GameObject upCorridorCeiling = Instantiate(ceilingPrefab, new Vector3(corridorCenter.x, 3, corridorCenter.y), Quaternion.Euler(180, 0, 0));
            upCorridorCeiling.transform.localScale = new Vector3(1, 1, corridorScale);

            GameObject upCorridorRightWall = Instantiate(wallPrefab, new Vector3(corridorCenter.x - 5, 1.5f, corridorCenter.y), Quaternion.Euler(0, 0, -90));
            upCorridorRightWall.transform.localScale = new Vector3(0.3f, 1, corridorScale);

            GameObject upCorridorLeftWall = Instantiate(wallPrefab, new Vector3(corridorCenter.x + 5, 1.5f, corridorCenter.y), Quaternion.Euler(0, 0, 90));
            upCorridorLeftWall.transform.localScale = new Vector3(0.3f, 1, corridorScale);
        }
        if (!isUpCorridor)
        {
            halfLength1 = (size1.y / 2) * 10;
            halfLength2 = (size2.y / 2) * 10;
            edgePoint1 = new Vector2(roomPos1.x + (size1.y / 2) * 10, roomPos1.y);
            edgePoint2 = new Vector2(roomPos2.x - (size2.y / 2) * 10, roomPos2.y);
            corridorScale = (mazeScaleFactor - (halfLength1 + halfLength2)) / 10;
            corridorCenter = (edgePoint1 + edgePoint2) / 2;
            GameObject rightCorridorFloor = Instantiate(floorPrefab, new Vector3(corridorCenter.x, 0, corridorCenter.y), Quaternion.identity);
            rightCorridorFloor.transform.localScale = new Vector3(corridorScale, 1, 1);

            GameObject rightCorridorCeiling = Instantiate(ceilingPrefab, new Vector3(corridorCenter.x, 3, corridorCenter.y), Quaternion.Euler(180, 0, 0));
            rightCorridorCeiling.transform.localScale = new Vector3(corridorScale, 1, 1);

            GameObject rightCorridorRightWall = Instantiate(wallPrefab, new Vector3(corridorCenter.x, 1.5f, corridorCenter.y - 5), Quaternion.Euler(0, 90, 90));
            rightCorridorRightWall.transform.localScale = new Vector3(0.3f, 1, corridorScale);

            GameObject rightCorridorLeftWall = Instantiate(wallPrefab, new Vector3(corridorCenter.x, 1.5f, corridorCenter.y + 5), Quaternion.Euler(0, 90, -90));
            rightCorridorLeftWall.transform.localScale = new Vector3(0.3f, 1, corridorScale);


        }



        //halfLength1 = (size1.x / 2) * 10;
        //halfLength2 = (size2.x / 2) * 10;
        //edgePoint1 = new Vector2(roomPos1.x, roomPos1.y + (size1.x / 2) * 10);
        //edgePoint2 = new Vector2(roomPos2.x, roomPos2.y - (size2.x / 2) * 10);
        //corridorCenter = (edgePoint1 + edgePoint2) / 2;
        //corridorScale = (mazeScaleFactor - (halfLength1 + halfLength2)) / 10;



        
    }
}
