using _Game.Logic.Infrastructure.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace _Game.Logic.Infrastructure.Systems
{
    partial struct TrackSelectedSystem : ISystem
    {
        private const int INDEX_SELECTED_MOUSE_BUTTON = 0;

        private float3 _startPosition;
        private float3 _endPosition;
        private bool _startSelected;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<MoveSpeed>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var camera = Camera.main;

            if (Input.GetMouseButtonDown(INDEX_SELECTED_MOUSE_BUTTON))
            {
                {
                    _startPosition = Input.mousePosition;
                    _startSelected = false;
                }
            }

            if (Input.GetMouseButton(INDEX_SELECTED_MOUSE_BUTTON))
            {
                _startSelected = true;
                _endPosition = Input.mousePosition;

                foreach (RefRW<SelectedComponent> selectedComponent in SystemAPI.Query<RefRW<SelectedComponent>>())
                {
                    selectedComponent.ValueRW.Selected = false;
                }

                if (!SystemAPI.TryGetSingleton(out PhysicsWorldSingleton physicsWorld))
                {
                    return;
                }

                var ray = camera.ScreenPointToRay(Input.mousePosition);

                var rayCastInput = new RaycastInput
                {
                    Start = ray.origin,
                    End = ray.origin + ray.direction * 100,
                    Filter = CollisionFilter.Default
                };

                if (physicsWorld.CastRay(rayCastInput, out RaycastHit hit))
                {
                    if (state.EntityManager.HasComponent<MoveSpeed>(hit.Entity))
                    {
                        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
                        ecb.AddComponent(hit.Entity, new SelectedComponent(true));
                        Debug.Log($"Move speed {hit.Entity}");
                        ecb.Playback(state.EntityManager);
                    }
                }
            }

            if (Input.GetMouseButtonUp(INDEX_SELECTED_MOUSE_BUTTON))
            {
                if (_startSelected)
                {
                    _endPosition = Input.mousePosition;

                    float3 min = math.min(_endPosition, _startPosition);
                    float3 max = math.max(_endPosition, _startPosition);

                    foreach (var (selectedComponent, transform) in SystemAPI
                                 .Query<RefRW<SelectedComponent>, RefRO<LocalTransform>>())
                    {
                        var screenTransform = camera.WorldToScreenPoint(transform.ValueRO.Position);

                        var isWithinX = screenTransform.x >= min.x && screenTransform.x <= max.x;
                        var isWithinY = screenTransform.y >= min.y && screenTransform.y <= max.y;
                        var isWithinZ = screenTransform.z >= 0;

                        if (isWithinX && isWithinY && isWithinZ)
                        {
                            selectedComponent.ValueRW.Selected = true;
                            Debug.Log("Selected component");
                        }
                    }
                }

                _startSelected = false;
            }
        }
    }
}