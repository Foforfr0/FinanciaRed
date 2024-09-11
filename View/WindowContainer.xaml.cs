using System;
using System.Windows;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for WindowContainer.xaml
    /// </summary>
    public partial class WindowContainer : Window {
        public WindowContainer () {
            InitializeComponent ();
            mainFrame.Navigate (new Login());
        }
    }
}
