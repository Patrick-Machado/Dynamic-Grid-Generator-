using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Node_IA : MonoBehaviour
{
    public bool imReachable = true;
    public Vector2 ID = new Vector2();

    //public int AVL_Leaf_ID = 0;

    public Edge_IA OesteEdge, LesteEdge, NorteEdge,
                      NE_Edge, NO_Edge, SulEdge, SE_Edge, SO_Edge;
    public New_Node_IA OesteNode, LesteNode, NorteNode,
                   NE_Node, NO_Node, SulNode, SE_Node, SO_Node;

    [HideInInspector]public Grid_Generator grid_Generator;

    bool generated_edges = false;
    public void Init(New_Node_IA[,] matrix, int pos_i, int pos_j, int max_x, int max_y, Grid_Generator grid_Generator_received)
    {
        grid_Generator = grid_Generator_received;

        if (CheckColideableObjectsAround())//checa se tem objetos de colisão por volta do node se tiver invalida o node e não executa o código abaixo
        {
            InvalidateNode();
            return;
        }
        

        if (NorteNode == null)
        {//norte
            if (pos_j < max_y - 1)
            {
                if (matrix[pos_i, pos_j + 1] != null)//NorteNode
                {
                    NorteNode = matrix[pos_i, pos_j + 1];
                }
            }
        }
        if (SulNode == null)//sul
        {
            if (pos_j < max_y && pos_j != 0)
            {
                if (matrix[pos_i, pos_j - 1] != null)//SulNode
                {
                    SulNode = matrix[pos_i, pos_j - 1];
                }
            }
        }
        if (LesteNode == null)//leste
        {
            if (pos_i < max_x - 1)
            {
                if (matrix[pos_i + 1, pos_j] != null)//LesteNode
                {
                    LesteNode = matrix[pos_i + 1, pos_j];
                }
            }
        }
        if (OesteNode == null)//oeste
        {
            if (pos_i < max_x && pos_i != 0)
            {
                if (matrix[pos_i - 1, pos_j] != null)//OesteNode
                {
                    OesteNode = matrix[pos_i - 1, pos_j];
                }
            }
        }
        if (NO_Node == null)
        {
            if (pos_i < max_x && pos_j < max_y -1 && /* Está no limite da matriz? */
                pos_i > 0 /*&& pos_j > 0*/) /* Está na coluna inicial da matriz? */
            {
                if (matrix[pos_i - 1, pos_j + 1] != null)//NoroesteNode
                {
                    NO_Node = matrix[pos_i - 1, pos_j + 1];
                }
            }
        }
        if (NE_Node == null)
        {
            if (pos_i < max_x - 1 && pos_j < max_y - 1)
            {
                if (matrix[pos_i + 1, pos_j + 1] != null)//NordesteNode
                {
                    NE_Node = matrix[pos_i + 1, pos_j + 1];
                }
            }
        }
        if (SE_Node == null)//editing
        {
            if (pos_i < max_x -1 && pos_j < max_y && /* Está no limite da matriz? */
                /*pos_i != 0 &&*/ pos_j != 0)/* é o primeiro node? */
            {
                if (matrix[pos_i + 1, pos_j - 1] != null)//SudesteNode
                {
                    SE_Node = matrix[pos_i + 1, pos_j - 1];
                }
            }
        }
        if (SO_Node == null)
        {
            if (pos_i < max_x && pos_j < max_y &&/* Está no limite da matriz? */
                pos_i != 0 && pos_j != 0)/* é o primeiro node? */
            {
                if (matrix[pos_i - 1, pos_j - 1] != null)//SudoesteNode
                {
                    SO_Node = matrix[pos_i - 1, pos_j - 1];
                }
            }
        }
    }

    private void Awake()
    {
        if (!imReachable)
        {
            gameObject.SetActive(false);
        }
        //SetAVL_Leaf_ID();
    }
    /*public void SetAVL_Leaf_ID() //erro do nó referente a arvore avl (n precisava armazenar o nó e sim os edges)
    {
        string x = (ID.x).ToString(); string y = (ID.y).ToString();
        string z = x + y;
        AVL_Leaf_ID = int.Parse(z);
    }*/
    bool CheckColideableObjectsAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grid_Generator.CollisionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Dontwalk")
            {
                return true;
            }
            i++;
        }
        return false;
    }

    private void OnDrawGizmosSelected()//desenha a esfera vermelha em volta do node (visualização apenas)
    {
        if(imReachable) Gizmos.color = Color.blue;
        else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, grid_Generator.CollisionRadius);
    }

    void InvalidateNode()//invalida os edges do node, chamado quando  se é verificado a distancia do nó de um objeto com tag Dontwalk
    {
        GetComponent<Renderer>().material = grid_Generator.Dontwalk;
        imReachable = false;
        Invoke("UnactiveEdges", 0.01f);
    }

    public void GenerateEdges()// gera os edges apartir dos nodes vizinhos (PS: esse método só pode ser chamado após todos nodes já estarem criados)
    {
        if (NorteNode != null && NorteNode.imReachable && imReachable && NorteNode.SulEdge   == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, NorteNode.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; NorteEdge = aux_e.GetComponent<Edge_IA>(); NorteNode.SulEdge = aux_e.GetComponent<Edge_IA>(); NorteEdge.Init(this, NorteNode);               /*NorteEdge.EdgeDirection = 8;*/ }
        if (NE_Node   != null && NE_Node.imReachable   && imReachable && NE_Node.SO_Edge     == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, NE_Node.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; NE_Edge = aux_e.GetComponent<Edge_IA>(); NE_Node.SO_Edge = aux_e.GetComponent<Edge_IA>(); NE_Edge.Init(this, NE_Node); NE_Edge.Weight = 1.4f ; /*NE_Edge.EdgeDirection   = 9;*/ }
        if (LesteNode != null && LesteNode.imReachable && imReachable && LesteNode.OesteEdge == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, LesteNode.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; LesteEdge = aux_e.GetComponent<Edge_IA>(); LesteNode.OesteEdge = aux_e.GetComponent<Edge_IA>(); LesteEdge.Init(this, LesteNode);             /*LesteEdge.EdgeDirection = 6;*/ }
        if (SE_Node   != null && SE_Node.imReachable   && imReachable && SE_Node.NO_Edge     == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, SE_Node.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; SE_Edge = aux_e.GetComponent<Edge_IA>(); SE_Node.NO_Edge = aux_e.GetComponent<Edge_IA>(); SE_Edge.Init(this, SE_Node); SE_Edge.Weight = 1.4f;  /*SE_Edge.EdgeDirection   = 3;*/ }
        if (SulNode   != null && SulNode.imReachable   && imReachable && SulNode.NorteEdge   == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, SulNode.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; SulEdge = aux_e.GetComponent<Edge_IA>(); SulNode.NorteEdge = aux_e.GetComponent<Edge_IA>(); SulEdge.Init(this, SulNode);                       /*SulEdge.EdgeDirection   = 2;*/ }
        if (SO_Node   != null && SO_Node.imReachable   && imReachable && SO_Node.NE_Edge     == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, SO_Node.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; SO_Edge = aux_e.GetComponent<Edge_IA>(); SO_Node.NE_Edge = aux_e.GetComponent<Edge_IA>(); SO_Edge.Init(this, SO_Node); SO_Edge.Weight = 1.4f;  /*SO_Edge.EdgeDirection   = 1;*/ }
        if (OesteNode != null && OesteNode.imReachable && imReachable && OesteNode.LesteEdge == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, OesteNode.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; OesteEdge = aux_e.GetComponent<Edge_IA>(); OesteNode.LesteEdge = aux_e.GetComponent<Edge_IA>(); OesteEdge.Init(this, OesteNode);             /*OesteEdge.EdgeDirection = 4;*/ }
        if (NO_Node   != null && NO_Node.imReachable   && imReachable && NO_Node.SE_Edge     == null) { GameObject aux_e = (Instantiate(grid_Generator.Edge, this.transform.position, Quaternion.LookRotation(transform.position, NO_Node.transform.position)) as GameObject); aux_e.gameObject.transform.parent = grid_Generator.Edges_Controller.transform; NO_Edge = aux_e.GetComponent<Edge_IA>(); NO_Node.SE_Edge = aux_e.GetComponent<Edge_IA>(); NO_Edge.Init(this, NO_Node); NO_Edge.Weight = 1.4f;  /*NO_Edge.EdgeDirection   = 7;*/ }
        if (EdgesColection() != null) { generated_edges = true; }
        nameEdges();
    }
    private void nameEdges()
    {
        foreach(Edge_IA e in EdgesColection())
        {
            e.gameObject.name = "Edge_"+ e.gameObject.GetInstanceID();
        }
    }
    public List<Edge_IA> EdgesColection() // retorna uma lista de Edges/Arestas deste node
    {
        List<Edge_IA> list = new List<Edge_IA>();

        if(NorteEdge != null) { list.Add(NorteEdge); }
        if(NE_Edge != null)   { list.Add(NE_Edge);   }
        if(LesteEdge != null) { list.Add(LesteEdge); }
        if(SE_Edge != null)   { list.Add(SE_Edge);   }
        if(SulEdge != null)   { list.Add(SulEdge);   }
        if(SO_Edge != null)   { list.Add(SO_Edge);   }
        if(OesteEdge != null) { list.Add(OesteEdge); }
        if(NO_Edge != null)   { list.Add(NO_Edge);   }

        return list;
    }

    public Edge_IA ConectedEdge(string key)// retorna um edge pesquisado recebendo uma chave (string)
    {
        foreach(Edge_IA e in EdgesColection())
        {
            if(e.name == key)
            {
                return e;
            }
        }
        return null;
    }
    private void UnactiveEdges()// desativa / destroi todos edges relacionados a este node
    {
        foreach (Edge_IA e in EdgesColection())
        {
            e.Unactive();
        }
        if (EdgesColection().Count==0)
        {
            generated_edges = false;
        }
    }
    public void CheckUpActiveButton()// checa se o developer ativa ou desativa no inspecto o botão ImReachable, ativando ou desativando o nó
    {
        if(imReachable && !generated_edges)       { GenerateEdges(); GetComponent<Renderer>().material = grid_Generator.Walk; }
        else if (!imReachable && generated_edges) { UnactiveEdges(); GetComponent<Renderer>().material = grid_Generator.Dontwalk; }
    }
    public void SwitchActiveBtn()// chamado pela classe Editor deste code (de fora) para mudar imReachable
    {
        if (imReachable) { imReachable = false; }
        else if (!imReachable) { imReachable = true; }
        CheckUpActiveButton(); 
    }
    private void OnDestroy()// desativa / destroi todos edges relacionados a este node, porém é chamado quando se está deletando-o
    {
        UnactiveEdges();
    }
    
}
