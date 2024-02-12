using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace Sdk {
    public class Token {
        public string access_token;
        public int expires_in;
    }

    public class TokenInfo {
        public DateTime t;
        public Token tok;
    }

    public class TokenUtils {
        private static Dictionary<string, TokenInfo> map = new Dictionary<string, TokenInfo>();
        private const int SECONDS_TO_RENEW = 60;

        public static Token GetToken(Config config, string scopes, X509Certificate2Collection certificates) {
            TokenInfo info = null;
            bool ok = map.TryGetValue(scopes, out info);
            
            if (ok) {
                TimeSpan diff = DateTime.Now.Subtract(info.t);
                if ((info.tok.expires_in - diff.TotalSeconds) < SECONDS_TO_RENEW) {
                    ok = false;
                }
            }

            if (!ok) {
                Token token = HttpUtils.ObterToken(config, scopes, certificates);
                info = new TokenInfo();
                info.tok = token;
                info.t = DateTime.Now;
                map[scopes] = info;
            }
            return info.tok;
        }
    }
}

