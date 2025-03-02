using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Etudiant
{
    /// <summary>
    /// Classe représentant un étudiant avec ses informations personnelles et ses notes.
    /// </summary>
    public class Etudiant
    {
        public int NO { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public double NoteCC { get; set; }
        public double NoteDevoir { get; set; }
        public double Moyenne => (NoteCC * 0.33) + (NoteDevoir * 0.67);
    }

    class Program
    {
        static void Main()
        {
            SortedList<int, Etudiant> etudiants = new SortedList<int, Etudiant>();
            int nombreEtudiants = DemanderEntier("Entrez le nombre d'étudiants : ", 1, 100);

            for (int i = 0; i < nombreEtudiants; i++)
            {
                Console.WriteLine($"\nÉtudiant {i + 1} :");
                int no = DemanderEntier("Numéro d'ordre : ", 1, 9999);
                string prenom = DemanderTexte("Prénom : ");
                string nom = DemanderTexte("Nom : ");
                double noteCC = DemanderDouble("Note de contrôle continu : ", 0, 20);
                double noteDevoir = DemanderDouble("Note de devoir : ", 0, 20);
                etudiants.Add(no, new Etudiant { NO = no, Prenom = prenom, Nom = nom, NoteCC = noteCC, NoteDevoir = noteDevoir });
            }

            double moyenneClasse = etudiants.Values.Average(e => e.Moyenne);
            int lignesParPage = DemanderEntier("Nombre de lignes par page (1-15) : ", 1, 15);
            int totalPages = (int)Math.Ceiling((double)etudiants.Count / lignesParPage);
            int page = 1;

            while (true)
            {
                Console.Clear();
                AfficherPage(etudiants, page, lignesParPage, moyenneClasse, totalPages);
                Console.WriteLine("(N) Suivant, (P) Précédent, (S) Sauvegarder, (Q) Quitter");
                char choix = Console.ReadKey().KeyChar;
                if (choix == 'N' || choix == 'n')
                    page = Math.Min(page + 1, totalPages);
                else if (choix == 'P' || choix == 'p')
                    page = Math.Max(1, page - 1);
                else if (choix == 'S' || choix == 's')
                    SauvegarderFichier(etudiants);
                else if (choix == 'Q' || choix == 'q')
                    break;
            }
        }

        static int DemanderEntier(string message, int min, int max)
        {
            int valeur;
            do { Console.Write(message); } while (!int.TryParse(Console.ReadLine(), out valeur) || valeur < min || valeur > max);
            return valeur;
        }

        static string DemanderTexte(string message)
        {
            string texte;
            do { Console.Write(message); texte = Console.ReadLine(); } while (string.IsNullOrWhiteSpace(texte));
            return texte;
        }

        static double DemanderDouble(string message, double min, double max)
        {
            double valeur;
            do { Console.Write(message); } while (!double.TryParse(Console.ReadLine(), out valeur) || valeur < min || valeur > max);
            return valeur;
        }

        static void AfficherPage(SortedList<int, Etudiant> etudiants, int page, int lignesParPage, double moyenneClasse, int totalPages)
        {
            Console.WriteLine($"Page {page}/{totalPages}");
            Console.WriteLine("NO\tPrénom\tNom\tNote CC\tNote Devoir\tMoyenne");
            Console.WriteLine(new string('-', 60));

            foreach (var etudiant in etudiants.Skip((page - 1) * lignesParPage).Take(lignesParPage))
            {
                Console.WriteLine($"{etudiant.Value.NO}\t{etudiant.Value.Prenom}\t{etudiant.Value.Nom}\t{etudiant.Value.NoteCC}\t{etudiant.Value.NoteDevoir}\t{etudiant.Value.Moyenne:F2}");
            }

            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Moyenne de la classe : {moyenneClasse:F2}\n");
        }

        static void SauvegarderFichier(SortedList<int, Etudiant> etudiants)
        {
            string nomFichier = "etudiants.txt";
            using (StreamWriter writer = new StreamWriter(nomFichier))
            {
                writer.WriteLine("NO\tPrénom\tNom\tNote CC\tNote Devoir\tMoyenne");
                writer.WriteLine(new string('-', 60));

                foreach (var etudiant in etudiants.Values)
                {
                    writer.WriteLine($"{etudiant.NO}\t{etudiant.Prenom}\t{etudiant.Nom}\t{etudiant.NoteCC}\t{etudiant.NoteDevoir}\t{etudiant.Moyenne:F2}");
                }

                writer.WriteLine(new string('-', 60));
            }

            Console.WriteLine($"\nLes données ont été sauvegardées dans le fichier '{nomFichier}'.\n");
        }
    }
}
