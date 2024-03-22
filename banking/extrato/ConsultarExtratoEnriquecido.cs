using System.Text;
using System.Collections.Generic;

namespace Sdk.BankingApi {
	public class ConsultarExtratoEnriquecido {
		public ExtratoEnriquecido Consultar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarExtratoEnriquecido filtro) {
			InterSdk.LogInfo("ConsultarExtratoEnriquecido {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro);
		}

		public List<TransacaoEnriquecida> Consultar(Config config, string dataInicial, string dataFinal, FiltroConsultarExtratoEnriquecido filtro) {
			InterSdk.LogInfo("ConsultarExtratoEnriquecido {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			ExtratoEnriquecido paginaTransacoes;
			List<TransacaoEnriquecida> transacoes = new List<TransacaoEnriquecida>();
			do {
				paginaTransacoes = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro);
				transacoes.AddRange(paginaTransacoes.Transacoes);
				pagina++;
			} while (pagina < paginaTransacoes.TotalPaginas);
			return transacoes;
		}

		public ExtratoEnriquecido GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarExtratoEnriquecido filtro) {
			string url = $"{Constants.URL_BANKING_EXTRATO_ENRIQUECIDO.Replace("AMBIENTE", config.Ambiente)}?dataInicio={dataInicial}&dataFim={dataFinal}&pagina={pagina}{(tamanhoPagina == 0 ? "" : "&tamanhoPagina=" + tamanhoPagina)}{Addfilters(filtro)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_EXTRATO_READ, "Erro ao consultar extrato enriquecido");
			return (ExtratoEnriquecido) SdkUtils.Deserialize(typeof(ExtratoEnriquecido), json);
		}

		public string Addfilters(FiltroConsultarExtratoEnriquecido filtro) {
			if (filtro == null ) {
				return "";
			}
			return filtro.ObterFiltroQuery();
		}

	}
}
