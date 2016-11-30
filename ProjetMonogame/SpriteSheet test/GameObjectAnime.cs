using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheet_test
{
    class GameObjectAnime
    {
        public Texture2D sprite;
        public int vitesse;
        public Vector2 direction;
        public Rectangle position;
        public Rectangle spriteAfficher; //Le rectangle affiché à l'écran

        public enum etats { attenteDroite, attenteGauche, runDroite, runGauche };
        public etats objetState;


        //Compteur qui changera le sprite affiché
        public int cpt = 0;

        //GESTION DES TABLEAUX DE SPRITES (chaque sprite est un rectangle dans le tableau)
        public int runState = 0; //État de départ
        public int nbEtatRun = 6; //Combien il y a de rectangles pour l’état “courrir”

        public Rectangle[] tabRunDroite = {
            new Rectangle(60, 30, 65, 65),
            new Rectangle(130, 30, 65, 65),
            new Rectangle(193, 30, 65, 65),
            new Rectangle(260, 30, 65, 65),
            new Rectangle(320, 30, 65, 65),
            new Rectangle(385, 30, 65, 65) };

        public Rectangle[] tabRunGauche = {
            new Rectangle(60, 95, 65, 65),
            new Rectangle(130, 95, 65, 65),
            new Rectangle(193, 95, 65, 65),
            new Rectangle(260, 95, 65, 65),
            new Rectangle(320, 95, 65, 65),
            new Rectangle(385, 95, 65, 65) };

        public int waitState = 0;
        public Rectangle[] tabAttenteDroite =
        {
            new Rectangle(194, 160, 65, 65)
        };


        public Rectangle[] tabAttenteGauche =
        {
            new Rectangle(194, 225, 65, 65)
        };


    }
}
