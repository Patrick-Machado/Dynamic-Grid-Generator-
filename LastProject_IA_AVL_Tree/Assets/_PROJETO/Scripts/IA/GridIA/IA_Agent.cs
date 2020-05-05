using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Agent : MonoBehaviour
{
    [Header("Agent Info")]
    public Vector2 PlayerStartNode = new Vector2(0,0);// onde o player starta no grafo

    Vector2 WichNodeIm;// coordenatas de onde se está no grafo/matriz

    float [,] grid_Matrix_copy;// cópia da grid matrix do gerador de grafo porém em formato float

    public List<Edge_IA> Path = new List<Edge_IA>();// salva o caminho do agente
    [Header("References")]
    public New_Node_IA Target;//destino final node
    public New_Node_IA LocalNodePos;//posição local do node
    public NodeCatcher StarTargetCatcher;//catador de node do target
    public NodeCatcher LocalCatcher;//catador de node local
    private Boid myBoid;
    public bool NPC;

    public AVL tree = new AVL();
    public bool TintEdges = true;

    private void Awake()
    {
        myBoid = GetComponent<Boid>();
        LocalNodePos = CheckForNode();

    }

    void FixedUpdate()
    {
        if(StarTargetCatcher.CurrentNode!=null) Target = StarTargetCatcher.CurrentNode;// seta posição do target pro node apontado pela estrela
        if(LocalCatcher.CurrentNode != null) LocalNodePos = LocalCatcher.CurrentNode;// seta o Node atual pro node referente ao componente NodeCatcher

        if (Input.GetKeyDown(KeyCode.Space))//apenas para teste REMOVER na build FINAL
        {
            MoveAgent();
        }
    }

    public void MoveAgent()
    {
        Edge_IA e = CalcMinorPath();
        New_Node_IA n = e.OtherPeerNode(LocalNodePos);
        myBoid.GoTo(n);
    }

    public New_Node_IA CheckForNode()
    {
        RaycastHit ray;
        if (Physics.Raycast(this.transform.position, Vector3.down, out ray, 2))
        {
            if (ray.transform.gameObject.tag == "Node")
            {
                return ray.transform.gameObject.GetComponent<New_Node_IA>();
            }
        }

        return null;
    }

    public void MoveAgent(New_Node_IA myTarget)
    {
        Target = myTarget;
        Edge_IA e = CalcMinorPath();
        New_Node_IA n = e.OtherPeerNode(LocalNodePos);
        myBoid.GoTo(n);
    }

    public void ReplaceNode(New_Node_IA ReceivedNode)//recebe troca de nodes vindas do boid pra ir pro próximo node
    {
        LocalNodePos = ReceivedNode;
        if (LocalNodePos != Target)
        {
            MoveAgent();
        }
    }
    public void tintEdge(Edge_IA e)// colore o edge de 
    {
        
        if (e == null) { Debug.Log("ERROR_On_tintEdge()"); return; }//se for nullo não pinta o edge
        //colore o edge:
        e.GetComponent<MeshRenderer>().material =
                    this.gameObject.GetComponent<MeshRenderer>().material;
    }
    Edge_IA CalcMinorPath()// retorna a direção do edge cuja proximidade baseada na heuristica + pese é menor do target
    {
        if(LocalNodePos.EdgesColection().Count==0) { Debug.Log("ERROR_On_CalcMinorPath()"); return null;  }

        List<Edge_IA> edges_in_node = LocalNodePos.EdgesColection();
        float minor_value = edges_in_node[0].Weight + CalcHeuristic(Target.transform.position, edges_in_node[0].OtherPeerNode(LocalNodePos));
        Edge_IA Edge_to_move = edges_in_node[0];

        foreach (Edge_IA e in edges_in_node)//atualiza minor value com o menor valor de edge_weigths
        {
            float w = e.Weight;
            float h = CalcHeuristic(Target.transform.position, e.OtherPeerNode(LocalNodePos));
            if ( (w+h) < minor_value)
            {
                minor_value    = (w+h);
                Edge_to_move = e;
            }
        }
        ValidateEdgeAVL(Edge_to_move);
        if (TintEdges) { tintEdge(Edge_to_move); }
        return Edge_to_move;
    }

    float CalcHeuristic(Vector3 targetPos, New_Node_IA destinyNode)
    {
        return Vector3.Distance(targetPos, destinyNode.transform.position);
    }

    void ValidateEdgeAVL(Edge_IA e)
    {
        AVL.Node_AVL_Tree NAT = tree.Find(e.GetEdgeAvlID());
        if (NAT == null)
        {
            e.Weight += 1;
            tree.Add(e);
        }
        else {
            NAT.Edge_From_Grid.Weight += 1;
        }
    }

}
