using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAutomatasII.Expresiones_Regulares
{
    public class Tipo
    {
        public string Nombre { get; set; }

        public string Valores { get; set; }
        public Tipo(string name, string cadena)
        {
            Nombre = name;
            Valores = cadena;
        }
    }
}
