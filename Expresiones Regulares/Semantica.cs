using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public static class Semantica
    {
        /// <summary>
        /// clase para comprobar semantica de las gramaticas y errores de gramaticas
        /// </summary>
        /// 
        static int i = 0;


        /// <summary>
        /// Metodo que genera la parte de los 1s y 0s correspondientes a las variables de una tabla de verdad
        /// </summary>
        /// <param name="tokens">Lista de tokens de la expresion con sintaxis correcta</param>
        /// <returns>Lista de listas de valores para tabla de verdad</returns>
        /// 

        public static List<List<int>> TablaVerdadParte1(List<Token> tokens)
        {

            int numeroTokens = 0;
            List<Token> variables = new List<Token>();
            foreach (Token token in tokens)
            {
                if (token.Nombre == "VARIABLE")
                {
                    numeroTokens++;
                    variables.Add(token);
                }
            }
            int numColumnas = variables.Count;
            double numFilas = Math.Pow(2, numColumnas);
            List<List<int>> tabla = new List<List<int>>();
            for (i = 0; i < numColumnas; i++)
            {
                double cambio = Math.Pow(2, i);
                int contadorCambio = 0;
                bool tocaUno = true ;
                List<int> valores = new List<int>();
                for (int j = 0; j < numFilas; j++)
                {
                    if (i == 0)
                    {
                        if (tocaUno)
                        {
                            valores.Add(1);
                            tocaUno = false;
                        }
                        else
                        {
                            valores.Add(0);
                            tocaUno = true;
                        }
                    }
                    else
                    {
                        if (tocaUno)
                        {
                            valores.Add(1);
                            contadorCambio++;
                            //tocaUno = false;
                        }
                        else
                        {
                            valores.Add(0);
                            contadorCambio++;
                            //tocaUno = true;
                        }
                    }
                    if (contadorCambio >= cambio)
                    {
                        if (tocaUno)
                        {
                            //valores.Add(1);
                            tocaUno = false;
                            contadorCambio = 0;
                        }
                        else
                        {
                            //valores.Add(0);
                            tocaUno = true;
                            contadorCambio = 0;
                        }
                    }
                    
                        


                }
                tabla.Add(valores);
            }

            return tabla;
        }

        /// <summary>
        /// Evalua las negaciones en un conjunto de tokens
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <param name="simbolos">Lista correspondiente a la tabla de simbolos</param>
        /// <returns>Lista de tokens con operacion Negacion aplicada</returns>
        /// 
        static List<Token> EvaluarNegacion(List<Token> tokens, List<Token> simbolos)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "NOT")
                {
                    if (nuevaExpresion[j + 1].Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j].Nombre = "VARIABLE";
                        nuevaExpresion[j].Valor = 0;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j].Valor = 1;
                        nuevaExpresion[j].Nombre = "VARIABLE";
                    }
                }
            }
            return nuevaExpresion;
        }

        /// <summary>
        /// Evalua las operaciones AND en un conjunto de tokens
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <param name="simbolos">Lista correspondiente a la tabla de simbolos</param>
        /// <returns>Lista de tokens con operacion AND aplicada</returns>
        static List<Token> EvaluarAnd(List<Token> tokens, List<Token> simbolos)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "AND")
                {
                    if (nuevaExpresion[j - 1].Valor == 1 && nuevaExpresion[j + 1].Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                        nuevaExpresion[j - 1].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Valor = 0;
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                    }
                    j--;
                }
            }
            return nuevaExpresion;
        }

        /// <summary>
        /// Evalua las operaciones OR en un conjunto de tokens
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <param name="simbolos">Lista correspondiente a la tabla de simbolos</param>
        /// <returns>Lista de tokens con operacion OR aplicada</returns>
        static List<Token> EvaluarOR(List<Token> tokens, List<Token> simbolos)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "OR")
                {
                    if (nuevaExpresion[j - 1].Valor == 1 || nuevaExpresion[j + 1].Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j-1].Nombre = "VARIABLE";
                        nuevaExpresion[j-1].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j-1].Valor = 0;
                        nuevaExpresion[j-1].Nombre = "VARIABLE";
                    }
                    j--;
                }
            }
            return nuevaExpresion;
        }


        /// <summary>
        /// Método que devuelve el valor del operador -> 
        /// </summary>
        /// <param name="tokens"> Lista de tokens identificados</param>
        /// <param name="simbolos">Lista de simbolos identificados</param>
        static List<Token> EvaluarEntonces(List<Token> tokens, List<Token> simbolos)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "ENTONCES")
                {
                    if (nuevaExpresion[j - 1].Valor == 1 && nuevaExpresion[j + 1].Valor == 0)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                        nuevaExpresion[j - 1].Valor = 0;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Valor = 1;
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                    }
                    j--;
                }
            }
            return nuevaExpresion;
        }

        static public List<Token> EvaluarDobleEntonces(List<Token> tokens, List<Token> simbolos)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "DOBLEENTONCES")
                {
                    if (nuevaExpresion[j - 1].Valor == nuevaExpresion[j + 1].Valor)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                        nuevaExpresion[j - 1].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j - 1].Valor = 0;
                        nuevaExpresion[j - 1].Nombre = "VARIABLE";
                    }
                    j--;
                }
            }
            return nuevaExpresion;
        }


        /// <summary>
        /// Evalua las operaciones de negacion, and y or y retorna un valor entero 1 o 0
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <returns>Int 1 o 0 </returns>
        public static int EvaluarExpresion(List<Token> tokens, List<Token> simbolos)
        {
            int i = 0;
            int resultado;
            List<Token> nuevaExpresion = tokens;
            nuevaExpresion = EvaluarNegacion(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarAnd(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarOR(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarEntonces(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarDobleEntonces(nuevaExpresion, simbolos);

            foreach (Token token in tokens)
            {
                if (token.Nombre == "IGUAL")
                {
                    if (tokens.Count > 3 && tokens[i + 1].Nombre == "CONSTANTE" && tokens[i + 2].Nombre == "TERMINADOR")
                    {
                        return tokens[i + 1].Valor;
                    }
                    else if (tokens.Count > 3 && tokens[i + 1].Nombre == "VARIABLE" && tokens[i + 2].Nombre == "TERMINADOR")
                    {
                        return tokens[i + 1].Valor;
                    }
                }
                i++;
            }
            int aux = 0;
            foreach (Token token1 in nuevaExpresion)
            {
                if (token1.Nombre == "VARIABLE")
                {
                    if (aux != 0)
                        return token1.Valor;
                }

                aux++;
            }
            resultado = nuevaExpresion[0].Valor;
            return resultado;
        }


        /// <summary>
        /// Metodo que analiza los tokens en busca de algunos tipos de instruccion predefinidos o ciertos tipos de errores
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string TipoInstruccion(List<Token> tokens)
        {
            //string resultado = "";
            string cadenaAux = "";
            int parentesisApertura = 0;
            int parentesisCierre = 0;
            foreach (Token token in tokens)
            {
                cadenaAux += token.Lexema;
                if (token.Nombre == "PARENTESISAPERTURA")
                    parentesisApertura++;
                else if (token.Nombre == "PARENTESISCIERRE")
                    parentesisCierre++;
            }
            if (parentesisApertura < parentesisCierre)
                return "Falta parentesis de apertura";
            else if (parentesisApertura > parentesisCierre)
                return "Falta parentesis de cierre";
            else if (tokens[tokens.Count - 1].Nombre != "TERMINADOR")
            {
                return "Falta terminador";
            }

            if (tokens[0].Nombre == "IMPRIMIRRETORNO")
            {
                if (Regex.IsMatch(cadenaAux, Class1.WINTRO))
                {
                    return "IMPRIMIRRETORNO";
                }

            }
            else if (tokens[0].Nombre == "IMPRIMIREXPRESION")
            {
                if (Regex.IsMatch(cadenaAux, Class1.WLOG))
                {
                    return "IMPRIMIREXPRESION";
                }
                else if (!Regex.IsMatch(cadenaAux, Class1.WLOG))
                {
                    return "Error en expresion";
                }

            }
            else if (tokens[0].Nombre == "IMPRIMIRCADENA")
            {
                if (Regex.IsMatch(cadenaAux, Class1.WSTR))
                {
                    return "IMPRIMIRCADENA";
                }
                else if (!Regex.IsMatch(cadenaAux, "\".+\""))
                {
                    return "Error en la cadena a imprimir";
                }

            }

            else if (tokens[0].Nombre == "VARIABLE")
            {
                if (Regex.IsMatch(cadenaAux, Class1.ASIGNACION))
                {
                    return "ASIGNACION";
                }
                else if (!Regex.IsMatch(cadenaAux, Class1.EXPRESION))
                {
                    return "Error en la expresion";
                }
            }
            else if (tokens[0].Nombre == "IMPRIMIRTABLA")
            {
                if (Regex.IsMatch(cadenaAux, Class1.WTABLA))
                {
                    return "IMPRIMIRTABLA";
                }
                else if (!Regex.IsMatch(cadenaAux, Class1.EXPRESION))
                {
                    return "Error en la expresion";
                }
            }
            else if (tokens[0].Nombre == "TAUTOLOGIA")
            {
                if (Regex.IsMatch(cadenaAux, Class1.TAUTO))
                {
                    return "TAUTOLOGIA";
                }
                else if (!Regex.IsMatch(cadenaAux, Class1.EXPRESION))
                {
                    return "Error en la expresion";
                }
            }
            else if (tokens[0].Nombre == "CONTRADICCION")
            {
                if (Regex.IsMatch(cadenaAux, Class1.CONTRA))
                {
                    return "CONTRADICCION";
                }
                else if (!Regex.IsMatch(cadenaAux, Class1.EXPRESION))
                {
                    return "Error en la expresion";
                }
            }



            return "Error en la expresion";
        }

        static List<Token> ValidarExpresionTablaVerdad(List<Token> tokens)
        {
            if(tokens[1].Nombre == "IGUAL")
            {
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
            }

            List<string> variables = new List<string>();
            
            if (tokens.Count > 0)
            {
                for (int z = 0; z < tokens.Count; z++)
                {
                    if (tokens[z].Nombre != "VARIABLE" && tokens[z].Nombre != "AND" && tokens[z].Nombre != "OR" && tokens[z].Nombre != "NOT" &&
                        tokens[z].Nombre != "ENTONCES" && tokens[z].Nombre != "DOBLEENTONCES")
                    {
                        tokens.RemoveAt(z);
                        z--;
                        if (z>0)
                        {
                            
                            if (tokens[z].Nombre != "VARIABLE" && tokens[z].Nombre != "AND" && tokens[z].Nombre != "OR" && tokens[z].Nombre != "NOT" &&
                        tokens[z].Nombre != "ENTONCES" && tokens[z].Nombre != "DOBLEENTONCES")
                            {
                                tokens.RemoveAt(z);
                            }
                        }
                        
                    }
                }
                return tokens;
            }
            return new List<Token>();
        }


        /// <summary>
        /// Metodo que genera una tabla de verdad
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static List<List<int>> TablaDeVerdadParte2(List<Token> tokens)
        {           
            List<List<int>> tablaDeVerdad = new List<List<int>>();
            List<Token> copiaTokens1 = CopiarLista(tokens);
            List<Token> tokensExpresion = ValidarExpresionTablaVerdad(copiaTokens1);
            if (tokensExpresion.Count > 0)
            {
                List<Token> listacopia = CopiarLista(tokensExpresion);
                List<Token> simbolos = new List<Token>();
                foreach (Token token in listacopia)
                {
                    if (token.Nombre == "VARIABLE")
                    {
                        bool simboloExiste = false;
                        foreach (Token token1 in simbolos)
                        {
                            if (token.Lexema == token1.Lexema)
                            {
                                simboloExiste = true;
                                token.Valor = token1.Valor;
                            }
                        }
                        if (!simboloExiste)
                        {
                            if (token.Valor < 0)
                            {
                                token.Valor = 0;
                                token.Posicion = 0;
                            }
                            simbolos.Add(token);
                        }
                    }
                }

                List<List<int>> valores = TablaVerdadParte1(simbolos);
                int j = valores[0].Count; //cuenta el numero de renglones
                List<int> resultados = new List<int>();


                for (int k = 0; k < j; k++) // ciclo para evaluar todas las lineas de la tabla de verdad
                {
                    copiaTokens1 = CopiarLista(tokens);
                    tokensExpresion = ValidarExpresionTablaVerdad(copiaTokens1);
                    int contadorAux = 0;
                    foreach (Token token2 in simbolos)
                    {
                        if (token2.Nombre == "VARIABLE")
                        {
                            token2.Valor = valores[contadorAux][k];
                            contadorAux++;
                        }
                        
                    }
                    foreach(Token token1 in simbolos)
                    {
                        foreach(Token token in copiaTokens1)
                        {
                            if (token1.Nombre == "VARIABLE" && token1.Lexema == token.Lexema)
                            {
                                token.Valor = token1.Valor;
                            }
                        }
                    }
                    resultados.Add(EvaluarExpresion(tokensExpresion, simbolos));

                }
                valores.Add(resultados);
                tablaDeVerdad = valores;
            }
            return tablaDeVerdad;

        }

        public static int EvaluarTautologia(List<Token>tokens)
        {
            List<List<int>> tablaVerdad = TablaDeVerdadParte2(tokens);
            int esTauto = 1;
            foreach(int valor in tablaVerdad[tablaVerdad.Count-1]) 
            {
                if (valor == 0)
                {
                    return 0;
                }
            }
        
            return esTauto;
        }

        public static int EvaluarContradiccion(List<Token> tokens)
        {
            List<List<int>> tablaVerdad = TablaDeVerdadParte2(tokens);
            int esTauto = 1;
            foreach (int valor in tablaVerdad[tablaVerdad.Count - 1])
            {
                if (valor == 1)
                {
                    return 0;
                }
            }

            return esTauto;
        }

        public static List<Token> CopiarLista(List<Token> tokens)
        {
            List<Token> tokens1 = new List<Token>();
            foreach(Token token in tokens )
            {
                Token aux = new Token(token.Nombre, token.Lexema, token.Posicion, token.Valor);
                tokens1.Add(aux);
            }
            return tokens1;
        }

    }
}
