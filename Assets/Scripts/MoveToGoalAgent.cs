using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform agentTransform;
    [SerializeField] private Transform goalTransform;

    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;


    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(Random.Range(+1f, +4f), 7.5f, Random.Range(-10f, -13.5f));
        goalTransform.localPosition = new Vector3(Random.Range(+7.54f, +10.33f), 7.309f, Random.Range(-9.27f, -15.64f));
    }


    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(agentTransform.localPosition);

    }


    public override void OnActionReceived(ActionBuffers actions){
       float moveX = actions.ContinuousActions[0];
       float moveZ = actions.ContinuousActions[1];

       float moveSpeed = 5f;

       transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    private void OnTriggerEnter (Collider other){
        if (other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }

}
