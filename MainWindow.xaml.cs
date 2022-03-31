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
using Microsoft.Win32;
using ProyectoAutomatasII.Expresiones_Regulares;

namespace ProyectoAutomatasII
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string nombreArchivo = ".txt";
        //string direccion = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {
            txtblResultado1.Text = "Resultado del Código";
            string aux = "";
            List<Token> tokens;
            txtblResultado.Text = "";
            try
            {
                for (int i = 0; i < txtEntrada.LineCount; i++)
                {
                    tokens = Class1.Tokens(txtEntrada.GetLineText(i));
                    txtblResultado1.Text += "\n";
                    foreach (Token a in tokens)
                    {
                        txtblResultado1.Text += a.Nombre + " ";
                    }
                    txtblResultado1.Text += "\n";
                    string errorLexico = VerificacionErroresLexicos.EliminacionTokens(txtEntrada.GetLineText(i), tokens);
                    string[] errores = errorLexico.Split(' ');

                    if (errores.Length > 0)
                    {
                        int j = 0;
                        foreach (string error in errores)
                        {
                            if (error != " " && error != "" && error != "\r" && error != "\n")
                            {
                                foreach (char letra in error)
                                {
                                    if (letra != '\r' && letra != '\n')
                                    {
                                        txtblResultado1.Text += "Error en la linea " + (i + 1) + ": Simbolo no definido: " + letra + "\n";
                                    }
                                    
                                }
                                
                                
                            }
                            j++;

                        }
                        
                    }

                    foreach (char l in txtEntrada.GetLineText(i))
                    {
                        
                        if (l != ' ' || l.ToString() != "" )
                        {
                            aux += l.ToString();
                        }
                    }
                   
                    if (aux.Length != 0)
                    {
                        if (Class1.GLEXPRESION(aux))
                            txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Correcto\n";
                        else if (aux != "\r\n")
                            //Indicar tipo de error
                            txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Incorrecto\n";
                        aux = "";
                    }
                }
            
            } catch (Exception x) { MessageBox.Show("Exeption" + x); }
            
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
            /*
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*txt)|*.txt";
            ofd.Multiselect = false;
            ofd.Title = "Abrir archivo";
            //ofd.InitialDirectory = "C:\\Users\\pablo\\Desktop";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            if ((bool)ofd.ShowDialog())
            {
                Limpiar();
                nombreArchivo = ofd.FileName;
                using (StreamReader red = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = red.ReadLine()) != null)
                    {
                        
                        txtEntrada.Text += (line + "\r\n");
                        
                    }
                }
            }
            */
        }

        private void btnVerificar_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        
        private void Limpiar()
        {
            txtEntrada.Clear();
            txtblResultado.Text = "";
            txtblResultado1.Text = "Resultado del Código";
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer  = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\reg.txt"), append:false, Encoding.ASCII))
            {
                int i = 0;
                string linea;
                while (i < txtEntrada.LineCount)
                {
                    linea = txtEntrada.GetLineText(i);
                    writer.WriteLine(linea);
                    i++;
                }
//                writer.Flush();
                writer.Close();
                
            }
            MessageBox.Show("Archivo guardado en el escritorio como: reg.txt");
        }

        private void btnAbrir_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*txt)|*.txt";
            ofd.Multiselect = false;
            ofd.Title = "Abrir archivo";
            //ofd.InitialDirectory = "C:\\Users\\pablo\\Desktop";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            

            if ((bool)ofd.ShowDialog())
            {
                Limpiar();
                nombreArchivo = ofd.FileName;
                using (StreamReader red = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = red.ReadLine()) != null)
                    {

                        txtEntrada.Text += (line + "\r\n");

                    }
                }
            }
        }
    }
}
