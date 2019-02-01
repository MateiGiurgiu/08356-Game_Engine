using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    public enum ComponentTypes
    {
        COMPONENT_NONE      = 0,
        COMPONENT_GEOMETRY  = 1 << 1,
        COMPONENT_MATERIAL  = 1 << 2,
        COMPONENT_VELOCITY  = 1 << 3,
        COMPONENT_TRANSFORM = 1 << 4,
        COMPONENT_COLLIDER  = 1 << 5,
        COMPONENT_SOUND     = 1 << 6,
        COMPONENT_UI        = 1 << 7,
        COMPONENT_CUBE      = 1 << 8
    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
