using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    South,
    East,
    West
}

public class Challenger : MonoBehaviour
{
    //DNA
    public DNA<Direction> dna = null;

    private Vector2 posInMaze;
    private List<List<int>> maze = null;
    [SerializeField]private float timeBetweenSteps = 0.1f;

    private Vector3 startWorldPos = Vector3.zero;

    private void Start() {
        maze = FindObjectOfType<MazeSolver>().maze;
        startWorldPos = transform.position;

        Initialize();
    }

    public void Initialize()
    {
        posInMaze = new Vector2(1,1);
        transform.position = startWorldPos;
        StartCoroutine(Solve());
    }

    IEnumerator Solve()
    {
        foreach(var move in dna.Genes)
        {
            if(!ReachedFinish(posInMaze))
            {
                yield return new WaitForSeconds(timeBetweenSteps);
                TryMove(move);
            }else{
                
                print(transform.name + " made it!");
                print(dna.ToString());
                DNAManager dnaManager = FindObjectOfType<DNAManager>();
                dnaManager.BestGenes = dna.Genes;
                dnaManager.Winner(this);
                dnaManager.BestFitness = dna.Fitness;
                break;
            }
        }

        ShowResult();
    }

    void ShowResult()
    {
        print(transform.name + " scored " + dna.Fitness.ToString("F2"));
    }

    void TryMove(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                if (CanMoveUp(posInMaze))
                {
                    posInMaze.x--;
                    transform.Translate(Vector2.up);
                }
                else{
                    dna.Fitness--;
                }
                break;
            case Direction.South:
                if (CanMoveDown(posInMaze))
                {
                    transform.Translate(Vector2.down);
                    posInMaze.x++;
                }
                else{
                    dna.Fitness--;
                }
                break;
            case Direction.East:
                if (CanMoveRight(posInMaze))
                {
                    transform.Translate(Vector2.right);
                    posInMaze.y++;
                }
                else{
                    dna.Fitness--;
                }
                break;
            case Direction.West:
                if (CanMoveLeft(posInMaze))
                {
                    transform.Translate(Vector2.left);
                    posInMaze.y--;
                }
                else{
                    dna.Fitness--;
                }
                break;
            default:
                break;
        }
    }

    bool ReachedFinish(Vector2 playerPos)
    {
        return maze[(int)playerPos.x][(int)playerPos.y] == 2;
    }

    bool CanMoveRight(Vector2 playerPos)
    {
        int nextPos = maze[(int)playerPos.x][(int)playerPos.y + 1];
        return (nextPos == 0 || nextPos == 2);
    }

    bool CanMoveLeft(Vector2 playerPos)
    {
        int nextPos = maze[(int)playerPos.x][(int)playerPos.y - 1];
        return (nextPos == 0 || nextPos == 2);
    }

    bool CanMoveUp(Vector2 playerPos)
    {
        int nextPos = maze[(int)playerPos.x - 1][(int)playerPos.y];
        return (nextPos == 0 || nextPos == 2);
    }

    bool CanMoveDown(Vector2 playerPos)
    {
        int nextPos = maze[(int)playerPos.x + 1][(int)playerPos.y];
        return (nextPos == 0 || nextPos == 2);
    }
}
