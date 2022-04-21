using NetCoreConsoleAutoUpdate.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetCoreConsoleAutoUpdate.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VersionChecker versionChecker = null;
        public MainWindow()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetEntryAssembly();
            version.Content = $"Current Version : {assembly.GetName().Version}";
            versionChecker = new VersionChecker("https://localhost:7143/App/GetVersion", 10);
            versionChecker.CheckUpdate();//一啟動即檢查
        }
    }
}
