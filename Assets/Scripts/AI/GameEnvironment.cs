using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public sealed class GameEnvironment 
{
    // Declare a private static variable to hold the single instance of this class
    private static GameEnvironment instance;
    
    // Initialize a private list to store the checkpoint GameObjects
    private List <GameObject> checkPoints = new List <GameObject> ();
    
    // Define a public read-only property to expose the checkpoints list safely
    public List<GameObject> CheckPoints 
    {
        // Return the private checkpoints list when accessed
        get => checkPoints;
    }

    // Define a public static property to access the singleton instance globally
    public static GameEnvironment singleTon
    {
      
        get 
        {
            // Check if the singleton instance has not been created yet
            if (instance == null)
            {
                // Create a new instance of the GameEnvironment class
                instance = new GameEnvironment ();
                
                // Find all GameObjects in the scene with the tag "Checkpoint" and add them to the list
                instance.checkPoints.AddRange (GameObject.FindGameObjectsWithTag("Checkpoint"));
                
                // Sort the checkpoints list alphabetically by their GameObject name 
                // Note: To apply the sort to the list, this should be: instance.checkPoints = instance.checkPoints.OrderBy(x => x.name).ToList();
                //instance.checkPoints.OrderBy(x => x.name).ToList();
                instance.checkPoints = instance.checkPoints.OrderBy(x => x.name).ToList();
            }

            return instance;
        }
    }
    public static void Reset()
    {
        instance = null;
    }
}
