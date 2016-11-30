using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Question5
{
    class Program
    {

        static int position = 0;
        static int tentative = 1;
        static bool possible = true;

        static bool[] tab = new bool[100];

        static void Main(string[] args)
        {
            //DÉCLARATION DES VARIABLES
            tab[0] = true;
            tab[99] = true;
            int valeur;

            int typeaffichage = 0;


            ConsoleKeyInfo cki;

            Random de = new Random();

            //DÉBUT DU PROGRAMME;
            #region Random de mes valeurs de 1 à 98
            for (int i = 1; i<99; i++)
            {
                valeur = de.Next(0, 2);
                if (valeur == 0)
                {
                    tab[i] = true;
                }
                if (valeur == 1)
                {
                    tab[i] = false;
                }
            }
            #endregion

            //Première commande;
            cki = Console.ReadKey();

            //Lancement de la boucle du jeu;
            while (cki.Key != ConsoleKey.Q)
            {
                int nbfaux = 0;
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Y)
                {
                    typeaffichage = 0;
                }

                if (cki.Key == ConsoleKey.P)
                {
                    typeaffichage = 1;
                }

                //Position du joueur
                if (cki.Key == ConsoleKey.A)
                {
                    position -= 3;
                }
                if (cki.Key == ConsoleKey.S)
                {
                    position -= 2;
                }
                if (cki.Key == ConsoleKey.D)
                {
                    position -= 1;
                }
                if (cki.Key == ConsoleKey.G)
                {
                    position += 2;
                }
                if (cki.Key == ConsoleKey.H)
                {
                    position += 4;
                }
                if (cki.Key == ConsoleKey.M)
                {
                    position = 99;
                }


                #region Vérification si le joueur est sur une case false. Si oui, le ramener au début et +1 aux tentatives;
                if (position < 0)
                {
                    tentative++;
                    position = 0;
                }

                try
                {
                    if (tab[position] == false)
                    {
                        tentative++;
                        position = 0;
                    }
                }
                catch { }
                #endregion
                #region Vérification si le tableau est possible:
                try { 
                    for (int i = position+1; i<position+5; i++)
                    {
                        if (tab[i] == false)
                            nbfaux++;
                    }
                    if (nbfaux == 4)
                        possible = false;
                }
                catch { }

                #endregion

                #region Affichage du jeu;
                //Affichage du jeu:
                Console.Clear();
                Console.WriteLine("Tentative #"+tentative);
                Console.WriteLine("Position du personnage: "+position);
                
                Console.WriteLine();
                if (typeaffichage == 0)
                {
                    AffichageEntier();
                }
                if (typeaffichage == 1)
                {
                    Affichage10();
                }
                if (possible == false)
                {
                    Console.WriteLine("TABLEAU IMPOSSIBLE!");
                }
                if (position == 99)
                {
                    Console.Clear();
                    Console.WriteLine("VOUS AVEZ GAGNÉ EN "+tentative+" ESSAIS!");
                    Console.WriteLine("FÉLICITATIONS!");
                    Console.WriteLine("Appuyez sur Q pour quitter.");
                }
                    #endregion
            }
        }




        static void AffichageEntier()
        {
            for (int i = 0; i < tab.Length; i++)
            {
                if (i == position)
                    Console.ForegroundColor=ConsoleColor.Blue;
                Console.Write(tab[i]);
                    Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();

        }

        static void Affichage10()
        {
            if (position < tab.Length - 10)
            {
                for (int i = position; i < position + 10; i++)
                {
                    if (i == position)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(tab[i] + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                Console.WriteLine("  O");
                Console.WriteLine("  |");
            }
            else
            {
                for (int i = tab.Length-10; i < tab.Length; i++)
                {
                    if (i == position)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write(tab[i] + " ");

                        Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                Console.WriteLine("  O");
                Console.WriteLine("  |");
            }
        }







        
    }
}
