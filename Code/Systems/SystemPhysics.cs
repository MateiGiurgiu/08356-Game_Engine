using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_VELOCITY);

        public string Name => "SystemPhysics";

        public void BeforeAction() { }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                ComponentTransform transform = entity.transform;
                ComponentVelocity velocity = entity.GetComponent<ComponentVelocity>();

                Motion(transform, velocity);
            }
        }

        private void Motion(ComponentTransform transform, ComponentVelocity vel)
        {
            transform.position += vel.Velocity * Time.deltaTime;
        }
    }
}
