using OpenGL_Game.Components;
using OpenGL_Game.Game_Objects;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenGL_Game
{
    static class EnvironmentGenerator
    {
        public static void GenerateFromFile(string filename, EntityManager entityManager, out List<Vector3> AIroute)
        {
            AIroute = null;
            Entity entity = new Entity("Env");
            entity.AddComponent(new ComponentGeometry("Environment Final2.obj"));
            entity.AddComponent(new ComponentMaterial("EnvironmentMap2K.png", "Unlit"));
            entityManager.AddEntity(entity);

            if (File.Exists(filename))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(filename))
                    {
                        SortedDictionary<char, Vector3> route = new SortedDictionary<char, Vector3>();
                        Geometry wallGeometry = ResourceManager.LoadGeometry("wall_test_cube.obj");
                        int i = 0;
                        for (string line = streamReader.ReadLine(); line != null; line = streamReader.ReadLine())
                        {
                            int j = 0;
                            foreach (char c in line)
                            {
                                if (c >= 97 && c <= 122) // if is a lower case letter
                                {
                                    // we have an AI path
                                    Vector3 position = new Vector3(i - 12f, 1.2f, j - 12f);
                                    route.Add(c, position);
                                }
                                else
                                {
                                    switch (c)
                                    {
                                        case '#': // wall collider
                                            entityManager.AddEntity(CreateObject_WallColider(i, j, wallGeometry.bounds));
                                            break;
                                        case '♥': // health pickup
                                            entityManager.AddEntity(CreateObject_HealthPickup(i, j));
                                            break;
                                        case '$': // speed pickup
                                            entityManager.AddEntity(CreateObject_SpeedPickup(i, j));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                j++;
                            }
                            i++;
                        }

                        // let's sort the AI route dictionary
                        AIroute = route.Values.ToList();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private static Entity CreateObject_WallColider(int i, int j, Bounds wallGeometryBounds)
        {
            Entity entity = new Entity(string.Format("Environment Wall Collider {0}x{1}", i.ToString(), j.ToString()));
            entity.AddComponent(new ComponentCollider_Box(wallGeometryBounds, entity.transform));
            Vector3 position = new Vector3(i - 12f, 0f, j - 12f);
            entity.transform.position = position;

            return entity;
        }

        private static Entity CreateObject_HealthPickup(int i, int j)
        {
            Vector3 position = new Vector3(i - 12f, 0f, j - 12f);
            Entity entity = new PickupItem(string.Format("Pickup Health {0}x{1}", i.ToString(), j.ToString()), PickupType.Health, position);
            return entity;
        }

        private static Entity CreateObject_SpeedPickup(int i, int j)
        {
            Vector3 position = new Vector3(i - 12f, 0.44f, j - 12f);
            Entity entity = new PickupItem(string.Format("Pickup Speed {0}x{1}", i.ToString(), j.ToString()), PickupType.Speed, position);
            return entity;
        }

        #region RandomOld
        static Random random = new Random();
        public static void Generate(EntityManager entityManager)
        {
            Entity entity;

            // create and add the base ground with walls
            entity = new Entity("Environment_ground");
            entity.AddComponent(new ComponentGeometry("environment_base_25x25.obj"));
            entity.AddComponent(new ComponentMaterial("default_texture.png"));
            entityManager.AddEntity(entity);
            entity = null;

            // walls
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    entity = PlaceRandomWall(i, j);
                    if (entity != null)
                    {
                        entityManager.AddEntity(entity);
                    }
                }
            }
        }
        private static Entity PlaceRandomWall(int i, int j)
        {
            // set the center of the map clear
            if ((i > 10 && i < 15) && (j > 10 && j < 15)) return null;
            //if ((i > 3 && i < 7) && (j > 3 && j < 7)) return null;


            int x = random.Next(0, 1000);
            if (x % 5 != 0)
                return null;



            Entity entity = new Entity(string.Format("Environment_wall_{0}x{1}", i.ToString(), j.ToString()));
            entity.AddComponent(new ComponentGeometry("wall_test_cube.obj"));
            entity.AddComponent(new ComponentCollider_Box(entity.GetComponent<ComponentGeometry>().Geometry().bounds, entity.transform));
            entity.AddComponent(new ComponentMaterial("Textures/default_texture.png"));

            // set position
            Vector3 position = new Vector3(i - 12f, 0f, j - 12f);
            //Vector3 position = new Vector3(((float)i * 2.5f) - 11.25f, 0f, ((float)j * 2.5f) - 11.25f);
            entity.transform.position = position;

            // set rotation
            int r = random.Next(0, 400);
            Quaternion rotation = Quaternion.Identity;
            if (r < 100) rotation = new Quaternion(0, 0, 0);
            else if (r < 200) rotation = new Quaternion(0, MathHelper.DegreesToRadians(90), 0);
            else if (r < 300) rotation = new Quaternion(0, MathHelper.DegreesToRadians(-90), 0);
            else if (r < 400) rotation = new Quaternion(0, MathHelper.DegreesToRadians(180), 0);
            entity.transform.rotation = rotation;



            // set scale
            //entity.transform.scale.X = 5;

            return entity;
        }
        #endregion

    }
}
