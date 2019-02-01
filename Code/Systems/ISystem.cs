using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public interface ISystem
    {
        void BeforeAction();
        void OnAction(Entity entity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
