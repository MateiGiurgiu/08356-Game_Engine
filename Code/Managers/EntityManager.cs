using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenTK;

namespace OpenGL_Game.Managers
{
    public class EntityManager
    {
        internal List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();

            toCreate = new List<Entity>();
            toDelete = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        public void ManageEntities()
        {
            if (toDelete.Count == 0 && toCreate.Count == 0) return;
            for(int i = 0; i < toDelete.Count; i++)
            {
                if (toDelete[i] == null) continue;
                entityList.Remove(toDelete[i]);
            }
            for (int i = 0; i < toCreate.Count; i++)
            {
                if (toCreate[i] == null) continue;
                AddEntity(toCreate[i]);
            }
            toDelete.Clear();
            toCreate.Clear();
        }

        public void ChangeEntityVelocity(Entity entity, int index, Vector3 vel)
        {
            entity.GetComponent<ComponentVelocity>().Velocity = vel;
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(e => e.Name == name);
        }

        public List<Entity> Entities()
        {
            return entityList;
        }

        public void DeleteEntity(Entity entity)
        {
            entityList.Remove(entity);
        }

        public void DeleteEntity(string entityName)
        {
            int indexToRemove = -1;
            for (int i = 0; i < entityList.Count; i++)
            {
                if(entityList[i].Name == entityName)
                {
                    indexToRemove = i;
                    break;
                }
            }
            if(indexToRemove != -1)
            {
                entityList.RemoveAt(indexToRemove);
            }
        }

        public Entity getEntity(string name)
        {
            try
            {
                for (int i = 0; i < entityList.Count; i++)
                {
                    if (entityList[i].Name == name)
                        return entityList[i];
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public void DeleteAllEntities()
        {
            entityList.Clear();
        }

        #region Delete/Create
        private static List<Entity> toCreate;
        private static List<Entity> toDelete;

        public static void Create(Entity entity)
        {
            toCreate.Add(entity);
        }

        public static void Delete(Entity entity)
        {
            toDelete.Add(entity);
        }
        #endregion
    }
}
