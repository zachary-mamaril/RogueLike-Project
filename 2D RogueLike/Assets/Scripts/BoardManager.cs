using UnityEngine;
using System;
using System.Collections.Generic; //Allows us to use Lists.
using Random = UnityEngine.Random;  //Tells Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum; //Minimum value for our Count class.
        public int maximum; //Maximum value for our Count class.

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;//Number of columns in our game board.
    public int rows = 8;  //Number of rows in our game board.
    public Count wallCount = new Count(5, 9);//Lower and upper limit for our random number of walls per level.
    public Count foodCount = new Count(1, 5);//Lower and upper limit for our random number of food items per level.
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>(); //A list of possible locations to place tiles.

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        gridPositions.Clear(); //Clear our list gridPositions.

        for (int x = 1; x < columns - 1; x++)  //Loop through x axis (columns).
        {
            for (int y = 1; y < rows - 1; y++) //Within each column, loop through y axis (rows).
            {
                gridPositions.Add(new Vector3(x, y, 0f));  //At each index add a new Vector3 to our list with the x and y coordinates of that position.
            }

        }
    }

    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform; // Instantiate Board and set boardHolder to its transform.

        for (int x = -1; x < columns + 1; x++)  //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        {
            for (int y = -1; y < rows + 1; y++) //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                GameObject instance = 
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), 
                    Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }
    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        
        int randomIndex = Random.Range(0, gridPositions.Count); //Sets value to a random number between 0 and the count of items in our List gridPositions.

        Vector3 randomPosition = gridPositions[randomIndex];  // Sets value to the entry at randomIndex from our List gridPositions.

        gridPositions.RemoveAt(randomIndex); //Remove the entry at randomIndex from the list so that it can't be re-used.
        return randomPosition; //Return the randomly selected Vector3 position.
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) //Accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    {
        int objectCount = Random.Range(minimum, maximum + 1);  //Choose a random number of objects to instantiate within the minimum and maximum limits

        
        for (int i = 0; i < objectCount; i++) //Instantiate objects until the randomly chosen limit objectCount is reached
        {
            Vector3 randomPosition = RandomPosition();  //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition

           
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];  //Choose a random tile from tileArray and assign it to tileChoice

            
            Instantiate(tileChoice, randomPosition, Quaternion.identity); //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
        }
    }


        
    public void SetupScene(int level) //SetupScene initializes our level and calls the previous functions to lay out the game board
    {     
        BoardSetup(); //Creates the outer walls and floor.
       
        InitialiseList();  //Reset our list of gridpositions.

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum); //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
       
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum); //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.

        int enemyCount = (int)Mathf.Log(level, 2f);  //Determine number of enemies based on current level number, based on a logarithmic progression

        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount); //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity); //Instantiate the exit tile in the upper right hand corner of our game board
    }

}