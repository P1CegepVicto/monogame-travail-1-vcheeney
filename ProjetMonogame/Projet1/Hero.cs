using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet1
{
    class Hero
    {
        //Position du héro
        public Vector2 position;
        public Vector2 origine;

        public Vector2 positioncanon;
        public Vector2 originecanon;
        public Texture2D spritecanon;

        public Vector2 direction;
        public Texture2D sprite;
        public bool estVivant;
        public float vitesse;
        public float angle;

        public int shootdelay = 0;
        public int delay = 1 * 60;

        public int nbbullet = 0;
        public int test = 0;

        //Système de vie
        public int health;
        public Texture2D spritehealthbar;
        public Rectangle afficherhealthbar;
        public Vector2 positionhealthbar; 

        //Manière d'entrer le héros
        public Hero(int positionXhero, int positionYhero, int origineXhero, int origineYhero, bool estVivant, int health, int origineXcanon, int origineYcanon, float angle)
        {
            this.position.X = positionXhero;
            this.position.Y = positionYhero;
            this.origine.X = origineXhero;
            this.origine.Y = origineYhero;

            this.originecanon.X = origineXcanon;
            this.originecanon.Y = origineYcanon;

            this.estVivant = estVivant;

            this.health = health;
            this.angle = angle;
        }



        //Hitbox du héro
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
