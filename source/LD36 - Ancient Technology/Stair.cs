using System;
using System.Drawing;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Image = NewEngine.Engine.components.UIComponents.Image;

namespace LD36___Ancient_Technology {
    public class Stair : GameComponent {
        private GameObject pressFToTravel;

        public Stair() {
            pressFToTravel = new GameObject("pressFToLoot");
            pressFToTravel.AddComponent(new Image(new RectTransform(650, 650, 0, 100), Color.White,
                new Texture("nextLevelText.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));
            LD36.Instance.AddObject(pressFToTravel);
        }


        public override void Update(float deltaTime) {
            var dist = CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.Position.Distance(Transform.Position);
            if (Math.Abs(dist - 9) < 1) {
                pressFToTravel.IsActive = true;
                if (Input.GetKeyDown(Key.F)) {
                    pressFToTravel.IsActive = false;
                    LD36.Instance.NextLevel();
                }
            }
            else
                pressFToTravel.IsActive = false;
        }
    }
}
