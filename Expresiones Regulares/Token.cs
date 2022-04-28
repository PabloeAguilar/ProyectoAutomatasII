
namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public class Token
    {
        string nombre = "";

        string lexema = "";

        int valor = 0;

        int posicion;

        public Token()
        {

        }

        public Token(string name, string lexemaAux, int pos, int value = -1)
        {
            Nombre = name;
            Lexema = lexemaAux;
            Valor = value;
            Posicion = pos;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Lexema { get => lexema; set => lexema = value; }
        public int Valor { get => valor; set => valor = value; }
        public int Posicion { get => posicion; set => posicion = value; }
    }
}
