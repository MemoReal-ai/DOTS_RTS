using _Game.Logic.Infrastructure.Components;
using Unity.Entities;

namespace _Game.Logic.Infrastructure.Authoring.Bakers
{
    public class BakerMoveSpeed : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveSpeed(authoring.Value));
        }
    }
}