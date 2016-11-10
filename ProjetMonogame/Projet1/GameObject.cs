using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet1
{
    class GameObject
    {
        public Vector2 position;
        public Vector2 origine;

        public Vector2 direction;
        public Texture2D sprite;
        public bool estVivant;
        public float vitesse = 2;

        public float Width;
        public float Height;

        public GameObject(int positionX, int positionY, int origineX, int origineY, bool estVivant)
        {
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = true;
        }
    }
}
