using UnityEngine;
using UnityEditor;

public class GridSaver : MonoBehaviour
{
    public char wichLevel;
    string path = "Assets/_PROJETO/Resources/Levels/Level_";
    public void SaveLevel()
    {
        //coment if building
    #region BlocoASerComentadoNaBuild  //Relacionado ao Editor de grafo, Comente para buildar, descomente para trabalhar com grid
        string localPath = path + wichLevel + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(this.gameObject, localPath, InteractionMode.UserAction);
    #endregion
    
    }
    public void LoadLevel()
    {
        try
        {
            Object myNewLevel = Resources.Load("Levels/Level_" + wichLevel);
            GameObject novoAndar = (GameObject)Instantiate(myNewLevel, transform.position, transform.rotation);
        }
        catch (System.Exception)
        {

            Debug.LogError("## NOT FOUND ##");
        }
    }
}
