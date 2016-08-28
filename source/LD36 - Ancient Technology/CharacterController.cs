using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Physics;
using OpenTK;
using OpenTK.Input;

namespace LD36___Ancient_Technology {
    public class CharacterController : GameComponent {
        private bool _useColliders = true;

        public override void Update(float deltaTime) {
            RayCastResult result;
            PhysicsEngine.Raycast(new Ray(Transform.Position, -Transform.Forward), 100, out result);

            //if (Input.GetKeyDown(Key.C))
            //    _useColliders = !_useColliders;

            //if (result != null)
            //    LogManager.Debug(Transform.Position.Distance(result.HitData.Location).ToString());

            if (Input.GetKeyDown(Key.W)) {
                if (_useColliders == true && result != null && result.HitData.Location != Vector3.Zero &&
                    (int)Transform.Position.Distance(result.HitData.Location) == LD36.Instance.TileSize / 2) {
                    return;
                }
                Transform.Position -= Transform.Forward * LD36.Instance.TileSize;
            }

            PhysicsEngine.Raycast(new Ray(Transform.Position, Transform.Forward), 100, out result);

            if (Input.GetKeyDown(Key.S)) {
                if (_useColliders == true && result != null && result.HitData.Location != Vector3.Zero &&
                    (int)Transform.Position.Distance(result.HitData.Location) == LD36.Instance.TileSize / 2) {
                    return;
                }
                Transform.Position += Transform.Forward * LD36.Instance.TileSize;
            }

            if (Input.GetKeyDown(Key.A)) {
                Transform.Rotate(Vector3.UnitY, MathHelper.DegreesToRadians(90));
            }
            if (Input.GetKeyDown(Key.D)) {
                Transform.Rotate(Vector3.UnitY, -MathHelper.DegreesToRadians(90));
            }
        }
    }
}
