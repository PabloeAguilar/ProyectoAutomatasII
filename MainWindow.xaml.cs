using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProyectoAutomatasII.Expresiones_Regulares;

namespace ProyectoAutomatasII
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {
            txtblResultado.Text = "";
            try
            {
                for (int i = 0; i < txtEntrada.LineCount; i++)
                {
                    if (Class1.GLASIGNACION(txtEntrada.GetLineText(i)))
                        txtblResultado.Text += "Renglón Correcto" + i.ToString() + "\n";
                    else
                        txtblResultado.Text += "Renglón Incorrecto" + i.ToString() + "\n";
                }
            
            } catch (Exception x) { MessageBox.Show("Exeption" + x); }
            
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void btnVerificar_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        
        private void Limpiar()
        {
            txtEntrada.Clear();
            txtblResultado.Text = "";
        }
    }
}
