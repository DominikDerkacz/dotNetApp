using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Villains
{
    public class VillainDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public List<string> Notes { get; set; }
    }
}
