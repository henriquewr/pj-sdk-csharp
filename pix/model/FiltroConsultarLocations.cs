using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Sdk.PixApi {
	[DataContract]
	public class FiltroConsultarLocations {
		[DataMember(Name="txIdPresente", EmitDefaultValue = false)]
		public bool? TxIdPresente;
		[DataMember(Name="tipoCob", EmitDefaultValue = false)]
		public string TipoCob;

		public static FiltroConsultarLocations Builder() {
			return new FiltroConsultarLocations();
		}

		public FiltroConsultarLocations Build() {
			return this;
		}

		public FiltroConsultarLocations SetTxIdPresente(bool? txIdPresente) {
			this.TxIdPresente = txIdPresente;
			return this;
		}

		public FiltroConsultarLocations SetTipoCob(string tipoCob) {
			this.TipoCob = tipoCob;
			return this;
		}
	}
}
