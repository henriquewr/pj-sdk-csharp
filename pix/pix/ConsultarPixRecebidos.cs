using System.Text;
using System.Collections.Generic;

namespace Sdk.PixApi {
	public class ConsultarPixRecebidos {
		public PaginaPix Consultar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarPixRecebidos filtro) {
			InterSdk.LogInfo("ConsultarPixRecebidos {0} {1}-{2} pagina={3}", config.ClientId, dataInicial, dataFinal, pagina.ToString());
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro);
		}

		public List<Pix> Consultar(Config config, string dataInicial, string dataFinal, FiltroConsultarPixRecebidos filtro) {
			InterSdk.LogInfo("ConsultarPixRecebidos {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			PaginaPix paginaPix;
			List<Pix> listaPix = new List<Pix>();
			do {
				paginaPix = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro);
				listaPix.AddRange(paginaPix.ListaPix);
				pagina++;
			} while (pagina < paginaPix.Parametros.Paginacao.QuantidadeDePaginas);
			return listaPix;
		}

		public PaginaPix GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarPixRecebidos filtro) {
			string url = $"{Constants.URL_PIX_PIX.Replace("AMBIENTE", config.Ambiente)}?inicio={dataInicial}&fim={dataFinal}&paginacao.paginaAtual={pagina}{(tamanhoPagina == 0 ? "" : "&paginacao.itensPorPagina=" + tamanhoPagina)}{Addfilters(filtro)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_PIX_READ, "Erro ao consultar pix recebidos");
			return (PaginaPix) SdkUtils.Deserialize(typeof(PaginaPix), json);
		}

		public string Addfilters(FiltroConsultarPixRecebidos filtro) {
			if (filtro == null ) {
				return "";
			}
			StringBuilder filter = new StringBuilder();
			if (filtro.TxId != null ) {
				filter.Append("&txId").Append("=").Append(filtro.TxId);
			}
			if (filtro.TxIdPresente.HasValue) {
				filter.Append("&txIdPresente").Append("=").Append(filtro.TxIdPresente.Value.ToString().ToLowerInvariant());
			}
			if (filtro.DevolucaoPresente.HasValue) {
				filter.Append("&devolucaoPresente").Append("=").Append(filtro.DevolucaoPresente.Value.ToString().ToLowerInvariant());
			}
			if (filtro.Cpf != null ) {
				filter.Append("&cpf").Append("=").Append(filtro.Cpf);
			}
			if (filtro.Cnpj != null ) {
				filter.Append("&cnpj").Append("=").Append(filtro.Cnpj);
			}

			return filter.ToString();
		}

	}
}
