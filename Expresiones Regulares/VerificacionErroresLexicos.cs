using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public static class VerificacionErroresLexicos
    {
        public static string EliminacionTokens(string cadena, List<Token> tokens)
        {
            string cadenaAux = cadena;
            foreach (Token token in tokens)
            {
                if (token.Nombre == "TAUTOLOGIA" || token.Nombre == "IMPRIMIRTABLA" || token.Nombre == "CONTRADICCION" || token.Nombre == "IMPRIMIRRETORNO" ||
                    token.Nombre == "IMPRIMIREXPRESION" || token.Nombre == "IMPRIMIRCADENA" )
                {
                    cadenaAux = cadenaAux.Replace(token.Lexema, "");
                }
                
            }
            foreach (Token token in tokens)
            {
                cadenaAux = cadenaAux.Replace(token.Lexema, "");
            }

            return cadenaAux;
        }

        public static string GenerarExpresionParaEvaluar(List<Token> tokens, List<Token> simbolos)
        {
            foreach(Token token in simbolos)
            {
                foreach(Token token1 in tokens)
                {
                    if (token.Lexema == token1.Lexema)
                    {
                        token1.Valor = token.Valor;
                    }
                }
            }
            string resultado = "";
            int i = 0;
            while (tokens[i].Nombre != "IGUAL" || i == tokens.Count )
            {
                i++;
            }
            if (i++ < tokens.Count)
            {
                if (tokens[i].Nombre == "TAUTOLOGIA" && tokens[i].Nombre == "CONTRADICCION")
                {

                    while (tokens[i].Nombre != "TERMINADOR")
                    {
                        if (tokens[i].Nombre == "VARIABLE")
                        {
                            if (tokens[i].Valor == 0 || tokens[i].Valor == -1)
                            {
                                resultado += "false ";
                            }
                            else if (tokens[i].Valor == 1)
                            {
                                resultado += "true ";
                            }
                        }
                        if (tokens[i].Nombre == "CONSTANTE")
                        {
                            if (tokens[i].Lexema == "0")
                            {
                                resultado += "false ";
                            }
                            else if (tokens[i].Lexema == "1")
                            {
                                resultado += "true ";
                            }
                        }
                        else if (tokens[i].Nombre == "NOT")
                        {
                            resultado += "not ";
                        }
                        else if (tokens[i].Nombre == "AND")
                        {
                            resultado += "and ";
                        }
                        else if (tokens[i].Nombre == "OR")
                        {
                            resultado += "or ";
                        }
                        else if (tokens[i].Nombre == "ENTONCES")
                        {
                            resultado += "implecation ";
                        }
                        else if (tokens[i].Nombre == "DOBLEENTONCES")
                        {
                            resultado += "eqvivalence ";
                        }
                        else if (tokens[i].Nombre == "PARENTESISAPERTURA")
                        {
                            resultado += "( ";
                        }
                        else if (tokens[i].Nombre == "PARENTESISCIERRE")
                        {
                            resultado += ") ";
                        }
                        i++;
                    }
                }
                else
                {
                    while (tokens[i].Nombre != "TERMINADOR")
                    {
                        if (tokens[i].Nombre == "VARIABLE")
                        {
                            if (tokens[i].Valor == 0 || tokens[i].Valor == -1)
                            {
                                resultado += "false ";
                            }
                            else if (tokens[i].Valor == 1)
                            {
                                resultado += "true ";
                            }
                        }
                        if (tokens[i].Nombre == "CONSTANTE")
                        {
                            if (tokens[i].Lexema == "0")
                            {
                                resultado += "false ";
                            }
                            else if (tokens[i].Lexema == "1")
                            {
                                resultado += "true ";
                            }
                        }
                        else if (tokens[i].Nombre == "NOT")
                        {
                            resultado += "not ";
                        }
                        else if (tokens[i].Nombre == "AND")
                        {
                            resultado += "and ";
                        }
                        else if (tokens[i].Nombre == "OR")
                        {
                            resultado += "or ";
                        }
                        else if (tokens[i].Nombre == "ENTONCES")
                        {
                            resultado += "implecation ";
                        }
                        else if (tokens[i].Nombre == "DOBLEENTONCES")
                        {
                            resultado += "eqvivalence ";
                        }
                        else if (tokens[i].Nombre == "PARENTESISAPERTURA")
                        {
                            resultado += "( ";
                        }
                        else if (tokens[i].Nombre == "PARENTESISCIERRE")
                        {
                            resultado += ") ";
                        }
                        i++;
                    }
                }
                
            }
            resultado = resultado.Substring(0, resultado.Count() - 1);
            return resultado;

        }

        public static string GenerarExpresionParaTabla(List<Token> tokens, List<Token> simbolos)
        {
            foreach (Token token in simbolos)
            {
                foreach (Token token1 in tokens)
                {
                    if (token.Lexema == token1.Lexema)
                    {
                        token1.Valor = token.Valor;
                    }
                }
            }
            string resultado = "";
            int i = 0;
            while (tokens[i].Nombre != "PARENTESISAPERTURA")
            {
                i++;
            }
            if (i < tokens.Count)
            {
                while (tokens[i].Nombre != "TERMINADOR")
                {
                    if (tokens[i].Nombre == "VARIABLE")
                    {
                        if (tokens[i].Valor == 0 || tokens[i].Valor == -1)
                        {
                            resultado += "false ";
                        }
                        else if (tokens[i].Valor == 1)
                        {
                            resultado += "true ";
                        }
                    }
                    if (tokens[i].Nombre == "CONSTANTE")
                    {
                        if (tokens[i].Lexema == "0")
                        {
                            resultado += "false ";
                        }
                        else if (tokens[i].Lexema == "1")
                        {
                            resultado += "true ";
                        }
                    }
                    else if (tokens[i].Nombre == "NOT")
                    {
                        resultado += "not ";
                    }
                    else if (tokens[i].Nombre == "AND")
                    {
                        resultado += "and ";
                    }
                    else if (tokens[i].Nombre == "OR")
                    {
                        resultado += "or ";
                    }
                    else if (tokens[i].Nombre == "ENTONCES")
                    {
                        resultado += "materialImplecation ";
                    }
                    else if (tokens[i].Nombre == "DOBLEENTONCES")
                    {
                        resultado += "materialEqvivalence ";
                    }
                    else if (tokens[i].Nombre == "PARENTESISAPERTURA")
                    {
                        resultado += "( ";
                    }
                    else if (tokens[i].Nombre == "PARENTESISCIERRE")
                    {
                        resultado += ") ";
                    }
                    i++;
                }
            }
            resultado = resultado.Substring(0, resultado.Count() - 1);
            return resultado;

        }

    }
}
