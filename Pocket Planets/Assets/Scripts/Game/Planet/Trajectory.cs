using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject trajectoryObject;

    //Amount of time to look ahead.
    [SerializeField] private float offsetTime = 1.0f;
    private float WIDTH = 64.0f;

    private LineRenderer trajectoryLineRenderer;
    private List<Vector2> vertices = new List<Vector2>();

    private float vertexSpacing = 1.0f;
    private float lineDistance = 0.0f;

    private Planet currentPlanet;

    private void OnEnable()
    {
        Managers.EventManager.OnDragBegan += UpdateStartingVelocity;
        Managers.EventManager.OnDragHeld += UpdateStartingVelocity;
        Managers.EventManager.OnDragEnded += UpdateStartingVelocity;

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
        Managers.EventManager.OnDragBegan -= UpdateStartingVelocity;
        Managers.EventManager.OnDragHeld -= UpdateStartingVelocity;
        Managers.EventManager.OnDragEnded -= UpdateStartingVelocity;
    }

    public void Show()
    {
        //Managers.GameState.Instance.RequestPause();
        //Debug.Log("SHOW");
        //Initialize the trajectory.
        trajectoryLineRenderer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        //Managers.GameState.Instance.RequestUnpause();
        //Clear the trajectory.
        ClearTrajectory();
        trajectoryLineRenderer.gameObject.SetActive(false);
    }

    private void UpdateStartingVelocity(Touch touch)
    {
        Vector3 worldTouchPos = Vector3.zero;
        Managers.DisplayManager.TouchPositionToWorldVector3(touch, ref worldTouchPos);

        Vector2 worldPos = worldTouchPos;

        ClearTrajectory();
        InitTrajectory(currentPlanet.PhysicsIntegrator.InitialVelocity);
    }

    private void InitTrajectory(Vector2 startingVelocity)
    {
        List<Planet> activePlanets = Managers.WorldPlanetTrackingManager.Instance.PlanetsInWorld;

        CreateTrajectory(activePlanets, startingVelocity, offsetTime);

        if (vertices.Count > 1)
        {
            //Say the trail is 100 pix long
            //texture is 64 pix
            //scale should be 64 / 100

            float xScale = lineDistance / (WIDTH * Managers.DisplayManager.TARGET_SCREEN_DENSITY / 1.2f);
            trajectoryLineRenderer.material.SetTextureScale("_MainTex", new Vector2(xScale, 1.0f));
        }
    }

    private void ClearTrajectory()
    {
        vertices.Clear();
        lineDistance = 0.0f;
        trajectoryLineRenderer.positionCount = 0;
    }

    private void CreateTrajectory(List<Planet> planetList, Vector2 startingVelocity, float offsetTime)
    {
        float iterTime = 0;
        float offsetTimeFactor = 1.0f;
        Vector2 position = currentPlanet.PhysicsIntegrator.Position + (startingVelocity.normalized * currentPlanet.Circumference);
        Vector2 velocity = startingVelocity;
        Vector2 frameAcceleration = Vector2.zero;

        if (offsetTime == 0)
        {
            return;
        }
        else if (offsetTime < 0)
        {
            offsetTimeFactor = -1.0f;
            offsetTime *= -1.0f;
        }

        if (vertices.Count == 0)
        {
            AddVertex(position);
        }

        while (iterTime < offsetTime)
        {
            foreach (Planet otherPlanet in planetList)
            {
                if (otherPlanet == currentPlanet) continue;

                Vector2 direction = otherPlanet.PhysicsIntegrator.Position - position;
                float distanceSquared = direction.sqrMagnitude;

                float distanceFromCenter = (currentPlanet.Radius + otherPlanet.Radius);

                if (direction.sqrMagnitude > distanceFromCenter * distanceFromCenter)
                {
                    float gravitationMag = (Managers.GravityManager.G * otherPlanet.CurrentMass / distanceSquared);
                    Vector2 gravitationalForce = direction.normalized * gravitationMag;
                    frameAcceleration += gravitationalForce;
                }
                else
                {
                    //We got absorbed, so we should stop drawing the line.
                    return;
                }
            }

            velocity = startingVelocity * offsetTimeFactor + (frameAcceleration * Time.fixedDeltaTime);
            position = position + velocity * Time.fixedDeltaTime;

            //Check to add vertex in list.

            if (Vector2.Distance(vertices.Last(), position) > vertexSpacing)
            {
                lineDistance += (vertices.Last() - position).magnitude;
                AddVertex(position);
            }

            iterTime += Time.fixedDeltaTime;
        }
    }

    private void AddVertex(Vector2 position)
    {
        vertices.Add(position);
        trajectoryLineRenderer.positionCount = vertices.Count;

        trajectoryLineRenderer.SetPosition(vertices.Count - 1, position);
    }
}
