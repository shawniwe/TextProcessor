using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;

namespace TextProcessor.Models
{
    public class WordModel : BaseModel
    {
        public string WordString { get; set; }
        public long RepeatsCount { get; set; }
    }
}
