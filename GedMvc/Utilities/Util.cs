using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utilities
{
    public class Util
    {
        public static void FinalizarDataReader(DbDataReader leitor)
        {
            if (!leitor.IsClosed)
            {
                leitor.Close();
            }
            leitor.Dispose();
        }

        public static string GetMimeType(string extensao)
        {
            string ret = "";

            switch (extensao)
            {
                case "txt":
                    ret = "application/txt";
                    break;
                case "doc":
                    ret = "application/msword";
                    break;
                case "pdf":
                    ret = "application/pdf";
                    break;
                case "avi":
                    ret = "video/avi";
                    break;
                case "dwg":
                    ret = "application/acad";
                    break;
                case "htm":
                    ret = "text/html";
                    break;
                case "html":
                    ret = "text/html";
                    break;
                case "jpe":
                    ret = "image/jpeg";
                    break;
                case "jpg":
                    ret = "image/jpeg";
                    break;
                case "jpeg":
                    ret = "image/jpeg";
                    break;
                case "mp3":
                    ret = "audio/mpeg3";
                    break;
                case "pps":
                    ret = "application/vnd.ms-powerpoint";
                    break;
                case "xls":
                    ret = "application/excel";
                    break;
                case "xml":
                    ret = "application/xml";
                    break;
                case "zip":
                    ret = "application/x-compressed";
                    break;
                case "docx":
                    ret = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "xlsx":
                    ret = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "pptx":
                    ret = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case "odt":
                    ret = "application/vnd.oasis.opendocument.text";
                    break;


            }

            return ret;
        }

        public static string CheckFileExists(string filename)
        {
            string msg = null;
            FileInfo f = new FileInfo(filename);
            if (f.Exists)
            {
                msg = "Já existe um arquivo com este nome no diretorio, favor verificar com o administrador";
            }
            return msg;
        }

        public static void Serializar(Configuracao conf, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            //Criação de objeto BinaryFormatter para efetuar serialização no formato binário    
            BinaryFormatter bf = new BinaryFormatter();
            //Uso do objeto BinaryFormatter para serializar os dados da instância "funci" no arquivo     
            bf.Serialize(fs, conf);
            //Fecha arquivo     
            fs.Close();
            fs.Dispose();
        }

        public static Configuracao Deserializar(string filepath, string path)
        {
            Configuracao funcionario = null;
            //FileInfo fi = new FileInfo(@"C:\gedrepositorio\Funcionario.bin");
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
            {
                //Abertura do arquivo binário para leitura     
                FileStream fs = new FileStream(filepath, FileMode.Open);
                //Criação de objeto BinaryFormatter para efetuar deserialização de formato binário    
                BinaryFormatter bf = new BinaryFormatter();
                //Uso do objeto BinaryFormatter para deserializar os dados do arquivo  
                funcionario = (Configuracao)bf.Deserialize(fs);
                //Fecha arquivo     
                fs.Close();
                fs.Dispose();
            }
            return funcionario;
        }

        public static void CriarRegistro(string path, string name, string value)
        {
            //string PATH = @"SOFTWARE\QX3\ROTAS\Hosts";
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(path);
            registryKey.SetValue(name, value);
            registryKey.Close();
        }

        public static string retornarRegitro(string path, string name)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("gedsys", true).OpenSubKey("guidkey", true);
            var ret = registryKey.GetValue(name);
            return ret.ToString();
        }
    }
}
