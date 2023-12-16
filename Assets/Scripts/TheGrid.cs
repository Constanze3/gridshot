using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheGrid : MonoBehaviour
{
    public Target targetPrefab;
    public int width;
    public int height;
    public int spacing;
    public int targetCount;

    private class TargetData
    {
        public Vector2Int position;
        public Target target;

        public TargetData(Vector2Int position, Target target)
        {
            this.position = position;
            this.target = target;
        }
    }

    private readonly List<TargetData> targets = new();

    // Note: positions are indexed from bottom-left to top-right
    private readonly List<Vector2Int> allPositions = new();


    private void Start()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allPositions.Add(new Vector2Int(i, j));
            }
        }

        for (int i = 0; i < targetCount; i++)
        {
            InstantiateRandomTarget();
        }
    }

    /// <summary>
    /// Instantiates a target at a random <b>unoccupied</b> grid position.
    /// </summary>
    private void InstantiateRandomTarget()
    {
        InstantiateTarget(RandomPosition());
    }

    /// <summary>
    /// Instantiates a target at the specified grid position.
    /// </summary>
    /// <param name="position">
    /// a position in the grid
    /// </param>
    private void InstantiateTarget(Vector2Int position)
    {
        Vector3 realPosition = TranslatePosition(position);
        Target target = Instantiate(targetPrefab, realPosition, Quaternion.identity);
        target.transform.SetParent(transform);
        targets.Add(new TargetData(position, target));

        target.OnDestroy += OnTargetDestroyed;
    }

    /// <summary>
    /// Whenever a target created by the InstantiateTarget() method is destroyed this will trigger.
    /// </summary>
    /// <param name="target">
    /// the target that was destroyed
    /// </param>
    private void OnTargetDestroyed(Target target)
    {
        targets.RemoveAll(td => td.target == target);
        InstantiateRandomTarget();
    }

    private class NoUnoccupiedPositionException : Exception { }

    /// <summary>
    /// Finds a random <b>unoccupied</b> position in the grid.
    /// </summary>
    /// <returns>
    /// The determined position.
    /// </returns>
    private Vector2Int RandomPosition()
    {
        List<Vector2Int> emptyPositions = allPositions.Where(p => !targets.Any(td => td.position.Equals(p))).ToList();
        if (emptyPositions.Count == 0)
        {
            throw new NoUnoccupiedPositionException();
        }
        int randomIndex = UnityEngine.Random.Range(0, emptyPositions.Count);
        return emptyPositions[randomIndex];
    }

    /// <summary>
    /// Translates a position in the grid into <b>world position</b>.
    /// </summary>
    /// <param name="position">a position in the grid</param>
    /// <returns>
    /// The translated position.
    /// </returns>
    private Vector3 TranslatePosition(Vector2Int position)
    {
        Vector3 bottomLeftCorner = GetBottomLeftCorner();

        Vector3 rightOffset = position.x * spacing * transform.right;
        Vector3 upOffset = position.y * spacing * transform.up;

        return bottomLeftCorner + rightOffset + upOffset;
    }

    /// <summary>
    /// Returns the world position of the bottom left corner of the grid.
    /// </summary>
    /// <returns>
    /// The position of the bottom left corner.
    /// </returns>
    private Vector3 GetBottomLeftCorner()
    {
        Vector3 leftOffset = (width / 2) * spacing * (-transform.right);
        Vector3 downOffset = (height / 2) * spacing * (-transform.up);

        return transform.position + leftOffset + downOffset;
    }
}