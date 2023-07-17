using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;

namespace TextProcessor.Models
{
    public class Word : BaseEntity
    {
        public string WordString { get; set; }
        public long RepeatsCount { get; set; }

        public override string ToString()
        {
            return $"{WordString} (повторяется {RepeatsCount} раз)";
        }
    }
}
