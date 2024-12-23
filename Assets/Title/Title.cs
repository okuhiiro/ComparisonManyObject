using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    const string MonoSceneName = "MonoScene";
    const string DOTSSceneName = "DOTSScene";
    
    [SerializeField] private TMP_InputField inputSpawnCount;
    [SerializeField] private int spawnCount = 1000;

    private void Awake()
    {
        if (SceneParameter.SpawnCount > 0)
            inputSpawnCount.text = SceneParameter.SpawnCount.ToString();
    }

    private static void ResetDefaultWorld()
    {
        var defaultWorld = World.DefaultGameObjectInjectionWorld;
        defaultWorld.EntityManager.CompleteAllTrackedJobs();
        foreach (var system in defaultWorld.Systems)
        {
            system.Enabled = false;
        }

        defaultWorld.Dispose();
        DefaultWorldInitialization.Initialize("Default World", false);
    }
    
    public void OnInputChanged()
    {
        if (int.TryParse(inputSpawnCount.text, out spawnCount))
        {
            spawnCount = Mathf.Min(spawnCount, 5000);
            SceneParameter.SpawnCount = spawnCount;
        }
        else
        {
            Debug.LogError("Input must be an integer");
        }
    }

    private void CommonLoadScene()
    {
        SceneParameter.ForTitleScene = true;
    }
    
    /// <summary>
    /// Mono
    /// </summary>
    public void OnClickMonoPhysics()
    {
        CommonLoadScene();
        SceneParameter.MonoMode = MonoMode.Physics;
        SceneManager.LoadScene(MonoSceneName, LoadSceneMode.Single);
    }
    
    public void OnClickMonoAll()
    {
        CommonLoadScene();
        SceneParameter.MonoMode = MonoMode.All;
        SceneManager.LoadScene(MonoSceneName, LoadSceneMode.Single);
    }
    
    /// <summary>
    /// DOTS
    /// </summary>
    public void OnClickDOTSPhysics()
    {
        CommonLoadScene();
        ResetDefaultWorld();
        SceneParameter.DOTSMode = DOTSMode.Physics;
        SceneManager.LoadScene(DOTSSceneName, LoadSceneMode.Single);
    }
    
    public void OnClickDOTSAllBurstCompiler()
    {
        CommonLoadScene();
        ResetDefaultWorld();
        SceneParameter.DOTSMode = DOTSMode.AllBustCompiler;
        SceneManager.LoadScene(DOTSSceneName, LoadSceneMode.Single);
    }
    
    public void OnClickDOTSAllBurstCompilerJob()
    {
        CommonLoadScene();
        ResetDefaultWorld();
        SceneParameter.DOTSMode = DOTSMode.AllBustCompilerJob;
        SceneManager.LoadScene(DOTSSceneName, LoadSceneMode.Single);
    }
    
    public void OnClickDOTSSpatial()
    {
        CommonLoadScene();
        ResetDefaultWorld();
        SceneParameter.DOTSMode = DOTSMode.Spatial;
        SceneManager.LoadScene(DOTSSceneName, LoadSceneMode.Single);
    }
}
