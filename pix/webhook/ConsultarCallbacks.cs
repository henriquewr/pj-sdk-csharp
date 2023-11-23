using System.Text;
using System.Collections.Generic;

namespace Sdk.PixApi {
	public class ConsultarCallbacks {
		public PaginaCallbacks Buscar(Config config, string dataHoraInicio, string dataHoraFim, int pagina, int tamanhoPagina, FiltroBuscarCallbacks filtro) {
			InterSdk.LogInfo("BuscarCallbacks {0} {1}-{2}", config.ClientId, dataHoraInicio, dataHoraFim);
			return GetPage(config, dataHoraInicio, dataHoraFim, pagina, tamanhoPagina, filtro);
		}

		public List<Callback> Buscar(Config config, string dataHoraInicio, string dataHoraFim, FiltroBuscarCallbacks filtro) {
			InterSdk.LogInfo("BuscarCallbacks {0} {1}-{2}", config.ClientId, dataHoraInicio, dataHoraFim);
			int pagina = 0;
			PaginaCallbacks paginaCallbacks;
			List<Callback> callbacks = new List<Callback>();
			do {
				paginaCallbacks = GetPage(config, dataHoraInicio, dataHoraFim, pagina, 0, filtro);
				callbacks.AddRange(paginaCallbacks.Callbacks);
				pagina++;
			} while (pagina < paginaCallbacks.TotalPaginas);
			return callbacks;
		}

		public PaginaCallbacks GetPage(Config config, string dataHoraInicio, string dataHoraFim, int pagina, int tamanhoPagina, FiltroBuscarCallbacks filtro) {
			string url = Constants.URL_PIX_WEBHOOK_PIX.Replace("AMBIENTE", config.Ambiente) + "/" + Constants.CALLBACKS + "?dataHoraInicio=" + dataHoraInicio + "&dataHoraFim=" + dataHoraFim + "&paginaAtual=" + pagina + SdkUtils.PrmTamanhoPagina(tamanhoPagina) + Addfilters(filtro);
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_WEBHOOK_READ, "Erro ao recuperar callbacks");
			return (PaginaCallbacks) SdkUtils.Deserialize(typeof(PaginaCallbacks), json);
		}

		public string Addfilters(FiltroBuscarCallbacks filtro) {
			if (filtro == null ) {
				return "";
			}
			StringBuilder filter = new StringBuilder();
			if (filtro.Txid != null ) {
				filter.Append("&txid").Append("=").Append(filtro.Txid);
			}
			return filter.ToString();
		}

	}
}
