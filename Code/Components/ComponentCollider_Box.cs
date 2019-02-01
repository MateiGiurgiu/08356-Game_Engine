using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenGL_Game.Components
{
    public class ComponentCollider_Box : ComponentCollider
    {
        public Bounds collisionBounds;

        public ComponentCollider_Box(Bounds collisionBounds, ComponentTransform transform) : base(transform)
        {
            this.collisionBounds = collisionBounds;
        }

        public override bool CheckCollision(ComponentCollider other, out float depenetration, out Vector3 normal)
        {
            throw new NotImplementedException();
        }
    }
}
