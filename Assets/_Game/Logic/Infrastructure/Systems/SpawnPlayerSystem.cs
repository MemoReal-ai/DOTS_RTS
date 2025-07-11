using _Game.Logic.Infrastructure.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Logic.Infrastructure.Systems
{
    public partial struct SpawnPlayerSystem : ISystem
    {
        private float3 _playerPosition;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _playerPosition = new float3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Spawn Player");
                var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

                foreach (var prefab in SystemAPI.Query<RefRO<EntityComponent>>())
                {
                    var instance = ecb.Instantiate(prefab.ValueRO.Value);

                    ecb.SetComponent(instance,
                        new LocalTransform
                        {
                            Position = _playerPosition, Rotation = quaternion.identity, Scale = 1f
                        });
                    break;
                }

                ecb.Playback(state.EntityManager);
            }
        }
    }
}