using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

public enum DOTSMode
{
    Physics, // 物理
    AllBustCompiler, // 全探索 BustCompiler
    AllBustCompilerJob, // 全探索 BustCompiler + job
    Spatial // 空間分割 BustCompiler + job
}

// 設定値の設定
// 大量のオブジェクトを生成する
[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct CreateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DOTSSceneParameter>();
        state.RequireForUpdate<DOTSSprite>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dotsSceneParameter = SystemAPI.GetSingleton<DOTSSceneParameter>();
        var dotsSprite = SystemAPI.GetSingleton<DOTSSprite>();
        
        Entity spriteEntity = dotsSprite.SpriteEntity;
        if (dotsSceneParameter.Mode == DOTSMode.Physics)
        {
            spriteEntity = dotsSprite.SpriteEntityPhysics;
        }
        
        var entities = state.EntityManager.Instantiate(spriteEntity, dotsSceneParameter.SpawnCount, Allocator.Temp);
        var rand = Random.CreateFromIndex(1);
        foreach (var entity in entities)
        {
            var f3 = rand.NextFloat3(-2.0f, 2.0f);
            f3.z = 0;
            var localTransform = LocalTransform.FromPosition(f3);
            SystemAPI.SetComponent(entity, localTransform);
        }
        
        entities.Dispose();

        state.Enabled = false;
    }
}