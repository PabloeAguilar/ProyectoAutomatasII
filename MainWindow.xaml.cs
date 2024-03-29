﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using ProyectoAutomatasII.Expresiones_Regulares;
using Proviant;

namespace ProyectoAutomatasII
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> resultados = new List<string>();
        List<string> codigoEnsamblador = new List<string>(); 
        List<string> asignaciones = new List<string>();
        bool hayErroresPrograma = false;
        string nombreArchivo = ".txt";
        private List<Token> simbolos = new List<Token>();
        //string direccion = "";
        public MainWindow()
        {
            InitializeComponent();
            List<Tipo> tipos = new List<Tipo>
            {
                new Tipo("Bool", "(1 | 0)")
            };
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
            
            codigoEnsamblador.Clear();
            codigoEnsamblador.Add(".code");
            codigoEnsamblador.Add("org 100h");
            codigoEnsamblador.Add("begin: jmp short inicio");
            codigoEnsamblador.Add(".data");
            
            hayErroresPrograma = false;
            resultados = new List<string>();
            //txtblResultado1.Text = "Resultado del Código";            
            List<Token> tokens;
            txtblResultado.Text = "";

            try
            {
                for (int i = 0; i < txtEntrada.LineCount; i++)
                {
                    bool hayErrorLexico = false;
                    tokens = Class1.Tokens(txtEntrada.GetLineText(i));
                    /*txtblResultado1.Text += "\n";
                    foreach (Token a in tokens)
                    {
                        txtblResultado1.Text += a.Nombre + " ";
                    }
                    txtblResultado1.Text += "\n";*/
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
                                        txtblResultado.Text += "Error en la linea " + (i + 1) + ": Simbolo no definido: " + letra + "\n";
                                        hayErrorLexico = true;
                                        hayErroresPrograma = true;
                                        break;
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
                                //txtblResultado.Text += "\n";
                                resultados.Add("\n");
                            }
                            else if (tipoInstruccion == "IMPRIMIRCADENA")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                foreach (Token token in tokens)
                                {
                                    if (token.Nombre == "CADENA")
                                    {
                                        //txtblResultado.Text += token.Lexema;// + "\n";
                                        resultados.Add(token.Lexema);
                                    }    
                                        
                                }

                            }

                            else if (tipoInstruccion == "ASIGNACION")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                int valorAux(bool valor) => valor ? 1 : 0;
                                int valorResultado = 0;
                                bool existeSimbolo = false;
                                var expr = new BooleanAlgebraExpression(VerificacionErroresLexicos.GenerarExpresionParaEvaluar(tokens, simbolos));
                                if (tokens[2].Nombre == "TAUTOLOGIA")
                                {
                                    List<Token> copia = new List<Token>();
                                    for (int h = 2; h < tokens.Count; h++)
                                    {
                                        copia.Add(new Token
                                        {
                                            Lexema = tokens[h].Lexema,
                                            Valor = tokens[h].Valor,
                                            Nombre = tokens[h].Nombre,
                                            Posicion = tokens[h].Posicion

                                        });
                                    }
                                    valorResultado= Semantica.EvaluarTautologia(copia);
                                    foreach (Token token in simbolos)
                                    {
                                        if (token.Lexema == tokens[0].Lexema)
                                        {
                                            existeSimbolo = true;
                                            token.Valor = valorResultado;
                                            
                                        }

                                    }

                                }
                                else if (tokens[2].Nombre == "CONTRADICCION")
                                {
                                    List<Token> copia = new List<Token>();
                                    for (int h = 2; h < tokens.Count; h++)
                                    {
                                        copia.Add(new Token
                                        {
                                            Lexema = tokens[h].Lexema,
                                            Valor = tokens[h].Valor,
                                            Nombre = tokens[h].Nombre,
                                            Posicion = tokens[h].Posicion

                                        });
                                    }
                                    valorResultado = Semantica.EvaluarContradiccion(copia);
                                    foreach (Token token in simbolos)
                                    {
                                        if (token.Lexema == tokens[0].Lexema)
                                        {
                                            existeSimbolo = true;
                                            token.Valor = valorResultado;
                                        }

                                    }

                                }
                                else
                                {
                                    bool valorbool = expr.Evaluate();
                                    foreach (Token token in simbolos)
                                    {
                                        if (token.Lexema == tokens[0].Lexema)
                                        {
                                            existeSimbolo = true;
                                            token.Valor = valorAux(valorbool);
                                            valorResultado = token.Valor;
                                            codigoEnsamblador.Add(token.Lexema + " db " + valorResultado.ToString());
                                        }

                                    }
                                }
                                if (!existeSimbolo)
                                    simbolos.Add(new Token(Name = "VARIABLE", tokens[0].Lexema, simbolos[simbolos.Count - 1].Posicion + 1, valorResultado));


                                //int valorAux = Semantica.EvaluarExpresion(tokens, simbolos);



                            }
                            else if (tipoInstruccion == "IMPRIMIREXPRESION")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                string auxNombreVariable = "";
                                if (tokens[2].Nombre == "VARIABLE")
                                {
                                    foreach (Token token in tokens)
                                    {
                                        if (token.Nombre == "VARIABLE")
                                        {
                                            Console.Write("a");
                                            auxNombreVariable = token.Lexema;
                                        }
                                    }
                                    bool existeVariable = false;
                                    foreach (Token token in simbolos)
                                    {
                                        if (token.Lexema == auxNombreVariable)
                                        {
                                            existeVariable = true;
                                            //txtblResultado.Text += token.Valor;
                                            resultados.Add(token.Valor.ToString()); ;
                                        }
                                    }
                                    if (!existeVariable)
                                    {
                                        simbolos.Add(new Token("VARIABLE", auxNombreVariable, simbolos[simbolos.Count - 1].Posicion + 1, 0));
                                        //txtblResultado.Text += "0";
                                        resultados.Add("0");
                                    }
                                }
                                else
                                {
                                    int valorAux(bool valor) => valor ? 1 : 0;
                                    int valorResultado = 0;
                                    var expr = new BooleanAlgebraExpression(VerificacionErroresLexicos.GenerarExpresionParaEvaluar(tokens, simbolos));
                                    if (tokens[2].Nombre == "TAUTOLOGIA")
                                    {
                                        List<Token> copia = new List<Token>();
                                        for (int h = 2; h < tokens.Count; h++)
                                        {
                                            copia.Add(new Token
                                            {
                                                Lexema = tokens[h].Lexema,
                                                Valor = tokens[h].Valor,
                                                Nombre = tokens[h].Nombre,
                                                Posicion = tokens[h].Posicion

                                            });
                                        }
                                        valorResultado = Semantica.EvaluarTautologia(copia);
                                        //txtblResultado.Text += valorResultado.ToString();
                                        resultados.Add(valorResultado.ToString());
                                    }
                                    else if (tokens[2].Nombre == "CONTRADICCION")
                                    {
                                        List<Token> copia = new List<Token>();
                                        for (int h = 2; h < tokens.Count; h++)
                                        {
                                            copia.Add(new Token
                                            {
                                                Lexema = tokens[h].Lexema,
                                                Valor = tokens[h].Valor,
                                                Nombre = tokens[h].Nombre,
                                                Posicion = tokens[h].Posicion

                                            });
                                        }
                                        valorResultado = Semantica.EvaluarContradiccion(copia);
                                        txtblResultado.Text += valorResultado.ToString();
                                    }
                                    else
                                    {
                                        bool valorbool = expr.Evaluate();
                                        valorResultado = valorAux(valorbool);
                                        //txtblResultado.Text += valorResultado.ToString();
                                        resultados.Add(valorResultado.ToString());
                                    }
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
                                        //txtblResultado.Text += variables[j] + "\t";
                                        resultados.Add(variables[j] + "\t");
                                    }
                                    //txtblResultado.Text += "Result\n";
                                    resultados.Add("Result\n");
                                    int filas = tablaVerdad[0].Count;
                                    int columnas = tablaVerdad.Count;
                                    for (int j = filas - 1; j >= 0; j--)
                                    {
                                        for (int k = 0; k < columnas-1 ; k++)
                                        {
                                            //txtblResultado.Text += tablaVerdad[k][j] + "\t";
                                            resultados.Add(tablaVerdad[k][j] + "\t");
                                        }
                                        //txtblResultado.Text += tablaVerdad[tablaVerdad.Count - 1][j] + "\t";
                                        //txtblResultado.Text += "\n";
                                        resultados.Add(tablaVerdad[tablaVerdad.Count - 1][j] + "\t");
                                        resultados.Add("\n");
                                    }
                                }
                            }
                            else if(tipoInstruccion == "TAUTOLOGIA")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                int esTauto = Semantica.EvaluarTautologia(tokens);
                                //txtblResultado.Text += esTauto + "\n";
                            }
                            else if (tipoInstruccion == "CONTRADICCION")
                            {
                                _ = Class1.generarTablaSimbolos(tokens, simbolos);
                                int esTauto = Semantica.EvaluarContradiccion(tokens);
                                //txtblResultado.Text += esTauto + "\n";
                            }

                            else
                            {
                                txtblResultado.Text += tipoInstruccion + ": Linea " + (i+1) + "\n\n";
                                hayErroresPrograma = true;
                                break;
                            }

                        } 

                    }
                    else
                    {
                        break;
                    }
                    
                }
                if (!hayErroresPrograma)
                {
                    tablaSimbolos.ItemsSource = simbolos;
                    foreach (string resultado in resultados)
                    {
                        txtblResultado.Text += resultado;

                    }
                    foreach (string variable in asignaciones)
                    {
                        codigoEnsamblador.Add(variable);
                    }

                    codigoEnsamblador.Add(".data ends");
                    codigoEnsamblador.Add("inicio proc near");
                    codigoEnsamblador.Add("ret");
                    using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\reg.log"), append: false, Encoding.ASCII))
                    {
                        

                        foreach (string linea in codigoEnsamblador)
                        {
                            writer.WriteLine(linea + "\n");
                            
                        }
                        //                writer.Flush();
                        writer.Close();

                    }
                }
                

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
            simbolos = new List<Token>();
            tablaSimbolos.ItemsSource = simbolos;
            //txtblResultado1.Text = "Resultado del Código";
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var direccion = new SaveFileDialog();
            direccion.Filter = "Archivos de texto(*.txt)|*.txt";
            if (direccion.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(direccion.FileName, append: false, Encoding.ASCII))
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
                    MessageBox.Show("Archivo guardado en el escritorio como: " + direccion.FileName);
                }
            }
            
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
            var direccion = new SaveFileDialog();
            direccion.Filter = "Archivos de texto(*.txt)|*.txt";
            if (direccion.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(direccion.FileName, append: false, Encoding.ASCII))
                //using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\resultados.log"), append: false, Encoding.ASCII))
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
                _ = MessageBox.Show("Archivos guardados en el escritorio como: "+ direccion.FileName);
            }
            
        }



    }
}
