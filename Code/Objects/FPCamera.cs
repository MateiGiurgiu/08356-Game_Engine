using OpenGL_Game.Components;
using OpenGL_Game.Game_Objects;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    class FPCamera
    {
        public float moveSpeed = 2f;
        private float runSpeed = 4f, runtime = 8f;
        public float mouseSensitivity = 0.001f;
        public float playerfloorheighj = 0;
        private bool onground = true;
        Vector3 listenerDirection = new Vector3(0, 0, -1);
        Vector3 listenerUp = Vector3.UnitY;
        BasicWeapon weapon;
        /*
        public FPCamera(string name, int height, int width, float fov) : base(name, height, width, fov)
        {
            callUpdate = true;
        }

        public FPCamera(string name, int height, int width, float fov, float near, float far) : base(name, height, width, fov, near, far)
        {
            callUpdate = true;
        }

        public override void Update()
        {
            float X = (InputManager.GetKeyDown(Key.A) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.D) ? -1.0f : 0.0f);
            float Y = 0;
            float Z = (InputManager.GetKeyDown(Key.W) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.S) ? -1.0f : 0.0f);
            if (onground)
            {
                Y = (InputManager.GetKeyDown(Key.Space) ? 10f : 0.0f);
            }
            if (InputManager.GetKeyDown(Key.W) && InputManager.GetKeyDown(Key.ShiftLeft))
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
            if (transform.position.Y > 1)
            {
                onground = false;
                Vector3 movement = Z * transform.forward + X * transform.right + Y * transform.up;
                transform.Translate(new Vector3(movement.X * moveSpeed * Time.deltaTime, (Y - 0.7f) * moveSpeed * Time.deltaTime, movement.Z * moveSpeed * Time.deltaTime));
            }
            else
            {
                onground = true;
                Vector3 movement = Z * transform.forward + X * transform.right + Y * transform.up;
                transform.Translate(new Vector3(movement.X * moveSpeed * Time.deltaTime, Y * moveSpeed * Time.deltaTime, movement.Z * moveSpeed * Time.deltaTime));
            }

            Vector3 rot = new Vector3(transform.rotation.X, transform.rotation.Y, transform.rotation.Z);
            AL.Listener(ALListener3f.Position, ref transform.position);
            AL.Listener(ALListenerfv.Orientation, ref rot, ref listenerUp);

            // rotation
            AddRotation(-InputManager.GetMouseDelta().X, -InputManager.GetMouseDelta().Y);
        }
        public void AddRotation(float x, float y)
        {
            x = -x * mouseSensitivity;
            y = y * mouseSensitivity;

            Quaternion rotQuatY = Quaternion.FromAxisAngle(Vector3.UnitY, x);
            Quaternion rotQuatX = Quaternion.FromAxisAngle(transform.right, y);

            transform.rotation = (rotQuatY * rotQuatX) * transform.rotation;
        }

        public override void OnTriggerEnter(Entity col)
        {
            Console.WriteLine("On Trigger Enter from player");
        }*/
    }
}
