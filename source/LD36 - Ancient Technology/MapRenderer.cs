using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Physics.PhysicsComponents;
using NewEngine.Engine.Rendering;
using OpenTK;

namespace LD36___Ancient_Technology {
    public class MapRenderer {
        private MapLoader _map;
        private List<GameObject> _spawnedMap; // used for easly clearing the map
        private Mesh _cube;
        private Mesh _chest;
        private Mesh _stair;
        private Material _floorMaterial;
        private Material _wallMaterial;
        private Material _doorMaterial;
        private Material _chestMaterial;
        private Material _pressureMaterial;
        private Material _stairMaterial;
        private GameObject _player;

        public MapRenderer() {
            CreateCamera();
            _cube = new Mesh("cube.obj");
            _stair = new Mesh("stair.obj");
            _chest = new Mesh("box.obj");
            _floorMaterial = new Material(new Texture("floor.png"), 0.02f, 32);
            _wallMaterial = new Material(new Texture("wall.png"), 0.02f, 32);
            _doorMaterial = new Material(new Texture("door.png"), 0.02f, 32);
            _chestMaterial = new Material(new Texture("Chest.png"), 0.02f, 32);
            _pressureMaterial = new Material(new Texture("PressurePlate.png"), 0.02f, 32);
            _stairMaterial = new Material(new Texture("stair.png"), 0.02f, 32);
            _spawnedMap = new List<GameObject>();
        }

        public void DrawMap(string map) {
            LD36.Instance.Keys = new List<int>();

            foreach (var gameObject in _spawnedMap) {
                gameObject.Destroy();
            }

            _map = new MapLoader(map);

            var tilesize = LD36.Instance.TileSize;

            for (var y = 0; y < _map.MapHeight; y++) {
                for (var x = 0; x < _map.MapWidth; x++) {
                    MapType type = _map.Map[x + y * _map.MapWidth];
                    switch (type) {
                        case MapType.Floor:
                            CreateFloorRoof(x, y);
                            break;
                        case MapType.Wall:
                            GameObject wall = new GameObject("wall");
                            wall.AddComponent(new MeshRenderer(_cube, _wallMaterial));
                            wall.AddComponent(new BoxCollider(8, 8, 8, 0));
                            wall.Transform.Scale = new Vector3(4, 4, 4);
                            wall.Transform.Position = new Vector3(x * tilesize, 0, y * tilesize);
                            LD36.Instance.AddObject(wall);
                            _spawnedMap.Add(wall);
                            break;
                        case MapType.PlayerStart:
                            CreateFloorRoof(x, y);
                            _player.Transform.Position = new Vector3(x * 8, 0, y * 8);
                            break;
                        case MapType.Trap:
                            CreateFloorRoof(x, y);
                            GameObject trap = new GameObject("box");
                            trap.AddComponent(new MeshRenderer(_cube, _pressureMaterial));
                            trap.AddComponent(new Trap());
                            trap.Transform.Scale = new Vector3(2, 0.1f, 2);
                            trap.Transform.Position = new Vector3(x * tilesize, -3.5f, y * tilesize);
                            LD36.Instance.AddObject(trap);

                            _spawnedMap.Add(trap);
                            break;
                        case MapType.DownStair:
                            GameObject roof = new GameObject("roof");
                            roof.AddComponent(new MeshRenderer(_cube, _floorMaterial));
                            roof.Transform.Scale = new Vector3(4, 0.2f, 4);
                            roof.Transform.Position = new Vector3(x * tilesize, 3.8f, y * tilesize);
                            LD36.Instance.AddObject(roof);

                            GameObject stair = new GameObject("stair");
                            stair.AddComponent(new MeshRenderer(_stair, _stairMaterial));
                            stair.AddComponent(new BoxCollider(8, 8, 8, 0));
                            stair.AddComponent(new Stair());
                            stair.Transform.Scale = new Vector3(4, 4, 4);
                            stair.Transform.Position = new Vector3(x * tilesize, -3.8f, y * tilesize);
                            LD36.Instance.AddObject(stair);

                            _spawnedMap.Add(roof);
                            _spawnedMap.Add(stair);

                            break;
                    }


                    int chestId = _map.Chests[x + y * _map.MapWidth];
                    int doorId = _map.Doors[x + y * _map.MapWidth];

                    if (chestId > 0) {
                        CreateFloorRoof(x, y);
                        GameObject box = new GameObject("box " + chestId);
                        box.AddComponent(new MeshRenderer(_chest, _chestMaterial));
                        box.AddComponent(new BoxCollider(8, 30, 8, 0));
                        box.Transform.Scale = new Vector3(4, 4, 4);
                        box.Transform.Position = new Vector3(x * tilesize, -4, y * tilesize);
                        box.AddComponent(new Chest(chestId));
                        LD36.Instance.AddObject(box);
                        _spawnedMap.Add(box);
                    }


                    if (doorId > 0) {
                        GameObject door = new GameObject("door");
                        door.AddComponent(new MeshRenderer(_cube, _doorMaterial));
                        door.AddComponent(new BoxCollider(8, 8, 8, 0));
                        door.AddComponent(new Door(doorId));
                        door.Transform.Scale = new Vector3(4, 4, 4);
                        door.Transform.Position = new Vector3(x * tilesize, 0, y * tilesize);
                        LD36.Instance.AddObject(door);
                        _spawnedMap.Add(door);
                    }
                }
            }
        }

        public void CreateCamera() {

            _player = new GameObject("main camera")
                //.AddComponent(new FreeLook(true, true)).AddComponent(new FreeMove())
                .AddComponent(new CharacterController())
                .AddComponent(new Camera(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70.0f), CoreEngine.GetWidth() / CoreEngine.GetHeight(), 0.1f, 1000)));


            _player.Transform.Rotate(new Vector3(0, 1, 0), MathHelper.DegreesToRadians(180));

            LD36.Instance.AddObject(_player);
            var flameColor = new Vector3(0.6f, 1, 1);

            var newPointLight =
                new GameObject("point light").AddComponent(new SpotLight(flameColor, 0.2f, new Attenuation(0, 0, 0.01f), MathHelper.DegreesToRadians(128)));

            var pos = CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.GetTransformedPosition();

            newPointLight.Transform.Position =
                new Vector3(pos.X + 1, 0, pos.Y - 1);

            //_player.AddChild(newPointLight);
        }

        public void CreateFloorRoof(int x, int y) {
            var tilesize = LD36.Instance.TileSize;

            GameObject floor = new GameObject("floor");
            floor.AddComponent(new MeshRenderer(_cube, _floorMaterial));
            floor.Transform.Scale = new Vector3(4, 0.2f, 4);
            floor.Transform.Position = new Vector3(x * tilesize, 3.8f, y * tilesize);
            LD36.Instance.AddObject(floor);

            GameObject roof = new GameObject("roof");
            roof.AddComponent(new MeshRenderer(_cube, _floorMaterial));
            roof.Transform.Scale = new Vector3(4, 0.2f, 4);
            roof.Transform.Position = new Vector3(x * tilesize, -3.8f, y * tilesize);
            LD36.Instance.AddObject(roof);

            _spawnedMap.Add(floor);
            _spawnedMap.Add(roof);

        }
    }
}
