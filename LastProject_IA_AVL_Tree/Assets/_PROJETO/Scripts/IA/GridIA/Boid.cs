using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [HideInInspector]public New_Node_IA ImediateTarget;

    public float         Velocidade =1;
    public float         Forcas     =1;
    public float         Massa      =1;
    public float dMax = 1.2f;
    public float dMin = 1;
    public float Vizinhanca = 10;
    public float PesoSeparacao     =1;
    public bool  MoveOn        = false;
    //public float PesoAlinhamento   =1;
    public void GoTo(New_Node_IA DestinyNode)
    {
        ImediateTarget = DestinyNode;
        MoveOn = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.7f, 0.7f, 0.3f);
        Gizmos.DrawSphere(this.transform.position, dMax);
        Gizmos.DrawSphere(this.transform.position, dMin);
    }
    private void FixedUpdate()
    {
        if (!MoveOn) { return; }

        Vector3 Desired, Vel, Steering;

        #region MOVIMENTOS
            Desired = (ImediateTarget.transform.position - this.transform.position).normalized*Velocidade;
            Vel = this.transform.forward * Velocidade;
            Steering = Desired - Vel;
        #endregion
        #region CHEGAR LENTAMENTE
            //ajusta os valores para um maximo de velocidade e forcas
            Steering = (Steering/*+Separacao*/* PesoSeparacao) / Massa;
            Steering = Vector3.ClampMagnitude(Steering, Forcas);
            Vel += Steering;
            Vel = Vector3.ClampMagnitude(Vel, Velocidade);
            float Distancia = Vector3.Distance(ImediateTarget.transform.position, this.transform.position);
            if (Distancia <= dMax)
            {
                Vel = Vel * (Distancia - dMin) / (dMax - dMin);
                Chegou();
            }
        #endregion
        #region MOVER O BOID
        //this.transform.LookAt(ImediateTarget.transform.position);
        Vector3 vectortolook = new Vector3(ImediateTarget.transform.position.x, transform.position.y ,ImediateTarget.transform.position.z); 
        this.transform.LookAt(vectortolook);
        this.transform.position += Vel * Time.deltaTime;
        #endregion
    }

    public void Chegou()
    {
        //MoveOn = false; Debug.Log("Chegou!");
        MoveOn = false;
        GetComponent<IA_Agent>().ReplaceNode(ImediateTarget);
    }
}
