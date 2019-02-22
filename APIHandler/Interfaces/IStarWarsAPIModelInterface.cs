using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler.Interfaces
{
    public interface IStarWarsAPIModel
    {
        int ID { get; set; }

        List<string> FullDisplayList { get; }

        string BriefDisplayTitle { get; }

        bool Loaded { get; set; }

        string Name { get; set; }

        List<string> GenerateFullDisplayList();



    }
}
