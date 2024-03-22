using System.Text;
using System.Collections.Generic;

namespace Sdk.PixApi {
	public class ConsultarLocationsCadastradas {
		public PaginaLocations Consultar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarLocations filtro) {
			InterSdk.LogInfo("ConsultarLocationsCadastradas {0} {1}-{2} pagina={3}", config.ClientId, dataInicial, dataFinal, pagina.ToString());
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro);
		}

		public List<Location> Consultar(Config config, string dataInicial, string dataFinal, FiltroConsultarLocations filtro) {
			InterSdk.LogInfo("ConsultarLocationsCadastradas {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			PaginaLocations paginaLocations;
			List<Location> locs = new List<Location>();
			do {
				paginaLocations = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro);
				locs.AddRange(paginaLocations.Locs);
				pagina++;
			} while (pagina < paginaLocations.Parametros.Paginacao.QuantidadeDePaginas);
			return locs;
		}

		public PaginaLocations GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarLocations filtro) {
			string url = $"{Constants.URL_PIX_LOCATIONS.Replace("AMBIENTE", config.Ambiente)}?inicio={dataInicial}&fim={dataFinal}&paginacao.paginaAtual={pagina}{(tamanhoPagina == 0 ? "" : "&paginacao.itensPorPagina=" + tamanhoPagina)}{Addfilters(filtro)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_LOCATION_READ, "Erro ao consultar locations");
			return (PaginaLocations) SdkUtils.Deserialize(typeof(PaginaLocations), json);
		}

		public string Addfilters(FiltroConsultarLocations filtro) {
			if (filtro == null ) {
				return "";
			}
			StringBuilder filter = new StringBuilder();
			if (filtro.TxIdPresente.HasValue) {
				filter.Append("&txIdPresente").Append("=").Append(filtro.TxIdPresente.Value.ToString().ToLowerInvariant());
			}
			if (filtro.TipoCob != null ) {
				filter.Append("&tipoCob").Append("=").Append(filtro.TipoCob.ToString());
			}
			return filter.ToString();
		}
	}
}
