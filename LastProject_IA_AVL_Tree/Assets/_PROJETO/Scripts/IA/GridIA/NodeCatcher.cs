using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NodeCatcher : MonoBehaviour
{// código responsável por pegar o node abaixo do gameObject (usa RayCast e depende de colisor nos Nodes)
 //sua utilização é opicional uma vez q pode-se passar as referencias manualmente
 //arrastando os nós para o player

    public New_Node_IA CurrentNode;
    public float LengthLine = 2f;

    New_Node_IA lastHilighted;
    public Grid_Generator gridRef;

    #region BlocoASerComentadoNaBuild  //Relacionado ao Editor de grafo, Comente para buildar, descomente para trabalhar com grid
//    /*
    [ExecuteInEditMode]
    private void Update()
    {
        if(Selection.activeGameObject != null && Selection.activeGameObject == this.gameObject)
        {
            Check();
        }
    }
//    */
    #endregion
    public void Check()
    {
        RaycastHit ray;
        //Debug.Log("LoadingConection");
        if(Physics.Raycast(this.transform.position, Vector3.down, out ray, LengthLine)){
            if(ray.transform.gameObject.tag == "Node")
            {
                CurrentNode= ray.transform.gameObject.GetComponent<New_Node_IA>();
                if(gridRef== null) { return; }
                if (lastHilighted != null && lastHilighted != CurrentNode)
                {
                    lastHilighted.GetComponent<MeshRenderer>().material =
                        gridRef.Walk;
                }
                CurrentNode.gameObject.GetComponent<MeshRenderer>().material =
                    this.gameObject.GetComponent<MeshRenderer>().material;
                lastHilighted = CurrentNode;
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position,new Vector3(transform.position.x, 
            transform.position.y -LengthLine,
            transform.position.z));
    }
}
