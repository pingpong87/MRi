using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace express_patt
{
    partial class Form2
    {
        const double EPSINON = 1e-6;
        //快速排序
        private static void quickSort(string[] arr, double[] xarr, int low, int high)
        {

            if (low < high)
            {

                int index = getIndex(arr, xarr, low, high);

                quickSort(arr, xarr, low, index - 1);
                quickSort(arr, xarr, index + 1, high);
            }

        }

        private static int getIndex(string[] arr, double[] xarr, int low, int high)
        {
            
            string tmpString = arr[low];
            double tmp = xarr[low];
            while (low < high)
            {
                
                while (low < high && xarr[high] >= tmp)
                {
                    high--;
                }
               
                arr[low] = arr[high];
                xarr[low] = xarr[high];
               
                while (low < high && xarr[low] <= tmp)
                {
                    low++;
                }
                
                arr[high] = arr[low];
                xarr[high] = xarr[low];

            }
            
            arr[low] = tmpString;
            xarr[low] = tmp;
            return low; 
        }
       
        private Dictionary<string, int> SortX()
        {
            Dictionary<string, int> sortXDic = new Dictionary<string, int>();
            List<string> sortXList = new List<string>();
            List<double> xlist = new List<double>();

            for (int i = 0; i < arList.Count(); i++)
                for (int j = 0; j < arList[i].Count(); j++)
                {
                    Instance tempIns = arList[i][j];
                    sortXList.Add(tempIns.ToString());
                    xlist.Add(tempIns.x);
                }

            string[] sortXArray = sortXList.ToArray();
            double[] xArray = xlist.ToArray();

            quickSort(sortXArray, xArray, 0, xArray.Length - 1);
            for (int i = 0; i < sortXArray.Length; i++)
                sortXDic.Add(sortXArray[i], i);
            return sortXDic;
        }


        //按与insPoint的极角排序,从小到大,
        private void sortPA(List<simpIns> candiList, simpIns insPoint)
        {

            for (int i = 0; i < candiList.Count - 1; i++)
            {
                int k = i;
                for (int j = i + 1; j < candiList.Count; j++)
                {
                    if (Math.Abs(multiply(candiList[j], candiList[k], insPoint)) <= EPSINON)
                    { if (distance(insPoint, candiList[j]) < distance(insPoint, candiList[k])) k = j; }
                    else if (multiply(candiList[j], candiList[k], insPoint) > 0) k = j;

                }
                candiList.Insert(i, candiList[k]);
                candiList.RemoveAt(k + 1);
            }
        }


        //重复Graham算法
        private void layer(List<simpIns> candilist, simpIns insPoint)
        {
            simpIns[] tmpArray = candilist.ToArray();
            List<simpIns> dealList = new List<simpIns>(tmpArray);
            dealList.Insert(0, insPoint);
            int currLayer = 1;
            while (dealList.Count > 3)
            {
                int top = 2;
                simpIns[] ch = new simpIns[dealList.Count];
                ch[0] = dealList[0];
                ch[1] = dealList[1];
                ch[2] = dealList[2];
                for (int i = 3; i < dealList.Count; i++)
                {
                    while (top - 1>=0&& multiply(dealList[i], ch[top], ch[top - 1]) > 0)
                        top--;
                    ch[++top] = dealList[i];
                }
                for (int i = 1; i <= top; i++)
                {
                    int tmp = candilist.IndexOf(ch[i]);
                    candilist[tmp].layer = currLayer;
                    dealList.Remove(ch[i]);
                }
                currLayer++;
            }
            for (int j = 1; j < dealList.Count; j++)//剩余点加上层数
            {
                int tmp = candilist.IndexOf(dealList[j]);
                candilist[tmp].layer = currLayer;
            }
        }
        //求向量的叉积
        private double multiply(simpIns p1, simpIns p2, simpIns p0)//返回<p0,p1>与<p0,p2>的叉积,
        {//返回值,>0说明<p0,p1>在<p0,p2>的顺时针,=0说明共线.
            double p1x = arList[p1.feaNo][p1.insNo].x;
            double p2x = arList[p2.feaNo][p2.insNo].x;
            double p0x = arList[p0.feaNo][p0.insNo].x;
            double p1y = arList[p1.feaNo][p1.insNo].y;
            double p2y = arList[p2.feaNo][p2.insNo].y;
            double p0y = arList[p0.feaNo][p0.insNo].y;

            return ((p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y));
        }

        //求距离
        private double distance(simpIns p1, simpIns p2)
        {
            double p1x = arList[p1.feaNo][p1.insNo].x;
            double p2x = arList[p2.feaNo][p2.insNo].x;
            double p1y = arList[p1.feaNo][p1.insNo].y;
            double p2y = arList[p2.feaNo][p2.insNo].y;
            if (p1.feaNo == p2.feaNo) return double.MaxValue;
            else return (Math.Sqrt((p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y)));
        }
        
        bool toLeft(simpIns p1, simpIns p2, simpIns p0)
        {
            return multiply(p1, p2, p0) > 0;
        }
        
        bool isInTriangle(simpIns currP, simpIns trianP1, simpIns trianP2, simpIns trianP3)
        {
            bool isInT = toLeft(trianP2, currP, trianP1);
            if (isInT != toLeft(trianP3, currP, trianP2))
                return false;
            if (isInT != toLeft(trianP1, currP, trianP3))
                return false;
         
            return true;
        }

        
        bool isInPoly(simpIns testPoint, List<simpIns> polyList)
        {
            sortPA(polyList, polyList[0]);
            for (int i = 1; i < polyList.Count - 1; i++)
                if (isInTriangle(testPoint, polyList[0], polyList[i], polyList[i + 1])) return true;
            return false;
        }
        //找列表中度最大的点.
        public int getMaxIndex(List<simpIns> arr)
        {
            int j = 0;
            int temp = arList[arr[j].feaNo][arr[j].insNo].neighbor.Count;
            for (int s = 1; s < arr.Count; s++)
                if (arList[arr[s].feaNo][arr[s].insNo].neighbor.Count > temp)
                {
                    j = s;
                    temp = arList[arr[s].feaNo][arr[s].insNo].neighbor.Count;
                }
            return j;
        }
        //根据层进行冒泡排序
        private void sortLayer(List<simpIns> LSet)
        {
            for (int i = 0; i < LSet.Count; i++)
                for (int j = 0; j < LSet.Count - i - 1; j++)
                {
                    if (LSet[j].layer > LSet[j + 1].layer) //交换 
                        LSet.Reverse(j, 2);

                }
        }


        //找到所有团
        private void enumMCCP(List<simpIns> currClique, simpIns currPoint, List<simpIns> candiList, List<simpIns> excludeList, List<simpIns> extreList)
        {
            List<simpIns> newCurrClique = new List<simpIns>(currClique);
            Dictionary<int, int> feaInCandi = new Dictionary<int, int>();
            foreach (simpIns tmpins in candiList)
                if (feaInCandi.ContainsKey(tmpins.feaNo)) feaInCandi[tmpins.feaNo]++;
                else feaInCandi.Add(tmpins.feaNo, 1);

            newCurrClique.Add(currPoint);
            //更新边界点P(M) extreList,内部点I(M)=当前团-P(M)
            List<simpIns> newExtreList = new List<simpIns>(extreList);
            List<simpIns> LSet = new List<simpIns>();
            
            if (newExtreList.Count >= 2)
            {
                sortPA(newExtreList, currPoint);
                simpIns minPoint = newExtreList[0];
                simpIns maxPoint = newExtreList[newExtreList.Count - 1];
                int i = 1;
                while (i < newExtreList.Count - 1)
                {
                    if (isInTriangle(newExtreList[i], minPoint, maxPoint, currPoint)) newExtreList.RemoveAt(i);
                    else i++;
                }

            }
            newExtreList.Add(currPoint);

            excludeList = excludeList.Intersect(arList[currPoint.feaNo][currPoint.insNo].neighbor).ToList();
            candiList = candiList.Intersect(arList[currPoint.feaNo][currPoint.insNo].neighbor).ToList();
            {
                int i = 0;
                while (i < candiList.Count)//移除候选中包含在新多边形的内部的点//prune C到当前团M中
                {
                  
                    if (isInPoly(candiList[i], newExtreList))
                    {
                        //特征约束
                        if (feaInCandi[candiList[i].feaNo] == 1)
                        {
                            newCurrClique.Add(candiList[i]);
                            List<simpIns> neiList = arList[candiList[i].feaNo][candiList[i].insNo].neighbor;
                            excludeList = excludeList.Intersect(neiList).ToList();
                            feaInCandi.Remove(candiList[i].feaNo);
                            candiList.RemoveAt(i);

                        }
                        else i++;
                    }

                    else i++;
                }
            }
            
            bool isInExculde = false;
            {
                foreach (simpIns tmpIns in excludeList)
                    if (isInPoly(tmpIns, newExtreList)) { isInExculde = true; break; }
            }
            if (!isInExculde)
            { //isInExculde时本次搜索结束.
                if (candiList.Count == 0 && excludeList.Count == 0)
                    cliqueList.Add(newCurrClique);
                else if (candiList.Count != 0)
                {
                    //剪枝
                    
                    int i = getMaxIndex(candiList);
                    int feano = candiList[i].feaNo;
                    int insno = candiList[i].insNo;
                    List<simpIns> neiList = arList[feano][insno].neighbor;
                    LSet = candiList.Except(neiList).ToList();


                    sortLayer(LSet);
                    
                    int j = 0;
                    while (j < LSet.Count)
                    {
                        int feano1 = LSet[j].feaNo;
                        int insno1 = LSet[j].insNo;
                        simpIns ins = new simpIns(feano1, insno1);
                        List<simpIns> neiList1 = arList[feano1][insno1].neighbor;

                        enumMCCP(newCurrClique, ins, candiList.Intersect(neiList1).ToList(), excludeList.Intersect(neiList1).ToList(), newExtreList);
                       
                        excludeList.Add(ins);
                        candiList.Remove(ins);

                        LSet.RemoveAt(j);
                    }
                }
            }

        }

    }
}
