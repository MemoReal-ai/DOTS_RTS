using _Game.Logic.Infrastructure.Components;
using Unity.Entities;

namespace _Game.Logic.Infrastructure.Authoring.Bakers
{
    class SelectedBakerBaker : Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new SelectedComponent(authoring.Value));
        }
    }
}