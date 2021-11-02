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

        private void Initialize()
        {
            ResourceManager resourceManager = new ResourceManager("CoffeeClock.Properties.Resources", Assembly.GetExecutingAssembly());

            //create tray icon object
            _notico = new NotifyIcon();
            _notico.Visible = true;
            _notico.Icon = ((System.Drawing.Icon)(resourceManager.GetObject("clock")));

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            //menuItem.Index = 1;
            menuItem.Text = "&Beenden";
            menuItem.Click += (sender, e) =>
            {
                Environment.Exit(0);
            };

            contextMenu.Items.Add(menuItem);

            _notico.ContextMenuStrip = contextMenu;

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
