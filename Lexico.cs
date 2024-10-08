using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic;


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
            log = new StreamWriter(nuevoDocumento + ".log");
            log.AutoFlush = true;
            if (File.Exists(nuevoDocumento))
            {
                //Verificar extensión cpp
                if (Path.GetExtension(nuevoDocumento) == ".cpp")
                {
                    string name = Path.GetFileNameWithoutExtension(nuevoDocumento);
                    Console.WriteLine(name);
                    linea = 1;
                    asm = new StreamWriter(name + ".asm");
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
            string prueba = "prueba.cpp";
            //Lector lineas
            string[] lineas = File.ReadAllLines(prueba);
            log.WriteLine("El archivo 'prueba' tiene " + (lineas.Length) + " líneas.");
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
                if (c == '\n')
                {
                    linea++;
                }
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
            //--------------------INICIO DE PROYECTO--------------------
            //----------INICIO----------NUMERO----------
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
                if (c == '.')
                {
                    buffer += c;
                    archivo.Read();
                    if (!(char.IsDigit(c = (char)archivo.Peek())))
                    {
                        throw new Error(linea + ": Se esperaba un dígito después del punto decimal.", log);
                    }
                    else
                    {
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                    }
                }
                if (c == 'e' || c == 'E')
                {
                    buffer += c;
                    archivo.Read();
                    if ((c = (char)archivo.Peek()) == '+' || (c = (char)archivo.Peek()) == '-')
                    {
                        buffer += c;
                        archivo.Read();
                        if ((char.IsDigit(c = (char)archivo.Peek())))
                        {
                            buffer += c;
                            archivo.Read();
                            while (char.IsDigit(c = (char)archivo.Peek()))
                            {
                                buffer += c;
                                archivo.Read();
                            }
                        }
                        else
                        {
                            throw new Error(linea + ": Se esperaba un exponente después de la e.", log);

                        }

                    }
                    else if ((char.IsDigit(c = (char)archivo.Peek())))
                    {
                        buffer += c;
                        archivo.Read();
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                    }
                    else
                    {
                        throw new Error(linea + ": Se esperaba un exponente después de la e.", log);

                    }
                }
            }
            //----------FIN----------NUMERO----------

            //----------INICIO----------CADENA----------
            else if (c == '"')
            {
                setClasificacion(Tipos.Cadena);
                while (((c = (char)archivo.Peek()) != '"') && (!finArchivo()))
                {
                    buffer += c;
                    archivo.Read();
                    if (((c = (char)archivo.Peek()) != '"') && (c == '\n'))
                    {
                        throw new Error(linea + ": Se esperaba finalizar con comillas.", log);

                    }
                    else
                    {
                        buffer += c;
                        archivo.Read();
                    }
                }
                buffer += c;
                archivo.Read();
            }
            //----------FIN----------CADENA----------

            //----------INICIO----------CARACTER----------
            else if (c == '\'')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '\'')
                {
                    buffer += c;
                    archivo.Read();
                    if ((c = (char)archivo.Peek()) == '\'')
                    {
                        buffer += c;
                        archivo.Read();
                        setClasificacion(Tipos.Caracter);
                    }
                    else
                    {
                        throw new Error(linea + ": Se esperaba finalizar con una comilla simple.", log);

                    }
                }
                else
                {
                    buffer += c;
                    archivo.Read();
                    if ((c = (char)archivo.Peek()) == '\'')
                    {
                        buffer += c;
                        archivo.Read();
                        setClasificacion(Tipos.Caracter);
                    }
                    else
                    {
                        throw new Error(linea + ": Se esperaba finalizar con una comilla simple.", log);

                    }
                }

            }
            else if (c == '#')
            {
                setClasificacion(Tipos.Caracter);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '@')
            {
                setClasificacion(Tipos.Caracter);
                archivo.Read();
            }

            //----------FIN----------CARACTER----------
            //--------------------FIN DE PROYECTO--------------------
            /*else if (c == '{')
            {
                setClasificacion(Tipos.InicioBloque);
            }
            else if (c == '}')
            {
                setClasificacion(Tipos.FinBloque);
            }
            else if (c == '<')
            {
                if ((c = (char)archivo.Peek()) == '=')  
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                }
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
            else if (c == '>')
            {
                setClasificacion(Tipos.OperadorRelacional);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '!')
            {
                setClasificacion(Tipos.OperadorLogico);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                }
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
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == ';')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c == '?')
            {
                setClasificacion(Tipos.OperadorTernario);
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
            }*/
            else
            {
                setClasificacion(Tipos.Caracter);
            }
            if (!finArchivo())
            {
                setContenido(buffer);
                string contenido = getContenido();
                string clasificacion = getClasificacion().ToString();

                // Ajusta el ancho según sea necesario para que quede alineado
                log.WriteLine($"{contenido,-12} == {clasificacion}");
            }

        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}