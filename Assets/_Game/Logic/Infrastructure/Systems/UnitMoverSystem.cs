using _Game.Logic.Infrastructure.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using RaycastHit = UnityEngine.RaycastHit;

namespace _Game.Logic.Infrastructure.Systems
{
    partial struct UnitMoverSystem : ISystem
    {
        private float _tresHoldDistance;
        private bool _hasTargetPoint;
        private float3 _targetPoint;

        public void OnCreate(ref SystemState state)
        {
            _tresHoldDistance = 0.4f;
            state.RequireForUpdate<TargetPoint>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

            foreach (var (localTransform, moveSpeed, velocity, point, selected, entity) in SystemAPI
                         .Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>,
                             RefRW<TargetPoint>,
                             RefRO<SelectedComponent>>()
                         .WithEntityAccess()) //перебираем всех ентети ку которых есть трансформ мувспид и велосити и таргет поинт
            {
                _targetPoint = point.ValueRO.Value;

                if (selected.ValueRO.Selected == false)
                {
                    var targetDistanceNoSelectedUnit = math.distancesq(localTransform.ValueRW.Position, _targetPoint);

                    if (targetDistanceNoSelectedUnit <= _tresHoldDistance)
                    {
                        velocity.ValueRW.Linear = float3.zero;
                        ecb.RemoveComponent<TargetPoint>(entity);
                    }


                    continue;
                }

                var moveDirection = _targetPoint - localTransform.ValueRO.Position;
                moveDirection = math.normalize(moveDirection);
                localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());
                velocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.Value;
                var distance = math.distancesq(localTransform.ValueRW.Position, _targetPoint);

                if (distance < (_tresHoldDistance))
                {
                    velocity.ValueRW.Linear = float3.zero;
                    ecb.RemoveComponent<TargetPoint>(entity);
                }

                velocity.ValueRW.Angular = float3.zero;
            }

            ecb.Playback(state.EntityManager);
            
        }
    }
}