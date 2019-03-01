using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019.Models
{
    public class StatementAssumption
    {
        public StatementAssumption(int numberOfPhotos)
        {
            NumberOfPhotos = numberOfPhotos;
        }
        public int NumberOfPhotos { get; set; }
    }
}
