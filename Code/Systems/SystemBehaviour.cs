using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemBehaviour : ISystem
    {
        public string Name => "System Behavior";

        public void BeforeAction() { }

        public void OnAction(Entity entity)
        {
            if (entity.callUpdate) entity.Update();
        }
    }
}
