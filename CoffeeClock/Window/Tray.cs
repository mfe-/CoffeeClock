using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace CoffeeClock.Window
{
    public class Tray
    {
        private NotifyIcon _notico;

        public Tray()
        {
            Initialize();
        }
        /// <summary>
        /// Initialisiert das NotifyIcon
        /// </summary>
        private void Initialize()
        {
            ResourceManager resourceManager = new ResourceManager("CoffeeClock.Properties.Resources", Assembly.GetExecutingAssembly());

            // NotifyIcon erzeugen
            _notico = new NotifyIcon();
            _notico.Visible = true;
            _notico.Icon = ((System.Drawing.Icon)(resourceManager.GetObject("clock")));

            ContextMenu contextMenu = new ContextMenu();

            // Kontextmenüeinträge erzeugen
            MenuItem menuItem = new MenuItem();
            menuItem.Index = 1;
            menuItem.Text = "&Beenden";
            menuItem.Click += (sender, e) =>
            {
                Environment.Exit(0);
            };

            contextMenu.MenuItems.Add(menuItem);

            _notico.ContextMenu = contextMenu;

        }

        public NotifyIcon NotifyIcon
        {
            get
            {
                return _notico;
            }
        }
    }
}
