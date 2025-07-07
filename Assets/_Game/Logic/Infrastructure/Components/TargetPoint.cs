using Unity.Entities;
using Unity.Mathematics;

namespace _Game.Logic.Infrastructure.Components
{
    public struct TargetPoint : IComponentData
    {
        public float3 Value;

        public TargetPoint(float3 value)
        {
            Value = value;
        }
    }
}