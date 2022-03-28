using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public class Token
    {
        string nombre = "";

        string lexema = "";

        int valor = 0;

        public Token()
        {

        }

        public Token(string name, string lexemaAux, int value = -1 )
        {
            Nombre = name;
            Lexema = lexemaAux;
            Valor = value;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Lexema { get => lexema; set => lexema = value; }
        public int Valor { get => valor; set => valor = value; }


    }
}
