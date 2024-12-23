using Unity.Entities;
using UnityEngine;

public class DOTSSpriteAuthoring : MonoBehaviour
{
    // Physics以外のスプライトオブジェクト
    public GameObject SpritePrefab;
    // Physics用のスプライトオブジェクト
    public GameObject SpritePrefabPhysics;

    class Baker : Baker<DOTSSpriteAuthoring>
    {
        public override void Bake(DOTSSpriteAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new DOTSSprite
            {
                SpriteEntity = GetEntity(authoring.SpritePrefab, TransformUsageFlags.Dynamic),
                SpriteEntityPhysics = GetEntity(authoring.SpritePrefabPhysics, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct DOTSSprite : IComponentData
{
    public Entity SpriteEntity;
    public Entity SpriteEntityPhysics;
}