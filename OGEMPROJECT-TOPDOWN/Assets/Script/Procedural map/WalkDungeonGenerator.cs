using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] private int iterations = 10;
    [SerializeField] public int walkLength = 10;
    [SerializeField] public bool startRandomlyEachIteration = true;
    
    protected override void RunProceduralGeneration()
    {
        tilemapVisualizer.Clear();

        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(startPosition, walkLength);
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }

}
