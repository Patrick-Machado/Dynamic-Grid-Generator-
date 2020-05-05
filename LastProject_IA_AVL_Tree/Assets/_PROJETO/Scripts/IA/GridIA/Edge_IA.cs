using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Edge_IA : MonoBehaviour
{
    public bool isActivated = false;
    public New_Node_IA A, B;

    public float Weight = 1;
    public string angelstring;
    public string highstring;

    Grid_Generator grid_Generator;
    public void Init(New_Node_IA a, New_Node_IA b)
    {
        A = a; B = b;
        grid_Generator = A.grid_Generator;
        Active();

    }
    bool Validate_Angle()
    {
        //voltar aki pra validação
        return false;
    }
    public void Active()
    {
        isActivated = true;
        AtualizaPosicaoRotacao();
    }
    public void Unactive()
    {
        isActivated = false;
        try
        {
            Destroy(this.gameObject);// esta parte do codigo ger aum erro que pode ser ignorado na unity 
        }
        catch { }
        finally { }
    }
    public void AtualizaPosicaoRotacao()
    {
        if (!isActivated) { return; }
        //validação angulo entre arestas:
        angelstring = calculo_angulo().ToString(); highstring = calcula_altura().ToString();
        if (float.Parse(angelstring) > grid_Generator.angletodeletedge || calcula_altura() > grid_Generator.hightodeletedge)
        { Invoke("Unactive", 0.01f); return; }
        //modificação aresta entre dois nodes:
        transform.up = (A.gameObject.transform.position - B.gameObject.transform.position).normalized; 
        transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(A.gameObject.transform.position, B.gameObject.transform.position) / 2, transform.localScale.z);
        transform.position = (A.gameObject.transform.position + B.gameObject.transform.position) / 2;

    }

    float calculo_angulo()// calcula angulo entre dois vetores 3D
    {
        float DeltaH = Mathf.Abs(A.gameObject.transform.position.y - B.gameObject.transform.position.y );
        float DeltaD = Vector2.Distance(new Vector2(A.gameObject.transform.position.x, A.gameObject.transform.position.z), 
                                        new Vector2(B.gameObject.transform.position.x, B.gameObject.transform.position.z));
        float Angulo = DeltaH / DeltaD;
        return Angulo;
    }
    float calcula_altura() // calcula altura entre dois vetores 3D
    {
        float DeltaH = Mathf.Abs(A.gameObject.transform.position.y - B.gameObject.transform.position.y);
        return DeltaH;
    }

    public New_Node_IA OtherPeerNode(New_Node_IA originNode)
    {
        //verifica se A ou B são nulos:
        if(A==null || B == null) { Debug.Log("AouB = NULL"); return null; }

        //retorna A ou B:
        if (A != originNode) { return A; }
        else { return B; }
    }
    
    public int GetEdgeAvlID()
    {
        string[] name = (this.gameObject.name).Split('-');
        int id = int.Parse(name[1]);
        return id;
    }
}
