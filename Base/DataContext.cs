namespace WpfApplication2
{
    using System;

    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Documents;
    using System.Windows.Forms;
    using System.IO;
    using System.Text;

    public partial class DataContext
    {
        public static string JsonFilePath =  System.Environment.CurrentDirectory + @"\Config\Config.cnf";


        public static bool AddOrUpdate(Group gp)
        {
            try
            {
                List<Group> list = new List<Group>();

                if (File.Exists(JsonFilePath))
                {
                    StreamReader sr1 = new StreamReader(JsonFilePath, Encoding.GetEncoding("utf-8"));
                    String myStr1 = sr1.ReadToEnd();
                    sr1.Close();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Group>>(myStr1);
                }
                if (list.Count(t => t.ID.Equals(gp.ID)) > 0)
                {
                    var temp = list.First(t => t.ID.Equals(gp.ID));
                    list.Remove(temp);
                }
                list.Add(gp);
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                FileStream fileStream = new FileStream(JsonFilePath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8"));
                streamWriter.Write(result);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static bool Delete(string ID)
        {
            try
            {
                List<Group> list = new List<Group>();
                if (File.Exists(JsonFilePath))
                {
                    StreamReader sr1 = new StreamReader(JsonFilePath, Encoding.GetEncoding("utf-8"));
                    String myStr1 = sr1.ReadToEnd();
                    sr1.Close();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Group>>(myStr1);
                }
                var temp = list.First(t => t.ID.Equals(ID));
                list.Remove(temp);
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                FileStream fileStream = new FileStream(JsonFilePath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8"));
                streamWriter.Write(result);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static List<Group> GetAll()
        {
            List<Group> list = new List<Group>();
            try
            {
                if (File.Exists(JsonFilePath))
                {
                    StreamReader sr1 = new StreamReader(JsonFilePath, Encoding.GetEncoding("utf-8"));
                    String myStr1 = sr1.ReadToEnd();
                    sr1.Close();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Group>>(myStr1);
                }
                return list;
            }
            catch
            {
                return list;
            }
        }

    }
}
