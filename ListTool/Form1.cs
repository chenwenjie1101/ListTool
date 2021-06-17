using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;

namespace ListTool
{
    public partial class Form1 : Form
    {
        #region FormBorderStyle属性为None的窗体移动
        [DllImport("user32")]
        public static extern int ReleaseCapture();
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wp, int lp);
        /// <summary>
        /// 是否允许移动
        /// </summary>
        bool IsMove = false;
        /// <summary>
        /// 判断鼠标是否在可移动范围内按下
        /// </summary>
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Rectangle rect = new Rectangle(1, 1, this.Width-20, this.Height-560);   //允许拖动的矩形范围
                this.IsMove = rect.Contains(new Point(e.X, e.Y));                       //鼠标按下的点是否在允许拖动范围内
            }
        }
        /// <summary>
        /// 鼠标弹起时取消移动
        /// </summary>
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsMove = false;
        }
        /// <summary>
        /// 移动窗体
        /// </summary>
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMove && e.Button == System.Windows.Forms.MouseButtons.Left && this.WindowState != FormWindowState.Maximized)
            {
                ReleaseCapture();
                SendMessage(Handle, 274, 61440 + 9, 0);
                return;
            }
        }
        #endregion

        #region 动态按钮触发事件
        bool startMove = false;
        int clickX = 0;  //记录上次点击的鼠标位置
        int clickY = 0;
        // 下面两个函数实现按钮拖拽效果
        private void button_MouseDown(object sender, MouseEventArgs e)
        {  //鼠标按下事件
            clickX = e.X;
            clickY = e.Y;
            startMove = true;
        }
        private void button_MouseUp(object sender, MouseEventArgs e)
        {  //鼠标松开事件
            startMove = false;
            //drawNS();
        }

        // 鼠标在按钮上移动,那么按钮是否跟着移动, 如果鼠标移动太快了,将超出范围
        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            if (startMove)
            {
                // e.X 是正负数,表示移动的方向
                Button btn = (Button)sender;
                int x = btn.Location.X + e.X - clickX;   //还要减去上次鼠标点击的位置
                int y = e.Y + btn.Location.Y - clickY;
                btn.Location = new System.Drawing.Point(x, y);
            }
        }

        //按钮点击事件
        private void button_Click(object sender, EventArgs e)
        {
            #region 加载按钮信息 绑定
            //string app_id = this.comboBox_app.SelectedValue.ToString();
            //string menu_id = this.comboBox_menu.SelectedValue.ToString();
            //string grid_id = this.comboBox_grid.SelectedValue.ToString();
            Button btn = (Button)sender;
            string sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.col_id='" + btn.Name + "'";
            DBConnection dbc = new DBConnection();
            DataSet ds = dbc.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                this.textBox_COL_ID.Text = dt.Rows[j]["COL_ID"].ToString();
                this.textBox_COL_NAME.Text = dt.Rows[j]["COL_NAME"].ToString();
                this.textBox_COL_INDEX.Text = dt.Rows[j]["COL_INDEX"].ToString();
                this.textBox_DATA_OPTION.Text = dt.Rows[j]["DATA_OPTION"].ToString();
                this.textBox_DATA_TYPE.Text = dt.Rows[j]["DATA_TYPE"].ToString();
                this.textBox_G_WIDTH.Text = dt.Rows[j]["G_WIDTH"].ToString();
                this.textBox_SYS_FLAG.Text = dt.Rows[j]["SYS_FLAG"].ToString();
                this.textBox_G_LINK.Text = dt.Rows[j]["G_LINK"].ToString();
                this.comboBox_COL_TYPE.SelectedValue = dt.Rows[j]["COL_TYPE"].ToString();//列类型   1-string/2-int/3-float/4-datatime/5-bool/6-clob
                this.comboBox_G_ALIGN.SelectedValue = dt.Rows[j]["G_ALIGN"].ToString();
                this.comboBox_G_EDIT.SelectedValue = dt.Rows[j]["G_EDIT"].ToString();
                this.comboBox_G_FORMAT.SelectedValue = dt.Rows[j]["G_FORMAT"].ToString();//日期显示格式 0=YYYY,1=YYYY-MM,2=YYYY-MM-DD,3=YYYY-MM-DD HH:mm,4=YYYY-MM-DD HH:mm:ss
                this.comboBox_G_SHOW_FLAG.SelectedValue = dt.Rows[j]["G_SHOW_FLAG"].ToString();
                this.comboBox_G_STYLE.SelectedValue = dt.Rows[j]["G_STYLE"].ToString();//显示类型 1_输入框/2_日期框/3_下拉框/4_弹出窗口

                break;
            }
            this.button_save_info.Show();
            #endregion

        }
        #endregion

        string app_id = "";
        string menu_id = "";
        string grid_id = "";

        #region 动态label 触发事件
        int panel2_x = 3;
        int panel2_y = 3;
        int panel3_x = 2;
        int panel3_y = 2;
        private void label_Click(object sender, EventArgs e)
        {
            //string app_id = this.comboBox_app.SelectedValue.ToString();
            //string menu_id = this.comboBox_menu.SelectedValue.ToString();
            //string grid_id = this.comboBox_grid.SelectedValue.ToString();
            Label lab = (Label)sender;
            string sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.col_id='" + lab.Name + "'";
            DBConnection dbc = new DBConnection();
            DataSet ds = dbc.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                this.textBox_COL_ID.Text = dt.Rows[j]["COL_ID"].ToString();
                this.textBox_COL_NAME.Text = dt.Rows[j]["COL_NAME"].ToString();
                this.textBox_COL_INDEX.Text = dt.Rows[j]["COL_INDEX"].ToString();
                this.textBox_DATA_OPTION.Text = dt.Rows[j]["DATA_OPTION"].ToString();
                this.textBox_DATA_TYPE.Text = dt.Rows[j]["DATA_TYPE"].ToString();
                this.textBox_G_WIDTH.Text = dt.Rows[j]["G_WIDTH"].ToString();
                this.textBox_SYS_FLAG.Text = dt.Rows[j]["SYS_FLAG"].ToString();
                this.textBox_G_LINK.Text = dt.Rows[j]["G_LINK"].ToString();
                this.comboBox_COL_TYPE.SelectedValue = dt.Rows[j]["COL_TYPE"].ToString();//列类型   1-string/2-int/3-float/4-datatime/5-bool/6-clob
                this.comboBox_G_ALIGN.SelectedValue = dt.Rows[j]["G_ALIGN"].ToString();
                this.comboBox_G_EDIT.SelectedValue = dt.Rows[j]["G_EDIT"].ToString();
                this.comboBox_G_FORMAT.SelectedValue = dt.Rows[j]["G_FORMAT"].ToString();//日期显示格式 0=YYYY,1=YYYY-MM,2=YYYY-MM-DD,3=YYYY-MM-DD HH:mm,4=YYYY-MM-DD HH:mm:ss
                this.comboBox_G_SHOW_FLAG.SelectedValue = dt.Rows[j]["G_SHOW_FLAG"].ToString();
                this.comboBox_G_STYLE.SelectedValue = dt.Rows[j]["G_STYLE"].ToString();//显示类型 1_输入框/2_日期框/3_下拉框/4_弹出窗口

                break;
            }

            this.label_MODIFY_TYPE.Text = "edit";
            this.button_save_info.Show();
        }

        //待选取点击
        private void label_DoubleClick2(object sender, EventArgs e)
        {

            Label lab = (Label)sender;
            //为了重载
            string sql = "update SCM_LIST_FORM_COLUMN a  set a.sys_flag=2,a.g_show_flag=1,a.order_num=nvl((select max(b.order_num)+1 from SCM_LIST_FORM_COLUMN b where a.app_id=b.app_id and a.menu_id=b.menu_id and a.grid_id=b.grid_id and b.sys_flag in (1,2) and b.g_show_flag=1 and b.order_num !=999),1) ";
            sql += " where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.col_id='" + lab.Name + "'";
            DBConnection dbc = new DBConnection();
            int temp = dbc.update(sql);
            //加到panel3
            //Label lab_new = lab;
            //lab_new.Location = new System.Drawing.Point(panel3_x, panel3_y);
            //this.panel3.Controls.Add(lab_new);
            //panel3_x += 131;
            //重载panel2列表
            lod_panle2_label();
            //重载panel3列表
            lod_panle3_label();
            this.panel3.HorizontalScroll.Value = this.panel3.HorizontalScroll.Maximum;

            //this.panel2.Controls.Remove(lab);
            //panel3_x += 131;

            //#region 重载panel2列表
            //panel2_y = 3;
            //foreach (var control in this.panel2.Controls)
            //{
            //    if (control.GetType() == typeof(Label))
            //    {
            //        Label lab_lod = (Label)control;
            //        Label lab_rem = (Label)control;
            //        this.panel2.Controls.Remove(lab_rem);
            //        lab_lod.Location = new System.Drawing.Point(panel2_x, panel2_y);
            //        this.panel2.Controls.Add(lab_lod);
            //        panel2_y += 25;
            //    }
            //}
            //#endregion

        }
        //显示区 点击
        private void label_DoubleClick3(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            //为了重载
            string sql = "update SCM_LIST_FORM_COLUMN a  set a.sys_flag=0,a.g_show_flag=0,a.order_num=999 where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.col_id='" + lab.Name + "'";
            DBConnection dbc = new DBConnection();
            int temp = dbc.update(sql);
            //加到panel2
            //Label lab_new = lab;
            //lab_new.Location = new System.Drawing.Point(panel2_x, panel2_y);
            //this.panel2.Controls.Add(lab_new);
            //panel2_y += 25;
            //重载panel2列表
            lod_panle2_label();
            //重载panel3列表
            lod_panle3_label();

            //this.panel3.Controls.Remove(lab);
            //panel2_y += 25;

            //#region 重载panel3列表
            //panel3_x = 2;
            //foreach (var control in this.panel3.Controls)
            //{
            //    if (control.GetType() == typeof(Label))
            //    {
            //        Label lab_lod = (Label)control;
            //        this.panel3.Controls.Remove(lab_lod);
            //        lab_lod.Location = new System.Drawing.Point(panel3_x, panel3_y);
            //        this.panel3.Controls.Add(lab_lod);
            //        panel3_x += 131;
            //    }
            //}
            //#endregion
        }
        #endregion

        #region
        class ListItem : System.Object
        {
            private string m_sValue = string.Empty;
            private string m_sText = string.Empty;

            /// <summary>
            /// 值
            /// </summary>
            public string Value
            {
                get { return this.m_sValue; }
            }
            /// <summary>
            /// 显示的文本
            /// </summary>
            public string Text
            {
                get { return this.m_sText; }
            }

            public ListItem(string value, string text)
            {
                this.m_sValue = value;
                this.m_sText = text;
            }
            public override string ToString()
            {
                return this.m_sText;
            }
            public override bool Equals(System.Object obj)
            {
                if (this.GetType().Equals(obj.GetType()))
                {
                    ListItem that = (ListItem)obj;
                    return (this.m_sText.Equals(that.Value));
                }
                return false;
            }
            public override int GetHashCode()
            {
                return this.m_sValue.GetHashCode(); ;
            }

        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            string Development_mode = ConfigurationManager.AppSettings["DEVELOPMENT_MODE"].ToString();
            if (Development_mode != "1")
            {
                this.button_add.Hide();
            }
            this.button_save_info.Hide();

            DBConnection dbc = new DBConnection();
            //应用条件下拉数据
            string sql = " select '0' as ID,'请选择' as name from dual union ";
            sql += "select a.app_id as id,a.app_name as name from pub_application a where a.delete_flag=0";
            sql += " and exists(select b.menu_id from  pub_menu b where b.delete_flag= 0 and b.end_flag= 1 and a.app_id= b.app_id ";
            sql += " and exists (select c.id from pub_list_tool_set c where c.type='FUNC' and c.id= b.func_id)) order by id";
            this.comboBox_app.DataSource = dbc.GetDataSet(sql).Tables[0];
            this.comboBox_app.DisplayMember = "name";
            this.comboBox_app.ValueMember = "id";


            //列显示
            List<ListItem> items_SHOW_FLAG = new List<ListItem>();
            items_SHOW_FLAG.Add(new ListItem("0", "隐藏"));
            items_SHOW_FLAG.Add(new ListItem("1", "显示"));
            items_SHOW_FLAG.Add(new ListItem("2", "隐藏(不可改)"));
            this.comboBox_G_SHOW_FLAG.DataSource = items_SHOW_FLAG;
            this.comboBox_G_SHOW_FLAG.DisplayMember = "text";
            this.comboBox_G_SHOW_FLAG.ValueMember = "value";

            //列对齐
            List<ListItem> items_ALIGN = new List<ListItem>();
            items_ALIGN.Add(new ListItem("0", "左对齐"));
            items_ALIGN.Add(new ListItem("1", "居中"));
            items_ALIGN.Add(new ListItem("2", "右对齐"));
            this.comboBox_G_ALIGN.DataSource = items_ALIGN;
            this.comboBox_G_ALIGN.DisplayMember = "text";
            this.comboBox_G_ALIGN.ValueMember = "value";

            //列类型   1-string/2-int/3-float/4-datatime/5-bool/6-clob
            List<ListItem> items_TYPE = new List<ListItem>();
            items_TYPE.Add(new ListItem("1", "string"));
            items_TYPE.Add(new ListItem("2", "int"));
            items_TYPE.Add(new ListItem("3", "float"));
            items_TYPE.Add(new ListItem("4", "datatime"));
            items_TYPE.Add(new ListItem("5", "bool"));
            items_TYPE.Add(new ListItem("6", "clob"));
            this.comboBox_COL_TYPE.DataSource = items_TYPE;
            this.comboBox_COL_TYPE.DisplayMember = "text";
            this.comboBox_COL_TYPE.ValueMember = "value";
            //列可编辑
            List<ListItem> items_EDIT = new List<ListItem>();
            items_EDIT.Add(new ListItem("0", "不可编辑"));
            items_EDIT.Add(new ListItem("1", "可编辑"));
            this.comboBox_G_EDIT.DataSource = items_EDIT;
            this.comboBox_G_EDIT.DisplayMember = "text";
            this.comboBox_G_EDIT.ValueMember = "value";

            //显示类型 1_输入框/2_日期框/3_下拉框/4_弹出窗口
            List<ListItem> items_STYLE = new List<ListItem>();
            items_STYLE.Add(new ListItem("1", "输入框"));
            items_STYLE.Add(new ListItem("2", "日期框"));
            items_STYLE.Add(new ListItem("3", "下拉框"));
            items_STYLE.Add(new ListItem("4", "弹出框"));
            this.comboBox_G_STYLE.DataSource = items_STYLE;
            this.comboBox_G_STYLE.DisplayMember = "text";
            this.comboBox_G_STYLE.ValueMember = "value";

            //日期显示格式 0=YYYY,1=YYYY-MM,2=YYYY-MM-DD,3=YYYY-MM-DD HH:mm,4=YYYY-MM-DD HH:mm:ss
            List<ListItem> items_FORMAT = new List<ListItem>();
            items_FORMAT.Add(new ListItem("0", "YYYY"));
            items_FORMAT.Add(new ListItem("1", "YYYY-MM"));
            items_FORMAT.Add(new ListItem("2", "YYYY-MM-DD"));
            items_FORMAT.Add(new ListItem("3", "YYYY-MM-DD HH:mm"));
            items_FORMAT.Add(new ListItem("4", "YYYY-MM-DD HH:mm:ss"));
            this.comboBox_G_FORMAT.DataSource = items_FORMAT;
            this.comboBox_G_FORMAT.DisplayMember = "text";
            this.comboBox_G_FORMAT.ValueMember = "value";

            //日期显示格式 0=YYYY,1=YYYY-MM,2=YYYY-MM-DD,3=YYYY-MM-DD HH:mm,4=YYYY-MM-DD HH:mm:ss
            List<ListItem> items_SORTABLE = new List<ListItem>();
            items_SORTABLE.Add(new ListItem("0", "否"));
            items_SORTABLE.Add(new ListItem("1", "是"));
            this.comboBox_G_SORTABLE.DataSource = items_SORTABLE;
            this.comboBox_G_SORTABLE.DisplayMember = "text";
            this.comboBox_G_SORTABLE.ValueMember = "value";


        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label_grid_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void comboBox_app_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string app_id = this.comboBox_app.SelectedValue.ToString();
            //菜单条件下拉数据
            string sql = " select '0' as ID,'请选择' as name from dual union ";
            sql += "select a.menu_id as id,a.menu_name as name from pub_menu a where a.delete_flag=0 and a.end_flag=1 and a.app_id='" + app_id + "' ";
            sql += " and exists(select b.id from pub_list_tool_set b where  b.type='FUNC' and b.id= a.func_id) order by id";
            DBConnection dbc = new DBConnection();
            this.comboBox_menu.DataSource = dbc.GetDataSet(sql).Tables[0];
            this.comboBox_menu.DisplayMember = "name";
            this.comboBox_menu.ValueMember = "id";
            this.panel2.Controls.Clear();
            this.panel3.Controls.Clear();
            this.textBox_COL_ID.Clear();
            this.textBox_COL_NAME.Clear();
            this.textBox_COL_INDEX.Clear();
            this.button_save_info.Hide();
            //this.panel4.Controls.Clear();
            app_id = "";
            menu_id = "";
            grid_id = "";
        }

        private void comboBox_menu_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string menu_id = this.comboBox_menu.SelectedValue.ToString();
            //菜单条件下拉数据
            string sql = " select '0' as ID,'请选择' as name from dual union ";
            sql += "select a.id,a.name from pub_list_tool_set a ";
            sql += " left join pub_menu b on a.relation_id=b.func_id where a.type='GRID' and b.menu_id='" + menu_id + "'";
            DBConnection dbc = new DBConnection();
            this.comboBox_grid.DataSource = dbc.GetDataSet(sql).Tables[0];
            this.comboBox_grid.DisplayMember = "name";
            this.comboBox_grid.ValueMember = "id";
            this.panel2.Controls.Clear();
            this.panel3.Controls.Clear();
            this.textBox_COL_ID.Clear();
            this.textBox_COL_NAME.Clear();
            this.textBox_COL_INDEX.Clear();
            this.button_save_info.Hide();
            //this.panel4.Controls.Clear();
            app_id = "";
            menu_id = "";
            grid_id = "";
        }
        private void comboBox_grid_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            this.panel3.Controls.Clear();
            this.textBox_COL_ID.Clear();
            this.textBox_COL_NAME.Clear();
            this.textBox_COL_INDEX.Clear();
            this.button_save_info.Hide();
            //this.panel4.Controls.Clear();
            app_id = "";
            menu_id = "";
            grid_id = "";
        }

        private void button_seach_Click(object sender, EventArgs e)
        {
            //this.panel2.Controls.Clear();
            //this.panel3.Controls.Clear();
            this.textBox_COL_ID.Clear();
            this.textBox_COL_NAME.Clear();
            this.textBox_COL_INDEX.Clear();
            this.button_save_info.Hide();
            //this.panel4.Controls.Clear();
            app_id = "";
            menu_id = "";
            grid_id = "";
            try
            {
                app_id = this.comboBox_app.SelectedValue.ToString();
                menu_id = this.comboBox_menu.SelectedValue.ToString();
                grid_id = this.comboBox_grid.SelectedValue.ToString();
            }
            catch
            {
                app_id = "";
                menu_id = "";
                grid_id = "";
                MessageBox.Show("请先选择条件！");
                return;
            }
            if (app_id == "" || menu_id == "" || grid_id == "")
            {
                MessageBox.Show("请先选择相应条件！");
                return;
            }
            DBConnection dbc = new DBConnection();
            //装载数据
            string sql = "insert into SCM_LIST_FORM_COLUMN(APP_ID,MENU_ID,FUNC_ID,GRID_ID,COL_ID,COL_NAME,COL_INDEX,G_PK_FLAG,ORDER_NUM,G_SHOW_FLAG,G_WIDTH,G_ALIGN,G_SORTABLE,G_FORMAT,COL_TYPE,DATA_TYPE,DATA_OPTION,G_STYLE,G_EDIT,G_LINK,SYS_FLAG) ";
            sql += " select '" + app_id + "' as APP_ID,'" + menu_id + "' as MENU_ID,a.FUNC_ID,a.GRID_ID,a.COL_ID,a.COL_NAME,a.COL_INDEX,a.G_PK_FLAG,a.ORDER_NUM,decode(a.G_SHOW_FLAG,2,2,0) as G_SHOW_FLAG,a.G_WIDTH,a.G_ALIGN,a.G_SORTABLE,a.G_FORMAT,a.COL_TYPE,a.DATA_TYPE,a.DATA_OPTION,a.G_STYLE,a.G_EDIT,a.G_LINK,0 as SYS_FLAG ";
            sql += " from SCM_LIST_FORM_COLUMN a where a.app_id = '*' and a.menu_id = '*' and a.grid_id = 'maingrid' ";
            sql += " AND EXISTS(SELECT z.*FROM pub_menu z where z.menu_id = '" + menu_id + "' and z.func_id = a.func_id)  ";
            sql += " and not exists(select y.* from scm_list_form_column y where a.func_id = y.func_id and a.grid_id = y.grid_id and a.col_id = y.col_id and y.menu_id = '" + menu_id + "') ";
            int temp = dbc.update(sql);
            //清除之前暂存的数据
            sql = "update SCM_LIST_FORM_COLUMN a  set a.sys_flag=0,a.g_show_flag=0,a.order_num=999 where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and a.sys_flag=2";
            temp = dbc.update(sql);
            //加载列表
            lod_panle2_label();
            lod_panle3_label();
            #region 
            ////加载未显示列
            //string sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id = '"+ app_id + "' and a.menu_id = '"+ menu_id + "' and a.grid_id = '" + grid_id + "' and a.sys_flag=0 and nvl(a.g_show_flag,0)=0 order by a.order_num ";
            //DBConnection dbc = new DBConnection();
            //DataSet ds = dbc.GetDataSet(sql);
            //DataTable dt = ds.Tables[0];
            //panel2_y = 3;
            //for (int j = 0; j < dt.Rows.Count; j++)
            //{
            //    string col_id = dt.Rows[j]["COL_ID"].ToString();
            //    string col_name = dt.Rows[j]["COL_NAME"].ToString();
            //    //Button btn = new Button();
            //    //btn.Location = new System.Drawing.Point(x, y);
            //    //btn.Name = col_id;
            //    //btn.Text = col_name;
            //    //btn.Size = new System.Drawing.Size(130, 23);
            //    //btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //    //btn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //    //btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
            //    //btn.UseVisualStyleBackColor = true;
            //    //btn.TabIndex = 20;
            //    ////btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseDown);
            //    ////btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_MouseUp);
            //    ////btn.MouseMove+= new System.Windows.Forms.MouseEventHandler(this.button_MouseMove);
            //    //btn.Click += new System.EventHandler(this.button_Click);
            //    //this.panel2.Controls.Add(btn);
            //    Label lab = new Label();
            //    lab.Location = new System.Drawing.Point(panel2_x, panel2_y);
            //    lab.Name = col_id;
            //    lab.Text = col_name;
            //    lab.Size = new System.Drawing.Size(130, 23);
            //    lab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //    lab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //    lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //    lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
            //    lab.TextAlign= System.Drawing.ContentAlignment.MiddleCenter; 
            //    lab.TabIndex = 20;
            //    lab.Click += new System.EventHandler(this.label_Click);
            //    lab.DoubleClick += new System.EventHandler(this.label_DoubleClick2);
            //    this.panel2.Controls.Add(lab);
            //    panel2_y += 25;
            //}

            ////加载显示列
            //sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id = '" + app_id + "' and a.menu_id = '" + menu_id + "' and a.grid_id = '" + grid_id + "' and a.sys_flag=1 and a.g_show_flag=1 order by a.order_num ";
            //ds = dbc.GetDataSet(sql);
            //dt = ds.Tables[0];
            //panel3_x = 2;
            //for (int j = 0; j < dt.Rows.Count; j++)
            //{
            //    string col_id = dt.Rows[j]["COL_ID"].ToString();
            //    string col_name = dt.Rows[j]["COL_NAME"].ToString();
            //    Label lab = new Label();
            //    lab.Location = new System.Drawing.Point(panel3_x, panel3_y);
            //    lab.Name = col_id;
            //    lab.Text = col_name;
            //    lab.Size = new System.Drawing.Size(130, 23);
            //    lab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //    lab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //    lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //    lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
            //    lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //    lab.TabIndex = 30;
            //    lab.Click += new System.EventHandler(this.label_Click);
            //    lab.DoubleClick += new System.EventHandler(this.label_DoubleClick3);
            //    this.panel3.Controls.Add(lab);
            //    panel3_x += 131;
            //}
            #endregion

        }

        private void lod_panle2_label() {
            //加载未显示列
            this.panel2.Controls.Clear();
            DBConnection dbc = new DBConnection();
            string sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id = '" + app_id + "' and a.menu_id = '" + menu_id + "' and a.grid_id = '" + grid_id + "' and a.sys_flag=0 and nvl(a.g_show_flag,0)=0 order by a.order_num ";
            DataSet ds = dbc.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            panel2_y = 3;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string col_id = dt.Rows[j]["COL_ID"].ToString();
                string col_name = dt.Rows[j]["COL_NAME"].ToString();
                //Button btn = new Button();
                //btn.Location = new System.Drawing.Point(x, y);
                //btn.Name = col_id;
                //btn.Text = col_name;
                //btn.Size = new System.Drawing.Size(130, 23);
                //btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                //btn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                //btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
                //btn.UseVisualStyleBackColor = true;
                //btn.TabIndex = 20;
                ////btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseDown);
                ////btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_MouseUp);
                ////btn.MouseMove+= new System.Windows.Forms.MouseEventHandler(this.button_MouseMove);
                //btn.Click += new System.EventHandler(this.button_Click);
                //this.panel2.Controls.Add(btn);
                Label lab = new Label();
                lab.Location = new System.Drawing.Point(panel2_x, panel2_y);
                lab.Name = col_id;
                lab.Text = col_name;
                lab.Size = new System.Drawing.Size(130, 23);
                lab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                lab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                //lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
                lab.ForeColor = System.Drawing.SystemColors.ControlText;
                lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lab.TabIndex = 20;
                lab.Click += new System.EventHandler(this.label_Click);
                lab.DoubleClick += new System.EventHandler(this.label_DoubleClick2);
                this.panel2.Controls.Add(lab);
                panel2_y += 25;
            }
        }

        private void lod_panle3_label()
        {
            //加载显示列
            this.panel3.Controls.Clear();
            string sql = "select a.* from SCM_LIST_FORM_COLUMN a where a.app_id = '" + app_id + "' and a.menu_id = '" + menu_id + "' and a.grid_id = '" + grid_id + "' and a.sys_flag in (1,2) and a.g_show_flag=1 order by a.order_num ";
            DBConnection dbc = new DBConnection();
            DataSet ds = dbc.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            panel3_x = 2;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string col_id = dt.Rows[j]["COL_ID"].ToString();
                string col_name = dt.Rows[j]["COL_NAME"].ToString();
                Label lab = new Label();
                lab.Location = new System.Drawing.Point(panel3_x, panel3_y);
                lab.Name = col_id;
                lab.Text = col_name;
                lab.Size = new System.Drawing.Size(130, 23);
                lab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                lab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                //lab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(193)))), ((int)(((byte)(255)))));
                lab.ForeColor = System.Drawing.SystemColors.ControlText;
                lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lab.TabIndex = 30;
                lab.Click += new System.EventHandler(this.label_Click);
                lab.DoubleClick += new System.EventHandler(this.label_DoubleClick3);
                this.panel3.Controls.Add(lab);
                panel3_x += 131;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel3.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel4.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void button_save_info_Click(object sender, EventArgs e)
        {
            string app_id = this.comboBox_app.SelectedValue.ToString();
            string menu_id = this.comboBox_menu.SelectedValue.ToString();
            string grid_id = this.comboBox_grid.SelectedValue.ToString();
            string modify_type = this.label_MODIFY_TYPE.Text;
            string col_id = this.textBox_COL_ID.Text;
            string g_width = this.textBox_G_WIDTH.Text;
            string g_align = this.comboBox_G_ALIGN.SelectedValue.ToString();
            string g_format = this.comboBox_G_FORMAT.SelectedValue != null ? this.comboBox_G_FORMAT.SelectedValue.ToString() : "";
            if (col_id == "")
            {
                MessageBox.Show("请先选择列！");
                return;
            }
            List<string> sqls = new List<string>();
            string sql = "";
            string sql_1 = "";
            if (modify_type == "add") {
                //新增
                string col_sql = "insert into scm_list_form_column(APP_ID, MENU_ID, FUNC_ID, GRID_ID ";
                string val_sql = "";
                string val_sql0 = "values('*', '*', (select a.func_id from pub_menu a where a.menu_id='"+ menu_id + "'), 'maingrid'";
                string val_sql1 = "values('"+ app_id + "', '"+ menu_id + "', (select a.func_id from pub_menu a where a.menu_id='" + menu_id + "'), 'maingrid'";

                #region
                col_sql += ",COL_ID ";//列代码
                val_sql += ",'"+ col_id + "' ";

                col_sql += ",COL_NAME ";//列名称
                val_sql += ",'" + this.textBox_COL_NAME.Text + "' ";

                col_sql += ",COL_INDEX ";//列INDEX
                val_sql += ",'" + this.textBox_COL_INDEX.Text + "' ";

                col_sql += ",G_PK_FLAG ";//列主键
                val_sql += ",0 ";

                col_sql += ",ORDER_NUM ";//排序序号
                val_sql += ",999 ";

                col_sql += ",G_SHOW_FLAG ";//列显示
                val_sql += ",0 ";

                col_sql += ",G_WIDTH ";//列宽度
                if (this.textBox_G_WIDTH.Text == "" || this.textBox_G_WIDTH.Text == null)
                    val_sql += ",100";
                else
                    val_sql += "," + this.textBox_G_WIDTH.Text + " ";

                col_sql += ",G_ALIGN ";//列对齐
                if (this.comboBox_G_ALIGN.SelectedValue == null)
                    val_sql += ",0";
                else
                    val_sql += "," + this.comboBox_G_ALIGN.SelectedValue + " ";

                col_sql += ",G_SORTABLE ";//列可排序
                if (this.comboBox_G_SORTABLE.SelectedValue == null)
                    val_sql += ",0";
                else
                    val_sql += "," + this.comboBox_G_SORTABLE.SelectedValue + " ";

                col_sql += ",G_FORMAT ";//列日期显示格式
                val_sql += ",'" + this.comboBox_G_FORMAT.SelectedValue + "' ";

                col_sql += ",COL_TYPE ";//列类型
                if (this.comboBox_COL_TYPE.SelectedValue == null)
                    val_sql += ",1";
                else
                    val_sql += "," + this.comboBox_COL_TYPE.SelectedValue + " ";

                col_sql += ",DATA_TYPE ";//值方式
                val_sql += ",'" + this.textBox_DATA_TYPE.Text + "' ";

                col_sql += ",DATA_OPTION ";//值选项
                val_sql += ",'" + this.textBox_DATA_OPTION.Text + "' ";

                col_sql += ",G_STYLE ";//显示类型
                if (this.comboBox_G_STYLE.SelectedValue == null)
                    val_sql += ",1";
                else
                    val_sql += "," + this.comboBox_G_STYLE.SelectedValue + " ";

                col_sql += ",G_EDIT ";//列可编辑
                if (this.comboBox_G_EDIT.SelectedValue == null)
                    val_sql += ",0";
                else
                    val_sql += "," + this.comboBox_G_EDIT.SelectedValue + " ";

                col_sql += ",G_LINK ";//列超链接
                val_sql += ",'" + this.textBox_G_LINK.Text + "' ";

                col_sql += ",SYS_FLAG ";//系统标志
                val_sql += ",0 ";
                #endregion

                col_sql += ")";
                val_sql += ")";
                sql = col_sql + val_sql0 + val_sql;
                sql_1 = col_sql + val_sql1 + val_sql;
                sqls.Add(sql);
                sqls.Add(sql_1);
            }
            else { 
                //配置人员只能改列宽/对齐方式/日期显示格式，其他属性由开发人员配置
                sql = "update SCM_LIST_FORM_COLUMN a  set a.g_width= "+ g_width + ",a.g_align="+ g_align + ",a.g_format='" + g_format + "' where a.app_id='" + app_id + "' and a.menu_id='"+ menu_id + "' and a.grid_id='"+ grid_id + "' and  a.col_id='"+ col_id + "'";
                sqls.Add(sql);
            }
            if (sql == "")
            {
                MessageBox.Show("请先选择操作");
            }
            DBConnection dbc = new DBConnection();
            string temp = dbc.updates(sqls);
            if (temp == "0")
            {
                MessageBox.Show("保存成功");
                lod_panle2_label();
                lod_panle3_label();
            }
            else
            {
                MessageBox.Show("保存失败\r\n" + temp);
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            //string app_id = this.comboBox_app.SelectedValue.ToString();
            //string menu_id = this.comboBox_menu.SelectedValue.ToString();
            //string grid_id = this.comboBox_grid.SelectedValue.ToString();
            int ORDER_NUM = 1;
            List<string> sqls = new List<string>();
            string sql = "update SCM_LIST_FORM_COLUMN a set a.order_num=999,a.sys_flag=0 where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "'";
            sqls.Add(sql);
            foreach (var control in this.panel3.Controls)
            {
                if (control.GetType() == typeof(Label))
                {
                    Label lab = (Label)control;
                    string id = lab.Name;
                    string name = lab.Text;
                    sql = "update SCM_LIST_FORM_COLUMN a set a.order_num="+ ORDER_NUM + ",a.g_show_flag=1,a.sys_flag=1 where a.app_id='" + app_id + "' and a.menu_id='"+ menu_id + "' and a.grid_id='"+ grid_id + "' and  a.col_id='"+ id + "'";
                    sqls.Add(sql);
                    ORDER_NUM++;
                }
            }
            //更新主键
            sql = "update SCM_LIST_FORM_COLUMN a set a.order_num=0,a.g_show_flag=2,a.sys_flag=1 where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.g_pk_flag=1";
            sqls.Add(sql);

            //更新不显示的字段
            sql = "update SCM_LIST_FORM_COLUMN a set a.g_show_flag=0 where a.app_id='" + app_id + "' and a.menu_id='" + menu_id + "' and a.grid_id='" + grid_id + "' and  a.sys_flag=0 and a.g_show_flag !=2";
            sqls.Add(sql);

            DBConnection dbc = new DBConnection();
            string temp = dbc.updates(sqls);
            if (temp == "0")
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败\r\n" + temp);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                app_id = this.comboBox_app.SelectedValue.ToString();
                menu_id = this.comboBox_menu.SelectedValue.ToString();
                grid_id = this.comboBox_grid.SelectedValue.ToString();
            }
            catch
            {
                app_id = "";
                menu_id = "";
                grid_id = "";
                MessageBox.Show("请先选择条件！");
                return;
            }
            if (grid_id == "") { 
                MessageBox.Show("请先选择对应列表");
                return;
            }
            this.textBox_COL_ID.Clear();
            this.textBox_COL_ID.ReadOnly = false;
            this.textBox_COL_INDEX.Clear();
            this.textBox_COL_INDEX.ReadOnly = false;
            this.textBox_COL_NAME.Clear();
            this.textBox_COL_NAME.ReadOnly = false;
            this.textBox_DATA_OPTION.Clear();
            this.textBox_DATA_OPTION.ReadOnly = false;
            this.textBox_DATA_TYPE.Clear();
            this.textBox_DATA_TYPE.ReadOnly = false;
            this.textBox_G_LINK.Clear();
            this.textBox_G_LINK.ReadOnly = false;
            this.textBox_G_WIDTH.Text= "100";
            this.textBox_G_WIDTH.ReadOnly = false;
            this.textBox_SYS_FLAG.Text = "0";
            this.comboBox_G_ALIGN.SelectedValue = "1";
            this.comboBox_G_EDIT.SelectedValue = "";
            this.comboBox_G_FORMAT.SelectedValue = "";
            this.comboBox_G_SHOW_FLAG.SelectedValue = "";
            this.comboBox_G_SORTABLE.SelectedValue = "";
            this.comboBox_G_STYLE.SelectedValue = "";

            this.label_MODIFY_TYPE.Text = "add";
            this.button_save_info.Show();
        }
    }
}
