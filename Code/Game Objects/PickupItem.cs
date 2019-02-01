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
    public enum PickupType
    {
        Health,
        Weapon,
        Speed
    }

    public class PickupItem : Entity
    {
        public PickupType pickupType { private set; get; }

        private Vector3 initialPosition;

        public PickupItem(string name, PickupType type, Vector3 position) : base(name)
        {
            callUpdate = true;
            this.pickupType = type;
            initialPosition = position;
            transform.position = position;
            switch(pickupType)
            {
                case PickupType.Health:
                    AddComponent(new ComponentGeometry("health.obj"));
                    AddComponent(new ComponentMaterial("health.png"));
                    AddComponent(new ComponentCollider_Sphere(0.15f, true, transform));
                    AddComponent(new ComponentSound(new string[] { "Audio/helthUp.wav" }));
                    break;
                case PickupType.Speed:
                    AddComponent(new ComponentGeometry("speed.obj"));
                    AddComponent(new ComponentMaterial("Bullet.png"));
                    AddComponent(new ComponentCollider_Sphere(0.15f, true, transform));
                    AddComponent(new ComponentSound(new string[] { "Audio/speedUp.wav" }));
                    break;

            }
        }

        public override void Update()
        {
            transform.RotateY(20 * Time.deltaTime);
            transform.position.Y = initialPosition.Y + ((float)(Math.Sin(Time.timeSinceGameStarted * 4)) * 0.12f);
        }

        public override void OnTriggerEnter(Entity col)
        {
            if(col is Player)
            {
                switch (pickupType)
                {
                        case PickupType.Health:
                        (col as Player).IncreaseLife();
                        GetComponent<ComponentSound>().playsoundonce(0);
                        break;
                        case PickupType.Speed:
                        (col as Player).SpeedBuff();
                        GetComponent<ComponentSound>().playsoundonce(0);
                        break;
                }

            }
            EntityManager.Delete(this);
        }
    }
}
