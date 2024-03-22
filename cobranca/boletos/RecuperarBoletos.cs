using System.Collections.Generic;
using System.Text;

namespace Sdk.CobrancaApi {
	public class RecuperarBoletos {
		public PaginaBoletos Recuperar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroRecuperarBoletos filtro, Ordenacao ordenacao) {
			InterSdk.LogInfo("RecuperarBoletos {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro, ordenacao);
		}

		public List<BoletoDetalhado> Recuperar(Config config, string dataInicial, string dataFinal, FiltroRecuperarBoletos filtro, Ordenacao ordenacao) {
			InterSdk.LogInfo("RecuperarBoletos {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			PaginaBoletos paginaBoletos;
			List<BoletoDetalhado> boletos = new List<BoletoDetalhado>();
			do {
				paginaBoletos = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro, ordenacao);
				boletos.AddRange(paginaBoletos.Boletos);
				pagina++;
			} while (pagina < paginaBoletos.TotalPaginas);
			return boletos;
		}

		public PaginaBoletos GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroRecuperarBoletos filtro, Ordenacao ordenacao) {
			string url = $"{Constants.URL_COBRANCA_BOLETOS.Replace("AMBIENTE", config.Ambiente)}?dataInicial={dataInicial}&dataFinal={dataFinal}&paginaAtual={pagina}{(tamanhoPagina == 0 ? "" : "&itensPorPagina=" + tamanhoPagina)}{Addfilters(filtro)}{AddSort(ordenacao)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_BOLETO_COBRANCA_READ, "Erro ao recuperar boletos");
			return (PaginaBoletos) SdkUtils.Deserialize(typeof(PaginaBoletos), json);
		}

		public string Addfilters(FiltroRecuperarBoletos filtro) {
			if (filtro == null ) {
				return "";
			}
			StringBuilder filter = new StringBuilder();
			if (filtro.FiltrarDataPor != null ) {
				filter.Append("&filtrarDataPor").Append("=").Append(filtro.FiltrarDataPor.ToString());
			}
			if (filtro.Situacao != null ) {
				filter.Append("&situacao").Append("=").Append(filtro.Situacao.ToString());
			}
			if (filtro.Nome != null ) {
				filter.Append("&nome").Append("=").Append(filtro.Nome);
			}
			if (filtro.Email != null ) {
				filter.Append("&email").Append("=").Append(filtro.Email);
			}
			if (filtro.CpfCnpj != null ) {
				filter.Append("&cpfCnpj").Append("=").Append(filtro.CpfCnpj);
			}
			if (filtro.NossoNumero != null ) {
				filter.Append("&nossoNumero").Append("=").Append(filtro.NossoNumero);
			}
			return filter.ToString();
		}

		public string AddSort(Ordenacao ordenacao) {
			if (ordenacao == null ) {
				return "";
			}
			StringBuilder order = new StringBuilder();
			if (ordenacao.OrdenarPor != null ) {
				order.Append("&ordenarPor").Append("=").Append(ordenacao.OrdenarPor.ToString());
			}
			if (ordenacao.TipoOrdenacao != null ) {
				order.Append("&tipoOrdenacao").Append("=").Append(ordenacao.TipoOrdenacao.ToString());
			}
			return order.ToString();
		}

	}
}
