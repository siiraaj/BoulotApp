using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace WebServiceBoulot
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod(MessageName = "GetEmployeXML", Description = "cette methode renvoie XML")]
        [System.Xml.Serialization.XmlInclude(typeof(Employe))]

        public Employe GetEmployeXML()
        {
            Employe e = new Employe();
            //e.id = 2;
            e.nom = "siraj";
            e.phone = "siiraaj@yahoo.fr";
            e.profession = "plombier";
            e.ville = "Marseille";
            return e;
        }

        [WebMethod(MessageName = "GetEmployeeDB", Description = "cette methode renvoie dee doneees sous format Json from the database ")]
        [System.Xml.Serialization.XmlInclude(typeof(Employe))]

        public String GetEmployeeDB()
        {
            string DB = "Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%";
            SqlConnection conn = new SqlConnection(DB);
            // here I have to creare list of contact calss
            List<Employe> contactslist = new List<Employe>();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    SqlDataReader dr;

                    SqlCommand cmd = new SqlCommand("select nom,age,ville,profession,phone from person ", conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var e = new Employe
                        {
                            //id = Int32.Parse(dr["id"].ToString()),
                            nom = dr["nom"].ToString(),
                            age = dr["age"].ToString(),
                            ville = dr["ville"].ToString(),
                            phone = dr["phone"].ToString(),
                            profession = dr["profession"].ToString()
                        };
                        contactslist.Add(e);
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {}
            finally
            {
                conn.Close();
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(contactslist);
        }

        /*********************************************partie d'inscription*********************************************/
        [WebMethod(MessageName = "inscription")]
        public string inscription(int id_ville, string nom, string prenom, int service, string tel, string pseudo, string password, string Comfirmpassword, string question, string indice)
        {
            if (nom.Equals("") || prenom.Equals("") || service.Equals("") || tel.Equals("") || password.Equals("") || Comfirmpassword.Equals("") || indice.Equals(""))
            {
                return "tous les champs sont obligatoire ! merci de verfier votre formulaire";
            }
            if (password != Comfirmpassword)
            {
                return "password n'est pas identique";
            }
            if (verfier_tel(tel) != 0)
            {
                return "le numero de telephone est deja utiliser";
            }
            if (verfier_pseudo(pseudo) != 0)
            {
                return "le nom utilisateur est deja utiliser";
            }
            if (valid_tel(tel) == false)
            {
                return "le format de telephone n'est pas correct";
            }
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO employer (ID_VILLE,PSEUDO, NOM_EMP,PRENOM_EMP,TEL_EMP,PASSWORD,QUESTION,INDICE,IMAGE) VALUES (@id_ville,@pseudo,@nom,@prenom,@tel,@password,@quest,@indice,@image)";
            command.Parameters.AddWithValue("@id_ville", id_ville);
            command.Parameters.AddWithValue("@nom", nom);
            command.Parameters.AddWithValue("@pseudo", pseudo);
            command.Parameters.AddWithValue("@prenom", prenom);
            command.Parameters.AddWithValue("@tel", tel);
            command.Parameters.AddWithValue("@password", MD5Hash(password));
            command.Parameters.AddWithValue("@quest", question);
            command.Parameters.AddWithValue("@indice", indice);
            command.Parameters.AddWithValue("@image", "www.google.com/lien_vers_image");
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            fournir(getID_emp(tel), service);
            return "votre inscription a été prise en compte! Bonne chance pour trouver un emploi";
        }
        /***MD5**/
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        //verfier si le numero de telephone et deja exist
        public int verfier_tel(string tel)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT count(*) FROM employer where TEL_EMP=@tel";
            command.Parameters.AddWithValue("@tel", tel);
            connection.Open();
            int nb = Int32.Parse(command.ExecuteScalar().ToString());
            connection.Close();
            return nb;//si le resultat est 0 ok 
        }
        //verfier si le pseudo et deja exist
        public int verfier_pseudo(string pseudo)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            connection.Open();
            string requete = "SELECT count(*) FROM employer where PSEUDO='" + pseudo + "'";
            SqlCommand cmd = new SqlCommand(requete, connection);
            int nb = Int32.Parse(cmd.ExecuteScalar().ToString());
            connection.Close();
            return nb;//si le resultat est 0 ok 
        }
        //validation de format de numero de telephon
        public bool valid_tel(string tel)
        {
            Regex myRegex = new Regex(@"^(06|07)[0-9]{8}$");
            return myRegex.IsMatch(tel); // retourne true ou false selon la vérification
        }
        public void fournir(int id_emp, int id_service)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO fournir ( ID_EMPLOYER , ID_SERVICE ) VALUES (@id_emp,@id_service)";
            command.Parameters.AddWithValue("@id_emp", id_emp);
            command.Parameters.AddWithValue("@id_service", id_service);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public int getID_emp(string tel)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");
            connection.Open();
            string requete = "SELECT ID_EMPLOYER FROM employer where TEL_EMP=" + tel;
            SqlCommand cmd = new SqlCommand(requete, connection);
            int nb = Int32.Parse(cmd.ExecuteScalar().ToString());
            connection.Close();
            return nb;
        }
        /*************************************************************************************************************/

        [WebMethod(MessageName = "login", Description = "cette function permet de connecte un employe(les message d'erreur doi etre geree a l'application android)")]
        public DataTable login(string tel, string pwd)
        {
            //les message d'erreur doi etre geree a l'application android
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");

            SqlCommand cmd = new SqlCommand("SELECT * FROM employer where TEL_EMP=@tel and PASSWORD=@pwd");
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@pwd", MD5Hash(pwd));
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = connection;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "employe";
            sda.Fill(dt);
            return dt;
        }

        /**************************************************************************************************************/
        /*****************************************partie recuperation password*********************************************************/

        [WebMethod(MessageName = "getQuestion")]
        public string getQuestion(string tel)
        {
            try
            {
                SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");
                connection.Open();
                string requete = "SELECT QUESTION FROM employer where TEL_EMP=" + tel;
                SqlCommand cmd = new SqlCommand(requete, connection);
                string question = cmd.ExecuteScalar().ToString();
                connection.Close();
                return question;
            }
            catch (Exception e)
            {
                return "aucune";
            }
        }

        [WebMethod(MessageName = "Valide_answer")]
        public string Valide_answer(string tel, string question, string answer)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");
            connection.Open();
            string requete = "SELECT count(*) FROM employer where TEL_EMP=@tel and QUESTION=@question and INDICE=@answer";
            SqlCommand cmd = new SqlCommand(requete, connection);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@question", question);
            cmd.Parameters.AddWithValue("@answer", answer);
            int nb = Int32.Parse(cmd.ExecuteScalar().ToString());
            connection.Close();
            if (nb == 0)
            {
                return "echec";
            }
            else
            {
                return "succes";
            }
        }

        [WebMethod(MessageName = "setPassword")]
        public string setPassword(string tel, string password, string Comfirmpassword)
        {
            if (password != Comfirmpassword)
            {
                return "password n'est pas identique";
            }
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92% ");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE employer SET PASSWORD=@password WHERE ID_EMPLOYER=@id";
            command.Parameters.AddWithValue("@id", getID_emp(tel));
            command.Parameters.AddWithValue("@password", MD5Hash(password));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "succes";
        }

        [WebMethod(MessageName = "GetEmployeJson", Description = "cette methide renvoie Json")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]

        public string GetEmployeJson()
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Employe e = new Employe();
            //e.id = 2;
            e.nom = "siraj";
            e.phone = "siiraaj@yahoo.fr";
            e.profession = "plombier";
            e.ville = "Marseille";
            return ser.Serialize(e);
        }
    }
}