using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Models
{
    public class FileWithErrors
    {
        public Guid Id { get; set; }
        public String FilePath { get; set; }
        public String Notes { get; set; }
    }
}
