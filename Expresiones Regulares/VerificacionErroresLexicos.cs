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

    }
}
