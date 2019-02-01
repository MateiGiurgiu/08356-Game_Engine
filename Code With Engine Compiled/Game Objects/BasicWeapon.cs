using OpenGL_Game.Components;
using OpenGL_Game.Game_Objects;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Game_Objects
{
    public class BasicWeapon : Entity
    {
        public Bullet Bullet;
        public int CountsBullets = 50; 
        private int bulletSpeed = 15;
        private float pretime = 0;
        private bool fireOR = true;
        private int burst = 1;
        private float damage = 10;

        private EntityManager entityManager;

        public BasicWeapon(string name, float damage) : base(name)
        {
            this.damage = damage;
            callUpdate = true;
        }

        public override void Update()
        {
            if (CountsBullets > 0 && InputManager.MousestateDown(OpenTK.Input.MouseButton.Left) && burst == 1)
            {
                burst--;
                Fire();
            }
            else if(InputManager.MousestateUp(OpenTK.Input.MouseButton.Left) && burst == 0)
            {
                burst++;
            }
        }

        public void Fire()
        {
            GetComponent<ComponentSound>().playsoundonce(0);
            Bullet = new Bullet("Bullet" + Time.timeSinceGameStarted, damage);
            Bullet.transform.position = transform.position;
            Bullet.transform.rotation = transform.rotation;
            Bullet.AddComponent(new ComponentVelocity(new Vector3(transform.forward * bulletSpeed)));
            EntityManager.Create(Bullet);
            CountsBullets -= 1;
        }
    }

}
