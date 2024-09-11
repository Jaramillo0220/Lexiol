using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


namespace Lexico1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int linea;
        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;
            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe", log);
            }
        }
        public Lexico(string nuevoDocumento)
        {
            if (File.Exists(nuevoDocumento))
            {
                //Verificar extensión cpp
                if (Path.GetExtension(nuevoDocumento) == ".cpp")
                {
                    string name = Path.GetFileNameWithoutExtension(nuevoDocumento);
                    Console.WriteLine(name);
                    linea = 1;
                    log = new StreamWriter(name + ".log");
                    asm = new StreamWriter(name + ".asm");
                    log.AutoFlush = true;
                    asm.AutoFlush = true;
                    archivo = new StreamReader("nuevoDocumento.cpp");
                }
            }
            else
            {
                throw new Error("El archivo nuevoDocumento.cpp no existe", log);
            }
        }
        public void Dispose()
        {
            // Definir la ruta del archivo
            string nuevoDocumento = "nuevoDocumento.cpp";

            //Lector lineas
            string[] lineas = File.ReadAllLines(nuevoDocumento);
            log.WriteLine("El archivo 'nuevoDocumento' tiene " + lineas.Length + " líneas.");

            archivo.Close();
            log.Close();
            asm.Close();
        }


        public void nextToken()
        {
            char c;
            string buffer = "";


            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
            }
            buffer += c;
            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '{')
            {
                setClasificacion(Tipos.InicioBloque);
            }
            else if (c == '}')
            {
                setClasificacion(Tipos.FinBloque);
            }
            //------------------------------ Inicio Proyecto -------------------------------------------------            
            else if (c == '<')
            {
                setClasificacion(Tipos.OperadorRelacional);
                if ((c = (char)archivo.Peek()) == '>')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '=')
            {
                setClasificacion(Tipos.OperadorTermino);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '<')
            {
                setClasificacion(Tipos.OperadorRelacional);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '>')
            {
                setClasificacion(Tipos.OperadorRelacional);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '>' || c == '<')
            {
                setClasificacion(Tipos.OperadorRelacional);
            }
            else if (c == '&')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '&')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '|')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '|')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '!')
            {
                setClasificacion(Tipos.OperadorLogico);
                if ((c = (char)archivo.Peek()) == '!' && c == '=')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '!')
            {
                setClasificacion(Tipos.OperadorLogico);
            }
            //------------------------------ Fin Proyecto -------------------------------------------------            
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == ';' || c == '?')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c == '+')
            {
                setClasificacion(Tipos.OperadorTermino);
                if ((c = (char)archivo.Peek()) == '+' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '-')
            {
                setClasificacion(Tipos.OperadorTermino);
                if ((c = (char)archivo.Peek()) == '-' || (c == '='))
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer += c;
                    archivo.Read();
                }
                else if (c == '>')
                {
                    setClasificacion(Tipos.OperadorPuntero);
                    buffer += c;
                    archivo.Read();
                }
            }

            else if (c == '*' || c == '/' || c == '%')
            {
                setClasificacion(Tipos.OperadorFactor);
            }
            else if (c == '/')
            {
                setClasificacion(Tipos.IncrementoFactor);
                if ((c = (char)archivo.Peek()) == '/' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '%')
            {
                setClasificacion(Tipos.IncrementoFactor);
                if ((c = (char)archivo.Peek()) == '%' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '$')
            {
                if ((c = (char)archivo.Peek()) == '$')
                {
                    setClasificacion(Tipos.Caracter);
                }
                else if (char.IsDigit(c))
                {
                    setClasificacion(Tipos.Moneda);
                    while (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        buffer += c;
                        archivo.Read();
                    }
                }
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }
            if (!finArchivo())
            {
                setContenido(buffer);
                log.WriteLine($"{getContenido()} = {getClasificacion()}");
            }
        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}