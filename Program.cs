using System;

namespace Lexico1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Leer archivo nuevoDocumento.cpp
                using (Lexico l = new Lexico("nuevoDocumento.cpp"))
                {
                    while (!l.finArchivo())
                    {
                        l.nextToken();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el primer procesamiento: " + e.Message);
            }

            try
            {
                using (Lexico l = new Lexico("prueba.cpp"))
                {
                    while (!l.finArchivo())
                    {
                        l.nextToken();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en el segundo procesamiento: " + e.Message);
            }
        }
    }
}
