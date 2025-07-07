using _Game.Logic.Infrastructure.Authoring;
using _Game.Logic.Infrastructure.Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace _Game.Logic.Infrastructure.Systems
{
    [UpdateBefore(typeof(UnitMoverSystem))]
    public partial struct TrackInputSystem : ISystem
    {
        private const int INDEX_RIGHT_MOUSE_BUTTON = 1;

        private TargetPoint _targetPoint;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MoveSpeed>(); //строка обозначающая что система будет
            //работать только если в мире есть компонент MoveSpeed
        }

        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(INDEX_RIGHT_MOUSE_BUTTON))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    _targetPoint = new TargetPoint(hit.point);

                    foreach (var transform in
                             SystemAPI.Query<RefRW<TargetPoint>>()) //обновляем TargetPoint после нажатия
                    {
                        transform.ValueRW = _targetPoint;
                    }

                    var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

                    foreach (var (_, entity)in SystemAPI.Query<RefRO<MoveSpeed>>().WithNone<TargetPoint>()
                                 .WithEntityAccess()) //Добавляем всем у кого нет TargetPoint-a но есть MoveSpeed компонент Target point  
                    {
                        ecb.AddComponent(entity, _targetPoint); //добовляем к каждой entity  наш TargetPoint
                    }

                    ecb.Playback(state.EntityManager);
                }
            }
        }
    }
}