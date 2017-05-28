using SendImageByJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SendForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string path = "";
            byte[] bytes = new byte[1024 * 100];
            Data data = new Data();

            //Load image and convert it into byte array
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bytes = File.ReadAllBytes(dlg.FileName);
                path = dlg.FileName;
                txtImageName.Text = Path.GetFileName(dlg.FileName);

                data.Name = txtImageName.Text;
                data.Image = bytes;

            }
            string url = "http://localhost:53666/api/Index/DataSender?name=" + txtImageName.Text;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(data);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            WebResponse httpResponse = httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var serializer = new JavaScriptSerializer();
               // var   jsonObject = serializer.DeserializeObject(streamReader.ReadToEnd());
                dynamic imageData = serializer.Deserialize(streamReader.ReadToEnd(), typeof(object));
                string imageName = imageData["Name"];
                byte[] imageBytes  = Convert.FromBase64String(imageData["Image"]);
                var fs = new BinaryWriter(new FileStream(Application.StartupPath+ txtImageName.Text, FileMode.Append, FileAccess.Write));
                fs.Write(imageBytes);
                fs.Close();
                MemoryStream ms = new MemoryStream(imageBytes);
                pictureBox1.Image = Image.FromStream(ms);
            }
        }

    }

}

