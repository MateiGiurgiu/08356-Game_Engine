using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenGL_Game.Components;
using OpenTK.Audio.OpenAL;
using OpenTK;

namespace OpenGL_Game.Objects
{
    public class Entity
    {
        readonly string name;
        readonly List<IComponent> componentList = new List<IComponent>();
        ComponentTypes mask;

        public bool deleteFlag = false;

        /// <summary>
        /// similar to bCanEverTick in Unreal Engine, usefull for not calling empty methods
        /// </summary>
        public bool callUpdate = false;

        // we know for sure that every object will have a transfom componment which will be stored here
        // for easy access, instead of using GetComponent
        public readonly ComponentTransform transform;
        
        /// <summary>
        /// Each entity MUST have a Transform component
        /// </summary>
        /// <param name="name"></param>
        public Entity(string name)
        {
            this.name = name;

            // add the default transform component
            transform = new ComponentTransform();
            componentList.Add(transform);
            mask |= transform.ComponentType;
        }

        public Entity(string name, params IComponent[] components)
        {
            this.name = name;

            // add the default transform component
            transform = new ComponentTransform();
            componentList.Add(transform);
            mask |= transform.ComponentType;

            for(int i = 0; i < components.Length; i++)
            {
                AddComponent(components[i]);
            }
        }

        /// <summary>Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");
            Debug.Assert(!(component is ComponentTransform), "There is already a Transform Component attached by default, no need to add another one");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public String Name => name;

        public ComponentTypes Mask => mask;

        public List<IComponent> Components => componentList;

        public T GetComponent<T>() where T: IComponent
        {
            Debug.Assert(!(typeof(T) is IComponent), "You should provide a class which implements IComponent. Not the IComponent interface. e.g. ComponentTransform");

            foreach (IComponent component in componentList)
            {
                if (component is T) return (T)component;
            }
            return default(T);
        }

        public virtual void Update()
        {
            for(int i = 0;i < GetComponent<ComponentSound>().souce.Count;i++)
            AL.Source(GetComponent<ComponentSound>().souce[i], ALSource3f.Position, ref transform.position);
        }

        public virtual void OnTriggerEnter(Entity col) { }

        public override bool Equals(object obj)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            return this.name == (obj as Entity).name;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Entity lhs, Entity rhs)
        {
            return !(lhs == rhs);
        }
    }
}
