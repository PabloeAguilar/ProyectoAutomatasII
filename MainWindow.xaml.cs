using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            string aux = "";

            txtblResultado.Text = "";
            try
            {
                for (int i = 0; i < txtEntrada.LineCount; i++)
                {
                    foreach (char l in txtEntrada.GetLineText(i))
                    {
                        if (l != ' ' || l.ToString() != "")
                        {
                            aux += l.ToString();
                        }
                    }
                    if (aux.Length != 0)
                    {
                        if (Class1.GLEXPRESION(aux))
                            //Indicar tipo de error
                            txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Correcto\n";
                        else if (aux != "\r\n")
                            txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Incorrecto\n";
                        aux = "";
                    }
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
