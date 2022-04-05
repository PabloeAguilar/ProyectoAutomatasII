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
        private List<Token> simbolos = new List<Token>();
        //string direccion = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {
            simbolos = new List<Token>();
            int posicionAux = 9000;
            txtblResultado1.Text = "Resultado del Código";
            string aux = "";
            List<Token> tokens;
            txtblResultado.Text = "";
            try
            {
                for (int i = 0; i < txtEntrada.LineCount; i++)
                {
                    bool hayErrorLexico = false;
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
                        
                        foreach (string error in errores)
                        {
                            if (error != " " && error != "" && error != "\r" && error != "\n")
                            {
                                foreach (char letra in error)
                                {
                                    if (letra != '\r' && letra != '\n')
                                    {
                                        txtblResultado1.Text += "Error en la linea " + (i + 1) + ": Simbolo no definido: " + letra + "\n";
                                        hayErrorLexico = true;
                                    }
                                    
                                }

                                
                            }

                        }
                        
                    }
                    //tabla de simbolos
                    foreach (Token token in tokens)
                    {
                        if (token.Nombre == "VARIABLE")
                        {
                            bool simboloExiste = false;
                            foreach (Token token1 in simbolos)
                            {
                                if (token.Lexema == token1.Lexema)
                                {
                                    simboloExiste = true;
                                }
                            }
                            if (!simboloExiste)
                            {
                                if (token.Valor < 0)
                                {
                                    token.Valor = 0;
                                    token.Posicion = ++posicionAux;
                                }
                                simbolos.Add(token);
                            }
                            
                            
                            
                        }
                    }

                    
                    // analisis lexico

                    if (!hayErrorLexico)
                    {
                        /*foreach (char l in txtEntrada.GetLineText(i))
                        {

                            if (l != ' ' || l.ToString() != "")
                            {
                                aux += l.ToString();
                            }
                        }

                        if (aux.Length != 0)
                        {
                            if (Class1.GLEXPRESION(aux))
                            {
                                txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Correcto\n";

                            }
                                
                            else if (aux != "\r\n")
                                //Indicar tipo de error
                                txtblResultado.Text += Regex.Replace(txtEntrada.GetLineText(i), "(\\r\\n)*", "") + " //Incorrecto\n";
                            aux = "";
                        }
                        */
                        if (tokens.Count > 0)
                        {

                            string tipoInstruccion = Semantica.TipoInstruccion(tokens);

                            if (tipoInstruccion == "IMPRIMIRRETORNO")
                            {
                                txtblResultado.Text += "Retorno \n\n";
                            }
                            else if (tipoInstruccion == "IMPRIMIRCADENA")
                            {
                                foreach (Token token in tokens)
                                {
                                    if (token.Nombre == "CADENA")
                                        txtblResultado.Text += token.Lexema + "\n";
                                }

                            }

                            else if (tipoInstruccion == "ASIGNACION")
                            {
                                int valorAux = Semantica.EvaluarExpresion(tokens, simbolos);
                                bool existeSimbolo = false;
                                foreach (Token token in simbolos)
                                {
                                    if (token.Lexema == tokens[0].Lexema)
                                    {
                                        existeSimbolo = true;
                                        token.Valor = valorAux;
                                    }

                                }
                                if (!existeSimbolo)
                                    simbolos.Add(new Token(Name = "VARIABLE", tokens[0].Lexema, simbolos[simbolos.Count - 1].Posicion + 1, valorAux));

                            }
                            else if (tipoInstruccion == "IMPRIMIREXPRESION")
                            {
                                string auxNombreVariable = "";
                                foreach (Token token in tokens)
                                {
                                    if (token.Nombre == "VARIABLE")
                                    {
                                        auxNombreVariable = token.Lexema;
                                    }
                                }
                                bool existeVariable = false;
                                foreach (Token token in simbolos)
                                {
                                    if (token.Lexema == auxNombreVariable)
                                    {
                                        existeVariable = true;
                                        txtblResultado.Text += token.Valor + "\n";
                                    }
                                }
                                if (!existeVariable)
                                {
                                    simbolos.Add(new Token("VARIABLE", auxNombreVariable, simbolos[simbolos.Count - 1].Posicion + 1, 0));
                                    txtblResultado.Text += "0 \n";
                                }
                            }
                            else if(tipoInstruccion == "IMPRIMIRTABLA")
                            {
                                List<string> variables = new List<string>();
                                foreach(Token token in tokens)
                                {
                                    if (token.Nombre == "VARIABLE")
                                        variables.Add(token.Lexema);
                                }
                                
                                List<Token> copiaTokens =  new List<Token>(tokens);
                                List<List<int>> tablaVerdad = Semantica.TablaDeVerdadParte2(copiaTokens);
                                if (tablaVerdad.Count > 0)
                                {
                                    for (int j = variables.Count - 1; j >= 0; j--)
                                    {
                                        txtblResultado.Text += variables[j] + "\t";
                                    }
                                    txtblResultado.Text += "Result\n";
                                    int filas = tablaVerdad[0].Count;
                                    int columnas = tablaVerdad.Count;
                                    for (int j = filas - 1 ; j >= 0; j--)
                                    {
                                        for (int k = columnas - 2; k >= 0; k--)
                                        {
                                            txtblResultado.Text += tablaVerdad[k][j] + "\t";
                                        }
                                        txtblResultado.Text += tablaVerdad[tablaVerdad.Count-1][j] + "\t";
                                        txtblResultado.Text += "\n";
                                    }
                                }
                            }

                            else
                            {
                                txtblResultado.Text += tipoInstruccion + ": Linea " + (i+1) + "\n\n";
                            }

                        } 

                    }
                    
                }
                tablaSimbolos.ItemsSource = simbolos;

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
