using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GW2MH.Core.Settings
{
    internal static class SettingsManager
    {

        static SettingsManager()
        {
            
               
            
        }

        internal static void Save()
        {
            var args = new KeyEventArgs(Keys.L | Keys.F);

            using (var fs = new FileStream("C:\\Users\\Johan\\Desktop\\test.bin", FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, args);
            }
        }

    }
}