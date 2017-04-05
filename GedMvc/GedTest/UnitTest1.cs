using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;

namespace GedTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateLicense()
        {
            Configuracao conf = new Configuracao();
            conf.DataInicioLicensa = DateTime.Now;
            conf.DataFimLicensa = conf.DataInicioLicensa.AddDays(360);
            conf.GuidValue = Guid.NewGuid().ToString();
            conf.NomeCliente = "Demo";

            string path = @"C:\gedrepositorio\config.bin";
            Util.Serializar(conf, path);
        }
    }
}
