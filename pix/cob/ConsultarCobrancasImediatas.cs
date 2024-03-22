using System.Text;
using System.Collections.Generic;

namespace Sdk.PixApi {
	public class ConsultarCobrancasImediatas {
		public PaginaCobrancas Consultar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarCobrancasImediatas filtro) {
			InterSdk.LogInfo("ConsultarCobrancasImediatas {0} {1}-{2} pagina={3}", config.ClientId, dataInicial, dataFinal, pagina.ToString());
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro);
		}

		public List<CobrancaDetalhada> Consultar(Config config, string dataInicial, string dataFinal, FiltroConsultarCobrancasImediatas filtro) {
			InterSdk.LogInfo("ConsultarCobrancasImediatas {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			PaginaCobrancas paginaCobrancas;
			List<CobrancaDetalhada> cobrancas = new List<CobrancaDetalhada>();
			do {
				paginaCobrancas = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro);
				cobrancas.AddRange(paginaCobrancas.Cobrancas);
				pagina++;
			} while (pagina < paginaCobrancas.Parametros.Paginacao.QuantidadeDePaginas);
			return cobrancas;
		}

		public PaginaCobrancas GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarCobrancasImediatas filtro) {
			string url = $"{Constants.URL_PIX_COBRANCAS_IMEDIATAS.Replace("AMBIENTE", config.Ambiente)}?inicio={dataInicial}&fim={dataFinal}&paginacao.paginaAtual={pagina}{(tamanhoPagina == 0 ? "" : "&paginacao.itensPorPagina=" + tamanhoPagina)}{Addfilters(filtro)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_PIX_COBRANCA_READ, "Erro ao consultar cobranças imediatas");
			return (PaginaCobrancas) SdkUtils.Deserialize(typeof(PaginaCobrancas), json);
		}

		public string Addfilters(FiltroConsultarCobrancasImediatas filtro) {
			if (filtro == null ) {
				return "";
			}
			StringBuilder filter = new StringBuilder();
			if (filtro.Cpf != null ) {
				filter.Append("&cpf").Append("=").Append(filtro.Cpf);
			}
			if (filtro.Cnpj != null ) {
				filter.Append("&cnpj").Append("=").Append(filtro.Cnpj);
			}
			if (filtro.LocationPresente.HasValue) {
				filter.Append("&locationPresente").Append("=").Append(filtro.LocationPresente.Value.ToString().ToLowerInvariant());
			}
			if (filtro.Status != null ) {
				filter.Append("&status").Append("=").Append(filtro.Status.ToString());
			}
			return filter.ToString();
		}
	}
}
