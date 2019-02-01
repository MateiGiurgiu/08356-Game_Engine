using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Game_Objects
{
    public class Player : Entity
    {
        public static Action ResetEvent;

        public List<BasicWeapon> weapons = new List<BasicWeapon>();
        public int Life = 3;
        public Camera attachedCamera;
        public float baseMoveSpeed = 2f, moveSpeed = 2f;
        private float runSpeed = 4f, runtime = 8f;
        private float moveSpeedBuff = 4f, buffTime = 8f;
        private float sinValue = 0;
        public float mouseSensitivity = 0.001f;
        public float playerfloorheighj = 0;
        private bool onground = true;
        private bool moveBuffActive = false;
        Vector3 listenerDirection = new Vector3(0, 0, -1);
        Vector3 listenerUp = Vector3.UnitY;

        private BasicWeapon[] weapon = new BasicWeapon[2];

        public Player(string name, Camera camera, EntityManager entityManager) : base(name)
        {
            callUpdate = true;
            camera.transform = transform;
            attachedCamera = camera;
            attachedCamera.deltaPosition.Z = -0.4f;

            AddComponent(new ComponentGeometry("Arm Only.obj"));
            AddComponent(new ComponentMaterial("arm.png"));
            AddComponent(new ComponentCollider_Sphere(0.46f, transform));
            AddComponent(new ComponentVelocity(0,0,0));
            AddComponent(new ComponentSound(new string[] { "Audio/death.wav" }));
            transform.scale = new Vector3(2);
            transform.position.Y = 1.2f;

            // set up first weapon
            weapon[0] = new BasicWeapon("Weapon 1", damage: 30);
            weapon[0].AddComponent(new ComponentGeometry("Weapon1.obj"));
            weapon[0].AddComponent(new ComponentMaterial("Weapon1.png"));
            weapon[0].AddComponent(new ComponentSound(new string[] {"Audio/weapon.wav" }));
            entityManager.AddEntity(weapon[0]);

            ResetEvent = null;
            ResetEvent += ResetPlayer;
        }

        public void ResetPlayer()
        {
            transform.position = new Vector3(0, 1.2f, 0);
        }

        public override void Update()
        {

            //float X = (InputManager.GetKeyDown(Key.Left) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.Right) ? -1.0f : 0.0f);
            float X = 0;
            float Y = 0;
            float Z = (InputManager.GetKeyDown(Key.Up) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.Down) ? -1.0f : 0.0f);
            if (onground)
            {
                Y = (InputManager.GetKeyDown(Key.Space) ? 10f : 0.0f);
            }
            if (InputManager.GetKeyDown(Key.Up) && InputManager.GetKeyDown(Key.ShiftLeft))
            {
                if (runtime > 0)
                {
                    Z = runSpeed;
                    runtime -= Time.deltaTime * 10;
                }
            }
            else
            {
                if (runtime < 8f)
                    runtime += 1 * Time.deltaTime * 10;
            }
            if (transform.position.Y > 1.2f)
            {
                onground = false;
                Vector3 movement = Z * transform.forward + X * transform.right + Y * transform.up;
                transform.Translate(new Vector3(movement.X * moveSpeed * Time.deltaTime, (Y - 0.7f) * moveSpeed * Time.deltaTime, movement.Z * moveSpeed * Time.deltaTime));
            }
            else
            {
                onground = true;
                Vector3 movement = Z * transform.forward + X * transform.right + Y * transform.up;
              
                Vector3 locomotion = new Vector3(movement.X * moveSpeed, Y * moveSpeed, movement.Z * moveSpeed);
                // add head bobing
                if (movement.Length > 0.1f)
                {
                    sinValue += Time.deltaTime * (Z / 4.2f);
                    attachedCamera.deltaPosition.Y = (float)Math.Sin(sinValue * 15) * 0.03f;
                }
                transform.Translate(Time.deltaTime * locomotion);
            }

            if (buffTime <= 0f && moveBuffActive)
            {
                SpeedBuffEnd();
            }

            if (moveBuffActive)
            {
                buffTime -= 1f;
            }
            
            Vector3 rot = new Vector3(transform.rotation.X, transform.rotation.Y, transform.rotation.Z);
            AL.Listener(ALListener3f.Position, ref transform.position);
            AL.Listener(ALListenerfv.Orientation, ref rot, ref listenerUp);

            // rotation
            //AddRotation(-InputManager.GetMouseDelta().X, -InputManager.GetMouseDelta().Y);
            AddRotation(-InputManager.GetMouseDelta().X, 0);


            // sync weapone transform with player transform
            weapon[0].transform.position = transform.position;
            weapon[0].transform.rotation = transform.rotation;
            weapon[0].transform.scale = transform.scale;
        }

        public void AddRotation(float x, float y)
        {
            x = -x * mouseSensitivity;
            y = y * mouseSensitivity;

            Quaternion rotQuatY = Quaternion.FromAxisAngle(Vector3.UnitY, x);
            Quaternion rotQuatX = Quaternion.FromAxisAngle(transform.right, y);

            transform.rotation = (rotQuatY * rotQuatX) * transform.rotation;
        }

        public void IncreaseLife()
        {
            Life += Life < 3 ? 1 : 0; 
        }

        public void SpeedBuff()
        {
            buffTime = 800f;
            moveBuffActive = true;
            moveSpeed = moveSpeedBuff;
        }

        public void SpeedBuffEnd()
        {
            if (buffTime <= 0f)
            {
                buffTime = 0f;
            }

            moveBuffActive = false;
            moveSpeed = baseMoveSpeed;
        }

        public void OnDroneHit()
        {
            GetComponent<ComponentSound>().playsoundonce(0);
            Life--;
            SpeedBuffEnd();
            ResetEvent?.Invoke();
        }
    }
}
