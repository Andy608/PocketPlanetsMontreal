using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject trajectoryObject;

    private float WIDTH = 64.0f;

    private LineRenderer trajectoryLineRenderer;
    private List<Vector2> vertices = new List<Vector2>();

    private float vertexSpacing = 1.0f;
    private float lineDistance = 0.0f;

    private bool isAbsorbed = false;

    private Planet currentPlanet;

    private void OnEnable()
    {
        Managers.EventManager.OnPlanetTheoreticallyAbsorbed += StopTrail;

        currentPlanet = GetComponent<Planet>();
        trajectoryLineRenderer = trajectoryObject.GetComponent<LineRenderer>();

        trajectoryLineRenderer.startWidth = WIDTH;
        trajectoryLineRenderer.endWidth = WIDTH;

        trajectoryLineRenderer.startColor = Color.Lerp(currentPlanet.CurrentColor, Managers.DisplayManager.Instance.BackgroundColor, 0.2f);
        trajectoryLineRenderer.endColor = Color.Lerp(currentPlanet.CurrentColor, Managers.DisplayManager.Instance.BackgroundColor, 0.8f);

        Hide();
    }

    private void OnDisable()
    {
        Managers.EventManager.OnPlanetTheoreticallyAbsorbed -= StopTrail;
    }

    public void Show()
    {
        Managers.GameStateManager.Instance.RequestPause();
        //Debug.Log("SHOW");
        //Initialize the trajectory.
        trajectoryLineRenderer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        //Debug.Log("SET TO UN PAUSE BITCH");
        Managers.GameStateManager.Instance.RequestUnpause();
        //Clear the trajectory.
        ClearTrajectory();
        trajectoryLineRenderer.gameObject.SetActive(false);
    }

    //public void InitTrajectory(Vector2 startingVelocity)
    //{
    //    List<Planet> activePlanets = Managers.WorldPlanetTrackingManager.Instance.PlanetsInWorld;
    //    CreateTrajectory(activePlanets, startingVelocity, offsetTime);

    //    if (vertices.Count > 1)
    //    {
    //        //Say the trail is 100 pix long
    //        //texture is 64 pix
    //        //scale should be 64 / 100

    //        float xScale = lineDistance / (WIDTH * Managers.DisplayManager.TARGET_SCREEN_DENSITY / 1.2f);
    //        trajectoryLineRenderer.material.SetTextureScale("_MainTex", new Vector2(xScale, 1.0f));
    //    }
    //}

    public void ClearTrajectory()
    {
        //Debug.Log("CLEAR");
        vertices.Clear();
        lineDistance = 0.0f;
        trajectoryLineRenderer.positionCount = 0;
    }

    //private void CreateTrajectory(List<Planet> planetList, Vector2 startingVelocity, float offsetTime)
    //{
    //    float iterTime = 0;
    //    float offsetTimeFactor = 1.0f;
    //    Vector2 position = currentPlanet.PhysicsIntegrator.Position + (startingVelocity.normalized * currentPlanet.Circumference);
    //    Vector2 velocity = startingVelocity;
    //    Vector2 frameAcceleration = Vector2.zero;

    //    if (offsetTime == 0)
    //    {
    //        return;
    //    }
    //    else if (offsetTime < 0)
    //    {
    //        offsetTimeFactor = -1.0f;
    //        offsetTime *= -1.0f;
    //    }

    //    if (vertices.Count == 0)
    //    {
    //        AddVertex(position);
    //    }

    //    while (iterTime < offsetTime)
    //    {
    //        foreach (Planet otherPlanet in planetList)
    //        {
    //            if (otherPlanet == currentPlanet) continue;

    //            Vector2 direction = otherPlanet.PhysicsIntegrator.Position - position;
    //            float distanceSquared = direction.sqrMagnitude;

    //            float distanceFromCenter = (currentPlanet.Radius + otherPlanet.Radius);

    //            if (direction.sqrMagnitude > distanceFromCenter * distanceFromCenter)
    //            {
    //                float gravitationMag = (Managers.GravityManager.G * otherPlanet.CurrentMass / distanceSquared);
    //                Vector2 gravitationalForce = direction.normalized * gravitationMag;
    //                frameAcceleration += gravitationalForce;
    //            }
    //            else
    //            {
    //                //We got absorbed, so we should stop drawing the line.
    //                return;
    //            }
    //        }

    //        velocity = startingVelocity * offsetTimeFactor + (frameAcceleration * Time.fixedDeltaTime);
    //        position = position + velocity * Time.fixedDeltaTime;

    //        //Check to add vertex in list.

    //        if (Vector2.Distance(vertices.Last(), position) > vertexSpacing)
    //        {
    //            lineDistance += (vertices.Last() - position).magnitude;
    //            AddVertex(position);
    //        }

    //        iterTime += Time.fixedDeltaTime;
    //    }
    //}

    private void StopTrail(Planet absorber, Planet absorbed)
    {
        if (absorbed == currentPlanet)
        {
            isAbsorbed = true;
        }
    }

    public void CreateTrajectory(Vector2 initialVelocity, float offsetTime)
    {
        isAbsorbed = false;

        if (gameObject.GetComponent<Planet>().PlanetProperties.IsAnchor)
        {
            return;
        }

        float counter = 0.0f;
        //Debug.Log("Before sync: " + currentPlanet.PhysicsIntegrator.prevVelocity + " pTV: " + currentPlanet.PhysicsIntegrator.prevTheoreticalVelocity);
        Managers.PhysicsIntegrationManager.Instance.SyncTheoretical();
        //Debug.Log("After sync: " + currentPlanet.PhysicsIntegrator.prevVelocity + " pTV: " + currentPlanet.PhysicsIntegrator.prevTheoreticalVelocity);

        Vector2 originalPosition = currentPlanet.PhysicsIntegrator.TheoreticalPosition;
        Vector2 position/* = currentPlanet.PhysicsIntegrator.TheoreticalPosition + (initialVelocity.normalized * currentPlanet.Circumference)*/;

        while (counter < offsetTime)
        {
            if (isAbsorbed)
            {
                return;
            }

            //Debug.Log("Counter: " + counter + " | Offset Time: " + offsetTime);

            Managers.PhysicsIntegrationManager.Instance.IntegrateTheoretical();
            position = currentPlanet.PhysicsIntegrator.TheoreticalPosition;

            if ((originalPosition - position).sqrMagnitude > currentPlanet.Circumference * currentPlanet.Circumference)
            {
                if (vertices.Count == 0)
                {
                    AddVertex(position);
                }
                else if (Vector2.Distance(vertices.Last(), position) > vertexSpacing)
                {
                    lineDistance += (vertices.Last() - position).magnitude;
                    AddVertex(position);
                    //Debug.Log("ADDING VERTEX");
                }
            }

            counter += Time.fixedDeltaTime;
        }

        if (vertices.Count > 1)
        {
            float xScale = lineDistance / (WIDTH * Managers.DisplayManager.TARGET_SCREEN_DENSITY / 1.2f);
            trajectoryLineRenderer.material.SetTextureScale("_MainTex", new Vector2(xScale, 1.0f));
        }
    }

    private void AddVertex(Vector2 position)
    {
        vertices.Add(position);
        trajectoryLineRenderer.positionCount = vertices.Count;

        trajectoryLineRenderer.SetPosition(vertices.Count - 1, position);
    }
}
