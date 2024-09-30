using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TP_Gestion.Views
{
    /// <summary>
    /// Lógica de interacción para PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public bool? DialogResultValue { get; private set; } 

        public PopUp(string text, string btn1, string btn2)
        {
            InitializeComponent();
            txt_texto.Text = text;
            btn_1.Content = btn1;
            btn_2.Content = btn2;
        }

        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            DialogResultValue = false;
            this.DialogResult = false;  
            this.Close();
        }

        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            DialogResultValue = true;
            this.DialogResult = true;
            this.Close();
        }
    }
}
