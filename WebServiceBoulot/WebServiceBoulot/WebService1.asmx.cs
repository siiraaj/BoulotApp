﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod(MessageName = "GetEmploye", Description = "Return contact data from DB")]
        [System.Xml.Serialization.XmlInclude(typeof(Employe))]

        public Employe GetEmploye()
        {
            Employe e = new Employe();
            e.id = 2;
            e.nom = "siraj";
            e.mail = "siiraaj@yahoo.fr";
            return e;
        }
    }
}