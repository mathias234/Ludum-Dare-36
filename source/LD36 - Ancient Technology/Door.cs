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
    public class Door : GameComponent {
        private int _id;
        private GameObject _pressFToOpen;

        public Door(int id) {
            _id = id;
            _pressFToOpen = new GameObject("pressFToLoot");
            _pressFToOpen.AddComponent(new Image(new RectTransform(650, 650, 0, 100), Color.White,
                new Texture("openDoorText.png", TextureTarget.Texture2D, TextureMinFilter.Nearest)));
            LD36.Instance.AddObject(_pressFToOpen);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            var dist = CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.Position.Distance(Transform.Position);

            if (Math.Abs(dist - 8) < 1) {
                _pressFToOpen.IsActive = true;
                if (Input.GetKeyDown(Key.F)) {
                    if (LD36.Instance.Keys.Contains(_id)) {
                        _pressFToOpen.Destroy();
                        gameObject.Destroy();
                    }
                }
            }
            else {
                _pressFToOpen.IsActive = false;
            }
        }
    }
}
