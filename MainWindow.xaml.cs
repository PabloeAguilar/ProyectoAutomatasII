using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
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
            List<Tipo> tipos = new List<Tipo>();
            tipos.Add(new Tipo("Bool", "(1 | 0)"));
            tablaTipos.ItemsSource = tipos;
        }

        /// <summary>
        /// Método de click del boton verificar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {
            simbolos = new List<Token>();
            
            txtblResultado1.Text = "Resultado del Código";
            
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
                    
                    // analisis lexico

                    if (!hayErrorLexico)
                    {
                        if (tokens.Count > 0)
                        {

                            string tipoInstruccion = Semantica.TipoInstruccion(tokens);

                            if (tipoInstruccion == "IMPRIMIRRETORNO")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                txtblResultado.Text += "Retorno \n\n";
                            }
                            else if (tipoInstruccion == "IMPRIMIRCADENA")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                foreach (Token token in tokens)
                                {
                                    if (token.Nombre == "CADENA")
                                        txtblResultado.Text += token.Lexema + "\n";
                                }

                            }

                            else if (tipoInstruccion == "ASIGNACION")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
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
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
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
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                List<string> variables = new List<string>();
                                List<Token> auxVariables = new List<Token>();
                                auxVariables = Class1.generarTablaSimbolos(tokens, auxVariables);
                                foreach (Token token in auxVariables)
                                {
                                    if (token.Nombre == "VARIABLE")
                                        variables.Add(token.Lexema);
                                }

                                List<Token> copiaTokens = Semantica.CopiarLista(tokens);
                                List<List<int>> tablaVerdad = Semantica.TablaDeVerdadParte2(copiaTokens);
                                if (tablaVerdad.Count > 0)
                                {
                                    for (int j = 0; j < variables.Count; j++)
                                    {
                                        txtblResultado.Text += variables[j] + "\t";
                                    }
                                    txtblResultado.Text += "Result\n";
                                    int filas = tablaVerdad[0].Count;
                                    int columnas = tablaVerdad.Count;
                                    for (int j = filas - 1; j >= 0; j--)
                                    {
                                        for (int k = 0; k < columnas-1 ; k++)
                                        {
                                            txtblResultado.Text += tablaVerdad[k][j] + "\t";
                                        }
                                        txtblResultado.Text += tablaVerdad[tablaVerdad.Count - 1][j] + "\t";
                                        txtblResultado.Text += "\n";
                                    }
                                }
                            }
                            else if(tipoInstruccion == "TAUTOLOGIA")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                int esTauto = Semantica.EvaluarTautologia(tokens);
                                txtblResultado.Text += esTauto + "\n";
                            }
                            else if (tipoInstruccion == "CONTRADICCION")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                int esTauto = Semantica.EvaluarContradiccion(tokens);
                                txtblResultado.Text += esTauto + "\n";
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

        /// <summary>
        /// Metodo del click del boton limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }
        
        /// <summary>
        /// Metodo para limpiar cajas de texto
        /// </summary>
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
                    writer.Write(linea);
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

        private void btnGuardarResultado_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\reg.log"), append: false, Encoding.ASCII))
            {
                int i = 0;
                string[] lineas = txtblResultado.Text.Split('\n');

                foreach (string linea in lineas)
                {
                    writer.WriteLine(linea);
                    i++;
                }
                //                writer.Flush();
                writer.Close();

            }
            using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\tokens.log"), append: false, Encoding.ASCII))
            {
                int i = 0;
                string[] lineas = txtblResultado1.Text.Split('\n');

                foreach (string linea in lineas)
                {
                    writer.WriteLine(linea);
                    i++;
                }
                writer.Close();

            }
            _ = MessageBox.Show("Archivos guardados en el escritorio como: reg.log y tokens.log");
        }
    }
}
