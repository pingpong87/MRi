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
    public partial class Form2 : Form
    {


        private List<Instance>[] arList;

        private double DISTTHRE;

        double SimThre = 0.6;
        List<Pattern> keys;
        
        private List<List<simpIns>> cliqueList;
        private Dictionary<CellClass, List<List<simpIns>>> Gridcliq;

        private Dictionary<Pattern, Matrix> expreslist;
        List<Dictionary<simpIns, List<int>>> nonbvectorlist = new List<Dictionary<simpIns, List<int>>>();
        Dictionary<simpIns, List<List<simpIns>>> LookCliqueId;
        int[] cotclipat;   

        int[] son;
        double[][] distArray;
        const string openFile = @"..\Beijing63.csv";//data
        const string PCPFile = @"..\beijing_combinations.csv";//co-location pattern
        int FEANUM;

        public Form2()
        {
            InitializeComponent();

        }

        
        
        public bool issuperset(List<simpIns> super, List<simpIns> sub)
        {

            int i = 0, j = 0;
            while (i < super.Count && j < sub.Count)
            {
                int a = super[i].isequal(sub[j]);
                if (a > 0) return false;
                else if (a == 0) { i++; j++; }
                else i++;
            }
            if (j < sub.Count) return false;
            else return true;
        }


        public void gen_oneclique(List<simpIns> clique, List<simpIns> interset)
        {
            for (int i = 0; i < interset.Count; i++)
            {
                List<simpIns> nextset = new List<simpIns>();

                simpIns Ins = new simpIns(interset[i].feaNo, interset[i].insNo);

                nextset = interset.Intersect(arList[interset[i].feaNo][interset[i].insNo].neighbor).ToList();

                clique.Add(Ins);
                if (nextset.Count == 0) 
                {
                    bool flag = true;
                    for (int sk1 = -1; sk1 <= 1 && flag == true; sk1++)
                        for (int sk2 = -1; sk2 <= 1 && flag == true; sk2++)
                        {
                            CellClass newcell = new CellClass(arList[Ins.feaNo][Ins.insNo].cell.cell0 + sk1, arList[Ins.feaNo][Ins.insNo].cell.cell1 + sk2);


                            if (Gridcliq.ContainsKey(newcell)) 
                            {
                                foreach (List<simpIns> aclique in Gridcliq[newcell])
                                {
                                    if (aclique.Count > clique.Count)
                                        if (issuperset(aclique, clique)) { flag = false; break; }
                                }
                            }

                        }
                   
                    if (flag == true)
                    {
                        if (!Gridcliq.ContainsKey(arList[Ins.feaNo][Ins.insNo].cell))
                        {
                            List<List<simpIns>> lll = new List<List<simpIns>>();
                            Gridcliq.Add(arList[Ins.feaNo][Ins.insNo].cell, lll);
                        }
                        List<simpIns> clique_arr = new List<simpIns>(clique);
                        Gridcliq[arList[Ins.feaNo][Ins.insNo].cell].Add(clique_arr);
                        cliqueList.Add(clique_arr);
                    }

                  
                }
                else gen_oneclique(clique, nextset);
                clique.Remove(Ins);
            }

        }

       

       

        //求多个有序的序列的交集，
        /*
         * 维护一个哈希表umap,遍历所有集合的所有元素arr[i][j];
    之后遍历哈希表umap,如果存在某元素的值umap[k]等于集合的个数arr.size(),即将其保存到结果集合中;

         */

        public void nonbvector(List<Pattern> keycoll)
        {
          
            int j;

            for (j = 0; j < keycoll.Count; j++)
            {
                cotclipat[j] = 0;
                Dictionary<simpIns, List<int>> onepatt = new Dictionary<simpIns, List<int>>();
                if (keycoll[j].feaList.Count == 2)//2阶
                {
                    foreach (simpIns kvp in LookCliqueId.Keys)
                    {
                       
                        if (kvp.feaNo > keycoll[j].getOneFea(0)) break;

                        List<int> alis = new List<int>();
                        onepatt.Add(kvp, alis);
                        for (int i = 0; i < LookCliqueId[kvp].Count; i++)                 
                            if (containInC(keycoll[j].feaList, LookCliqueId[kvp][i]) == 1)
                            {
                                onepatt[kvp].Add(i);
                                cotclipat[j]++;
                            }
                       
                        if (onepatt[kvp].Count == 0) onepatt.Remove(kvp);
                    }
                }
                else                
                {
                    
                    List<int> sons = new List<int>();
                    sons.Add(son[j]);
                    for (int cut = keycoll[j].feaList.Count - 2; cut >= 0; cut--)
                        for (int i = sons[sons.Count - 1] + 1; i < j; i++)
                        {
                            List<int> otherFea = new List<int>(keycoll[j].feaList);
                            otherFea.RemoveAt(cut);
                            if (keycoll[i].Equals(otherFea))
                            {
                                sons.Add(i);
                                break;
                            }
                        }
                    
                    Dictionary<simpIns, int> inter_Ins = new Dictionary<simpIns, int>();
                    foreach (simpIns kvp in nonbvectorlist[son[j]].Keys)
                    {
                        inter_Ins.Add(kvp, 1);
                    }
                    for (int j1 = 1; j1 < sons.Count; j1++)
                    {
                        foreach (simpIns kvp in nonbvectorlist[sons[j1]].Keys)
                        {
                            if (inter_Ins.ContainsKey(kvp))
                                inter_Ins[kvp]++;
                        }

                    }
                    
                    foreach (simpIns kvp in inter_Ins.Keys)
                    {

                        if (inter_Ins[kvp] == sons.Count)
                        {
                            List<int> alis = new List<int>();
                            onepatt.Add(kvp, alis);

                            Dictionary<int, int> inter_MRIid = new Dictionary<int, int>();
                            foreach (int idkvp in nonbvectorlist[son[j]][kvp])
                            {
                                inter_MRIid.Add(idkvp, 1);
                            }
                            for (int j1 = 1; j1 < sons.Count; j1++)
                            {
                                foreach (int idkvp1 in nonbvectorlist[sons[j1]][kvp])
                                {
                                    if (inter_MRIid.ContainsKey(idkvp1))
                                        inter_MRIid[idkvp1]++;
                                }

                            }
                            foreach (int MRIid in inter_MRIid.Keys)
                            {
                                if (inter_MRIid[MRIid] == sons.Count)
                                {
                                    onepatt[kvp].Add(MRIid);
                                    cotclipat[j]++;
                                }

                            }
                            
                            if (onepatt[kvp].Count == 0) onepatt.Remove(kvp);
                            inter_MRIid.Clear();
                        }
                    }


                    inter_Ins.Clear();


                }

                nonbvectorlist.Add(onepatt);
            }

          
            
            resulTextBox.AppendText("vector generation\n");
        }
       
      
      
       
        //=====================================check whether the generate pattern is a candidate pattern ======================
        private void loaddbutton_Click(object sender, EventArgs e)
        {
            ReadConfFromFile();

            StoreData();//存储数据

            

            NeigbGraph();//求邻近图
            resulTextBox.AppendText("save data。。。。" + "NO. of features:  " + arList.Length.ToString() + "\n");

            load_PCP();

            Star_To_Nei();//整理邻近关系

        }

        public int containInC(List<int> feali, List<simpIns> clique)//feali包含在团clique（按特征排好序的）中，则为1
        {
            if (feali.Count > clique.Count) return 0;
            else
            {
                int i = 0, j = 0;
                while (i < feali.Count && j < clique.Count)
                {
                    if (feali[i] == clique[j].feaNo) { i++; j++; }
                    else j++;
                }
                if (i < feali.Count) return 0;
                else return 1;
              
            }

        }


        public int Insert_Count(List<int> L1, List<int> L2)
        {
            int resu = 0;
            int i = 0, j = 0;
            while (i < L1.Count && j < L2.Count)
            {
                if (L1[i] == L2[j]) { i++; j++; resu++; }
                else if (L1[i] < L2[j]) { i++; }
                else { j++; }
            }
            return resu;
        }
        public double simi_nonvectro(int pa, int pb)
        {
            int couint = 0;
            if (cotclipat[pa] == 0 || cotclipat[pb] == 0) return 1;

            if (nonbvectorlist[pa].Count <= nonbvectorlist[pb].Count)
            {
                foreach (simpIns asimins in nonbvectorlist[pa].Keys)
                {
                    if (nonbvectorlist[pb].ContainsKey(asimins))
                    { couint = couint + Insert_Count(nonbvectorlist[pb][asimins], nonbvectorlist[pa][asimins]); }
                    
                }
            }
            else
            {
                foreach (simpIns asimins in nonbvectorlist[pb].Keys)
                {
                    if (nonbvectorlist[pa].ContainsKey(asimins))
                    { couint = couint + Insert_Count(nonbvectorlist[pb][asimins], nonbvectorlist[pa][asimins]); }
                }
            }
            return Convert.ToDouble(couint) / Math.Sqrt(Convert.ToDouble(cotclipat[pa]) * Convert.ToDouble(cotclipat[pb]));

        }
       


        private void load_PCP()
        {
            string mindata;
          
            expreslist = new Dictionary<Pattern, Matrix>();
            FileStream fs = File.OpenRead(PCPFile);
            StreamReader str = new StreamReader(fs);
            mindata = str.ReadLine();
            while (mindata != null)
            {
                string[] linedata = mindata.Split(',');
               
                {
                    Matrix amatrix = new Matrix(1, 1);
                    List<int> feaList = new List<int>();
                    for (int t = 0; t < linedata.Length ; t++)
                        feaList.Add(Convert.ToInt32(linedata[t]));
                    Pattern prob = new Pattern(feaList);
                    
                    if(!expreslist.Keys.Contains(prob)) expreslist.Add(prob, amatrix);
                    
                }
                mindata = str.ReadLine();

            }
            resulTextBox.AppendText("Frequent patterns loaded!! Number of pattern:" + expreslist.Count().ToString() + "\n");

        }



        /**MRI表达
         *vector-space
         * MRI_DIST
         * RSPCP miner:ReprMine
         */

        private void button6_Click(object sender, EventArgs e)
        {


            cliqueList = new List<List<simpIns>>();
            Dictionary<string, int> sortXDic = SortX();
            int s = 0;
            foreach (KeyValuePair<string, int> kvp in sortXDic)
            {
                s++;
                //=======变量
                string[] dealPoint = kvp.Key.Split('.');
                int feano = Convert.ToInt32(dealPoint[0]);
                int insno = Convert.ToInt32(dealPoint[1]);
                simpIns insPoint = new simpIns(feano, insno);
                insPoint.layer = 0;
                //初始化E,C
                List<simpIns> candiList = new List<simpIns>();
                List<simpIns> excludeList = new List<simpIns>();
                foreach (simpIns temsimpIns in arList[feano][insno].neighbor)
                    if (sortXDic[temsimpIns.ToString()] < kvp.Value) excludeList.Add(temsimpIns);
                    else
                    {
                        temsimpIns.layer = 0;
                        candiList.Add(temsimpIns);
                    }
                
                if (candiList.Count == 0) continue;
                //sort by p polar angles
                sortPA(candiList, insPoint);
                //测试包
                //writeTxt(candiList, insPoint);
                //computer layer
                layer(candiList, insPoint);

               
                List<simpIns> currClique = new List<simpIns>();
                List<simpIns> extreList = new List<simpIns>();
                Dictionary<int, int> newfeaInCandi = new Dictionary<int, int>();

                enumMCCP(currClique, insPoint, candiList, excludeList, extreList);


            }


            resulTextBox.AppendText("End of enumeration\n");

            Gen_Ve();
            MRI_DIST();

            ReprMine();


        }
        void Gen_Ve()
        {
            
            SortCliqueLi();
            keys = new List<Pattern>(expreslist.Keys);
            son = new int[keys.Count];
            cotclipat = new int[keys.Count];
            for (int j = 0; j < keys.Count; j++)
            {
                cotclipat[j] = 0;
                if (keys[j].feaList.Count == 2) son[j] = -1;
                else
                    for (int i = 0; i < keys.Count; i++)
                    {
                        List<int> otherFea = new List<int>(keys[j].feaList);
                        otherFea.RemoveAt(keys[j].feaList.Count - 1);
                        if (keys[i].Equals(otherFea))
                        {
                            son[j] = i;
                            break;
                        }
                    }
            }
            LookCliqueId = new Dictionary<simpIns, List<List<simpIns>>>();
            {
                
                List<List<int>> count = new List<List<int>>();
                for (int i = 0; i < arList.Count(); i++)
                {
                    List<int> tmpcount = new List<int>();
                    for (int j = 0; j < arList[i].Count; j++)
                    {
                        tmpcount.Add(0);
                        List<List<simpIns>> tmp = new List<List<simpIns>>();
                        LookCliqueId.Add(new simpIns(i, j), tmp);
                    }
                    count.Add(tmpcount);
                }
                //index
                for (int i = 0; i < cliqueList.Count; i++)
                {
                    
                    LookCliqueId[cliqueList[i][0]].Add(cliqueList[i]);
                    count[cliqueList[i][0].feaNo][cliqueList[i][0].insNo] = 1;
                }
              
                
                for (int i = 0; i < arList.Count(); i++)
                    for (int j = 0; j < arList[i].Count; j++)
                    {
                        if (count[i][j] == 0)
                            LookCliqueId.Remove(new simpIns(i, j));

                    }
            }

         
            nonbvector(keys);
            
            resulTextBox.AppendText("Vector generation\n");
        }

        void MRI_DIST()
        {
            DateTime dt1, dt2;
            Stopwatch stopWatch = new Stopwatch();


            dt1 = DateTime.Now;

            int i, j;
            distArray = new double[expreslist.Count][];//Distance between co-locaitons

            List<Cluster> clusters = new List<Cluster>();

            for (i = 0; i < expreslist.Count; i++)
                distArray[i] = new double[expreslist.Count];
            for (i = 0; i < expreslist.Count; i++)
            {
                distArray[i][i] = 0;

                for (j = i + 1; j < expreslist.Count; j++)
                {

                    double distance;
                    if (keys[i].isrealsubset(keys[j])) distance = 1 - Math.Sqrt(Convert.ToDouble(cotclipat[j]) / Convert.ToDouble(cotclipat[i]));
                    else distance = 1 - simi_nonvectro(i, j);
                   

                    distArray[j][i] = distance;
                    distArray[i][j] = distance;
                   
                }
             

            }


            resulTextBox.AppendText("End of distance calculation\n");
        }

        void ReprMine()
        {

            //
            /*patttern，patt,PI，pre_patts
             *dist,pattern，pattern，dist
             * clustering
             *
             */
            Dictionary<string, int> rongyu_pat_id = new Dictionary<string, int>();

            for (int j = 0; j < keys.Count; j++)
                rongyu_pat_id.Add(keys[j].ToString(), j);
           

            List<double> dist_all = new List<double>();

           
            DPCcluster(distArray, keys, rongyu_pat_id);


        }


        private void DPCcluster(double[][] distArray, List<Pattern> pre_patt_list, Dictionary<string, int> rongyu_pat_id)
        {
            int i = 0, j = 0;
            double maxdelta = 0;

            Double[] eposi = new Double[pre_patt_list.Count];
            Double[] delta = new Double[pre_patt_list.Count];



            for (i = 0; i < pre_patt_list.Count; i++)
            {
                eposi[i] = 0;
                delta[i] = double.MaxValue;
            }

    
            int knn = 100;

            for (i = 0; i < pre_patt_list.Count; i++)
            {
                double epo = 0;

                List<double> knn_list = new List<double>();
                for (j = 0; j < pre_patt_list.Count; j++)
                {
                    knn_list.Add(distArray[i][j]);

                }
                int[] knn_id = sortDes_ID(knn_list.ToArray());

                double knn_dist = 0;
                for (j = 0; j < knn; j++)
                {
                    int k_id = knn_id[pre_patt_list.Count - 1 - j];
                    knn_dist = knn_dist + distArray[i][k_id] * distArray[i][k_id]; 
                }
                epo = Math.Exp((-1) / Convert.ToDouble(knn) * knn_dist);

                eposi[i] = epo;

            }


            double mineposi = eposi.Min();
            double maxeposi = eposi.Max();

            int[] sortDesid = sortDes_ID(eposi);

            for (i = 1; i < sortDesid.Count(); i++)
            { 
                for (j = 0; j < i; j++)
                {
                    if (distArray[sortDesid[i]][sortDesid[j]] < delta[sortDesid[i]])
                        delta[sortDesid[i]] = distArray[sortDesid[i]][sortDesid[j]];
                }
            }
          
            delta[sortDesid[0]] = double.MinValue;//compute max
            for (i = 1; i < sortDesid.Count(); i++)
                if (distArray[sortDesid[i]][sortDesid[0]] > delta[sortDesid[0]])
                    delta[sortDesid[0]] = distArray[sortDesid[i]][sortDesid[0]];

            double[] r = new double[pre_patt_list.Count];
            for (i = 0; i < pre_patt_list.Count; i++)
                r[i] = (Convert.ToDouble(eposi[i] - mineposi) / (maxeposi - mineposi)) * delta[i];

      
            int[] r_id = sortDes_ID(r);
            int[] clus_center = new int[pre_patt_list.Count];
          
            List<int> center = new List<int>();
            bool is_all_process = false;
            for (i = 0; i < pre_patt_list.Count; i++)
            {
                clus_center[i] = -2;
            }
         
            int now_r_xuhaoid = 0;
            while (!is_all_process && now_r_xuhaoid < r_id.Count())
            {

                is_all_process = true;
                int cand_id = r_id[now_r_xuhaoid];

                if (clus_center[cand_id] == -2)
                {
                    clus_center[cand_id] = -1;
                    
                    for (i = 0; i < pre_patt_list.Count; i++)
                        if (clus_center[i] == -2 && distArray[i][cand_id] < SimThre)
                            clus_center[i] = cand_id;
                }
                for (i = 0; i < pre_patt_list.Count; i++)
                    if (clus_center[i] == -2) is_all_process = false;
                now_r_xuhaoid++;
            }
            int cout_cluster = 0;
            for (i = 0; i < pre_patt_list.Count; i++)//output
            {
                if (clus_center[i] == -1)
                {
                    cout_cluster++;
                    resulTextBox.AppendText(pre_patt_list[i].ToString()+"\t");
                }
            }
           
            
        }
    }
}

