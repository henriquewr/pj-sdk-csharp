using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Sdk
{
    public class InterSdk
    {
        private Config config;
        List<string> Avisos;
        private CobrancaSdk cobranca;
        private BankingSdk banking;
        private PixSdk pix;
        private static string logFilePath;

        public static string GetVersion()
        {
            return "inter-sdk-csharp v1.0.0-2023-09-26";
        }

        public InterSdk(string certificado, string senha, string clientId, string clientSecret, string logPath = "logs")
        {
            config = Config.Builder()
                .SetCertificado(certificado)
                .SetSenha(senha)
                .SetClientId(clientId)
                .SetClientSecret(clientSecret)
                .SetControleRateLimit(true)
                .Build();
            Avisos = new List<string>();
            OpenLog(logPath);
            DateTime notAfter = DateTime.Now;
            bool ok = CheckCertificate(certificado, senha, ref notAfter);
            if (!ok)
            {
                Avisos.Add(String.Format("Certificado próximo de expirar. Menos de {0} dias. Expira em {1}.", Constants.DAYS_TO_EXPIRE, notAfter));
            }
        }

        public InterSdk(string ambiente, string certificado, string senha, string clientId, string clientSecret, string logPath = "logs") : this(certificado, senha, clientId, clientSecret, logPath)
        {
            SetAmbiente(ambiente);
        }

        public void SetDebug(bool debug)
        {
            config.Debug = debug;
        }

        public void SetAmbiente(string ambiente)
        {
            config.Ambiente = ambiente;
        }

        public void SetContaCorrente(string contaCorrente)
        {
            config.ContaCorrente = contaCorrente;
        }

        public string GetContaCorrente()
        {
            return config.ContaCorrente;
        }

        public void SetControleRateLimit(bool controle)
        {
            config.ControleRateLimit = controle;
        }

        public bool IsControleRateLimit()
        {
            return config.ControleRateLimit;
        }

        public CobrancaSdk CobrancaSdk()
        {
            if (cobranca == null)
            {
                cobranca = new CobrancaSdk(config);
            }
            return cobranca;
        }

        public BankingSdk BankingSdk()
        {
            if (banking == null)
            {
                banking = new BankingSdk(config);
            }
            return banking;
        }

        public PixSdk PixSdk()
        {
            if (pix == null)
            {
                pix = new PixSdk(config);
            }
            return pix;
        }

        public List<string> ListaAvisos()
        {
            return Avisos;
        }

        private void OpenLog(string logPath)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            string tomorrow = DateTime.Now.AddDays(1).DayOfWeek.ToString();
            logFilePath = Path.Combine(logPath, $"inter-sdk-{tomorrow}.log");
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
            string today = DateTime.Now.DayOfWeek.ToString();

            logFilePath = Path.Combine(logPath, $"inter-sdk-{today}.log");

            InterSdk.LogInfo("{0}", GetVersion());
        }

        public static void LogInfo(string format, params string[] prms)
        {
            using (StreamWriter log = new StreamWriter(logFilePath, true))
            {
                string text = String.Format(format, prms);
                string date = DateTime.Now.ToString();
                log.WriteLine($"{date} {text}");
            }
        }

        private bool CheckCertificate(string certificado, string senha, ref DateTime notAfter)
        {
            X509Certificate2Collection certificates = new X509Certificate2Collection();
            certificates.Import(certificado, senha, X509KeyStorageFlags.PersistKeySet);
            using (X509Certificate2 cert = certificates[0])
            {
                TimeSpan diff = cert.NotAfter.Subtract(DateTime.Now);
                if (diff.Days < Constants.DAYS_TO_EXPIRE)
                {
                    notAfter = cert.NotAfter;
                    return false;
                }
                return true;
            }
        }
    }
}