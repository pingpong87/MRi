using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace express_patt
{
    public class CellClass
    {
        public int cell0 { get; set; }
        public int cell1 { get; set; }

        public CellClass (int c0,int c1)
        {
            this.cell0 = c0;
            this.cell1 = c1;
        }
        public override int GetHashCode()
        {
            return cell0^cell1;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            CellClass p = obj as CellClass;
            if ((System.Object)p == null) return false;
            return (cell0 == p.cell0) && (cell1 == p.cell1);
            
        }
    }
}
