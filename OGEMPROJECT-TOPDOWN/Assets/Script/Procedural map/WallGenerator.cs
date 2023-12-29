using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        //this iterates every floor tile then it iterates every direction. It checks if side of the currents floor tile is empty. If so it adds wall tile.
        foreach (var position in floorPositions)    
        {   
            foreach (var direction in directionsList)
            {   
                var neighbourPosition = position + direction; //Next to floor positions.
                if(floorPositions.Contains(neighbourPosition) == false) //Check if the side of the current floor doesn't contain wall.
                    wallPositions.Add(neighbourPosition); //Add wall tile.
            }
        }
        return wallPositions;
    }
}
