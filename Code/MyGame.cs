using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using OpenGL_Game.Game_Objects;
using OpenTK.Graphics;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : GameWindow
    {
        public enum GameState
        {
            MainMenu,
            MainGame,
            GameOver,
            GameWin
        }

        public static GameState gameState = GameState.MainMenu;
        public static MyGame gameInstance;
        readonly EntityManager entityManager;
        readonly SystemManager systemManager;

        // game related variables
        private Camera mainCamera;
        private Camera miniMapCamera;
        public Light mainLight { get; private set; }
        private int numberOfDrones = 3;
        private Entity[] lifeHearths = new Entity[3];
        private Player player;
        private Drone[] drones;
        private Entity entity;
        private float lastKeyPress = 0;
        private float stateEnterTime;

        public MyGame() : base(1280, 760)
        {
            Title = "We're All Doomed";
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            AudioContext AC = new AudioContext();
            InputManager.Initialise(this);
        }

        public void StartMainMenu()
        {
            gameState = GameState.MainMenu;
            stateEnterTime = Time.timeSinceGameStarted;
            CursorVisible = true;

            entityManager.DeleteAllEntities();
            entity = new Entity("Main Menu Background", new ComponentUI("MainMenu.png"));
            entity.transform.scale = new Vector3(1280, 760, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 2, 0);
            entityManager.AddEntity(entity);

            entity = new Entity("Click To Start", new ComponentUI("Click To Start.png"));
            entity.transform.scale = new Vector3(491, 70, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 3f, 0f);
            entityManager.AddEntity(entity);

            systemManager.DeleteAllSystems();
            systemManager.AddSystem(new SystemRenderUI(Width, Height));
        }

        public void StartMainGame()
        {
            gameState = GameState.MainGame;

            CursorVisible = false;

            entityManager.DeleteAllEntities();
            Entity entity;

            // light
            mainLight = new Light("Directional Light");
            mainLight.transform.position = new Vector3(40, 100, 20);
            mainLight.transform.LookAt(Vector3.Zero);

            // environment
            List<Vector3> AIroute;
            EnvironmentGenerator.GenerateFromFile(@"Map Layout/Layout1.txt", entityManager, out AIroute);

            // HUD/UI creation
            lifeHearths[0] = new Entity("Star 1", new ComponentUI("Hearth Icon.png"));
            lifeHearths[0].transform.position.Xy = new Vector2(Width * 0.85f, Height * 0.95f);
            lifeHearths[0].transform.scale = new Vector3(75f);
            entityManager.AddEntity(lifeHearths[0]);

            lifeHearths[1] = new Entity("Star 2", new ComponentUI("Hearth Icon.png"));
            lifeHearths[1].transform.position.Xy = new Vector2(Width * 0.91f, Height * 0.95f);
            lifeHearths[1].transform.scale = new Vector3(75f);
            entityManager.AddEntity(lifeHearths[1]);

            lifeHearths[2] = new Entity("Star 3", new ComponentUI("Hearth Icon.png"));
            lifeHearths[2].transform.position.Xy = new Vector2(Width * 0.97f, Height * 0.95f);
            lifeHearths[2].transform.scale = new Vector3(75f);
            entityManager.AddEntity(lifeHearths[2]);

            entity = new Entity("Mini Map", new ComponentUI(miniMapCamera.activeRenderTexture, "RenderTexture"));
            entity.transform.position.Xy = new Vector2(Width * 0.12f, Height * 0.2f);
            entity.transform.scale = new Vector3(300f);
            entityManager.AddEntity(entity);

            entity = new Entity("Crosshair", new ComponentUI("Crosshair.png"));
            entity.transform.position.Xy = new Vector2(Width * 0.5f, Height * 0.5f);
            entity.transform.scale = new Vector3(150f);
            entityManager.AddEntity(entity);

            // player
            player = new Player("Orbit Camera", mainCamera, entityManager);
            entityManager.AddEntity(player);

            // create drones
            Drone.isDisabled = false;
            drones = new Drone[numberOfDrones];
            int[] startIndices = CustomRandom.GetDifferentValues(numberOfDrones, 0, AIroute.Count - 1);
            for (int i = 0; i < numberOfDrones; i++)
            {
                drones[i] = new Drone("Drone" + (i + 1).ToString(), AIroute, startIndices[i], player);
                entityManager.AddEntity(drones[i]);
            }

            entity = new Entity("Skybox", new ComponentCube(new string[]
                {
                    "Textures/skybox_ft.png",
                    "Textures/skybox_bk.png",
                    "Textures/skybox_up.png",
                    "Textures/skybox_dn.png",
                    "Textures/skybox_lf.png",
                    "Textures/skybox_rt.png"
                }));
            entityManager.AddEntity(entity);

            systemManager.DeleteAllSystems();
            systemManager.AddSystem(new SystemBehaviour());
            systemManager.AddSystem(new SystemPhysics());
            systemManager.AddSystem(new SystemRender(miniMapCamera, mainLight));
            systemManager.AddSystem(new SystemRender(mainCamera, mainLight));
            systemManager.AddSystem(new SystemRenderSkybox(mainCamera, mainLight));
            systemManager.AddSystem(new SystemCollision(entityManager));
            systemManager.AddSystem(new SystemRenderUI(Width, Height));
        }

        public void StartGameOver()
        {
            gameState = GameState.GameOver;
            stateEnterTime = Time.timeSinceGameStarted;
            CursorVisible = true;

            entityManager.DeleteAllEntities();
            entity = new Entity("Game Over Background", new ComponentUI("GameOver.png"));
            entity.transform.scale = new Vector3(1280, 760, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 2, 0);
            entityManager.AddEntity(entity);

            entity = new Entity("Click To Restart", new ComponentUI("Click To Restart.png"));
            entity.transform.scale = new Vector3(406, 70, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 5f, 0f);
            entityManager.AddEntity(entity);

            systemManager.DeleteAllSystems();
            systemManager.AddSystem(new SystemRenderUI(Width, Height));
        }

        public void StartGameWin()
        {
            gameState = GameState.GameWin;
            stateEnterTime = Time.timeSinceGameStarted;
            CursorVisible = true;

            entityManager.DeleteAllEntities();
            entity = new Entity("Game Over Background", new ComponentUI("GameWin.png"));
            entity.transform.scale = new Vector3(1280, 760, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 2, 0);
            entityManager.AddEntity(entity);

            entity = new Entity("Click To Restart", new ComponentUI("Click To Restart.png"));
            entity.transform.scale = new Vector3(406, 70, 0);
            entity.transform.position = new Vector3(Width / 2, Height / 4f, 0f);
            entityManager.AddEntity(entity);


            systemManager.DeleteAllSystems();
            systemManager.AddSystem(new SystemRenderUI(Width, Height));
        }

        /// <summary>
        /// Allows the game to setup the environment and matrices.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);



            // create cameras
            mainCamera = new Camera(Height, Width, 45f, 0.1f, 200f);
            miniMapCamera = new Camera(Matrix4.CreateOrthographic(26, 26, 0.01f, 120));
            miniMapCamera.transform.position = new Vector3(0, 40, 0);
            miniMapCamera.transform.RotateX(90);
            miniMapCamera.activeRenderTexture = new RenderTexture(Width, Height);

            StartMainMenu();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Time.deltaTime = (float)e.Time;
            entityManager.ManageEntities();

            switch (gameState)
            {
                case GameState.MainMenu:
                    entity = entityManager.getEntity("Click To Start");
                    if(entity != null)
                    {
                        entity.transform.position.Y = Height / 3f + (float)Math.Sin(Time.timeSinceGameStarted * 2.5f) * 20;
                    }
                    if (InputManager.MousestateDown(MouseButton.Left) && stateEnterTime + 0.5f < Time.timeSinceGameStarted)
                    {
                        StartMainGame();
                    }
                    break;
                case GameState.MainGame:
                    // debug things
                    if(InputManager.GetKeyDown(Key.C) && lastKeyPress + 0.1 < Time.timeSinceGameStarted)
                    {
                        lastKeyPress = Time.timeSinceGameStarted;
                        player.GetComponent<ComponentCollider_Sphere>().isTriggerCollider = !player.GetComponent<ComponentCollider_Sphere>().isTriggerCollider;
                    }
                    // debug things
                    if (InputManager.GetKeyDown(Key.D) && lastKeyPress + 0.1 < Time.timeSinceGameStarted)
                    {
                        lastKeyPress = Time.timeSinceGameStarted;
                        Drone.isDisabled = !Drone.isDisabled;
                    }

                    // update UI
                    for (int i = 0; i < 3; i++)
                    {
                        if (i > player.Life - 1)
                        {
                            lifeHearths[i].GetComponent<ComponentUI>().tintColor = Color4.Black;
                        }
                        else
                        {
                            lifeHearths[i].GetComponent<ComponentUI>().tintColor = Color4.White;
                        }
                    }
                    if(player.Life <= 0)
                    {
                        StartGameOver();
                    }

                    // check if all the drones are dead
                    bool allDronesDead = true;
                    for(int i = 0; i < numberOfDrones; i++)
                    {
                        if (!drones[i].isDead) allDronesDead = false;
                    }
                    if (allDronesDead) StartGameWin();
                    break;
                case GameState.GameOver:
                    entity = entityManager.getEntity("Click To Restart");
                    if (entity != null)
                    {
                        entity.transform.position.Y = Height / 5f + (float)Math.Sin(Time.timeSinceGameStarted * 2.5f) * 20;
                    }
                    if (InputManager.MousestateDown(MouseButton.Left) && stateEnterTime + 2 < Time.timeSinceGameStarted)
                    {
                        StartMainMenu();
                    }
                    break;
                case GameState.GameWin:
                    entity = entityManager.getEntity("Click To Restart");
                    if (entity != null)
                    {
                        entity.transform.position.Y = Height / 4f + (float)Math.Sin(Time.timeSinceGameStarted * 2.5f) * 20;
                    }
                    if (InputManager.MousestateDown(MouseButton.Left) && stateEnterTime + 2 < Time.timeSinceGameStarted)
                    {
                        StartMainMenu();
                    }
                    break;
                default:
                    break;
            }   

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
           
            systemManager.ActionSystems(entityManager);

            GL.Flush();
            SwapBuffers();
        }
    }
}
