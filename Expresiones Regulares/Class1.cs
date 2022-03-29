using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public static class Class1
    {
        static string TERMINADOR = ";";
        static string IGUAL = "=";
        static string AND = "\\*";
        static string OR = "\\+";
        static string NOT = "-";
        static string ENTONCES = "->";
        static string DOBLEENTONCES = "<->";
        static string PARENTESISAPERTURA = "\\(";
        static string PARENTESISCIERRE = "\\)";
        static string IMPRIMIREXPRESION = "writelog";
        static string IMPRIMIRCADENA = "writestr";
        static string IMPRIMIRRETORNO = "writeintro";
        static string CADENA = "\".+\"";
        static string VARIABLE = "[a-zA-Z][a-zA-Z0-9]*|_[a-zA-Z0-9]+";
        static string TAUTOLOGIA = "tauto";
        static string IMPRIMIRTABLA = "writetabla";
        static string CONTRADICCION = "contra";
        static string DECIDIBLE = "deci";

        static string OPERADOR = "(" + AND + "|" + OR + "|" + ENTONCES + "|" + DOBLEENTONCES + ")";
        static string EXPRESION = "((" + NOT + ")?" + VARIABLE + ")((" + OPERADOR + ")(((" + NOT + ")?" +
                                   VARIABLE + ")|(" + NOT + ")?" + PARENTESISAPERTURA + "(((" + NOT + ")?" +
                                   VARIABLE + ")(" + OPERADOR + "))*((" + NOT + ")?" + VARIABLE + ")" + PARENTESISCIERRE + "))*";

        static string WLOG = "^(" + IMPRIMIREXPRESION + PARENTESISAPERTURA + EXPRESION + PARENTESISCIERRE + TERMINADOR + ")($|\\s)";
        static string WSTR = "^(" + IMPRIMIRCADENA + PARENTESISAPERTURA + CADENA + PARENTESISCIERRE + TERMINADOR + ")($|\\s)";
        static string WINTRO = "^(" + IMPRIMIRRETORNO + PARENTESISAPERTURA + PARENTESISCIERRE + TERMINADOR + ")($|\\s)";
        static string WTABLA = "^(" + IMPRIMIRTABLA + PARENTESISAPERTURA + EXPRESION + PARENTESISCIERRE + TERMINADOR + ")($|\\s)";

        static string TAUTO = "(" + TAUTOLOGIA + PARENTESISAPERTURA + EXPRESION + PARENTESISCIERRE + ")";
        static string CONTRA = "(" + CONTRADICCION + PARENTESISAPERTURA + EXPRESION + PARENTESISCIERRE + ")";
        static string DECI = "(" + DECIDIBLE + PARENTESISAPERTURA + EXPRESION + PARENTESISCIERRE + ")";
        static string FUNCIONES = "(" + CONTRA + "|" + TAUTO + "|" + DECI + ")";
        static string ASIGNACION = "^(" + VARIABLE + ")(" + IGUAL + ")((" + EXPRESION + "|" + FUNCIONES + ")|(" + PARENTESISAPERTURA + ")" +
            "(" + EXPRESION + "|" + FUNCIONES + ")(" + PARENTESISCIERRE + "))(" + TERMINADOR + ")($|\\s)";
        
        public static bool TTerminador(string cadena)
        {
            return Regex.IsMatch(cadena, TERMINADOR);
        }

        public static bool TIgual(string cadena)
        {
            return Regex.IsMatch(cadena, IGUAL);
        }

        public static bool TAND(string cadena)
        {
            return Regex.IsMatch(cadena, AND);
        }

        public static bool TOR(string cadena)
        {
            return Regex.IsMatch(cadena, OR);
        }

        public static bool TNOT(string cadena)
        {
            return Regex.IsMatch(cadena, NOT);
        }

        public static bool TENTONCES(string cadena)
        {
            return Regex.IsMatch(cadena, ENTONCES);
        }

        public static bool TDOBLEENTONCES(string cadena)
        {
            return Regex.IsMatch(cadena, DOBLEENTONCES);
        }

        public static bool TPARENTESISAPERTURA(string cadena)
        {
            return Regex.IsMatch(cadena, PARENTESISAPERTURA);
        }

        public static bool TPARENTESISCIERRE(string cadena)
        {
            return Regex.IsMatch(cadena, PARENTESISCIERRE);
        }

        public static bool TIMPRIMIREXPRESION(string cadena)
        {
            return Regex.IsMatch(cadena, IMPRIMIREXPRESION);
        }

        public static bool TIMPRIMIRCADENA(string cadena)
        {
            return Regex.IsMatch(cadena, IMPRIMIRCADENA);
        }

        public static bool TIMPRIMIRRETORNO(string cadena)
        {
            return Regex.IsMatch(cadena, IMPRIMIRRETORNO);
        }

        public static bool TCADENA(string cadena)
        {
            return Regex.IsMatch(cadena, CADENA);
        }

        public static bool TVARIABLE(string cadena)
        {
            return Regex.IsMatch(cadena, VARIABLE);
        }

        public static bool TTAUTOLOGIA(string cadena)
        {
            return Regex.IsMatch(cadena, TAUTOLOGIA);
        }

        public static bool TIMPRIMIRTABLA(string cadena)
        {
            return Regex.IsMatch(cadena, IMPRIMIRTABLA);
        }

        public static bool TCONTRADICCION(string cadena)
        {
            return Regex.IsMatch(cadena, CONTRADICCION);
        }

        public static bool TDECIDIBLE(string cadena)
        {
            return Regex.IsMatch(cadena, DECIDIBLE);
        }
        
        public static bool GLEXPRESION(string cadena)
        {
            if (Regex.IsMatch(cadena, ASIGNACION) == true)
            {
                return true;
            } else if (Regex.IsMatch(cadena, WLOG))
            {
                return true;
            } else if (Regex.IsMatch(cadena, WSTR))
            {
                return true;
            } else if (Regex.IsMatch(cadena, WINTRO))
            {
                return true;
            } else if (Regex.IsMatch(cadena, WTABLA))
            {
                return true;
            } else if (Regex.IsMatch(cadena, TAUTO))
            {
                return true;
            } else if (Regex.IsMatch(cadena, CONTRA))
            {
                return true;
            } else if (Regex.IsMatch(cadena, DECI))
            {
                return true;
            }
            else
                return false;        
        }

    }
}
