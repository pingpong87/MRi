using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace express_patt
{
    partial class Form2
    {
        //降序排列
        
        private int[] sortDes_ID(double[] arr)
        {
            int i = 0, j = 0;
            int[] des_id = new int[arr.Count()];
            for (j = 0; j < arr.Count(); j++)
                des_id[j] = j;

            for (i = 1; i < arr.Count(); i++)
            { 
                for (j = 0; j < arr.Count() - 1; j++)
                { 
                    if (arr[des_id[j]] < arr[des_id[j + 1]])
                    {
                        int temp = des_id[j];
                        des_id[j] = des_id[j + 1];
                        des_id[j + 1] = temp;
                    }
                }
            }

            return des_id;
        }

        //邻近关系
        private void Star_To_Nei()
        {
            for (int i = arList.Count() - 1; i >= 0; i--)
                for (int j = arList[i].Count - 1; j >= 0; j--)
                {
                    foreach (simpIns neigh in arList[i][j].neighbor)
                    {
                        arList[neigh.feaNo][neigh.insNo].neighbor.Insert(0, new simpIns(i, j));
                    }
                }
        }
        
        
        //冒泡排序
        private void Bubble_sort_Ins(List<simpIns> arr)
        {            
            int size = arr.Count;
            for (int i=0;i<size-1;i++)
            {
                int count = 0;
                for (int j = 0; j < size - 1 - i; j++)  
                {
                    if (arr[j].feaNo > arr[j + 1].feaNo)
                    { arr.Reverse(j, 2);                        
                        count = 1;                        
                    }
                }
                if (count == 0)         //如果某一趟没有交换位置，则说明已经排好序，直接退出循环
                    break;	


            }
        }

       
        private void SortCliqueLi()
        {
            for(int i=0;i<cliqueList.Count;i++)
            {
                Bubble_sort_Ins(cliqueList[i]);
            }
        }
        private void ReadConfFromFile()
        {
            string str = openFile;
            str = str.Substring(0, str.Length - 3) + "conf";
            StreamReader s = File.OpenText(str);
            String line = null;
            int counter = 1;
            while ((line = s.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (counter == 3)
                {
                    DISTTHRE = Convert.ToDouble(line);
                }
                
                else if (counter == 2)
                {
                    FEANUM = Convert.ToInt16(line);
                }
                counter++;
            }
        }

        private void StoreData()
        {
            string mindata;
            string[] linedata;


            int insNo = 0;


            arList = new List<Instance>[FEANUM];

            FileStream fs = File.OpenRead(openFile);
            StreamReader str = new StreamReader(fs);

            int[] midindex = new int[FEANUM];
            for (int i = 0; i < FEANUM; i++)
                midindex[i] = 0;
            mindata = str.ReadLine();
            string[] feastring = new string[FEANUM];
            int AddfeaID = 0;
            while (mindata != null )
            {
                linedata = mindata.Split(',');
                mindata = str.ReadLine();
                string onefeastring = Convert.ToString(linedata[0]);
                int fea;

                if (!feastring.Contains(onefeastring))
                {
                    feastring.SetValue(onefeastring, AddfeaID);
                    fea = AddfeaID;
                    AddfeaID++;
                }
                else
                {
                    int midId;
                    for (midId = AddfeaID - 1; midId >= 0; midId--)
                        if (feastring[midId].Equals(onefeastring)) break;
                    fea = midId;

                }
               
                if (midindex[fea] == 0)//该特征未有实例
                {
                    List<Instance> newlist = new List<Instance>();
                    Instance ins = new Instance(fea, midindex[fea], Convert.ToSingle(linedata[1]), Convert.ToSingle(linedata[2]));
                    newlist.Add(ins);
                    arList[fea] = newlist;
                    midindex[fea]++;
                }
                else
                {
                    Instance ins = new Instance(fea, midindex[fea], Convert.ToSingle(linedata[1]), Convert.ToSingle(linedata[2]));
                    arList[fea].Add(ins);
                    midindex[fea]++;


                }

            }

           
        }

        //算邻近
        private void caclu_neigh(Dictionary<CellClass, List<simpIns>> Gridfornei)
        {
            for (int i = 0; i < arList.Count(); i++)
            {
                for (int j = 0; j < arList[i].Count(); j++)
                {

                    arList[i][j].initnei_list();//网格计算

                    for (int dix = -1; dix <= 1; dix++)
                        for (int diy = -1; diy <= 1; diy++)
                        {
                            CellClass ncell = new CellClass(arList[i][j].cell.cell0 + dix, arList[i][j].cell.cell1 + diy);
                            if (Gridfornei.ContainsKey(ncell))
                                foreach (simpIns ones in Gridfornei[ncell])
                                {
                                    if (ones.feaNo <= i) continue;
                                    double dist = 0;
                                    dist = (arList[i][j].x - arList[ones.feaNo][ones.insNo].x) * (arList[i][j].x - arList[ones.feaNo][ones.insNo].x);
                                    dist = dist + (arList[i][j].y - arList[ones.feaNo][ones.insNo].y) * (arList[i][j].y - arList[ones.feaNo][ones.insNo].y);
                                    dist = System.Math.Sqrt(dist);
                                    if (dist <= DISTTHRE) arList[i][j].add_sortedli(ones);

                                }
                        }

                }
            }
        }

        //求邻近图
        private void NeigbGraph()
        {
            Dictionary<CellClass, List<simpIns>> Gridfornei = new Dictionary<CellClass, List<simpIns>>();
            
            for (int i = 0; i < arList.Count(); i++)
            {
                for (int j = 0; j < arList[i].Count(); j++)
                {
                    arList[i][j].cell = new CellClass((int)(arList[i][j].x / DISTTHRE), (int)(arList[i][j].y / DISTTHRE));

                    if (!Gridfornei.ContainsKey(arList[i][j].cell))
                    {
                        List<simpIns> slfornei = new List<simpIns>();
                        Gridfornei.Add(arList[i][j].cell, slfornei);
                    }
                    simpIns siforn = new simpIns(i, j);
                    Gridfornei[arList[i][j].cell].Add(siforn);
                }
            }
            caclu_neigh(Gridfornei);

            Gridfornei.Clear();
        }
    }
}

