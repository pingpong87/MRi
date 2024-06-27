using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace express_patt
{
    public class simpIns
    {
        public int feaNo { get; set; }
        public int insNo { get; set; }
        public int layer { get; set; }//实例所在的层.为0表示未分配
        public bool isselect { get; set; }
        public simpIns(int fea,int ins)
        {
            this.feaNo = fea;
            this.insNo = ins;
            this.isselect = false;
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
        public bool Equals(simpIns p)
        {
            if ((object)p == null) return false;

            return (feaNo == p.feaNo) && (insNo == p.insNo);
        }
        public override int GetHashCode()
        {
            return feaNo.GetHashCode() ^ insNo.GetHashCode();
        }
    }
}
