using _Game.Logic.Infrastructure.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace _Game.Logic.Infrastructure.Systems
{
    partial struct TrackSelectedSystem : ISystem
    {
        private const int INDEX_SELECTED_MOUSE_BUTTON = 0;


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<MoveSpeed>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(INDEX_SELECTED_MOUSE_BUTTON))
            {
                foreach (RefRW<SelectedComponent> selectedComponent in SystemAPI.Query<RefRW<SelectedComponent>>())
                {
                    selectedComponent.ValueRW.Selected = false;
                }

                if (!SystemAPI.TryGetSingleton(out PhysicsWorldSingleton physicsWorld))
                {
                    return;
                }

                var camera = Camera.main;
                var ray = camera.ScreenPointToRay(Input.mousePosition);

                var rayCastInput = new RaycastInput
                {
                    Start = ray.origin,
                    End = ray.origin + ray.direction * 100,
                    Filter = CollisionFilter.Default
                };

                Debug.Log($"Mouse button {INDEX_SELECTED_MOUSE_BUTTON}");

                if (physicsWorld.CastRay(rayCastInput, out RaycastHit hit))
                {
                    if (state.EntityManager.HasComponent<MoveSpeed>(hit.Entity))
                    {
                        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
                        ecb.AddComponent(hit.Entity, new SelectedComponent(true));
                        Debug.Log($"Move speed {hit.Entity}");
                        ecb.Playback(state.EntityManager);
                    }


                    Debug.DrawRay(rayCastInput.Start, rayCastInput.End, Color.red);
                }
            }
        }
    }
}