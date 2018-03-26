using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

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
            {

            }
            finally
            {
                conn.Close();
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();

            return ser.Serialize(contactslist);




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
