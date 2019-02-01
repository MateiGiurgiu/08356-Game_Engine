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
    public class OrbitCamera : Entity
    {
        public float moveSpeed = 2f;
        public float mouseSensitivity = 0.001f;
        Vector3 listenerDirection = new Vector3(0, 0, -1);
        Vector3 listenerUp = Vector3.UnitY;

        public OrbitCamera(string name, Camera camera) : base(name)
        {
            callUpdate = true;
            camera.transform = transform;
        }

        public override void Update()
        {
            // movement
            float X = (InputManager.GetKeyDown(Key.A) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.D) ? -1.0f : 0.0f);
            float Y = (InputManager.GetKeyDown(Key.E) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.Q) ? -1.0f : 0.0f);
            float Z = (InputManager.GetKeyDown(Key.W) ? 1.0f : 0.0f) + (InputManager.GetKeyDown(Key.S) ? -1.0f : 0.0f);
            Vector3 movement = Z * transform.forward + X * transform.right + Y * transform.up;
            transform.Translate(movement * moveSpeed * Time.deltaTime);

            AL.Listener(ALListener3f.Position, ref transform.position);
            AL.Listener(ALListenerfv.Orientation, ref listenerDirection, ref listenerUp);
            

            // rotation
            AddRotation(-InputManager.GetMouseDelta().X, -InputManager.GetMouseDelta().Y);
        }

        public void AddRotation(float x, float y)
        {
            x = -x * mouseSensitivity;
            y = y * mouseSensitivity;
            Quaternion rotQuatY = Quaternion.FromAxisAngle(Vector3.UnitY, x);
            Quaternion rotQuatX = Quaternion.FromAxisAngle(transform.right, y);

           // AL.Listener(ALListenerfv.Orientation, ref direc);

            // do not change the order of the quaternion multiplication, the * operator in quaternions is NOT
            // commutative => q1 * q2 != q2 * q1
            transform.rotation = (rotQuatY * rotQuatX) * transform.rotation;
        }
    }
}
