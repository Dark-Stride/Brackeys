using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public GameObject startRoom, endRoom;
    public GameObject[] roomPrefabs;
    public int totalRooms = 10;
    public float roomOffset = 20f; // Distance between rooms

    private List<Vector2> roomPositions = new();

    void Start()
    {
        WorldSeedManager.Initialize();
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector2 currentPos = Vector2.zero;
        roomPositions.Add(currentPos);
        Instantiate(startRoom, Vector3.zero, Quaternion.identity, transform);

        for (int i = 0; i < totalRooms; i++)
        {
            // Pick a random direction (North, South, East, West)
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            Vector2 dir = directions[Random.Range(0, directions.Length)];
            currentPos += dir * roomOffset;

            if (!roomPositions.Contains(currentPos))
            {
                roomPositions.Add(currentPos);
                GameObject prefab = (i == totalRooms - 1) ? endRoom : roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                Instantiate(prefab, new Vector3(currentPos.x, 0, currentPos.y), Quaternion.identity, transform);
            }
            else { i--; } // Try again if position taken
        }
    }
}
