using System.Collections.Generic;
using Code.Gameplay.Common.Collisions;
using UnityEngine;

namespace Code.Gameplay.Common.GamePhysics._3D
{
    public class PhysicsService3D : IPhysicsService3D
    {
        private static readonly RaycastHit[] Hits = new RaycastHit[128];
        private static readonly Collider[] OverlapHits = new Collider[128];

        private readonly ICollisionRegistry _collisionRegistry;

        public PhysicsService3D(ICollisionRegistry collisionRegistry)
        {
            _collisionRegistry = collisionRegistry;
        }

        public IEnumerable<GameEntity> RaycastAll(Vector3 worldPosition, Vector3 direction, int layerMask)
        {
            int hitCount = Physics.RaycastNonAlloc(worldPosition, direction, Hits, Mathf.Infinity, layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = Hits[i];
                if (hit.collider == null)
                    continue;

                GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());
                if (entity == null)
                    continue;

                yield return entity;
            }
        }

        public GameEntity Raycast(Vector3 worldPosition, Vector3 direction, int layerMask)
        {
            int hitCount = Physics.RaycastNonAlloc(worldPosition, direction, Hits, Mathf.Infinity, layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = Hits[i];
                if (hit.collider == null)
                    continue;

                GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());
                if (entity == null)
                    continue;

                return entity;
            }

            return null;
        }

        public GameEntity LineCast(Vector3 start, Vector3 end, int layerMask)
        {
            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);

            int hitCount = Physics.RaycastNonAlloc(start, direction, Hits, distance, layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = Hits[i];
                if (hit.collider == null)
                    continue;

                GameEntity entity = _collisionRegistry.Get<GameEntity>(hit.collider.GetInstanceID());
                if (entity == null)
                    continue;

                return entity;
            }

            return null;
        }

        public IEnumerable<GameEntity> SphereCast(Vector3 position, float radius, int layerMask)
        {
            int hitCount = OverlapSphere(position, radius, OverlapHits, layerMask);

            DrawDebug(position, radius, 1f, Color.red);

            for (int i = 0; i < hitCount; i++)
            {
                GameEntity entity = _collisionRegistry.Get<GameEntity>(OverlapHits[i].GetInstanceID());
                if (entity == null)
                    continue;

                yield return entity;
            }
        }

        public int SphereCastNonAlloc(Vector3 position, float radius, int layerMask, GameEntity[] hitBuffer)
        {
            int hitCount = OverlapSphere(position, radius, OverlapHits, layerMask);

            DrawDebug(position, radius, 1f, Color.green);

            for (int i = 0; i < hitCount; i++)
            {
                GameEntity entity = _collisionRegistry.Get<GameEntity>(OverlapHits[i].GetInstanceID());
                if (entity == null)
                    continue;

                if (i < hitBuffer.Length)
                    hitBuffer[i] = entity;
            }

            return hitCount;
        }

        public TEntity OverlapPoint<TEntity>(Vector3 worldPosition, int layerMask) where TEntity : class
        {
            // В 3D нет OverlapPoint, эмулируем точечный OverlapSphere радиусом почти 0
            int hitCount = Physics.OverlapSphereNonAlloc(worldPosition, 0.01f, OverlapHits, layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                Collider hit = OverlapHits[i];
                if (hit == null)
                    continue;

                TEntity entity = _collisionRegistry.Get<TEntity>(hit.GetInstanceID());
                if (entity == null)
                    continue;

                return entity;
            }

            return null;
        }

        public int OverlapSphere(Vector3 worldPos, float radius, Collider[] hits, int layerMask) =>
            Physics.OverlapSphereNonAlloc(worldPos, radius, hits, layerMask);

        private static void DrawDebug(Vector3 worldPos, float radius, float seconds, Color color)
        {
            Debug.DrawRay(worldPos, radius * Vector3.up, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.down, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.left, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.right, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.forward, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.back, color, seconds);
        }
    }
}