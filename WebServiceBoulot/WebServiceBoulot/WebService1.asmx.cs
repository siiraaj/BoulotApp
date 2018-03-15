using System;
using System.Collections.Generic;
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
