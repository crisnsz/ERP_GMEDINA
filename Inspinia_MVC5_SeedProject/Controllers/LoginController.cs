using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;
using Microsoft.Owin.Security;


namespace ERP_GMEDINA.Controllers
{
    public class LoginController : Controller
    {
        FARSIMANEntities db = new FARSIMANEntities();

        Helpers Function = new Helpers();
        Helpers Help = new Helpers();

        // GET: Login
        public ActionResult Index()
        {
            Help.FCerrarSesion();
            return View();
        }

        [HttpPost]
        public ActionResult Index(tbUser tbUser, string txtPassword)
        {
            try
            {
                var UserExist = db.tbUsers.Where(x => x.user_Username == tbUser.user_Username && x.user_Password == txtPassword).ToList();

                //Paso 1: Validar si el usuario existe.
                if (UserExist.Count > 0)
                {
                    foreach (tbUser UserLogin in UserExist)
                    {
                        //Paso 2: Validar que el usuario esté activo.
                        if (UserLogin.user_IsActive)
                        {

                            //Si esta bien, recuperar la informacion del usuario.
                            //var Listado = db.SDP_Acce_GetUserRols(UserLogin.usu_Id, "").ToList(); //Accesos
                            //var ListadoRol = db.SDP_Acce_GetRolesAsignados(UserLogin.usu_Id).ToList(); //Rol
                            Session["UserNombreUsuario"] = UserLogin.user_Username;
                            Session["UserNombresApellidos"] = UserLogin.tbEmployee.employee_Name;
                            Session["UserLogin"] = UserLogin.user_ID;
                            Session["UserLoginEsAdmin"] = UserLogin.user_IsAdmin;
                            Session["UserEstado"] = UserLogin.user_IsActive;
                            //Si el usuario no es admin, recuperar la información del rol y sus accesos
                            if (!UserLogin.user_IsAdmin)
                            {
                                //foreach (SDP_Acce_GetRolesAsignados_Result Rol in ListadoRol)
                                //{
                                //    Session["UserRolEstado"] = Rol.rol_Estado;
                                //    Session["UserLoginRols"] = Listado;
                                //    Session["UserRol"] = ListadoRol.Count();
                                //}
                            }

                        }
                        else
                        {
                            //Si el usuario no es activo que muestre mensaje y retorne al login una vez mas.
                            ModelState.AddModelError("user_Username", "Usuario inactivo, contacte al Administrador");
                            return View(tbUser);
                        }

                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("user_Username", "Usuario o Password incorrecto");
                    return View(tbUser);
                }
            }
            catch (Exception Ex)
            {
                return View(tbUser);
            }
        }

        public ActionResult CerrarSesion()
        {
            Help.FCerrarSesion();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SinAcceso()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult NotFound()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SinRol()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }
    }
}