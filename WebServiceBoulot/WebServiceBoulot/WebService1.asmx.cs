﻿using System;
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
        /*************************************************recherche par ville et service***************************************************/
        [WebMethod(MessageName = "recherche")]
        public DataTable recherche(int id_ville, int id_service)
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT employer.ID_EMPLOYER,NOM_EMP,PRENOM_EMP,TEL_EMP,IMAGE FROM employer,service,fournir where employer.ID_EMPLOYER=fournir.ID_EMPLOYER and service.ID_SERVICE=fournir.ID_SERVICE and service.ID_SERVICE=@id_service and ID_VILLE=@id_ville");
            cmd.Parameters.AddWithValue("@id_service", id_service);
            cmd.Parameters.AddWithValue("@id_ville", id_ville);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "employes";
            sda.Fill(dt);
            return dt;
        }
        [WebMethod(MessageName = "setDisponibilite", Description = "cette function permet changer l'etat de l'employe")]
        public string setDisponibilite(int id, bool disp)
        {
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE employer SET dispo=@disp WHERE ID_EMPLOYER=@id";
            command.Parameters.AddWithValue("?id", id);
            command.Parameters.AddWithValue("?disp", disp);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "succes";
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
        /******************************************************************************************************************************/

        /******************************************partie affichage des services et employes*******************************************************/
        [WebMethod(MessageName = "getAllServices")]
        public DataTable getAllServices()
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM service");
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "services";
            sda.Fill(dt);
            return dt;
        }
        [WebMethod(MessageName = "getAllEmployes")]
        public DataTable getAllEmployes(int id_services)
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("select employer.ID_EMPLOYER,NOM_EMP,PRENOM_EMP,IMAGE,NOM_VILLEE,NOM_SERVICE,TEL_EMP,DISPO from service,employer,fournir,ville where fournir.ID_EMPLOYER=employer.ID_EMPLOYER and fournir.ID_SERVICE=service.ID_SERVICE and employer.ID_VILLE=ville.ID_VILLE and service.ID_SERVICE=@id_service");
            cmd.Parameters.AddWithValue("@id_service", id_services);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "Employes";
            sda.Fill(dt);
            return dt;
        }
        [WebMethod(MessageName = "getAllEmployesSerch")]
        public DataTable getAllEmployesSerch(string nom, int id_services)
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("select employer.ID_EMPLOYER,NOM_EMP,PRENOM_EMP,IMAGE,NOM_VILLEE,NOM_SERVICE,TEL_EMP,DISPO from service,employer,fournir,ville where fournir.ID_EMPLOYER=employer.ID_EMPLOYER and fournir.ID_SERVICE=service.ID_SERVICE and employer.ID_VILLE=ville.ID_VILLE and service.ID_SERVICE=@id_service and ( NOM_EMP like('" + nom + "%') or PRENOM_EMP like('" + nom + "%') )");
            cmd.Parameters.AddWithValue("@id_service", id_services);

            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "Employes";
            sda.Fill(dt);
            return dt;
        }
        [WebMethod(MessageName = "SCORE")]
        public float SCORE(int id_emp)
        {
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            connection.Open();
            string requete = "select isnull(AVG(SCORE),0)from consulte where ID_EMPLOYER=" + id_emp;
            SqlCommand cmd = new SqlCommand(requete, connection);
            float score = float.Parse(cmd.ExecuteScalar().ToString());
            connection.Close();
            return score;
        }


        /*****************************************************************************************************************************************/

        /******************************************************partie review**************************************************************/
        [WebMethod(MessageName = "review")]
        public string review(int id_emp, string mail, float score)
        {
            int id_client;
            if (validate_mail_format(mail) == false)
            {
                return "Adresse e-mail incorrecte, verifier votre email";
            }
            if (verifier_mail(mail) == 1)
            {
                id_client = getIdCLient(mail);
            }
            else
            {
                addClient(mail);
                id_client = getIdCLient(mail);
            }
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO consulte (ID_CLT,ID_EMPLOYER,SCORE,DATE,COMMENTAIRE) VALUES (@id_clt,@id_emp,@score,@date,@commentaire)";
            command.Parameters.AddWithValue("@id_clt", id_client);
            command.Parameters.AddWithValue("@id_emp", id_emp);
            command.Parameters.AddWithValue("@score", score);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@commentaire", "good job");
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "sucess";
        }
        public bool validate_mail_format(string mail)
        {
            Regex myRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            return myRegex.IsMatch(mail);

        }
        public void addClient(string email)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO client (EMAIL_CLT) VALUES (@email)";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
        public int verifier_mail(string email)
        {
            SqlConnection connection = new SqlConnection("Data Source = 213.246.49.103; Initial Catalog = ajahot127969com28897_WebServiceTest; User ID = ajahot127969com28897_sirajTest; Password = 1imOk92%");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT count(*) FROM client where EMAIL_CLT=@mail";
            command.Parameters.AddWithValue("@mail", email);
            connection.Open();
            int nb = Int32.Parse(command.ExecuteScalar().ToString());
            connection.Close();
            return nb;//si le resultat est 0 ok 
        }
        public int getIdCLient(string mail)
        {
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            connection.Open();
            string requete = "SELECT ID_CLT FROM client where EMAIL_CLT='" + mail + "'";
            SqlCommand cmd = new SqlCommand(requete, connection);
            int nb = Int32.Parse(cmd.ExecuteScalar().ToString());
            connection.Close();
            return nb;
        }

        
        /**********************************************************************************************************************************/

        /************************************************************partie galerie*********************************************************/
        [WebMethod(MessageName = "uploadGalerie")]
        public string uploadGalerie(string[] galerieUrl, int id_emp, int service)
        {
            for (int i = 0; i < galerieUrl.Length; i++)
            {
                uploadPict(galerieUrl[i], id_emp, service);
                saveGalerie_DB(galerieUrl[i], id_emp, service);
            }
            return "succes";
        }
        [WebMethod(MessageName = "uploadImageGalerie")]
        public void uploadPict(string source, int emp_id, int service_id)
        {
            String ftpurl = "ftp://ecinemaroc.co.nf/" + emp_id + "_" + service_id + "_" + Path.GetFileName(source);
            String ftpusername = "2052696_bricole";
            String ftppassword = "saber123**";
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpusername, ftppassword);
                client.UploadFile(ftpurl, "STOR", source);
            }
        }
        public string saveGalerie_DB(string source, int emp_id, int service_id)
        {
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO galerie(ID_SERVICE, IMAGES, ID_EMPLOYER) VALUES (@service_id,@image,@employe_id)";
            command.Parameters.AddWithValue("@service_id", service_id);
            command.Parameters.AddWithValue("@image", "ftp://2052696_bricole:saber123**@ecinemaroc.co.nf/" + emp_id + "_" + service_id + "_" + Path.GetFileName(source));
            command.Parameters.AddWithValue("@employe_id", emp_id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "succes";
        }
        [WebMethod(MessageName = "get_galerie")]
        public DataTable get_galerie(int id_employe, int id_service)
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT IMAGES from galerie where ID_EMPLOYER=@id_employer and ID_SERVICE=id_service");
            cmd.Parameters.AddWithValue("@id_service", id_service);
            cmd.Parameters.AddWithValue("@id_employer", id_employe);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "employes";
            sda.Fill(dt);
            return dt;
        }
        /**********************************************************************************************************************************/

        /*****************************************************partie de modification******************************************************/
        [WebMethod(MessageName = "updateProfile")]
        public string updateProfile(int id_emp, int id_ville, string nom, string pseudo, string prenom, int service, string tel, string password)
        {
            if (nom.Equals("") || prenom.Equals("") || service.Equals("") || tel.Equals("") || password.Equals(""))
            {
                return "tous les champs sont obligatoire !";
            }
            if (valid_tel(tel) == false)
            {
                return "le format de telephone n'est pas correct";
            }
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE employer SET ID_VILLE=@id_ville,NOM_EMP=@nom,PRENOM_EMP=@prenom,PSEUDO=@pseudo,TEL_EMP=@tel,PASSWORD=@password WHERE ID_EMPLOYER=@id_emp";
            command.Parameters.AddWithValue("@id_ville", id_ville);
            command.Parameters.AddWithValue("@nom", nom);
            command.Parameters.AddWithValue("@prenom", prenom);
            command.Parameters.AddWithValue("@pseudo", pseudo);
            command.Parameters.AddWithValue("@tel", tel);
            command.Parameters.AddWithValue("@password", MD5Hash(password));
            command.Parameters.AddWithValue("@id_emp", id_emp);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "succes";
        }
       
        [WebMethod(MessageName = "uploadImage")]
        public void uploadImage(string source, int emp_id)
        {
            string url_image = new Uri(source).AbsolutePath;
            String ftpurl = "ftp://gestion-conference-fs.com/httpdocs/Conference_Web/Users/" + emp_id + "_" + Path.GetFileName(url_image);
            String ftpusername = "spotmusic";
            String ftppassword = "wI$x9a82";
            using (WebClient client = new WebClient())
            {
                NetworkCredential myCred = new NetworkCredential(ftpusername, ftppassword);
                CredentialCache myCache = new CredentialCache();
                myCache.Add(new Uri(ftpurl), "Basic", myCred);
                client.Credentials = myCache;
                client.UploadFile(ftpurl, "STOR", url_image);
            }
        }

        public void saveDB(int id_emp, string source)
        {
            SqlConnection connection = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE employer SET IMAGE=@image WHERE ID_EMPLOYER=@id";
            command.Parameters.AddWithValue("@id", id_emp);
            command.Parameters.AddWithValue("@image", "ftp://2052696_bricole:saber123**@ecinemaroc.co.nf/USERS/" + id_emp + "_" + Path.GetFileName(source));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
         /***********************************************************getAllVille***********************************************/
        [WebMethod(MessageName = "getAllVilles")]
        public DataTable getAllVilles()
        {
            SqlConnection con = new SqlConnection(DBConnection.ConnectionString);
            SqlCommand cmd = new SqlCommand("select NOM_VILLEE from ville ORDER BY ID_VILLE");
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "Villes";
            sda.Fill(dt);
            return dt;
        }
        /**********************************************************************************************************************/
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