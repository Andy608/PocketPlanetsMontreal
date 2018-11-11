using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumOrbitalType
{
    CHILD,
    PARENT
}

public class OrbitTracker : MonoBehaviour
{
    private Planet currentPlanet;

    private List<OrbitData> childList = new List<OrbitData>();
    private List<OrbitData> parentList = new List<OrbitData>();

    public List<OrbitData> ChildList { get { return childList; } }
    public List<OrbitData> ParentList { get { return parentList; } }

    public void OnEnable()
    {
        currentPlanet = GetComponent<Planet>();

        Managers.EventManager.OnPlanetEnteredGravitationalPull += ShouldTrackOrbit;
        Managers.EventManager.OnPlanetExitedGravitationalPull += UntrackFromGravitationalPull;
        Managers.EventManager.OnPlanetAbsorbed += UntrackFromAbsorption;
        Managers.EventManager.OnPlanetDestroyed += UntrackFromDeletion;
    }

    public void OnDisable()
    {
        Managers.EventManager.OnPlanetEnteredGravitationalPull -= ShouldTrackOrbit;
        Managers.EventManager.OnPlanetExitedGravitationalPull -= UntrackFromGravitationalPull;
        Managers.EventManager.OnPlanetAbsorbed -= UntrackFromAbsorption;
        Managers.EventManager.OnPlanetDestroyed -= UntrackFromDeletion;
    }

    private void ShouldTrackOrbit(Planet firstPlanet, Planet secondPlanet)
    {
        if (firstPlanet != null && secondPlanet != null)
        {
            if (firstPlanet == currentPlanet)
            {
                if (firstPlanet.CurrentMass > secondPlanet.CurrentMass)
                {
                    TrackChildOrbit(secondPlanet);
                }
                else if (firstPlanet.CurrentMass == secondPlanet.CurrentMass &&
                    !secondPlanet.GetComponent<OrbitTracker>().IsTrackingChildOrbit(firstPlanet))
                {
                    TrackChildOrbit(secondPlanet);
                }
                else
                {
                    TrackParentOrbit(secondPlanet);
                }
            }
        }
    }

    private void UntrackFromAbsorption(Planet absorber, Planet absorbed)
    {
        UntrackOrbitRelationship(absorbed);
        UntrackOrbitRelationship(absorber);

        if (absorber != currentPlanet)
        {
            if (absorber.CurrentMass > currentPlanet.CurrentMass)
            {
                TrackParentOrbit(absorber);
            }
            else
            {
                TrackChildOrbit(absorber);
            }
        }
    }

    private void UntrackFromDeletion(Planet deletedPlanet)
    {
        UntrackOrbitRelationship(deletedPlanet);
    }

    private void UntrackFromGravitationalPull(Planet parent, Planet child)
    {
        if (parent == currentPlanet)
        {
            UntrackOrbitRelationship(child);
        }
        else if (child == currentPlanet)
        {
            UntrackOrbitRelationship(parent);
        }
    }

    private void TrackParentOrbit(Planet parent)
    {
        int i = 0;
        for (; i < parentList.Count; ++i)
        {
            if (parentList[i].OrbitParent.GetInstanceID() == parent.GetInstanceID())
            {
                //Already tracking orbit.
                return;
            }
        }

        OrbitData data = new OrbitData(parent, currentPlanet);
        parentList.Add(data);
    }

    private void TrackChildOrbit(Planet child)
    {
        int i = 0;
        for (; i < childList.Count; ++i)
        {
            if (childList[i].OrbitChild.GetInstanceID() == child.GetInstanceID())
            {
                //Already tracking orbit.
                return;
            }
        }

        OrbitData data = new OrbitData(currentPlanet, child);
        childList.Add(data);
    }

    private void UntrackParentOrbit(Planet parent)
    {
        int i = 0;
        for (; i < parentList.Count; ++i)
        {
            if (parentList[i].OrbitParent.GetInstanceID() == parent.GetInstanceID())
            {
                parentList.RemoveAt(i);
                break;
            }
        }
    }

    private void UntrackChildOrbit(Planet child)
    {
        int i = 0;
        for (; i < childList.Count; ++i)
        {
            if (childList[i].OrbitChild.GetInstanceID() == child.GetInstanceID())
            {
                childList.RemoveAt(i);
                break;
            }
        }
    }

    private void UntrackOrbitRelationship(Planet another)
    {
        UntrackParentOrbit(another);
        UntrackChildOrbit(another);
    }

    private bool IsTrackingChildOrbit(Planet child)
    {
        int i = 0;
        for (; i < childList.Count; ++i)
        {
            if (childList[i].OrbitChild.GetInstanceID() == child.GetInstanceID())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTrackingParentOrbit(Planet parent)
    {
        int i = 0;
        for (; i < parentList.Count; ++i)
        {
            if (parentList[i].OrbitParent.GetInstanceID() == parent.GetInstanceID())
            {
                return true;
            }
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (Managers.GameStateManager.Instance.IsState(Managers.GameStateManager.EnumGameState.RUNNING))
        {
            for (int i = 0; i < childList.Count; ++i)
            {
                OrbitData currentData = childList[i];

                if (currentData.ChildTransform == null || currentData.ParentTransform == null)
                {
                    childList.Remove(currentData);
                    --i;
                }
                else
                {
                    currentData.UpdateOrbit();
                }
            }
        }
    }
}
