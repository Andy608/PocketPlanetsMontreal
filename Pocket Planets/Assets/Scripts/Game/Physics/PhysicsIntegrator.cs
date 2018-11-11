using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsIntegrator : MonoBehaviour
{
    private Vector2 prevPosition = new Vector2();
    private Vector2 prevVelocity = new Vector2();
    private Vector2 prevAcceleration = new Vector2();

    private Vector2 position = new Vector2();
    private Vector2 velocity = new Vector2();
    private Vector2 acceleration = new Vector2();

    ///////////////////////////////////////////////////////

    private Vector2 prevTheoreticalPosition = new Vector2();
    private Vector2 prevTheoreticalVelocity = new Vector2();
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

    public Vector2 InitialVelocity { get { return initialVelocity; } set { initialVelocity = value; prevVelocity = initialVelocity; } }

    private void OnEnable()
    {
        objectTransform = transform;

        prevPosition = objectTransform.position;
        prevVelocity = initialVelocity;
        AddAcceleration(initialAcceleration);

        position = objectTransform.position;
    }

    public void Integrate()
    { 
        velocity = prevVelocity + acceleration * Time.fixedDeltaTime;
        position = prevPosition + velocity * Time.fixedDeltaTime;

        prevAcceleration = acceleration;
        prevVelocity = velocity;
        prevPosition = position;

        //Debug.Log("Vel: " + velocity);
        objectTransform.position = position;

        acceleration = Vector2.zero;
    }
    
    public void SyncTheoreticalData()
    {
        prevTheoreticalPosition = prevPosition;
        prevTheoreticalVelocity = prevVelocity;
        prevTheoreticalAcceleration = prevAcceleration;

        theoreticalPosition = position;
        theoreticalVelocity = velocity;
        theoreticalAcceleration = acceleration;
    }
     
    public void IntegrateTheoretical(float deltaTime)
    {
        theoreticalVelocity = prevTheoreticalVelocity + theoreticalAcceleration * deltaTime;
        theoreticalPosition = prevTheoreticalPosition + theoreticalVelocity * deltaTime;
     
        prevTheoreticalAcceleration = theoreticalAcceleration;
        prevTheoreticalVelocity = theoreticalVelocity;
        prevTheoreticalPosition = theoreticalPosition;

        theoreticalAcceleration = Vector2.zero;
    }
    
    public void AddAcceleration(Vector2 acc)
    {
        acceleration += acc;
    }

    public void AddTheoreticalAcceleration(Vector2 acc)
    {
        theoreticalAcceleration += acc;
    }
}
