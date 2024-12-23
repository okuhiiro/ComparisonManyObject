using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct DOTSSceneParameter : IComponentData
{
    public DOTSMode Mode;
    public int SpawnCount;
}

public class DOTSScene : MonoBehaviour
{
    [SerializeField] private DOTSMode mode = DOTSMode.Physics;
    [SerializeField] [Range(0, 5000)] private int spawnCount = 1000;
    
    void Awake()
    {
        if (SceneParameter.ForTitleScene)
        {
            if (SceneParameter.SpawnCount > 0)
                spawnCount = SceneParameter.SpawnCount;
            mode = SceneParameter.DOTSMode;
        }
        
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var query = entityManager.CreateEntityQuery(typeof(DOTSSceneParameter));
        if (!query.TryGetSingletonEntity<DOTSSceneParameter>(out var entity))
        {
            entity = entityManager.CreateSingleton<DOTSSceneParameter>();
        }
        
        entityManager.SetComponentData(entity, new DOTSSceneParameter
        {
            Mode = mode,
            SpawnCount = spawnCount
        });
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
