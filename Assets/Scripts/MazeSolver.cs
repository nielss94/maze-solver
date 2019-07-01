using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour
{
    public Vector2 mazeSpawnStart;
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject finishPrefab;

    private List<List<int>> maze = new List<List<int>>
    {
        new List<int> { 1, 1, 1, 1, 1, 1, 1},
        new List<int> { 1, 2, 1, 1, 0, 3, 1},
        new List<int> { 1, 0, 1, 0, 0, 0, 1},
        new List<int> { 1, 0, 1, 0, 0, 1, 1},
        new List<int> { 1, 0, 0, 0, 0, 1, 1},
        new List<int> { 1, 0, 0, 0, 0, 0, 1},
        new List<int> { 1, 1, 1, 1, 1, 1, 1}
    };

    private GameObject activePlayer;

    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public Direction[] firstMoves;

    void Awake()
    {
        DrawMaze();
    }

    private void Start()
    {
        IEnumerator testMove = TestMove();
        StartCoroutine(testMove);
    }

    void Update()
    {
        
    }

    IEnumerator TestMove()
    {
        foreach (var item in firstMoves)
        {
            yield return new WaitForSeconds(.5f);
            TryMove(item);
        }
    }

    void TryMove(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                if (CanMoveUp(GetPlayerPos()))
                {
                    Vector2 playerPos = GetPlayerPos();
                    maze[(int)playerPos.x][(int)playerPos.y] = 0;
                    maze[(int)playerPos.x - 1][(int)playerPos.y] = 2;
                    activePlayer.transform.Translate(Vector2.up);
                }
                    
                break;
            case Direction.South:
                if (CanMoveDown(GetPlayerPos()))
                {
                    Vector2 playerPos = GetPlayerPos();
                    print(playerPos);
                    maze[(int)playerPos.x][(int)playerPos.y] = 0;
                    maze[(int)playerPos.x + 1][(int)playerPos.y] = 2;
                    print(GetPlayerPos());
                    activePlayer.transform.Translate(Vector2.down);
                }
                break;
            case Direction.East:
                if (CanMoveRight(GetPlayerPos()))
                {
                    Vector2 playerPos = GetPlayerPos();
                    maze[(int)playerPos.x][(int)playerPos.y] = 0;
                    maze[(int)playerPos.x][(int)playerPos.y + 1] = 2;
                    activePlayer.transform.Translate(Vector2.right);
                }
                break;
            case Direction.West:
                if (CanMoveRight(GetPlayerPos()))
                {
                    Vector2 playerPos = GetPlayerPos();
                    maze[(int)playerPos.x][(int)playerPos.y] = 0;
                    maze[(int)playerPos.x][(int)playerPos.y - 1] = 2;
                    activePlayer.transform.Translate(Vector2.left);
                }
                break;
            default:
                break;
        }
    }
    
    
    Vector2 GetPlayerPos()
    {
        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] == 2)
                    return new Vector2(i, j);
            }
        }
        return Vector2.zero;
    }


    bool CanMoveRight(Vector2 playerPos)
    {
        return maze[(int)playerPos.x][(int)playerPos.y + 1] == 0;
    }

    bool CanMoveLeft(Vector2 playerPos)
    {
        return maze[(int)playerPos.x][(int)playerPos.y - 1] == 0;
    }

    bool CanMoveUp(Vector2 playerPos)
    {
        return maze[(int)playerPos.x - 1][(int)playerPos.y] == 0;
    }

    bool CanMoveDown(Vector2 playerPos)
    {
        return maze[(int)playerPos.x + 1][(int)playerPos.y] == 0;
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
                        objToSpawn = playerPrefab;
                        break;
                    case 3:
                        objToSpawn = finishPrefab;
                        break;
                    default:
                        break;
                }
                if(objToSpawn != null)
                {
                    if(maze[i][j] == 2)
                    {
                        activePlayer = Instantiate(objToSpawn, new Vector2(mazeSpawnStart.x + (1 * j), mazeSpawnStart.y - (1 * i)), Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        Instantiate(objToSpawn, new Vector2(mazeSpawnStart.x + (1 * j), mazeSpawnStart.y - (1 * i)), Quaternion.identity);
                    }
                }
            }
        }
    }
}
