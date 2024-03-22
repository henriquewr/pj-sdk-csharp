using System.Text;
using System.Collections.Generic;

namespace Sdk.PixApi {
	public class ConsultarCobrancasComVencimento {
		public PaginaCobrancasVencimento Consultar(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarCobrancasComVencimento filtro) {
			InterSdk.LogInfo("ConsultarCobrancasComVencimento {0} {1}-{2} pagina={3}", config.ClientId, dataInicial, dataFinal, pagina.ToString());
			return GetPage(config, dataInicial, dataFinal, pagina, tamanhoPagina, filtro);
		}

		public List<CobrancaVencimentoDetalhada> Consultar(Config config, string dataInicial, string dataFinal, FiltroConsultarCobrancasComVencimento filtro) {
			InterSdk.LogInfo("ConsultarCobrancasComVencimento {0} {1}-{2}", config.ClientId, dataInicial, dataFinal);
			int pagina = 0;
			PaginaCobrancasVencimento paginaCobrancas;
			List<CobrancaVencimentoDetalhada> cobrancas = new List<CobrancaVencimentoDetalhada>();
			do {
				paginaCobrancas = GetPage(config, dataInicial, dataFinal, pagina, 0, filtro);
				cobrancas.AddRange(paginaCobrancas.Cobrancas);
				pagina++;
			} while (pagina < paginaCobrancas.Parametros.Paginacao.QuantidadeDePaginas);
			return cobrancas;
		}

		public PaginaCobrancasVencimento GetPage(Config config, string dataInicial, string dataFinal, int pagina, int tamanhoPagina, FiltroConsultarCobrancasComVencimento filtro) {
			string url = $"{Constants.URL_PIX_COBRANCA_COM_VENCIMENTO.Replace("AMBIENTE", config.Ambiente)}?inicio={dataInicial}&fim={dataFinal}&paginacao.paginaAtual={pagina}{(tamanhoPagina == 0 ? "" : "&paginacao.itensPorPagina=" + tamanhoPagina)}{Addfilters(filtro)}";
			string json = HttpUtils.CallGet(config, url, Constants.ESCOPO_PIX_COBRANCA_VENCIMENTO_READ, "Erro ao consultar cobranças imediatas");
			return (PaginaCobrancasVencimento) SdkUtils.Deserialize(typeof(PaginaCobrancasVencimento), json);
		}

		public string Addfilters(FiltroConsultarCobrancasComVencimento filtro) {
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
