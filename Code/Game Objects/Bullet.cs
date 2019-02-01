using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Game_Objects
{
    public class Bullet : Entity
    {
        private float damage;

        public Bullet(string name, float damage) : base(name)
        {
            this.damage = damage;

            AddComponent(new ComponentGeometry("sphere.obj"));
            AddComponent(new ComponentMaterial("Bullet.png"));
            transform.scale = new Vector3(0.1f);
            AddComponent(new ComponentCollider_Sphere(0.07f, true, transform));
        }

        public override void OnTriggerEnter(Entity col)
        {
            if (col is Player) return;

            if (col is Drone)
            {
                (col as Drone).ApplyDamage(damage);
            }
            EntityManager.Delete(this);
        }
    }
}
