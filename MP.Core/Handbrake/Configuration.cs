using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Handbrake
{
    class Configuration : HandbrakeCliWrapper.HandbrakeConfiguration
    {
        public string Profile { get; set; }
        public string PresetImportFile { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Profile != null)
            {
                if (PresetImportFile != null)
                {
                    sb.Append($"--preset-import-file \"{PresetImportFile}\" ");
                }
                sb.Append($"--preset \"{Profile}\" ");
                sb.Append("--verbose 0 ");
            } else
            {
                return base.ToString();
            }

            return sb.ToString();
        }
    }
}
