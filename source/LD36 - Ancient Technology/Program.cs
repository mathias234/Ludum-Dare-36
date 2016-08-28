using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEngine.Engine.Core;
using OpenTK;

namespace LD36___Ancient_Technology {
    class Program {
        static void Main(string[] args) {
            using (var engine = new CoreEngine(800 * 2, 600 * 2, VSyncMode.On, new LD36())) {
                engine.CreateWindow("LD36 - Ancient Technology");
                engine.Start();
            }
        }
    }
}
