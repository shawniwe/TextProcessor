using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor.Abstract
{
    public abstract class BaseModel
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
    }
}
