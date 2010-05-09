using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace gfimaxtray
{
    public partial class frmCheckresults : Form
    {
        public frmCheckresults()
        {
            InitializeComponent();
        }

        public void init(ArrayList FailedDevices)
        {
            ArrayList tmpList = new ArrayList();
            foreach (failedCheck tmpCheck in FailedDevices)
            {
                if (!tmpCheck.IsOffline)
                {
                    tmpList.Add(tmpCheck);
                }
            }

            dataGrid1.DataSource = tmpList;

            AutosizeColumns();

/*            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = "checks";

            //hide the column headers 
            tableStyle.ColumnHeadersVisible = false;

            // make the dataGrid use our new tablestyle and bind it to our table 
            dataGrid1.TableStyles.Clear();
            dataGrid1.TableStyles.Add(tableStyle);

            AutoSizeCol(1);*/
        }

        public void AutoSizeCol(int col)
        {
            float width = 0;
            int numRows = ((ArrayList)dataGrid1.DataSource).Count;

            Graphics g = Graphics.FromHwnd(dataGrid1.Handle);
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size;

            for (int i = 0; i < numRows; ++i)
            {
                size = g.MeasureString(dataGrid1[i, col].ToString(), dataGrid1.Font, 500, sf);
                if (size.Width > width)
                    width = size.Width;
            }

            g.Dispose();

            dataGrid1.TableStyles["checks"].GridColumnStyles[col].Width = (int)width + 8; // 8 is for leading and trailing padding 
        }


        public void AutosizeColumns()
        {
           ArrayList DataSource=((ArrayList)dataGrid1.DataSource);

           IList list = null;
           if(DataSource is IList)
           {
              list = (IList)DataSource;
           }
           else if(DataSource is IListSource)
           {
              list = ((IListSource)DataSource).GetList();
           }     

           if(list == null || list.Count < 0)
           {
              return;
           }
           
           PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(list[0]);	
           DataGridTableStyle dataGridTableStyle = new DataGridTableStyle();         
           dataGridTableStyle.MappingName = GetMappingName(list);

           using(Graphics g = CreateGraphics())
           {
              for(int i = 0; i < pdc.Count; i++)
              {
                 SizeF maxSize = g.MeasureString(pdc[i].DisplayName, Font);
                
                 foreach(object o in list)
                 {
                    object result = pdc[i].GetValue(o);
                    string value = pdc[i].Converter.ConvertToString(result);

                    SizeF size = g.MeasureString(value, Font);
                    if(size.Width > maxSize.Width)
                       maxSize = size;
                 }

                 DataGridColumnStyle dataGridColumnStyle = new DataGridTextBoxColumn();
                 dataGridColumnStyle.MappingName = pdc[i].Name;
                 dataGridColumnStyle.HeaderText = pdc[i].DisplayName;
                 dataGridColumnStyle.Width = (int)(maxSize.Width + 5);
                 dataGridTableStyle.GridColumnStyles.Add(dataGridColumnStyle);
              }
              dataGrid1.TableStyles.Add(dataGridTableStyle);
           }
        }

        protected string GetMappingName(IList list)
        {
            string result;

            if (list is ITypedList)
            {
                result = ((ITypedList)list).GetListName(null);
            }
            else
            {
                result = list.GetType().Name;
            }

            return result;
        }




        private void frmCheckresults_Load(object sender, EventArgs e)
        {
            this.Width= Screen.PrimaryScreen.Bounds.Width/2;
        }
    }
}
