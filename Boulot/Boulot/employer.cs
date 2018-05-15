using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Boulot
{
    class employer
    {
        private int id;
        private string imageEmp;

        private float rating;

        private string nom, prenom, pseudo, tele, password, ville, service;


        public employer(int id, string Nom, string Prenom, string vill, string servic,
            string tel, string imageEmp, float rating)
        {
            this.Id = id;
            this.nom = Nom;
            this.prenom = Prenom;
            this.imageEmp = imageEmp;
            this.ville = vill;
            this.service = servic;
            this.rating = rating;
            this.tele = tel;



        }

        public employer()
        {

        }
        public employer(string n, string p, string pw, string t, string pass)
        {

            this.Nom = n;
            this.Prenom = p;
            this.Pseudo = pw;
            this.Tele = t;
            this.Password = pass;



        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string Pseudo
        {
            get
            {
                return pseudo;
            }

            set
            {
                pseudo = value;
            }
        }

        public string Tele
        {
            get
            {
                return tele;
            }

            set
            {
                tele = value;
            }
        }



        public string Ville
        {
            get
            {
                return ville;
            }

            set
            {
                ville = value;
            }
        }

        public string Service
        {
            get
            {
                return service;
            }

            set
            {
                service = value;
            }
        }

        public string ImageEmp
        {
            get
            {
                return imageEmp;
            }

            set
            {
                imageEmp = value;
            }
        }

        public float Rating
        {
            get
            {
                return rating;
            }

            set
            {
                rating = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }


    }
}