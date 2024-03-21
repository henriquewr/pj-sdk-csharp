using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Sdk.PixApi {
	[DataContract]
	public class FiltroConsultarPixRecebidos {
		[DataMember(Name="txId", EmitDefaultValue = false)]
		public string TxId;
		[DataMember(Name="txIdPresente", EmitDefaultValue = false)]
		public bool? TxIdPresente;
		[DataMember(Name="devolucaoPresente", EmitDefaultValue = false)]
		public bool? DevolucaoPresente;
		[DataMember(Name="cpf", EmitDefaultValue = false)]
		public string Cpf;
		[DataMember(Name="cnpj", EmitDefaultValue = false)]
		public string Cnpj;

		public static FiltroConsultarPixRecebidos Builder() {
			return new FiltroConsultarPixRecebidos();
		}

		public FiltroConsultarPixRecebidos Build() {
			return this;
		}

		public FiltroConsultarPixRecebidos SetTxId(string txId) {
			this.TxId = txId;
			return this;
		}

		public FiltroConsultarPixRecebidos SetTxIdPresente(bool? txIdPresente) {
			this.TxIdPresente = txIdPresente;
			return this;
		}

		public FiltroConsultarPixRecebidos SetDevolucaoPresente(bool? devolucaoPresente) {
			this.DevolucaoPresente = devolucaoPresente;
			return this;
		}

		public FiltroConsultarPixRecebidos SetCpf(string cpf) {
			this.Cpf = cpf;
			return this;
		}

		public FiltroConsultarPixRecebidos SetCnpj(string cnpj) {
			this.Cnpj = cnpj;
			return this;
		}
	}
}
