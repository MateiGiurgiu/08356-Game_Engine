using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public enum ColliderType
    {
        Box = 0,
        Sphere = 1
    }

    public abstract class ComponentCollider : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_COLLIDER;

        public bool isTriggerCollider = false;
        // reference to transform, used by collision detection algorithms
        public readonly ComponentTransform transform;

        public bool inCollision;

        public ComponentCollider(ComponentTransform transform)
        {
            this.transform = transform;
        }

        public ComponentCollider(ComponentTransform transform, bool isTriggerCollider)
        {
            this.transform = transform;
            this.isTriggerCollider = isTriggerCollider;
        }

        public abstract bool CheckCollision(ComponentCollider other, out float depenetration, out Vector3 normal);
    }
}
