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
using OpenTK.Input;
using Image = NewEngine.Engine.components.UIComponents.Image;

namespace LD36___Ancient_Technology {
    public class Chest : GameComponent {
        private int _id;
        private GameObject pressFToLoot;
        private bool alreadyLooted;


        public Chest(int id) {
            _id = id;
            pressFToLoot = new GameObject("pressFToLoot");
            pressFToLoot.AddComponent(new Image(new RectTransform(650, 650,0,100), Color.White,
                new Texture("lootChestText.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));
            LD36.Instance.AddObject(pressFToLoot);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (alreadyLooted == true) {
                pressFToLoot.IsActive = false;
                return;
            }

            var dist = CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.Position.Distance(Transform.Position);
            if (Math.Abs(dist - 9) < 1) {
                if (LD36.Instance.Health <= 0) {
                    pressFToLoot.IsActive = false;
                }

                pressFToLoot.IsActive = true;
                if (Input.GetKeyDown(Key.F)) {
                    LD36.Instance.Keys.Add(_id);
                    alreadyLooted = true;
                }
            }
            else
                pressFToLoot.IsActive = false;

        }
    }
}
