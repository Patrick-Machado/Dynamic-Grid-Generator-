using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Grid_Generator : MonoBehaviour
{
    [Header("Variable Attributes")]
    public float Granularidade = 1.0f;
    public int Size = 10;
    public float CollisionRadius =0.7f;
    public float angletodeletedge = 1.5f;
    public float hightodeletedge = 0.8f;
    [Header("Prefabs References")]
    public GameObject Node;
    public GameObject Edge;
    public Material Walk, Dontwalk;

    public New_Node_IA[,] GridMatrix;

    [HideInInspector]public GameObject Edges_Controller;
    [HideInInspector]public GameObject Nodes_Controller;

    public void GenerateGraph()// principal método gerador do grafo
    {
        GridMatrix = new New_Node_IA[Size, Size];

        RaycastHit ray;

        CreateEdges_Controller();//cria o gameobject Master dos Edges (Arestas)
        CreateNodes_Controller();//cria o gameobject Master dos Vertex (Nodes)

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (Physics.Raycast(this.transform.position + new Vector3(i * Granularidade, 0, j * Granularidade), Vector3.down, out ray, 100.0f))
                {
                    GameObject aux = Instantiate(Node, ray.point, Quaternion.identity);
                    aux.transform.parent = Nodes_Controller.transform;
                    New_Node_IA aux_n = aux.GetComponent<New_Node_IA>();

                    if (ray.collider.tag == "Walk")// nodes que são passáveis 
                    {
                        aux.GetComponent<Renderer>().material = Walk;
                        aux_n.imReachable = true;
                    }
                    else if (ray.collider.tag == "Dontwalk")// nodes que não são passáveis 
                    {
                        aux.GetComponent<Renderer>().material = Dontwalk;
                        aux_n.imReachable = false;
                    }
                    aux_n.ID = new Vector2(i, j );// muda o ID cartesiano discreto do node (X,Y)
                    aux_n.gameObject.name = "Node_" + Mathf.Abs(aux_n.ID.x) +","+ Mathf.Abs(aux_n.ID.y);// muda o nome do gameObject pro nome com ID

                    GridMatrix[i, j] = aux_n;

                }
            }
        }
        //inicialização de cada node da matriz
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (GridMatrix[i, j]!=null)
                {
                    GridMatrix[i, j].Init(GridMatrix, i, j, Size, Size, this);
                }
            }
        }
        //geração de edges (após a matrix estar toda inicializada
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {

                if (GridMatrix[i, j]!=null)
                {
                    GridMatrix[i, j].GenerateEdges();
                }
                    
            }
        }

    }

    public void CreateEdges_Controller()
    {
        Edges_Controller = new GameObject() as GameObject;
        Edges_Controller.name = "Edges_Controller";
        Edges_Controller.transform.parent = this.gameObject.transform;
    }
    public void CreateNodes_Controller()
    {
        Nodes_Controller = new GameObject() as GameObject;
        Nodes_Controller.name = "Nodes_Controller";
        Nodes_Controller.transform.parent = this.gameObject.transform;
    }

    #region BlocoASerComentadoNaBuild  //Relacionado ao Editor de grafo, Comente para buildar, descomente para trabalhar com grid
//    /*
    [ExecuteInEditMode]
    private void Update()
    {

        if (Selection.activeGameObject != null && Selection.activeGameObject.tag == "Node")
        {
            if (Selection.activeGameObject.GetComponent<New_Node_IA>().EdgesColection() != null)
            {
                foreach(Edge_IA e in Selection.activeGameObject.GetComponent<New_Node_IA>().EdgesColection())
                {
                    e.AtualizaPosicaoRotacao();
                    
                }

                Selection.activeGameObject.GetComponent<New_Node_IA>().CheckUpActiveButton();
            }
        }
    }
//    */
    #endregion
}
