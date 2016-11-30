using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet1
{
    class Layers
    {
        public Vector2 position1;
        public Vector2 position2;

        public Texture2D sprite1;
        public Texture2D sprite2;

        public float vitesse;
        public float time;

        public Layers(int position1X, int position1Y)
        {
            this.position1.X = position1X;
            this.position1.Y = position1Y;
        }

        public Layers(int position1X, int position1Y, int position2X, int position2Y)
        {
            this.position1.X = position1X;
            this.position1.Y = position1Y;

            this.position2.X = position1X;
            this.position2.Y = position1Y;
        }
    }
}
