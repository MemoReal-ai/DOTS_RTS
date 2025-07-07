using Unity.Entities;
using Unity.Mathematics;

namespace _Game.Logic.Infrastructure.Components
{
    public struct TargetPoint : IComponentData
    {
        public float3 Position;

        public TargetPoint(float3 position)
        {
            Position = position;
        }
    }
}