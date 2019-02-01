using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Game_Objects
{
    public class Drone : Entity
    {
        private enum State
        {
            Patrol,
            Attack
        }

        public List<Vector3> route;
        // reference for the player, useful for checking if he is nearby
        public Player player;

        private int currentIndex;
        private float currentSpeed = 2f;
        private float currentLife = 100f;
        private int startIndex;
        private State state = State.Patrol;
        private ComponentMaterial c_material;
        public bool isDead = false;
        public static bool isDisabled = false;

        public Drone(string name, List<Vector3> route, int startIndex, Player player) : base(name)
        {
            this.route = route;
            this.player = player;
            this.startIndex = startIndex;

            AddComponent(new ComponentGeometry("Drone.obj"));
            AddComponent(new ComponentMaterial("Drone.png"));
            AddComponent(new ComponentCollider_Sphere(0.5f, true, transform));
            AddComponent(new ComponentSound(new string[] { "Audio/buzz.wav", "Audio/dying.wav" }));
            callUpdate = true;

            c_material = GetComponent<ComponentMaterial>();

            Initialize();

            Player.ResetEvent += Initialize;
        }

        private void Initialize()
        {
            currentIndex = startIndex;
            transform.position = route[currentIndex];
            NextPoint();
            currentSpeed = 2f;
            state = State.Patrol;
            currentLife = 100f;
            GetComponent<ComponentSound>().playsoundloop(0);
        }

        private void NextPoint()
        {

            if (currentIndex == route.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++; //= Pathfinder();
            }
        }

        private int Pathfinder()
        {
            float f;
            float newSmallest = 25f;
            float g;
            float h;
            int temp = 0;
            for (var i = 0; i < route.Count; i++)
            {

                g = Utilities.Distance(this.transform.position, route[i]);
                h = Utilities.Distance(route[i], this.player.transform.position);
                f = h + g;

                if (f > 1 && f < newSmallest)
                {
                    newSmallest -= f;
                    temp = i;
                }

            }
            return temp;
        }

        public override void Update()
        {
            if (isDisabled) return;
            if (state == State.Patrol)
            {
                if (Utilities.Distance(transform.position, route[currentIndex]) > 0.1f)
                {
                    Vector3 direction = (route[currentIndex] - transform.position).Normalized();
                    transform.Translate(direction * currentSpeed * Time.deltaTime);
                    transform.LookAtDir(direction);
                }
                else
                {
                    NextPoint();
                }

                // check if player in range
                if (Utilities.Distance(player.transform.position, transform.position) < 2.2f)
                {
                    state = State.Attack;
                    currentSpeed = 1.7f;
                }
            }
            else if (state == State.Attack)
            {
                Vector3 direction = (player.transform.position - transform.position).Normalized();
                transform.Translate(direction * currentSpeed * Time.deltaTime);
                transform.LookAtDir(direction);
            }

            c_material.tintColor.B += c_material.tintColor.B < 1 ? Time.deltaTime * 1.5f : 0;
            c_material.tintColor.G += c_material.tintColor.G < 1 ? Time.deltaTime * 1.5f : 0;

            // manually check collision with player, using collisiion system is fine but the radiuses of the collision
            // spheres are too large so the collision happens too fast
            if (Utilities.Distance(player.transform.position, transform.position) < 0.75f)
            {
                player.OnDroneHit();
            }
        }

        internal void ApplyDamage(float damage)
        {
            currentLife -= damage;
            c_material.tintColor = Color4.Red;

            GetComponent<ComponentSound>().playsoundonce(1);
            if (currentLife < 0)
            {
                isDead = true;
                EntityManager.Delete(this);

                GetComponent<ComponentSound>().playsoundstop(0);
            }
        }
    }
}
