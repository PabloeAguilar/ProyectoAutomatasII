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
        /// <param name="tokens">Lista de tokens ordenados con sintaxis correcta</param>
        /// <returns>Lista de listas de valores para tabla de verdad</returns>
        /// 

        public static List<List<int>> TablaVerdadParte1(List<Token> tokens) 
        {
            
            
            int numeroTokens = 0;
            List<Token> variables = new List<Token>();
            foreach(Token token in tokens)
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
                bool primerCambio = true;
                bool tocaUno = false;
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
                            //tocaUno = false;
                        }
                        else
                        {
                            valores.Add(0);
                            //tocaUno = true;
                        }
                    }
                    if (tabla.Count > 0 && tabla[i-1][j] != valores[j])
                    {
                        if (tocaUno && !primerCambio)
                        {
                            //valores.Add(1);
                            tocaUno = false;
                            primerCambio = true;
                        }
                        else
                        {
                            //valores.Add(0);
                            primerCambio = true;
                            tocaUno = true;
                        }
                    }
                    
                    /*if (j % Math.Pow(2, i + 1) == 0)
                    {
                        if (tocaUno)
                            tocaUno = false;
                        else
                            tocaUno = true;
                    }
                    */
                    /*if (Math.Pow(2, numeroFilas) )
                    //bool tocaUno;
                    if (Math.Pow(2, numeroFilas) % ((i+1)*2) == 2)
                    {
                        valores.Add(0);
                    }
                    else
                    {
                        valores.Add(1);
                    }*/
                    
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
            Token token1 = new Token();
            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < tokens.Count; j++)
            {
                if (tokens[j].Nombre == "NOT")
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[j + 1].Lexema)
                        {
                            token1 = auxToken1;
                        }
                    }
                    nuevaExpresion.Remove(tokens[j]);
                    nuevaExpresion[j].Nombre = "VARIABLE";
                    if (nuevaExpresion[j].Valor == 0)
                    {
                        nuevaExpresion[j].Valor = 1;
                        token1.Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion[j].Valor = 0;
                        token1.Valor = 0;
                    }
                        
                }
                i++;
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
            Token token1 = new Token() ;
            Token token2 = new Token();


            List<Token> nuevaExpresion = tokens;

            for (int j = 0; j < tokens.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "AND")
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[j - 1].Lexema)
                        {

                            token1 = auxToken1;
                        }
                    }
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[j + 1].Lexema)
                        {

                            token2 = auxToken1;
                        }
                    }
                    if (nuevaExpresion[j-1].Valor == 1 && nuevaExpresion[j+1].Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j + 1]);
                        nuevaExpresion[j].Nombre = "VARIABLE";
                        nuevaExpresion[j].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j + 1]);
                        nuevaExpresion[j].Nombre = "VARIABLE";
                        nuevaExpresion[j].Valor = 0;
                    }

                }

                i++;
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
            bool existeToken1 = false;
            bool existeToken2 = false;
            Token token1 = new Token();
            Token token2 = new Token();
            



            List<Token> nuevaExpresion = tokens;
            for (int j = 0; j < nuevaExpresion.Count; j++)
            {
                if (nuevaExpresion[j].Nombre == "OR")
                {
                    /*foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[j - 1].Lexema)
                        {
                            existeToken1 = true;
                            token1 = auxToken1;
                        }
                    }
                    if (!existeToken1)
                    {
                        simbolos.Add(new Token("VARIABLE", nuevaExpresion[j - 1].Lexema, simbolos[simbolos.Count - 1].Valor + 1, 0));
                    }
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[j + 1].Lexema)
                        {
                            existeToken2 = true;
                            token2 = auxToken1;
                        }
                    }
                    if (!existeToken2)
                    {
                        simbolos.Add(new Token("VARIABLE", nuevaExpresion[j + 1].Lexema, simbolos[simbolos.Count - 1].Valor + 1, 0));
                    }*/
                    if (nuevaExpresion[j-1].Valor == 1 || nuevaExpresion[j+1].Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j]);
                        nuevaExpresion[j].Nombre = "VARIABLE";
                        nuevaExpresion[j].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[j - 1]);
                        nuevaExpresion.Remove(nuevaExpresion[j + 1]);
                        nuevaExpresion[j].Valor = 0;
                        nuevaExpresion[j].Nombre = "VARIABLE";
                    }

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
            EvaluarNegacion(tokens, simbolos);
            i = 0;
            Token token1 = new Token();
            Token token2 = new Token();


            List<Token> nuevaExpresion = tokens;
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "ENTONCES")
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i - 1].Lexema)
                        {

                            token1 = auxToken1;
                        }
                    }
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i + 1].Lexema)
                        {

                            token2 = auxToken1;
                        }
                    }
                    if (token1.Valor == 1 && token2.Valor == 0)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i - 1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
                        nuevaExpresion[i].Valor = 0;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i - 1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
                        nuevaExpresion[i].Valor = 1;
                    }

                }

                i++;
            }
            return nuevaExpresion;
        }

        static public List<Token> EvaluarDobleEntonces (List<Token> tokens, List<Token> simbolos)
        {
            EvaluarNegacion(tokens, simbolos);
            i = 0;
            Token token1 = new Token();
            Token token2 = new Token();


            List<Token> nuevaExpresion = tokens;
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "DOBLEENTONCES")
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i - 1].Lexema)
                        {

                            token1 = auxToken1;
                        }
                    }
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i + 1].Lexema)
                        {

                            token2 = auxToken1;
                        }
                    }
                    if (token1.Valor == token2.Valor )
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i - 1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
                        nuevaExpresion[i].Valor = 1;
                    }
                    else
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i - 1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
                        nuevaExpresion[i].Valor = 0;
                    }

                }

                i++;
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
            foreach(Token token in tokens)
            {
                if (token.Nombre == "IGUAL")
                {
                    if (tokens.Count > 3 && tokens[i + 1].Nombre == "CONSTANTE" && tokens[i+2].Nombre == "TERMINADOR")
                    {
                        return tokens[i + 1].Valor;
                    }
                }
                i++;
            }
            foreach (Token token1 in nuevaExpresion)
            {
                if (token1.Nombre == "VARIABLE")
                    return token1.Valor;
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
            else if (tokens[tokens.Count-1].Nombre != "TERMINADOR")
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
            else if (tokens[0].Nombre == "IMPRIMIREXPRESION" )
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
            else if(tokens[0].Nombre == "IMPRIMIRTABLA")
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



            return "Error en la expresion";
        }

        static Token[] ValidarExpresionTablaVerdad(List<Token> tokens)
        {
            int i = 0;
            Token[] auxTokensExpresion = new Token[0];
            foreach(Token token in tokens)
            {
                if (token.Nombre == "PARENTESISAPERTURA")
                {
                    auxTokensExpresion = new Token[tokens.Count - 4];
                    tokens.CopyTo(i + 1, auxTokensExpresion, 0, tokens.Count - 4);
                }
                i++;
            }
            if (auxTokensExpresion.Length > 0)
            {
                
                foreach (Token token in auxTokensExpresion)
                {
                    if (token.Nombre != "VARIABLE" && token.Nombre != "AND" && token.Nombre != "OR" && token.Nombre != "NOT")
                        return new Token[0];
                }
                return auxTokensExpresion;
            }
            return new Token[0];
        }


        /// <summary>
        /// Metodo que genera una tabla de verdad
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static List<List<int>> TablaDeVerdadParte2(List<Token> tokens)
        {
            
            List<List<int>> tablaDeVerdad = new List<List<int>>();
            Token[] tokensExpresion = ValidarExpresionTablaVerdad(tokens);
            if (tokensExpresion.Length > 0)
            {
                List<List<int>> valores = TablaVerdadParte1(tokens);
                int j = valores[0].Count; //cuenta el numero de renglones
                List<int> resultados = new List<int>();
                for (int k = 0; k < j; k++) // ciclo para evaluar todas las lineas de la tabla de verdad
                {
                    int auxCuentaVariable = 0;
                    List<Token> simbolos = new List<Token>();
                    //string variableActual = "";
                    foreach(Token token in tokensExpresion) // genera una nueva tabla de simbolos
                    {
                        if (token.Nombre == "VARIABLE")
                        {

                            token.Valor = valores[auxCuentaVariable++][k];
                            //simbolos.Add(new Token("VARIABLE", token.Lexema, 0, valores[auxCuentaVariable++][k]));
                        }
                        
                    }
                    List<Token> copiaTokens = new List<Token>(tokens);
                    resultados.Add(EvaluarExpresion(copiaTokens, simbolos));
                }
                valores.Add(resultados);
                tablaDeVerdad = valores;
            }
            


            return tablaDeVerdad;

        }
    }
}
