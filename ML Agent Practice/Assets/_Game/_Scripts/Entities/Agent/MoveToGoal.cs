using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{
    #region Variáveis
    [Header("Movimentação:")] 
    [SerializeField] private float moveSpeed;

    [Header("Referências:")] 
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private Transform targetTransform;
    #endregion
    
    #region Funções MLAgent
    // Ao iniciar a iteração, reinicie a posição do agente
    public override void OnEpisodeBegin() => transform.localPosition = spawnPointTransform.localPosition;
    
    // Coletando Dados
    public override void CollectObservations(VectorSensor sensor)
    {
        // Registre a posição do Agente
        sensor.AddObservation(transform.localPosition);
        
        // Registre a posição do Alvo
        sensor.AddObservation(targetTransform.localPosition);
    }
    
    // Processando Dados
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Processando direções para alcançar o alvo
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        // Debug Console
        Debug.Log(moveX);
        Debug.Log(moveZ);
        
        // Aplicando movimentação
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed; 
    }
    
    // Testes
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = (int)Input.GetAxisRaw("Horizontal");
        continuousActions[1] = (int)Input.GetAxisRaw("Vertical");
    }
    #endregion

    #region Funções Unity
    // Detecção de Colisão
    private void OnTriggerEnter(Collider other)
    {
        // Ao colidir com o Alvo
        if (other.CompareTag("Target"))
        {
            AddReward(1f); // Aumentar a pontuação da IA
            floorMeshRenderer.material = winMaterial;
        }
        else if (other.CompareTag("DeadEnd")) // Ao colidir com um limite
        {
            AddReward(-1f); // Reduzir a pontuação da IA
            floorMeshRenderer.material = loseMaterial;
        }
        
        EndEpisode(); // Terminar a iteração
    }
    #endregion
}
