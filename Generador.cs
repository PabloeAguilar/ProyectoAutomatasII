using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace ProyectoAutomatasII
{
    [DisassemblyDiagnoser(printSource:true, printInstructionAddresses:true)]
    [RyuJitX64Job]
    public static class  Generador
    {
        [Benchmark]
        public static void Prueba()
        {
            Console.WriteLine("* el que lo lea");
        }
    }
}
