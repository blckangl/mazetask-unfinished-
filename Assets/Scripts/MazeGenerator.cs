using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    int[,] maze = { { 0, 1,0,0 }, { 0, 0,1,0 } };

    public int x = 5;
    public int y = 6;
    public GameObject cellShape;
    public GameObject cellFloor;
    public GameObject PlayerPrefab;

    public Transform mazeParent;

    private GameObject player;

    
    void Start()
    {
        generateMaze();
        DebugMaze();
        player = Instantiate(PlayerPrefab,new Vector3(0,1),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateMaze()
    {
        
        int [,] mazeTemp = new int  [x,y];
        mazeTemp = generatePathWay(mazeTemp);
        int fakeways = Random.Range(0, 5);
        for (int i = 0; i < fakeways; i++)
        {
            mazeTemp = generateRandomWays(mazeTemp);

        }
 
        maze = mazeTemp;
        DrawMaze();
    }

    private int[,] generatePathWay(int[,] maze)
    {
        int row = 0;
        int col=0;
        int[,] tempMaze = maze;
        Cell currentCell = new Cell(0, 1);
        Cell nextCell = currentCell;
        tempMaze[currentCell.x, currentCell.y] = 1;
        bool isLeft = false;
        bool isRight = false;

        while (currentCell.x < maze.GetLength(0)-1)
        {
            if(currentCell.x == 0)
            {
                currentCell = new Cell(currentCell.x+1, currentCell.y);
                tempMaze[currentCell.x, currentCell.y] = 1;
                row++;
            }

            else
            {
                if (currentCell.y - 1 == 0)
                {
                    //closed wall to the left
                    //0 bottom 1 right
                    int dir = Random.Range(0, 2);
                    switch (dir)
                    {
                        case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x+1,currentCell.y); break; }
                        case 1: { isRight = true; isLeft = false; currentCell = new Cell(currentCell.x , currentCell.y+1); break; }
                    }
                    tempMaze[currentCell.x, currentCell.y] = 1;
                    row++;
                }
                else if (currentCell.y + 1 == maze.GetLength(1)-1)
                {
                    //closed wall to the right
                    //0 bottom 1 left
                
                    int dir = Random.Range(0, 1);
                    switch (dir)
                    {
                        case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                        case 1: { isRight = false;isLeft = true; currentCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                    }
                    tempMaze[currentCell.x, currentCell.y] = 1;
                    row++;
                }
                else
                {
                    if (isRight)
                    {
                        int dir = Random.Range(0, 2);
                        switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = true; isLeft = false; currentCell = new Cell(currentCell.x, currentCell.y + 1); break; }
                        }
                        tempMaze[currentCell.x, currentCell.y] = 1;
                    }
                    else if (isLeft)
                    {
                        int dir = Random.Range(0, 2);
                        switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = false; isLeft = true; currentCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                        }
                        tempMaze[currentCell.x, currentCell.y] = 1;
                    }
                    else
                    {
                        int dir = Random.Range(0, 3);

                            switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; nextCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = false; isLeft = true; nextCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                            case 2: { isRight = true; isLeft = false; nextCell = new Cell(currentCell.x, currentCell.y + 1); ; break; }
                        }
                        if (checkAdjacentCells(tempMaze, currentCell, nextCell))
                        {
                            currentCell = nextCell;
                            tempMaze[currentCell.x, currentCell.y] = 1;
                        }

                           
                    }
             

                    row++;
                }
            }

        }
        return tempMaze;
    }
    private void DebugMaze()
    {
        string mazeString = "";
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                row += maze[i,j]+ " ";
            }
            mazeString += row + "\n";
        }

        Debug.Log(mazeString);
    }

    private void DrawMaze()
    {
        for (int i = 0; i < mazeParent.childCount; i++)
        {
            DestroyImmediate(mazeParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 0)
                {
                    Instantiate(cellShape, new Vector2(i, j),Quaternion.identity,mazeParent);
                }
                else
                {
                   GameObject objCell = Instantiate(cellFloor, new Vector2(i, j), Quaternion.identity, mazeParent) as GameObject;
                    CellFloor floor = objCell.GetComponent<CellFloor>();
                    floor.x = i;
                    floor.y = j;

                }
            }
        }
    }

    private bool checkAdjacentCells(int [,] currentMat,Cell currentCell,Cell nextCell)
    {
        List<Cell> adjacentOpenCells= new List<Cell>();

        if (x+1<currentMat.GetLength(0)-1&& currentMat[nextCell.x + 1, nextCell.y] == 1)
        {
            adjacentOpenCells.Add(new Cell(x+1,y));
        }
        if (x-1>0 && currentMat[nextCell.x - 1, nextCell.y] == 1)
        {
            adjacentOpenCells.Add(new Cell(x - 1, y));
        }
        if(y-1>0 && currentMat[nextCell.x, nextCell.y-1] == 1)
        {
            adjacentOpenCells.Add(new Cell(x , y-1));

        }
        if (y + 1 < currentMat.GetLength(1)-1 && currentMat[nextCell.x, nextCell.y + 1] == 1)
        {
            adjacentOpenCells.Add(new Cell(x, y + 1));

        }
        if (adjacentOpenCells.Count > 1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    private bool checkAdjacentCells(int[,] currentMat, Cell currentCell)
    {
        List<Cell> adjacentOpenCells = new List<Cell>();
        List<Cell> adjacentClosedCells = new List<Cell>();

        if (x + 1 < currentMat.GetLength(0) - 1 && currentMat[currentCell.x + 1, currentCell.y] == 0)
        {
            adjacentClosedCells.Add(new Cell(x + 1, y));
        }
        if (x - 1 > 0 && currentMat[currentCell.x - 1, currentCell.y] == 0)
        {
            adjacentClosedCells.Add(new Cell(x - 1, y));
        }
        if (y - 1 > 0 && currentMat[currentCell.x, currentCell.y - 1] == 0)
        {
            adjacentClosedCells.Add(new Cell(x, y - 1));

        }
        if (y + 1 < currentMat.GetLength(1) - 1 && currentMat[currentCell.x, currentCell.y + 1] == 0)
        {
            adjacentClosedCells.Add(new Cell(x, y + 1));

        }
        if (adjacentClosedCells.Count == 2 && adjacentOpenCells.Count == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private int[,] generateRandomWays(int [,] currentMat)
    {
        int row = 0;
        int col = 0;
        int[,] tempMaze = currentMat;
        Cell currentCell = new Cell(1, Random.Range(1,currentMat.GetLength(1)-2));
        Cell nextCell = currentCell;
        tempMaze[currentCell.x, currentCell.y] = 1;
        bool isLeft = false;
        bool isRight = false;

        while (currentCell.x < currentMat.GetLength(0) - 2)
        {
            if (currentCell.x == 0)
            {
                currentCell = new Cell(currentCell.x + 1, currentCell.y);
                tempMaze[currentCell.x, currentCell.y] = 1;
                row++;
            }

            else
            {
                if (currentCell.y - 1 == 0)
                {
                    //closed wall to the left
                    //0 bottom 1 right
                    int dir = Random.Range(0, 2);
                    switch (dir)
                    {
                        case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                        case 1: { isRight = true; isLeft = false; currentCell = new Cell(currentCell.x, currentCell.y + 1); break; }
                    }
                    tempMaze[currentCell.x, currentCell.y] = 1;
                    row++;
                }
                else if (currentCell.y + 1 == maze.GetLength(1) - 1)
                {
                    //closed wall to the right
                    //0 bottom 1 left

                    int dir = Random.Range(0, 1);
                    switch (dir)
                    {
                        case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                        case 1: { isRight = false; isLeft = true; currentCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                    }
                    tempMaze[currentCell.x, currentCell.y] = 1;
                    row++;
                }
                else
                {
                    if (isRight)
                    {
                        int dir = Random.Range(0, 2);
                        switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = true; isLeft = false; currentCell = new Cell(currentCell.x, currentCell.y + 1); break; }
                        }
                        tempMaze[currentCell.x, currentCell.y] = 1;
                    }
                    else if (isLeft)
                    {
                        int dir = Random.Range(0, 2);
                        switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; currentCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = false; isLeft = true; currentCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                        }
                        tempMaze[currentCell.x, currentCell.y] = 1;
                    }
                    else
                    {
                        int dir = Random.Range(0, 3);

                        switch (dir)
                        {
                            case 0: { isRight = false; isLeft = false; nextCell = new Cell(currentCell.x + 1, currentCell.y); break; }
                            case 1: { isRight = false; isLeft = true; nextCell = new Cell(currentCell.x, currentCell.y - 1); break; }
                            case 2: { isRight = true; isLeft = false; nextCell = new Cell(currentCell.x, currentCell.y + 1); ; break; }
                        }
                        if (checkAdjacentCells(tempMaze, currentCell, nextCell))
                        {
                            currentCell = nextCell;
                            tempMaze[currentCell.x, currentCell.y] = 1;
                        }


                    }


                    row++;
                }
            }

        }
        return tempMaze;
    }

    public void MoveTo(int x ,int y)
    {
        List<Cell> path = findPath(new Cell(x,y));

        path.ForEach(x =>
        {
            Debug.Log($"x:${x.x} y:${x.y}");
        });

    }
    List<Cell> findPath(Cell target)
    {
        List<Cell> paths = new List<Cell>();
        Cell currentCell = new Cell(0, 1);
        while (currentCell != target &&currentCell.x<maze.GetLength(0)-1 && currentCell.x>=0&&currentCell.y<maze.GetLength(1)&&currentCell.y>=0)
        {
            if(maze[currentCell.x+1, currentCell.y]==1) {
                currentCell = new Cell(x + 1, y);
                
                    paths.Add(currentCell);

                        }
            else if (maze[currentCell.x - 1, currentCell.y] == 1)
            {
                currentCell = new Cell(x - 1, y);
             
                    paths.Add(currentCell);

                
            }
            else if (maze[currentCell.x, currentCell.y+1] == 1)
            {
                currentCell = new Cell(x , y+1);
              
                    paths.Add(currentCell);

                
            }
            else if (maze[currentCell.x , currentCell.y-1] == 1)
            {
                currentCell = new Cell(x , y-1);
             
               
                    paths.Add(currentCell);

                
            }
            
        }
        return paths;
    }
}

public class Cell{
    public int x;
    public int y;
    public Cell(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}
