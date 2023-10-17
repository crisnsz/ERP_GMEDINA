using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace ERP_GMEDINA.Models
{
    public class Helpers
    {

        FARSIMANEntities db = new FARSIMANEntities();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        //Cerrar sesion
        public void FCerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
            HttpContext.Current.Response.Expires = -1500;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            AuthenticationManager.SignOut();
            HttpContext.Current.Session["UserNombreUsuario"] = null;
            HttpContext.Current.Session["UserNombresApellidos"] = null;
            HttpContext.Current.Session["UserLogin"] = null;
            HttpContext.Current.Session["UserLoginEsAdmin"] = null;
            HttpContext.Current.Session["UserLoginSesion"] = null;
            HttpContext.Current.Session["UserLoginRols"] = null;
            HttpContext.Current.Session["UserRol"] = null;
            HttpContext.Current.Session["UserRolEstado"] = null;
            HttpContext.Current.Session["UserEstado"] = null;
        }

        public void ValidateUser(string sPantalla,out bool UserState, out bool IsAdmin, out int UserPosition, out bool AccessPantalla)
        {
            UserState = Convert.ToBoolean(HttpContext.Current.Session["UserEstado"]);
            IsAdmin = Convert.ToBoolean(HttpContext.Current.Session["UserLoginIsAdmin"]);
            UserPosition = Convert.ToInt32(HttpContext.Current.Session["UserPosition"]);
            AccessPantalla = GetUserAccessPosition(sPantalla);

           
        }


        public bool GetUserAccessPosition(string sPantalla)
        {
            bool Retorno = false;
            try
            {
                int UserPositionID = Convert.ToInt32(HttpContext.Current.Session["UserPosition"]);
                if (!Convert.ToBoolean(HttpContext.Current.Session["UserLoginEsAdmin"]))
                {

                    //var listUserAccess = HttpContext.Current.Session["UserLoginRols"];

                    var listPositionAccess = (from position in db.tbPositions
                                 join access in db.tbAccesses on position.position_ID equals access.position_ID
                                 join obj in db.tbObjects on access.object_ID equals obj.object_ID
                                 where position.position_ID == UserPositionID
                                 select new
                                 {
                                     PositionName = position.position_Name,
                                     ObjectName = obj.object_Name,
                                     ObjectReference = obj.object_Reference
                                 }).ToList();



                    //var list = (IEnumerable<SDP_Acce_GetUserRols_Result>)HttpContext.Current.Session["UserLoginRols"];
                    var BuscarList = listPositionAccess.Where(x => x.ObjectReference == sPantalla);
                    int Conteo = BuscarList.Count();

                    if (Conteo > 0)
                        Retorno = true;
                }
                else
                    Retorno = true;

            }
            catch (Exception Ex)
            {
            }
            return Retorno;
        }
        
        public bool GetUserLogin()
        {
            bool Estado = false;
            try
            {
                var isLogged = HttpContext.Current.Session["UserLogin"];
                if (isLogged == null)
                {
                    return Estado;
                }

                int user = (int)HttpContext.Current.Session["UserLogin"];
                if (user != 0)
                    Estado = true;
            }
            catch (Exception)
            {

            }
            return Estado;
        }


        public List<tbUser> getUserInformation()
        {
            int user = 0;
            List<tbUser> UsuarioList = new List<tbUser>();
            try
            {
                user = (int)HttpContext.Current.Session["UserLogin"];
                if (user != 0)
                {
                    UsuarioList = db.tbUsers.Where(s => s.user_ID == user).ToList();
                }
            }
            catch (Exception Ex)
            {

            }
            return UsuarioList;
        }

        public int GetUser()
        {
            int user = 0;
            try
            {
                user = (int)HttpContext.Current.Session["UserLogin"];
            }
            catch (Exception Ex)
            {
            }
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime DatetimeNow()
        {
            DateTime dt = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(-6)).DateTime;
            return dt;
        }

    }
}