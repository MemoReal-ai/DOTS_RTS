using _Game.Logic.Infrastructure.Components;
using Unity.Burst;
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
        private TargetPoint Position { get; set; }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    Position = new TargetPoint(hit.point);
                    Debug.Log(Position.Position.ToString());
                }
                
                foreach ((RefRW<LocalTransform> localTransform, RefRO<MoveSpeed> moveSpeed,
                             RefRW<PhysicsVelocity> velocity) in SystemAPI
                             .Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
                {
                    var targetPoint = Position.Position; //Здесь клик от мышки должен быть 
                    var moveDirection = targetPoint - localTransform.ValueRO.Position;
                    moveDirection = math.normalize(moveDirection);

                    localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());

                    velocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.Value;
                    velocity.ValueRW.Angular = float3.zero;
                }
            }
        }
    }
}