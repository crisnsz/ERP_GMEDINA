﻿using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERP_GMEDINA.Attribute
{
    public class SessionManagerAttribute : ActionFilterAttribute
    {
        private readonly string _screenId;
        Helpers Help = new Helpers();
        public SessionManagerAttribute()
        {
        }

        public SessionManagerAttribute(string screenId)
        {
            _screenId = screenId;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool UsuarioEstado = false;
            bool EsAdmin = false;
            int UsuarioRol = 0;
            bool AccesoPantalla = false;

            var valuesUsuarioInactivo = new RouteValueDictionary(new { action = "Index", controller = "Login" });//--//
            var valuesSinAcceso = new RouteValueDictionary(new { action = "SinAcceso", controller = "Login" });
            var valuesSinRol = new RouteValueDictionary(new { action = "SinRol", controller = "Login" });
            var valuesIndex = new RouteValueDictionary(new { action = "Index", controller = "Login" });

            //Paso 1: Validar que la sesion no haya expirado.
            if (Help.GetUserLogin())
            {
                //Paso 4: Validar si el usuario no es admin.
                if (!EsAdmin)
                {
                    //Paso 5: Validar si el usuario tiene un rol asignado.
                    if (UsuarioRol != 0)
                    {
                        //Paso 6: Validar si el usuario tiene acceso a la pantalla u objeto.
                        if (!AccesoPantalla)
                            filterContext.Result = new RedirectToRouteResult(valuesSinAcceso);
                    }
                    else
                        filterContext.Result = new RedirectToRouteResult(valuesSinRol);
                }


                ////Paso 1: Validar que el estado del usuario siga siendo activo.
                //if (UsuarioEstado)
                //{


                //}
                //else
                //    filterContext.Result = new RedirectToRouteResult(valuesUsuarioInactivo);
            }
            else
                filterContext.Result = new RedirectToRouteResult(valuesIndex);
        }
    }
}