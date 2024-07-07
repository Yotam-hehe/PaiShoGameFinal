using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Harmony : MonoBehaviour
{
    public GameObject linePrefab;


    // Dictionary to store harmonies
    private Dictionary<(GameObject, GameObject), GameObject> harmonies;


    // Start is called before the first frame update
    void Start()
    {
        harmonies = new Dictionary<(GameObject, GameObject), GameObject>();
        linePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Function to create a line between two pieces
    public void CreateHarmonyLine(GameObject piece1, GameObject piece2)
    {
        Debug.Log("Inside");
        // Instantiate a new harmony line GameObject
        GameObject line = Instantiate(linePrefab);
        if(line != null)
        {
            Debug.Log("Okay so its not null, Wtf");
        }

        // Set positions and rotation of the line
        line.transform.position = (piece1.transform.position + piece2.transform.position) / 2f;
        line.transform.LookAt(piece2.transform.position);
        Vector3 currentRotation = line.transform.eulerAngles;
        currentRotation.x = 90;
        line.transform.eulerAngles = currentRotation;

        // Adjust the scale of the line based on the distance between pieces
        float distance = Vector3.Distance(piece1.transform.position, piece2.transform.position);
        line.transform.localScale = new Vector3(0.5f, (0.5f*distance)-0.4f, 0.5f);


        // Add to dictionary
        harmonies.Add((piece1, piece2), line);

    }

    // Function to remove a line between two pieces
    public void RemoveHarmonyLine(GameObject piece1, GameObject piece2)
    {
        // Check if the harmony exists
        if (harmonies.ContainsKey((piece1, piece2)))
        {
            // Destroy the line object
            Destroy(harmonies[(piece1, piece2)]);
            // Remove from dictionary
            harmonies.Remove((piece1, piece2));
        }
    }

    // Function to check and update harmonies
    public void UpdateHarmonies(GameObject movedPiece, GameObject[] teamPieces, GameObject[] opponentPieces)
    {
        // Check if Old Harmonies need to be deleted
        if (harmonies != null)
        {
            foreach (var harmony in harmonies.Keys.ToArray())
            {
                GameObject piece1 = harmony.Item1;
                GameObject piece2 = harmony.Item2;


                //|| IsObstructed(piece1, piece2, teamPieces) || IsObstructed(piece1, piece2, opponentPieces)
                // Check if the harmony is still valid
                if (!IsAligned(piece1, piece2) || IsObstructed(piece1, piece2, teamPieces) || IsObstructed(piece1, piece2, opponentPieces))
                {
                    // Remove the line
                    RemoveHarmonyLine(piece1, piece2);
                }
            }
        }


        // Check for new harmonies
        foreach (GameObject piece1 in teamPieces)
        {
            // If the piece is a white lotus it can harmonize with both sides
            if (piece1.GetComponent<PieceScript>().type == PieceType.WhiteLotus)
            {
                if (piece1.transform.position.x > 0 && piece1.transform.position.x < 19 && piece1.transform.position.z > 0 && piece1.transform.position.z < 19)
                {
                    foreach (GameObject piece2 in teamPieces)
                    {
                            if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                            {
                                if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                                {
                                    if (piece2.GetComponent<PieceScript>().basicTile)
                                    {
                                        // Create a new harmony line
                                        CreateHarmonyLine(piece1, piece2);
                                    }
                                }
                            }
                    }
                    foreach(GameObject piece2 in opponentPieces)
                    {
                        if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                        {
                            if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                            {
                                if (piece2.GetComponent<PieceScript>().basicTile)
                                {
                                    // Create a new harmony line
                                    CreateHarmonyLine(piece1, piece2);
                                }
                            }
                        }
                    }
                }
            }

            // If the piece isnt a white lotus it can harmonize only with its team's pieces
            else if (piece1.transform.position.x > 0 && piece1.transform.position.x < 19 && piece1.transform.position.z > 0 && piece1.transform.position.z < 19)
            {
                foreach (GameObject piece2 in teamPieces)
                {
                    if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                    {
                        if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                        {
                            if (piece1.GetComponent<PieceScript>().HarmonyCheck(piece2))
                            {
                                // Create a new harmony line
                                CreateHarmonyLine(piece1, piece2);
                            }
                        }
                    }
                }
            }
        }

        // Check for new harmonies Opponent
        foreach (GameObject piece1 in opponentPieces)
        {
            // If the piece is a white lotus it can harmonize with both sides
            if (piece1.GetComponent<PieceScript>().type == PieceType.WhiteLotus)
            {
                if (piece1.transform.position.x > 0 && piece1.transform.position.x < 19 && piece1.transform.position.z > 0 && piece1.transform.position.z < 19)
                {
                    foreach (GameObject piece2 in teamPieces)
                    {
                        if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                        {
                            if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                            {
                                if (piece2.GetComponent<PieceScript>().basicTile)
                                {
                                    // Create a new harmony line
                                    CreateHarmonyLine(piece1, piece2);
                                }
                            }
                        }
                    }
                    foreach (GameObject piece2 in opponentPieces)
                    {
                        if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                        {
                            if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                            {
                                if (piece2.GetComponent<PieceScript>().basicTile)
                                {
                                    // Create a new harmony line
                                    CreateHarmonyLine(piece1, piece2);
                                }
                            }
                        }
                    }
                }
            }

            // If the piece isnt a white lotus it can harmonize only with its team's pieces
            else if (piece1.transform.position.x > 0 && piece1.transform.position.x < 19 && piece1.transform.position.z > 0 && piece1.transform.position.z < 19)
            {
                foreach (GameObject piece2 in opponentPieces)
                {
                    if (piece2.transform.position.x > 0 && piece2.transform.position.x < 19 && piece2.transform.position.z > 0 && piece2.transform.position.z < 19)
                    {
                        if (piece1 != piece2 && IsAlignedNew(piece1, piece2) && (!harmonies.ContainsKey((piece1, piece2))) && !IsObstructed(piece1, piece2, teamPieces) && !IsObstructed(piece1, piece2, opponentPieces))
                        {
                            if (piece1.GetComponent<PieceScript>().HarmonyCheck(piece2))
                            {
                                // Create a new harmony line
                                CreateHarmonyLine(piece1, piece2);
                            }
                        }
                    }
                }
            }
        }
    }

    // Function to check if two pieces are aligned on the same x or y value
    private bool IsAlignedNew(GameObject piece1, GameObject piece2)
    {
        return piece1.transform.position.x == piece2.transform.position.x || piece1.transform.position.z == piece2.transform.position.z;
    }

    // Function to check if two pieces are aligned on the same x or y value and that their distance from eachother didnt change
    private bool IsAligned(GameObject piece1, GameObject piece2)
    {
        float distance = Vector3.Distance(piece1.transform.position, piece2.transform.position);
        if(harmonies.ContainsKey((piece1, piece2)))
        {
            bool uno = (piece1.transform.position.x == piece2.transform.position.x || piece1.transform.position.z == piece2.transform.position.z);

            float one = harmonies[(piece1, piece2)].transform.localScale.y;

            float two = (0.5f * distance) - 0.4f;

            bool dos = (one == two);
            return uno && dos;
        }
        return false;
    }


    /// <summary>
    ///  Function to check if there is an obstructing piece between two pieces
    /// </summary>
    /// <param name="piece1"></param>
    /// <param name="piece2"></param>
    /// <param name="pieces"></param>
    /// <returns></returns>
    private bool IsObstructed(GameObject piece1, GameObject piece2, GameObject[] pieces)
    {
        foreach (GameObject obstructingPiece in pieces)
        {
            if(obstructingPiece!=piece1 && obstructingPiece != piece2)
            {
                if (IsOnLineBetween(obstructingPiece, piece1, piece2))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsOnLineBetween(GameObject objBetween, GameObject obj1, GameObject obj2)
    {
        // Calculate distances
        float distanceObj1 = Vector3.Distance(objBetween.transform.position, obj1.transform.position);
        float distanceObj2 = Vector3.Distance(objBetween.transform.position, obj2.transform.position);
        float distanceBetween = Vector3.Distance(obj1.transform.position, obj2.transform.position);

        // Tolerance threshold for comparison
        float threshold = 0.1f;

        // Check if objBetween is reasonably close to the line segment
        if (Mathf.Abs(distanceObj1 + distanceObj2 - distanceBetween) < threshold)
        {
            return true;
        }
        return false;
    }




    // Function to check for the win condition: 4 or more differant harmonies of the same team creating a shape around the middle point (9,9)
    public GameObject Win()
    {
        // Create a boolean dictionary to keep track of visited harmonies
        Dictionary<GameObject, bool> visited = new Dictionary<GameObject, bool>();

        foreach (var harmonyPair in harmonies)
        {
            visited[harmonyPair.Value] = false; // Initialize all harmonies as unvisited
        }

        // Iterate through each harmony to find connected components
        foreach (var harmony in harmonies)
        {
            var harmonyVal = harmony.Value;
            if (!visited[harmonyVal])
            {
                // If this harmony hasn't been visited, perform a depth-first search to find connected harmonies
                List<GameObject> connectedHarmonies = new List<GameObject>();
                DFS(harmonyVal, harmonies, connectedHarmonies, visited);

                // Check if there are 8 or more connected harmonies, each counts as 2
                if (connectedHarmonies.Count >= 8)
                {
                    GameObject Val2 = connectedHarmonies[connectedHarmonies.Count-1];
                    GameObject Val1 = connectedHarmonies[0];
                    if (IsConnected(Val1,Val2, harmonies))
                    {
                        return harmony.Key.Item1;
                    }
                }
            }
        }
        return null;
    }
    public bool IsConnected(GameObject harmony, GameObject currentHarmony, Dictionary<(GameObject, GameObject), GameObject> harmonies)
    {
        GameObject piece1 = GetKeysForValue(harmony, harmonies)[0].Item1;
        GameObject piece2 = GetKeysForValue(harmony, harmonies)[0].Item2;
        GameObject currentPiece1 = GetKeysForValue(currentHarmony, harmonies)[0].Item1;
        GameObject currentPiece2 = GetKeysForValue(currentHarmony, harmonies)[0].Item2;

        if( (piece1==currentPiece1&& piece2!=currentPiece2) || (piece1 == currentPiece2 && piece2 != currentPiece1) || (piece2 == currentPiece1 && piece1 != currentPiece2)|| (piece2 == currentPiece2 && piece1 != currentPiece1)) 
        {
            return true;
        }
        return false;
    }

    //Depth-first search function
    private void DFS(GameObject harmony, Dictionary<(GameObject, GameObject), GameObject> harmonies, List<GameObject> connectedHarmonies, Dictionary<GameObject, bool> visited)
    {
        // Mark the current harmony as visited
        visited[harmony] = true;
        connectedHarmonies.Add(harmony);
        
        // Go through the harmonies connected to the current harmony
        foreach (var harmonyPair in harmonies)
        {
            var currentHarmony = harmonyPair.Value;
            if (!visited[currentHarmony] && IsConnected(harmony, currentHarmony, harmonies))
            {
                // Depth-first search for connected harmonies
                DFS(currentHarmony, harmonies, connectedHarmonies, visited);
            }
        }
    }
    private List<(GameObject, GameObject)> GetKeysForValue(GameObject value, Dictionary<(GameObject, GameObject), GameObject> dictionary)
    {
        List<(GameObject, GameObject)> keys = new List<(GameObject, GameObject)>();
        foreach (var pair in dictionary)
        {
            if (pair.Value == value)
            {
                keys.Add(pair.Key);
            }
        }
        return keys;
    }
}
