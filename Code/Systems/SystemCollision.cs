using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Managers;
using OpenGL_Game.Components;
using OpenTK;

namespace OpenGL_Game.Systems
{
    public class SystemCollision : ISystem
    {
        public string Name => "Collision System";

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_COLLIDER);

        private EntityManager entityManager;
        public SystemCollision(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void BeforeAction() { }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                foreach (Entity other in entityManager.entityList)
                {
                    if (other.GetComponent<ComponentCollider>() == null) continue;
                    if (entity == other) continue;

                    // due to the type of game I assume that there are never going to be objects with a size larger
                    // than 1 so I can reduce the number of computations by this simple check
                    if (Utilities.Distance(entity.transform.position, other.transform.position) < 3)
                    {
                        DoCollisions(entity, other);
                    }
                }
            }
        }

        private void DoCollisions(Entity a, Entity b)
        {
            ComponentCollider colliderA = a.GetComponent<ComponentCollider>();
            ComponentCollider colliderB = b.GetComponent<ComponentCollider>();

            bool isTriggerCollision = colliderA.isTriggerCollider | colliderB.isTriggerCollider; 

            float depentration;
            Vector3 normal;
            if(colliderA.CheckCollision(colliderB, out depentration, out normal))
            {
                if (isTriggerCollision)
                {
                    if(!colliderA.inCollision)
                    {
                        a.OnTriggerEnter(b);
                        colliderA.inCollision = true;
                    }
                    if(!colliderB.inCollision)
                    {
                        b.OnTriggerEnter(a);
                        colliderB.inCollision = true;
                    }
                }
                else
                {
                    // solve the collision
                    colliderA.transform.position += normal * depentration;
                }
            }
            else
            {
                // there is no collision
                if(isTriggerCollision)
                {
                    colliderA.inCollision = false;
                    colliderB.inCollision = false;
                }
            }
        }
    }
}
