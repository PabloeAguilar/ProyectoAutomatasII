using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public static class Semantica
    {
        /// <summary>
        /// clase para comprobar semantica de las gramaticas
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
        /// <returns>Lista de tokens con operacion Negacion aplicada</returns>
        static List<Token> EvaluarNegacion(List<Token> tokens)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            foreach(Token token in nuevaExpresion)
            {
                if (token.Nombre == "NOT") 
                {
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
        /// <returns>Lista de tokens con operacion AND aplicada</returns>
        static List<Token> EvaluarAnd(List<Token> tokens)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "AND")
                {
                    if(nuevaExpresion[i-1].Lexema == "1" && nuevaExpresion[i + 1].Lexema == "1")
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i-1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
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
        /// Evalua las operaciones OR en un conjunto de tokens
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <returns>Lista de tokens con operacion OR aplicada</returns>
        static List<Token> EvaluarOR(List<Token> tokens)
        {
            i = 0;
            List<Token> nuevaExpresion = tokens;
            foreach (Token token in nuevaExpresion)
            {
                if (token.Nombre == "OR")
                {
                    if (nuevaExpresion[i - 1].Lexema == "1" || nuevaExpresion[i + 1].Lexema == "1")
                    {
                        nuevaExpresion.Remove(nuevaExpresion[i - 1]);
                        nuevaExpresion.Remove(token);
                        nuevaExpresion[i].Nombre = "VARIABLE";
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
        /// Evalua las operaciones de negacion, and y or y retorna un valor entero 1 o 0
        /// </summary>
        /// <param name="tokens">Lista de tokens ordenados</param>
        /// <returns>Int 1 o 0 </returns>
        public static int EvaluarExpresion(List<Token> tokens)
        {
            int resultado;
            List<Token> nuevaExpresion = tokens;
            nuevaExpresion = EvaluarNegacion(nuevaExpresion);
            nuevaExpresion = EvaluarAnd(nuevaExpresion);
            nuevaExpresion = EvaluarOR(nuevaExpresion);
            resultado = nuevaExpresion[0].Valor;
            return resultado;
        }
    }
}
