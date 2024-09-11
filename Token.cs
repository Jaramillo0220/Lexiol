using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico1
{
    public class Token
    {
        public enum Tipos
        {
            Identificador, Numero, Caracter, FinSentencia,
            InicioBloque, FinBloque, OperadorTernario,
            OperadorTermino, OperadorFactor, IncrementoTermino,
            OperadorRelacional, OperadorLogico, OperadorPuntero,
            IncrementoFactor, IncrementoPuntero, Moneda
        }

        private string contenido;
        private Tipos clasificacion;

        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
        }

        // Corregir setContenido y getContenido para manejar correctamente el tipo string
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }

        public string getContenido()
        {
            return this.contenido;
        }

        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }

        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }
    }
}
