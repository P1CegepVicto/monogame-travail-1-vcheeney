using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet1
{
    class BulletHero
    {
        public Vector2 position;
        public Vector2 origine;

        public Vector2 direction;
        public Texture2D sprite;
        public bool estVivant;
        public float vitesse;
        public float angle;


        public BulletHero(int positionX, int positionY, int origineX, int origineY, bool estVivant)
        {
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = estVivant;
        }

        public BulletHero(int positionX, int positionY, int origineX, int origineY, bool estVivant, int health)
        {
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = estVivant;
        }

        public BulletHero(int positionX, int positionY, int origineX, int origineY, bool estVivant, int health, float angle)
        {
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = estVivant;
            this.angle = angle;
        }

        public Rectangle rectColision = new Rectangle();
        public Rectangle hitbox(int nbP)
        {
            rectColision.X = (int)this.position.X + nbP;
            rectColision.Y = (int)this.position.Y + nbP;
            rectColision.Width = this.sprite.Width - (nbP * 2);
            rectColision.Height = this.sprite.Height - (nbP * 2);

            return rectColision;
        }



    }
}
