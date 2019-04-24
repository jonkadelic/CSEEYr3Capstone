using System;
using System.Collections.Generic;
using System.Text;

namespace IoTControllerApplication.Models
{
    public enum MenuItemType
    {
        Devices,
        Routines,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
