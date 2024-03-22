using System.Runtime.Serialization;
using System.Text;

namespace Sdk.BankingApi {
	[DataContract]
	public class FiltroConsultarExtratoEnriquecido {
		[DataMember(Name="tipoOperacao", EmitDefaultValue = false)]
		public string TipoOperacao;
		[DataMember(Name="tipoTransacao", EmitDefaultValue = false)]
		public string TipoTransacao;

		public static FiltroConsultarExtratoEnriquecido Builder() {
			return new FiltroConsultarExtratoEnriquecido();
		}

		public FiltroConsultarExtratoEnriquecido Build() {
			return this;
		}

		public FiltroConsultarExtratoEnriquecido SetTipoOperacao(string tipoOperacao) {
			this.TipoOperacao = tipoOperacao;
			return this;
		}

		public FiltroConsultarExtratoEnriquecido SetTipoTransacao(string tipoTransacao) {
			this.TipoTransacao = tipoTransacao;
			return this;
		}

		public string ObterFiltroQuery() {
            StringBuilder filter = new StringBuilder();

            if (TipoOperacao != null)
            {
                filter.Append("&tipoOperacao=").Append(TipoOperacao);
            }

            if (TipoTransacao != null)
            {
                filter.Append("&tipoTransacao=").Append(TipoTransacao);
            }

            return filter.ToString();
		}
	}
}
