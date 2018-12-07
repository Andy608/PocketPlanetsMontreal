﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsIntegrator : MonoBehaviour
{
    private Vector2 prevPosition = new Vector2();
    public Vector2 prevVelocity = new Vector2();
    private Vector2 prevAcceleration = new Vector2();

    private Vector2 position = new Vector2();
    private Vector2 velocity = new Vector2();
    private Vector2 acceleration = new Vector2();

    ///////////////////////////////////////////////////////

    private Vector2 prevTheoreticalPosition = new Vector2();
    public Vector2 prevTheoreticalVelocity = new Vector2();
    private Vector2 prevTheoreticalAcceleration = new Vector2();

    private Vector2 theoreticalPosition = new Vector2();
    private Vector2 theoreticalVelocity = new Vector2();
    private Vector2 theoreticalAcceleration = new Vector2();

    [SerializeField] private Vector2 initialVelocity = new Vector2();
    [SerializeField] private Vector2 initialAcceleration = new Vector2();

    private Transform objectTransform;

    ///////////////////////////////////////////////////////

    public Vector2 Acceleration { get { return acceleration; } }
    public Vector2 Velocity { get { return velocity; } }
    public Vector2 Position { get { return position; } }

    public Vector2 InitialVelocity {
        get { return initialVelocity; }
        set
        {
            initialVelocity = value;
            prevVelocity = initialVelocity;
            prevTheoreticalVelocity = initialVelocity;
        }
    }

    public Vector2 TheoreticalPosition { get { return theoreticalPosition; } }
    public Vector2 TheoreticalVelocity { get { return theoreticalVelocity; } }


    private void OnEnable()
    {
        objectTransform = transform;

        prevPosition = objectTransform.position;
        AddAcceleration(initialAcceleration);

        position = objectTransform.position;

        SyncTheoreticalData();
        //Debug.Log("Initing position data: " + position + " ID: " + GetInstanceID());
    }

    public void Integrate()
    {
        //if (!gameObject.GetComponent<Planet>().PlanetProperties.IsAnchor)
        //{
        //    Debug.Log("Integrate P Vel: " + prevVelocity + " | Vel: " + velocity);
        //}

        velocity = prevVelocity + acceleration * Time.fixedDeltaTime;
        position = prevPosition + velocity * Time.fixedDeltaTime;

        prevAcceleration = acceleration;
        prevVelocity = velocity;
        prevPosition = position;

        acceleration = Vector2.zero;

        objectTransform.position = position;
    }

    public void IntegrateTheoretical()
    {
        theoreticalVelocity = prevTheoreticalVelocity + theoreticalAcceleration * Time.fixedDeltaTime;
        theoreticalPosition = prevTheoreticalPosition + theoreticalVelocity * Time.fixedDeltaTime;

        prevTheoreticalAcceleration = theoreticalAcceleration;
        prevTheoreticalVelocity = theoreticalVelocity;
        prevTheoreticalPosition = theoreticalPosition;

        theoreticalAcceleration = Vector2.zero;

        //objectTransform.position = theoreticalPosition;
    }

    public void SyncTheoreticalData()
    {
        prevTheoreticalAcceleration = prevAcceleration;
        prevTheoreticalVelocity = prevVelocity;
        prevTheoreticalPosition = prevPosition;

        //if (!gameObject.GetComponent<Planet>().PlanetProperties.IsAnchor)
        //{
        //    Debug.Log("SYNC P VEL: " + prevVelocity);
        //}

        theoreticalAcceleration = acceleration;
        theoreticalVelocity = velocity;
        theoreticalPosition = position;
    }

    public void AddAcceleration(Vector2 acc)
    {
        acceleration += acc;
    }

    public void AddTheoreticalAcceleration(Vector2 acc)
    {
        theoreticalAcceleration += acc;
    }

    public void SetPhysicsIntegrator(PhysicsIntegrator otherPlanet)
    {
        prevAcceleration = otherPlanet.prevAcceleration;
        acceleration = otherPlanet.acceleration;

        prevVelocity = otherPlanet.prevVelocity;
        velocity = otherPlanet.velocity;

        prevPosition = otherPlanet.prevPosition;
        position = otherPlanet.position;

        initialVelocity = otherPlanet.initialVelocity;
        initialAcceleration = otherPlanet.initialAcceleration;

        prevTheoreticalAcceleration = otherPlanet.prevTheoreticalAcceleration;
        theoreticalAcceleration = otherPlanet.theoreticalAcceleration;

        prevTheoreticalVelocity = otherPlanet.prevTheoreticalVelocity;
        theoreticalVelocity = otherPlanet.theoreticalVelocity;

        prevTheoreticalPosition = otherPlanet.prevTheoreticalPosition;
        theoreticalPosition = otherPlanet.theoreticalPosition;
    }
}