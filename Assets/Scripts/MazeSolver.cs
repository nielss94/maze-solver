using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour
{
    public Vector2 mazeSpawnStart;
    public GameObject wallPrefab;
    public GameObject finishPrefab;


    [HideInInspector]
    public List<List<int>> maze = new List<List<int>>
    {
        new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        new List<int> { 1, 0, 1, 1, 0, 0, 0, 0, 0, 2, 1},
        new List<int> { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1},
        new List<int> { 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1},
        new List<int> { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
        new List<int> { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
        new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };


    void Awake()
    {
        DrawMaze();
    }

    void DrawMaze()
    {
        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < maze[i].Count; j++)
            {
                GameObject objToSpawn = null;
                switch (maze[i][j])
                {
                    case 1:
                        objToSpawn = wallPrefab;
                        break;
                    case 2:
                        objToSpawn = finishPrefab;
                        break;
                    default:
                        break;
                }
                if(objToSpawn != null)
                {
                    Instantiate(objToSpawn, new Vector2(mazeSpawnStart.x + (1 * j), mazeSpawnStart.y - (1 * i)), Quaternion.identity);
                }
            }
        }
    }
}
