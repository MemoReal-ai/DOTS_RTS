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

                    foreach (var (transform, selected) in
                             SystemAPI
                                 .Query<RefRW<TargetPoint>,
                                     RefRO<SelectedComponent>>()) //обновляем TargetPoint после нажатия
                    {
                        if (selected.ValueRO.Selected == false)
                        {
                            continue;
                        }

                        transform.ValueRW = _targetPoint;
                    }

                    var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

                    foreach (var (_, selected, entity)in SystemAPI.Query<RefRO<MoveSpeed>, RefRO<SelectedComponent>>()
                                 .WithNone<TargetPoint>()
                                 .WithEntityAccess()) //Добавляем всем у кого нет TargetPoint-a но есть MoveSpeed компонент Target point  
                    {
                        if (selected.ValueRO.Selected == false)
                        {
                            continue;
                        }

                        ecb.AddComponent(entity, _targetPoint); //добовляем к каждой entity  наш TargetPoint
                    }

                    ecb.Playback(state.EntityManager);
                }
            }
        }
    }
}