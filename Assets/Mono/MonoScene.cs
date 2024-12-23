using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum MonoMode
{
    Physics, // 物理
    All, // 全探索
}

public class MonoScene : MonoBehaviour
{
    private const float Radius = 0.1f;

    [SerializeField] public GameObject spritePrefab;
    [SerializeField] public GameObject spritePrefabPhysics;
    [SerializeField] private MonoMode mode = MonoMode.Physics;
    [SerializeField] [Range(0, 5000)] private int spawnCount = 1000;
    
    private List<Tuple<Transform, GameObject>> objects = new ();
    
    void Start()
    {
        if (SceneParameter.ForTitleScene)
        {
            if (SceneParameter.SpawnCount > 0)
                spawnCount = SceneParameter.SpawnCount;
            mode = SceneParameter.MonoMode;
        }
        
        GameObject createSpritePrefab = spritePrefab;
        if (mode == MonoMode.Physics)
        {
            createSpritePrefab = spritePrefabPhysics;
        }
        for (int i = 0; i < spawnCount; i++)
        {
            float x = Random.Range(-2.0f, 2.0f);
            float y = Random.Range(-2.0f, 2.0f);
            var go = Instantiate(createSpritePrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
            objects.Add(Tuple.Create(go.transform, go));
        }
    }

    void Update()
    {
        switch (mode)
        {
            case MonoMode.Physics:
                Physics2D.Simulate(Time.fixedDeltaTime);
                break;
            
            case MonoMode.All:
                var count = objects.Count;
                for (int i = 0; i < count; i++)
                {
                    Vector3 Displacement = Vector3.zero;
                    uint Weight = 0;
                    
                    var transform = objects[i].Item1;
                    for (int j = 0; j < count; j++)
                    {
                        var otherTransform = objects[j].Item1;
                        Vector3 towards = transform.localPosition - otherTransform.localPosition;

                        float distancesq = towards.sqrMagnitude;
                        float radiusSum = Radius + Radius;
                        if (distancesq > radiusSum * radiusSum || objects[i].Item2 == objects[j].Item2)
                            continue;
                        
                        float distance = towards.magnitude;
                        float penetration;
                        if (distance < 0.0001f)
                        {
                            penetration = 0.01f;
                        }
                        else
                        {
                            penetration = radiusSum - distance;
                            penetration = (penetration / distance);
                        }
                        
                        Displacement += towards * penetration;
                        Weight++;
                    }
                    
                    if (Weight > 0)
                    {
                        Displacement /= Weight;
                        transform.localPosition += Displacement;
                    }
                }
                break;
        }
    }
    
    public void OnClickBack()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
