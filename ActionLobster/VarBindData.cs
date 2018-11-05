using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionLobster
{
    class VarBindData
    {
        public string Oid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public VarBindData(string oid, string name, string value) {
            Oid = oid;
            Name = name;
            Value = value;
        }
    }
}
