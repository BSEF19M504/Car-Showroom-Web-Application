﻿using System.Web;
using System.Web.Mvc;

namespace Car_Showroom_Web_Application
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}