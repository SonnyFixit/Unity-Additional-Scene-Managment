using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "ScenePackage", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]

public class SpawnManagerScriptableObject : ScriptableObject, IEnumerable
{

    public List<SceneAsset> unitySceneAssets = new List<SceneAsset>();


     public IEnumerator<SceneAsset> GetEnumerator()
    {
        return unitySceneAssets.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return unitySceneAssets.GetEnumerator();
    }
}