using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Graphics
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>W
        [STAThread]
        static void Main()
        {
            try
            {
                string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

                SoundPlayer player = new SoundPlayer(projectPath + "\\sounds\\The Pink Panther Theme Song (Original Version).wav");
                //SoundPlayer player = new SoundPlayer("C:\\Users\\Dr Tony\\Desktop\\Graphics\\Graphics Project\\Graphics\\sounds\\The Pink Panther Theme Song (Original Version).wav");
                player.PlayLooping();             
            }
            catch (Exception)
            {
                throw;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphicsForm());
        }
    }
}
