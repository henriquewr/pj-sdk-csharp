using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Sdk.PixApi {
	[DataContract]
	public class FiltroConsultarCobrancasComVencimento {
		[DataMember(Name="cpf", EmitDefaultValue = false)]
		public string Cpf;
		[DataMember(Name="cnpj", EmitDefaultValue = false)]
		public string Cnpj;
		[DataMember(Name="locationPresente", EmitDefaultValue = false)]
		public bool? LocationPresente;
		[DataMember(Name="status", EmitDefaultValue = false)]
		public string Status;

		public static FiltroConsultarCobrancasComVencimento Builder() {
			return new FiltroConsultarCobrancasComVencimento();
		}

		public FiltroConsultarCobrancasComVencimento Build() {
			return this;
		}

		public FiltroConsultarCobrancasComVencimento SetCpf(string cpf) {
			this.Cpf = cpf;
			return this;
		}

		public FiltroConsultarCobrancasComVencimento SetCnpj(string cnpj) {
			this.Cnpj = cnpj;
			return this;
		}

		public FiltroConsultarCobrancasComVencimento SetLocationPresente(bool? locationPresente) {
			this.LocationPresente = locationPresente;
			return this;
		}

		public FiltroConsultarCobrancasComVencimento SetStatus(string status) {
			this.Status = status;
			return this;
		}
	}
}
