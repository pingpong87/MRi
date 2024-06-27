using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace express_patt
{
    public class Instance
    {
        public CellClass cell;
        public int feaNo { get; set; }
        public int insNo { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        //存放对应网格

        public List<simpIns> neighbor;//邻近关系
        public List<int> clidegree;
        

        public Instance(int index, double a, double b)
        {
            this.feaNo = index;
            this.x = a;
            this.y = b;

        }
        public Instance(int index,int insno, double a, double b)
        {
            this.feaNo = index;
            this.insNo = insno;
            this.x = a;
            this.y = b;

        }
        public void initnei_list()
        {
            this.neighbor = new List<simpIns>();
        }
        public void add_list(simpIns nei)
        {

            this.neighbor.Add(nei);
        }
        public void add_sortedli(simpIns nei)
        {
            this.neighbor.Add(nei);
            if (neighbor.Count > 1)
            {
                int i;
                for (i = neighbor.Count - 2; i >= 0; i--)
                    if (neighbor[i].isequal(nei) > 0) neighbor[i + 1] = neighbor[i];
                    else break;
                neighbor[i + 1] = nei;
            }
        }
        public override string ToString()
        {
            return feaNo.ToString() + "." + insNo.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            simpIns p = obj as simpIns;
            if ((System.Object)p == null) return false;
            return (feaNo == p.feaNo) && (insNo == p.insNo);
        }

        public int isequal(object obj)//<0：this<p,0:equal,>0:this>p.空小于所有的
        {
            if (obj == null) return 1;
            simpIns p = obj as simpIns;
            if ((System.Object)p == null) return 1;
            if (feaNo == p.feaNo) return insNo - p.insNo;
            else return feaNo - p.feaNo;
        }
        public override int GetHashCode()
        {
            return feaNo.GetHashCode() ^ insNo.GetHashCode();
        }
    }
}
