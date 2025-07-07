using _Game.Logic.Infrastructure.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = UnityEngine.RaycastHit;

namespace _Game.Logic.Infrastructure.Systems
{
    partial struct UnitMoverSystem : ISystem
    {
        private float _tresHoldDistance;
        private bool _hasTargetPoint;

        public void OnCreate(ref SystemState state)
        {
            _tresHoldDistance = 0.1f * 0.1f;
            state.RequireForUpdate<TargetPoint>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRW<LocalTransform> localTransform, RefRO<MoveSpeed> moveSpeed,
                         RefRW<PhysicsVelocity> velocity, RefRO<TargetPoint> point) in SystemAPI
                         .Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>, RefRO<TargetPoint>>())
            {
                var targetPoint = point.ValueRO.Value;
                var moveDirection = targetPoint - localTransform.ValueRO.Position;
                moveDirection = math.normalize(moveDirection);
                localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());
                velocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.Value;
                var distance = math.distancesq(localTransform.ValueRW.Position, targetPoint);

                if (distance < (_tresHoldDistance))
                {
                    velocity.ValueRW.Linear = float3.zero;
                }

                velocity.ValueRW.Angular = float3.zero;
            }
        }
    }
}