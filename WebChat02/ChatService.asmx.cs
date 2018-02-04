using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Web.Script.Services;
using System.Globalization;

namespace WebChat02
{
    /// <summary>
    /// Summary description for ChatService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ChatService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public void GetOnlineUsers()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            String strResponse = String.Empty;
            List<User> OnlineUsers;
            if (Session["OnlineUsers"] == null)
            {
                OnlineUsers = new List<User>();
                Session["OnlineUsers"] = OnlineUsers;
            }
            else
            {
                OnlineUsers = (List<User>)Session["OnlineUsers"];
            }
            strResponse = js.Serialize(OnlineUsers);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", strResponse.Length.ToString());
            Context.Response.Flush();
            Context.Response.Write(strResponse);
            HttpContext.Current.ApplicationInstance.CompleteRequest();


        }


        [WebMethod(EnableSession = true)]
        public void GetMessages(string username)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            String strResponse = String.Empty;

            User newUser = new User(username);
            DateTime userLoginTime = DateTime.Now;

            List<User> OnlineUsers;
            List<Msg> AllMessages;
            List<Msg> UserMessages = new List<Msg>();


            if (Session["OnlineUsers"] == null)
            {
                userLoginTime = DateTime.ParseExact(newUser.logintime, "HH:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                OnlineUsers = (List<User>)Session["OnlineUsers"];
                foreach (User user in OnlineUsers)
                {
                    if (user.CompareTo(newUser) == 1)
                    {
                        userLoginTime = DateTime.ParseExact(user.logintime, "HH:mm", CultureInfo.InvariantCulture);
                        break;
                    }
                }
            }

            if (Session["Messages"] == null)
            {
                AllMessages = new List<Msg>();
            }
            else
            {
                AllMessages = (List<Msg>)Session["Messages"];

            }

            foreach (var msg in AllMessages)
            {
                DateTime msgTime = DateTime.ParseExact(msg.time, "HH:mm", CultureInfo.InvariantCulture);
                if (msgTime >= userLoginTime)
                    UserMessages.Add(msg);
            }

            strResponse = js.Serialize(UserMessages);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", strResponse.Length.ToString());
            Context.Response.Flush();
            Context.Response.Write(strResponse);
            HttpContext.Current.ApplicationInstance.CompleteRequest();


        }


        [WebMethod(EnableSession = true)]
        public void SendMsg(Msg incoming)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //String strResponse = String.Empty;

            incoming.time = DateTime.Now.ToShortTimeString();

            List<Msg> Messages;
            if (Session["Messages"] == null)
            {
                Messages = new List<Msg>();
            }
            else
            {
                Messages = (List<Msg>)Session["Messages"];

            }
            Messages.Add(incoming);
            Session["Messages"] = Messages;

            //strResponse = js.Serialize(incoming);
            //Context.Response.Clear();
            //Context.Response.ContentType = "application/json";
            //Context.Response.AddHeader("content-length", strResponse.Length.ToString());
            //Context.Response.Flush();
            //Context.Response.Write(strResponse);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        [WebMethod(EnableSession = true)]
        public void Login(string username)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            String strResponse = String.Empty;
            User newUser = new User(username);
            bool userfound = false;
            if (Session["OnlineUsers"] == null)
            {
                List<User> OnlineUsers = new List<User>();
                OnlineUsers.Add(newUser);
                Session["OnlineUsers"] = OnlineUsers;
                strResponse = js.Serialize("logged in successfuly");

            }
            else
            {
                List<User> OnlineUsers = (List<User>)Session["OnlineUsers"];
                foreach (User user in OnlineUsers)
                {
                    if (user.CompareTo(newUser) == 1)
                    {
                        userfound = true;
                        strResponse = js.Serialize("user " + user.name + " already logged in since " + user.logintime);
                        break;

                    }

                }
                if (!userfound)
                {
                    OnlineUsers.Add(newUser);
                    Session["OnlineUsers"] = OnlineUsers;
                    strResponse = js.Serialize("logged in successfuly");
                }


            }


            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", strResponse.Length.ToString());
            Context.Response.Flush();
            Context.Response.Write(strResponse);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public class Msg
        {
            public User user { get; set; }
            public string msg { get; set; }
            public string time { get; set; }
        }

        public struct User : IComparable<User>
        {
            public User(string name)
            {
                this.name = name ?? throw new ArgumentNullException(nameof(name));
                this.logintime = DateTime.Now.ToShortTimeString();
            }

            public string name { get; set; }
            public string logintime { get; set; }


            public int CompareTo(User other)
            {
                if (this.name == other.name)
                    return 1;
                return -1;
            }
        }

    }
}
