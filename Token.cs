using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
    Requirimiento 1: Sobrecargar el constructor Lexico para que 
                    reciba como argumento el nombre del arvhico a compilar
    Requerimiento 2: Tener un contador
*/
namespace Lexico1
{
    public class Token
    {
        public enum Tipos
        {
            Identificador, Numero, Caracter, FinSentencia,
            InicioBloque, FinBloque, OperadorTernario
            OperadorTermino
        }
        private string contenido;
        private Tipos clasificacion;
        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
            clasificacion = Tipos.FinBloque;
            clasificacion = Tipos.InicioBloque;
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }

    }
}