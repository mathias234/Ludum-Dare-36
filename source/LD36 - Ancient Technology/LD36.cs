using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Image = NewEngine.Engine.components.UIComponents.Image;

namespace LD36___Ancient_Technology {
    public class LD36 : Game {
        public static LD36 Instance;

        private GameObject _directionalLightObj;

        public int TileSize = 8;
        public MapRenderer MapRenderer;

        public List<int> Keys;


        public int currentLevel;

        private bool _onLastLevel = false;

        public float Health = 100;

        public override void Start() {
            Instance = this;

            MapRenderer = new MapRenderer();

            MapRenderer.DrawMap("map1.png");
            currentLevel = 1;


            _directionalLightObj = new GameObject("Directinal Light");
            var directionalLight = new DirectionalLight(new Vector3(1), 0.5f);
            _directionalLightObj.AddComponent(directionalLight);
            _directionalLightObj.Transform.Rotation *= Quaternion.FromAxisAngle(new Vector3(1, 0, 0), (float)MathHelper.DegreesToRadians(-40));

            //AddObject(_directionalLightObj);
            CoreEngine.GetCoreEngine.RenderingEngine.SetSkybox("skybox/top.jpg", "skybox/bottom.jpg", "skybox/front.jpg",
                "skybox/back.jpg", "skybox/left.jpg", "skybox/right.jpg");

            healthBar = new GameObject("health bar");

            healthBarImage = new Image(new RectTransform(25, 300, 0, -(CoreEngine.GetHeight() / 2) + 90), Color.White,
                new Texture("healthbar.png"));
            healthBar.Transform.Rotation *= Quaternion.FromAxisAngle(new Vector3(1, 1, 1), MathHelper.DegreesToRadians(90));
            healthBar.AddComponent(healthBarImage);

            healthBar.IsActive = true;

            AddObject(healthBar);

            dead = new GameObject("dead");
            dead.AddComponent(new Image(new RectTransform(746, 207, 0, 100), Color.White,
                new Texture("dead.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));
            AddObject(dead);
        }

        public void NextLevel() {
            currentLevel++;

            if (File.Exists("./res/Textures/map" + currentLevel + ".png"))
                MapRenderer.DrawMap("map" + currentLevel + ".png");
            else {
                LogManager.Debug("hey");
                _onLastLevel = true;
                var end = new GameObject("end").AddComponent(new Image(new RectTransform(650, 650, 0, 100), Color.White, new Texture("end.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));

                end.IsActive = true;

                AddObject(end);

                MapRenderer.DrawMap("lastLevel.png");
            }
        }

        private float _timeToApplicationQuit = 10;
        private GameObject healthBar;
        private Image healthBarImage;
        private GameObject dead;

        public override void Update(float deltaTime) {
            if (Health <= 0) {
                dead.IsActive = true;
                return;
            }

            // if base.Update is not called the game will "freeze"
            base.Update(deltaTime);

            var healthbarSize = (Health - 0) / (100 - 0) * 300;

            healthBarImage._rectTransform.Scale = new Vector3(healthbarSize, 25, 0);

            if (_onLastLevel) {
                _timeToApplicationQuit -= deltaTime;

                if (_timeToApplicationQuit <= 0)
                    CoreEngine.GetCoreEngine.Exit();
            }
        }

    }
}
