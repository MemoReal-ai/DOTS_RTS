using Unity.Entities;

namespace _Game.Logic.Infrastructure.Components
{
    public struct MoveSpeed : IComponentData
    {
        public float Value;

        public MoveSpeed(float value)
        {
            Value = value;
        }
    }
}