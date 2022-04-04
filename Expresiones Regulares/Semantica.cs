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
        /// 
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
            int numeroFilas = variables.Count;
            List<List<int>> tabla = new List<List<int>>();
            
            for (i = 0; i < numeroFilas; i++)
            {
                List<int> valores = new List<int>();
                for (int j = 0; j < numeroFilas; j++)
                {
                    //bool tocaUno;
                    if (j >= Math.Pow(2, i))
                    {
                        valores.Add(0);
                    }
                    else
                    {
                        valores.Add(1);
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
            Token token1 = new Token();
            List<Token> nuevaExpresion = tokens;

            foreach(Token token in nuevaExpresion)
            {
                if (token.Nombre == "NOT") 
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i - 1].Lexema)
                        {

                            token1 = auxToken1;
                        }
                    }
                    nuevaExpresion.Remove(token);
                    nuevaExpresion[i].Nombre = "VARIABLE";
                    if (nuevaExpresion[i].Valor == 0)
                    {
                        nuevaExpresion[i].Valor = 1;
                    }
                    else 
                        nuevaExpresion[i].Valor = 0;
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
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "AND")
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
                    if (token1.Valor == 1 && token2.Valor == 1)
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i-1]);
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
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "OR")
                {
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i - 1].Lexema)
                        {
                            existeToken1 = true;
                            token1 = auxToken1;
                        }
                    }
                    if (!existeToken1)
                    {
                        simbolos.Add(new Token("VARIABLE", nuevaExpresion[i - 1].Lexema, simbolos[simbolos.Count-1].Valor+1, 0));
                    }
                    foreach (Token auxToken1 in simbolos)
                    {
                        if (auxToken1.Lexema == nuevaExpresion[i + 1].Lexema)
                        {
                            existeToken2 = true;
                            token2 = auxToken1;
                        }
                    }
                    if (!existeToken2)
                    {
                        simbolos.Add(new Token("VARIABLE", nuevaExpresion[i + 1].Lexema, simbolos[simbolos.Count - 1].Valor+1, 0));
                    }
                    if (token1.Valor == 1 || token2.Valor == 1)
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
                        nuevaExpresion[i].Valor = 0;
                        nuevaExpresion[i].Nombre = "VARIABLE";
                    }
                        
                }

                i++;
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
            int resultado;
            List<Token> nuevaExpresion = tokens;
            nuevaExpresion = EvaluarNegacion(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarAnd(nuevaExpresion, simbolos);
            nuevaExpresion = EvaluarOR(nuevaExpresion, simbolos);
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
            string resultado = "";
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



            return resultado;
        }
    }
}
