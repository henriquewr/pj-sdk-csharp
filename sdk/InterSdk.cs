using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Sdk.CobrancaApi;
using Sdk.BankingApi;
using Sdk.PixApi;

namespace Sdk {
    public class InterSdk {
        private Config config;
        List<string> Avisos;
        private CobrancaSdk cobranca;
        private BankingSdk banking;
        private PixSdk pix;
        private static StreamWriter log;

        public static string GetVersion() {
           return "inter-sdk-csharp v1.0.0-2023-09-26";
        }

        public InterSdk(string certificado, string senha, string clientId, string clientSecret) {
            config = Config.Builder()
                .SetCertificado(certificado)
                .SetSenha(senha)
                .SetClientId(clientId)
                .SetClientSecret(clientSecret)
                .SetControleRateLimit(true)
                .Build();
            Avisos = new List<string>();
            OpenLog();
            DateTime notAfter = DateTime.Now;
            bool ok = CheckCertificate(certificado, senha, ref notAfter);
            if (!ok) {
                Avisos.Add(String.Format("Certificado próximo de expirar. Menos de {0} dias. Expira em {1}.", Constants.DAYS_TO_EXPIRE, notAfter));
            }
        }

        public InterSdk(string ambiente, string certificado, string senha, string clientId, string clientSecret): this(certificado, senha, clientId, clientSecret) {
            this.config.Ambiente = ambiente;
        }

        public void SetDebug(bool debug) {
            config.Debug = debug;
        }

        public void SetAmbiente(string ambiente) {
            config.Ambiente = ambiente;
        }

        public void SetContaCorrente(string contaCorrente) {
            config.ContaCorrente = contaCorrente;
        }

        public string GetContaCorrente() {
            return config.ContaCorrente;
        }

        public void SetControleRateLimit(bool controle) {
            config.ControleRateLimit = controle;
        }

        public bool IsControleRateLimit() {
            return config.ControleRateLimit;
        }

        public CobrancaSdk CobrancaSdk() {
            if (cobranca == null) {
                cobranca = new CobrancaSdk(config);
            }
            return cobranca;
        }

        public BankingSdk BankingSdk() {
            if (banking == null) {
                banking = new BankingSdk(config);
            }
            return banking;
        }

        public PixSdk PixSdk() {
            if (pix == null) {
                pix = new PixSdk(config);
            }
            return pix;
        }

        public List<string> ListaAvisos() {
            return Avisos;
        }

        private void OpenLog() {
            if (!File.Exists("logs")) {
                Directory.CreateDirectory("logs");
            } 
            string tomorrow = DateTime.Now.AddDays(1).DayOfWeek.ToString();
            string path = "logs/inter-sdk-" + tomorrow + ".log";
            if (File.Exists(path)) {
                File.Delete(path);
            }
            string today = DateTime.Now.DayOfWeek.ToString();
            path = "logs/inter-sdk-" + today + ".log";
            FileStream fileStream = null;
            if (File.Exists(path)) {
                fileStream = new FileStream(path, FileMode.Append);
            } else {
                fileStream = new FileStream(path, FileMode.Create);
            }
            log = new StreamWriter(fileStream);
            InterSdk.LogInfo("{0}", GetVersion());
        }

        public static void LogInfo(string format, params string[] prms) {
            string text = String.Format(format, prms);
            log.Write(DateTime.Now.ToString());
            log.WriteLine(" " + text);
            log.Flush();
        }

        private bool CheckCertificate(string certificado, string senha, ref DateTime notAfter) {
            X509Certificate2Collection certificates = new X509Certificate2Collection();
            certificates.Import(certificado, senha, X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert = certificates[0];
            TimeSpan diff = cert.NotAfter.Subtract(DateTime.Now);
            if (diff.TotalDays < Constants.DAYS_TO_EXPIRE) {
                notAfter = cert.NotAfter;
                return false;
            }
            return true;
        }
    }
}