using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEngine.Engine.Core;

namespace LD36___Ancient_Technology {
    public class MapLoader {
        public MapType[] Map;
        public int[] Doors;
        public int[] Chests;
        public int MapHeight;
        public int MapWidth;

        public MapLoader(string mapName) {
            if (File.Exists(Path.Combine("./res/textures", mapName)) == false)
                LogManager.Error("Map " + mapName + " does not exists");

            var image = new Bitmap(Path.Combine("./res/textures", mapName));

            var lbmap = new LockBitmap(image);
            lbmap.LockBits();

            Map = new MapType[image.Height * image.Width];
            Doors = new int[image.Height * image.Width];
            Chests = new int[image.Height * image.Width];

            for (var j = 0; j < image.Height; j++) {
                for (var i = 0; i < image.Width; i++) {
                    var color = lbmap.GetPixel(i, j);
                    var arrPos = i + j * image.Width;
                    if (color == Color.FromArgb(255, 0, 0))
                        Map[arrPos] = MapType.Floor;
                    else if (color == Color.FromArgb(0, 0, 255))
                        Map[arrPos] = MapType.Wall;
                    else if (color == Color.FromArgb(255, 255, 0))
                        Map[arrPos] = MapType.PlayerStart;
                    else if (color == Color.FromArgb(30, 30, 30))
                        Map[arrPos] = MapType.Trap;
                    else if (color.G == 255 && color.B == 255) {
                        Map[arrPos] = MapType.Floor;
                        Doors[arrPos] = color.R;
                    }
                    else if (color.G == 255) {
                        Map[arrPos] = MapType.Floor;
                        Chests[arrPos] = color.R;
                    }
                    else if (color == Color.FromArgb(0, 100, 100)) {
                        Map[arrPos] = MapType.DownStair;
                    }
                }
            }

            MapHeight = image.Height;
            MapWidth = image.Width;

            lbmap.UnlockBits();
            image.Dispose();
        }
    }

    public enum MapType {
        Air,
        Wall,
        Floor,
        PlayerStart,
        Trap,
        DownStair,
    }
}
