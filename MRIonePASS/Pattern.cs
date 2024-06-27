using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace express_patt
{
    //====================================pattern class ==================================
    public class Pattern
    {
        public double pi { get; set; }
        public List<int> feaList;
        public List<List<int>> insList; //table instance of a pattern
        public List<List<int>> worldId;

        public Pattern(List<int> fea)
        {
            this.feaList = fea;
        }
        public Pattern(string str)
        {
            string[] splitstr=str.Split(',');
            List<int> intlist = new List<int>();
            foreach(string s in splitstr)
            {
                int i = int.Parse(s);
                intlist.Add(i);
            }
            this.feaList = intlist;
        }
        public Pattern(Pattern pa)
        {
            this.feaList = pa.feaList;
        }

        public List<int> getFeaList()
        {
            return this.feaList;
        }
        public int getOneFea(int i)
        {
            return this.feaList[i];
        }
        public int getLastFea()
        {
            return this.feaList[feaList.Count-1];
        }
        public void add_list(int a,int b)
        {
            List<int> addl = new List<int>();
            addl.Add(a);
            addl.Add(b);
            this.insList.Add(addl);
        }
        
        public bool inslistEqu(List<int> other)
        { int i;
            for(i=0;i<insList.Count;i++)
            {
                int j;
                for ( j = 0; j < other.Count; j++) if (insList[i][j] != other[j]) break;
                if (j==other.Count) return true;
            }
            return false;
        }
        public bool isbig_lexi(Pattern other)//同阶模式的比较
        {
            int i;
            for (i = 0; i < feaList.Count; i++)
            {
                if (feaList[i] > other.feaList[i]) return true;
                else if (feaList[i] < other.feaList[i]) return false;
            }
            return false;
        }
        public Pattern merge(Pattern other) //merge two patterns to produce a new pattern 
        {
            Pattern result;
            List<int> resultFea = new List<int>();
            foreach (int a in this.feaList)
                resultFea.Add(a);
            resultFea.Add(other.getOneFea(this.feaList.Count - 1));
            result = new Pattern(resultFea);
            return result;
        }
        public bool Equals(Pattern other)
        {
            if (this.feaList.Count != other.feaList.Count) return false;
            for (int i = 0; i < this.feaList.Count; i++)
                if (this.feaList[i] != other.getOneFea(i)) return false;
            return true;
        }
        public bool Equals(List<int> other)
        {
            if (this.feaList.Count != other.Count) return false;
            for (int i = 0; i < this.feaList.Count; i++)
                if (this.feaList[i] != other[i]) return false;
            return true;
        }
        public int RemoveAt(int i)
        {
            int r = this.feaList[i];
            this.feaList.RemoveAt(i);
            return r;
        }
        public void InsertAt(int i, int feano)
        {
            this.feaList.Insert(i, feano);
        }
        public bool isrealsubset(Pattern other)//this模式是不是其他模式的子集，是的话返回true
        { bool fa = true;
            if (feaList.Count >= other.feaList.Count) return false;
            foreach (int x in feaList)
            {
                if (!other.feaList.Contains(x)) { fa = false; break; }
            }
            return fa;
        }
       

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < feaList.Count-1; i++)
                str = str + feaList[i].ToString()+",";
            str = str + feaList[feaList.Count - 1].ToString();
            return str;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Pattern pa = obj as Pattern;
            if ((System.Object)pa == null) return false;
            if (this.feaList.Count == pa.feaList.Count)
            {
                int i = 0;
                while (i < this.feaList.Count && this.feaList[i] == pa.feaList[i]) i++;
                if (i < this.feaList.Count) return false;
                else return true;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            int result = 0;
            for (int i = 0; i < this.feaList.Count; i++)
                result = result ^ feaList[i].GetHashCode();
            return result;
        }

    }
}
