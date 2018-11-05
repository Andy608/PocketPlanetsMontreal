using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitData
{
    //Used in-editor
    private static readonly List<Color> USED_COLORS = new List<Color>();
    public readonly Color DEBUG_COLOR = GenerateRandomColor();
    /////////////////

    private static readonly float FULL_ORBIT = 360.0f;

    private Planet orbitParent;
    private Planet orbitChild;

    private Transform parentTransform;
    private Transform childTransform;

    private Vector2 startDirection;
    private Vector2 currentDirection;
    private Vector2 prevDirection;

    private float duration;
    private float deltaDuration;

    private int orbitCount;

    public Planet OrbitChild { get { return orbitChild; } }
    public Planet OrbitParent { get { return orbitParent; } }
    public Transform ParentTransform { get { return parentTransform; } }
    public Transform ChildTransform { get { return childTransform; } }
    public Vector2 StartDirection { get { return startDirection; } }
    public Vector2 CurrentDirection { get { return currentDirection; } }
    public float Duration { get { return duration; } }
    public int OrbitCount { get { return orbitCount; } }

    public Planet GetOrbitPlanet(EnumOrbitalType type)
    {
        if (type == EnumOrbitalType.CHILD)
        {
            return OrbitChild;
        }
        else
        {
            return OrbitParent;
        }
    }

    public OrbitData(Planet parent, Planet child)
    {
        orbitParent = parent;
        orbitChild = child;

        parentTransform = orbitParent.transform;
        childTransform = orbitChild.transform;

        startDirection = childTransform.position - parentTransform.position;

        prevDirection = startDirection;
        currentDirection = startDirection;

        duration = 0.0f;
    }

    public void UpdateOrbit()
    {
        deltaDuration = Vector2.Angle(prevDirection, currentDirection);

        prevDirection = currentDirection;
        currentDirection = childTransform.position - parentTransform.position;

        duration += deltaDuration;

        if (duration <= -FULL_ORBIT || duration >= FULL_ORBIT)
        {
            duration = 0.0f;

            Debug.Log("ORBIT!");
            ++orbitCount;

            if (Managers.EventManager.OnOrbitOccurred != null)
            {
                Managers.EventManager.OnOrbitOccurred(this);
            }
        }
    }

    //Used for in-editor purposes.
    private static Color GenerateRandomColor()
    {
        Color c;

        do
        {
            c = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0, 1.0f));

        } while (USED_COLORS.Contains(c));

        return c;
    }
}
