using System;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Linq;
using System.IO;

namespace BaiTapLinQ
{
    public partial class Form1 : Form
    {
        string path = "../../Hinh";
        QLSVDataContext _db;
        Table<SINHVIEN> Bang_SINHVIEN;
        Table<LOP> Bang_LOP;
        BindingManagerBase dssv;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (txttk.Text == "Nhập vào tên sinh viên cần tìm...")
            {
                txttk.Text = "";
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (txttk.Text == "Nhập vào tên sinh viên cần tìm...")
            {
                txttk.Text = "";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _db = new QLSVDataContext();
            Bang_SINHVIEN = _db.SINHVIENs;
            Bang_LOP = _db.LOPs;

            LoadCBOLop();
            Loaddgv();

            using (QLSVDataContext _db = new QLSVDataContext())
            {
                txtma.DataBindings.Add("text", Bang_SINHVIEN, "MaSV", true);
                txtten.DataBindings.Add("text", Bang_SINHVIEN, "HoTen", true);
                dtpngs.DataBindings.Add("text", Bang_SINHVIEN, "NgaySinh", true);
                rdbnam.DataBindings.Add("Checked", Bang_SINHVIEN, "GioiTinh", true);
                cmblop.DataBindings.Add("SelectedValue", Bang_SINHVIEN, "MaLop", true);
                txtdiachi.DataBindings.Add("text", Bang_SINHVIEN, "DiaChi", true);
                pictureBox1.DataBindings.Add("ImageLocation", Bang_SINHVIEN, "Hinh", true);

                dssv = this.BindingContext[Bang_SINHVIEN];
                enableNutLenh(false);
            }
                
        }

        private void LoadCBOLop()
        {
            using (QLSVDataContext _db = new QLSVDataContext())
            {
                cmblop.DataSource = Bang_LOP;
                cmblop.DisplayMember = "TenLop";
                cmblop.ValueMember = "MaLop";
            }
        }
        
        private void Loaddgv()
        {
            using (QLSVDataContext _db = new QLSVDataContext())
            {
                dgv1.DataSource = Bang_SINHVIEN;
            }
        }

        private void enableNutLenh(bool capnhat)
        {
            btnthem.Enabled = !capnhat;
            btnxoa.Enabled = !capnhat;
            btnsua.Enabled = !capnhat;
            btnthoat.Enabled = !capnhat;
            btnluu.Enabled = capnhat;
            btnhuy.Enabled = capnhat;
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            dssv.AddNew();
            enableNutLenh(true);
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
                try
                {
                    dssv.EndCurrentEdit();
                    _db.SubmitChanges();
                    enableNutLenh(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            dssv.CancelCurrentEdit();
            ChangeSet cs = _db.GetChangeSet();
            _db.Refresh(RefreshMode.OverwriteCurrentValues, cs.Updates);
            enableNutLenh(false);
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            dssv.RemoveAt(dssv.Position);
            _db.SubmitChanges();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            enableNutLenh(true);
        }

        private void btnch_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG files|*.jpg|PNG files|*.png|All files|*.*";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.SafeFileName;
                string pathFile = path + "/" + fileName;
                if (!File.Exists(pathFile))
                    File.Copy(openFileDialog1.FileName, pathFile);
                pictureBox1.ImageLocation = pathFile;

            }
        }

        private void btnloc_Click(object sender, EventArgs e)
        {
            dssv.Position = _db.SINHVIENs.ToList().FindIndex(sv => sv.MaSV.Contains(txttk.Text));
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnloc_MouseDown(object sender, MouseEventArgs e)
        {
            txttk.Text = "";
        }
    }
}
