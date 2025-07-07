using _Game.Logic.Infrastructure.Components;
using Unity.Entities;

namespace _Game.Logic.Infrastructure.Authoring.Bakers
{
    class EntityBakerBaker : Baker<PrefabEntityAuthoring>
    {
        public override void Bake(PrefabEntityAuthoring authoring)
        {
            var entity = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
            var refPrefab = CreateAdditionalEntity(TransformUsageFlags.None);
            AddComponent(refPrefab, new EntityComponent(entity));
        }
    }
}