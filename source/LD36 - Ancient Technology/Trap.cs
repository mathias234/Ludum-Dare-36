using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Rendering;
using OpenTK.Graphics.OpenGL;
using Image = NewEngine.Engine.components.UIComponents.Image;

namespace LD36___Ancient_Technology {
    public class Trap : GameComponent {
        private GameObject steppedOnTrap;


        public Trap() {
            steppedOnTrap = new GameObject("pressFToLoot");
            steppedOnTrap.AddComponent(new Image(new RectTransform(1600 /2, 552 / 2, 0, 300), Color.White,
                new Texture("steppedOnTrap.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));
            LD36.Instance.AddObject(steppedOnTrap);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            var dist =
                CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.Position.Distance(Transform.Position);
            if (Math.Abs(dist - 3.5f) < 1) {
                steppedOnTrap.IsActive = true;
            }
            else
                steppedOnTrap.IsActive = false;

            if (steppedOnTrap.IsActive) {
                LD36.Instance.Health -= 0.7f;

                if (LD36.Instance.Health <= 0) {
                    steppedOnTrap.IsActive = false;
                }
            }
        }
    }
}
