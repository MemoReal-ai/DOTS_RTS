using Unity.Entities;

namespace _Game.Logic.Infrastructure.Components
{
    public struct EntityComponent : IComponentData
    {
        public Entity Value;

        public EntityComponent(Entity value)
        {
            Value = value;
        }
    }
}