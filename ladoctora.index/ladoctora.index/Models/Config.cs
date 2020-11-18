using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ladoctora.index.Models
{
    public class Config
    {
        public Dia lunes { get; set; }
        public Dia martes { get; set; }
        public Dia miercoles { get; set; }
        public Dia viernes { get; set; }
        public Dia jueves { get; set; }
        public Dia sabado { get; set; }
        public Dia domingo { get; set; }

        public int minutos { get; set; }
        public int atencionesPorVez { get; set; }


    }
}
