using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MapsManager : EditorWindow
{

    private bool normalMapPick = false;
    private bool vectorMapPick = false;
    private bool debugMapPick = false;

    private List<SceneAsset> unitySceneAssets = new List<SceneAsset>();
    
    private  Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";

    private List <SpawnManagerScriptableObject> spawnManagerItems = new List <SpawnManagerScriptableObject>();
    

    #region

    private SpawnManagerScriptableObject OldMapPackage;
    private SpawnManagerScriptableObject VectorMapPackage;
    private SpawnManagerScriptableObject TestDebugPackage;
    private SpawnManagerScriptableObject SelectedMapPackage;
    
    #endregion


    [MenuItem("ScenesManager/ScenesManager")]
    public static void ShowWindow()
    {
        
        EditorWindow.GetWindow(typeof(MapsManager));
    }

    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////
        

    void OnGUI()
    {

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(600));

        GUILayout.Space(20);

        GUILayout.Label("Scenes to include in a build - manual selection:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);

        for (int i = 0; i < unitySceneAssets.Count; ++i)
        {
            unitySceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(unitySceneAssets[i], typeof(SceneAsset), false);
        }
        if (GUILayout.Button("Add scene to build"))
        {
            unitySceneAssets.Add(null);
        }
        GUILayout.Space(10);

        if (GUILayout.Button("Remove scene"))
        {
            unitySceneAssets.RemoveAt(unitySceneAssets.Count - 1);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Remove all"))
        {
            unitySceneAssets.Clear();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Apply to build settings", GUILayout.Height(30)))
        {
            SetEditorBuildSettingsScenes();
        }

        GUILayout.Space(10);

        ////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////
        

        GUILayout.Label("Scenes to include in the build - ready packages:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(20);

        ChangeValue1(GUILayout.Toggle(normalMapPick, "Normal Map Package"));
        GUILayout.Space(20);
        ChangeValue2 (GUILayout.Toggle(vectorMapPick, "Vector Map Package"));
        GUILayout.Space(20);
        ChangeValue3 (GUILayout.Toggle(debugMapPick, "Debug Mag Package"));
        
        
        if (normalMapPick == true)
        {
            unitySceneAssets.Clear();
            SetNormalMap();
        }


        else if (vectorMapPick == true )
        {
            unitySceneAssets.Clear();
            SetVectorMap();
        }

        else if (debugMapPick == true)
        {
            unitySceneAssets.Clear();
            SetDebugMap();
        }

        GUILayout.Space(20);

        
    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////
    
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Project quality settings labels:", EditorStyles.boldLabel);
        GUILayout.Space(10);
    

        string[] names = QualitySettings.names;
        GUILayout.BeginVertical();
        for (int i = 0; i < names.Length; i++)
        {
            if (GUILayout.Button(names[i]))
            {
                QualitySettings.SetQualityLevel(i, true);
            }
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Prepare build:", EditorStyles.boldLabel);
        GUILayout.Space(20);

        if (GUILayout.Button("Open quality settings window", GUILayout.Height(30)))
        {

            //EditorWindow.GetWindow(System.Type.GetType("UnityEditor.ProjectSettingWindow/QualityWindow, UnityEditor"));

            SettingsService.OpenProjectSettings("Project/Quality");

        }
        

        if (GUILayout.Button("Open build window", GUILayout.Height(30)))
        {

            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));

        }

         EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

    }


    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////


    private void SetEditorBuildSettingsScenes()
    {
        
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        foreach (var sceneAsset in unitySceneAssets)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }  
        }


        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();


    }

    private void SetVectorMap()
    {
        VectorMapPackage = (SpawnManagerScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Scenes/VectorMapPackage.asset", typeof(SpawnManagerScriptableObject));
        
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        for (int i = 0; i < VectorMapPackage.unitySceneAssets.Count; i++)
        {

            string scenePath = AssetDatabase.GetAssetPath(VectorMapPackage.unitySceneAssets[i]);

            //Debug.Log("This asset [i] path is: " + AssetDatabase.GetAssetPath(VectorMapPackage.unitySceneAssets[i]));
        
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                //Debug.Log("Scene was added");
            }  

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }

    private void SetNormalMap()
    {
        OldMapPackage = (SpawnManagerScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Scenes/OldMapPackage.asset", typeof(SpawnManagerScriptableObject));
        
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        for (int i = 0; i < OldMapPackage.unitySceneAssets.Count; i++)
        {

            string scenePath = AssetDatabase.GetAssetPath(OldMapPackage.unitySceneAssets[i]);

           //Debug.Log("This asset [i] path is: " + AssetDatabase.GetAssetPath(OldMapPackage.unitySceneAssets[i]));
        
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                //Debug.Log("Scene was added");
            }  

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }

    private void SetDebugMap()
    {
        TestDebugPackage = (SpawnManagerScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Scenes/DebugMapPackage.asset", typeof(SpawnManagerScriptableObject));
        
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        for (int i = 0; i < TestDebugPackage.unitySceneAssets.Count; i++)
        {

            string scenePath = AssetDatabase.GetAssetPath(TestDebugPackage.unitySceneAssets[i]);

            //Debug.Log("This asset [i] path is: " + AssetDatabase.GetAssetPath(TestDebugPackage.unitySceneAssets[i]));
        
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                //Debug.Log("Scene was added");
            }  

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }

    private void SetTheMap()
    {

        if (normalMapPick == true)
        {
            SelectedMapPackage = (SpawnManagerScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Scenes/OldMapPackage.asset", typeof(SpawnManagerScriptableObject));
        
        }

    }

    #region ValueChanges

    private void ChangeValue1(bool newValue)
    {
        if (newValue)
        {
            vectorMapPick = false;
            debugMapPick = false;
        }

        normalMapPick = newValue;
    }

    private void ChangeValue2(bool newValue)
    {
        if (newValue)
        {
            normalMapPick = false;
            debugMapPick = false;
        }

        vectorMapPick = newValue;
    }

    private void ChangeValue3(bool newValue)
    {
        if (newValue)
        {
            normalMapPick = false;
            vectorMapPick = false;
        }

        debugMapPick = newValue;
    }

    #endregion 


}
