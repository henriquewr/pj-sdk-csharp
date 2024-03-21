using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Sdk.PixApi {
	[DataContract]
	public class FiltroConsultarCobrancasImediatas {
		[DataMember(Name="cpf", EmitDefaultValue = false)]
		public string Cpf;
		[DataMember(Name="cnpj", EmitDefaultValue = false)]
		public string Cnpj;
		[DataMember(Name="locationPresente", EmitDefaultValue = false)]
		public bool? LocationPresente;
		[DataMember(Name="status", EmitDefaultValue = false)]
		public string Status;

		public static FiltroConsultarCobrancasImediatas Builder() {
			return new FiltroConsultarCobrancasImediatas();
		}

		public FiltroConsultarCobrancasImediatas Build() {
			return this;
		}

		public FiltroConsultarCobrancasImediatas SetCpf(string cpf) {
			this.Cpf = cpf;
			return this;
		}

		public FiltroConsultarCobrancasImediatas SetCnpj(string cnpj) {
			this.Cnpj = cnpj;
			return this;
		}

		public FiltroConsultarCobrancasImediatas SetLocationPresente(bool? locationPresente) {
			this.LocationPresente = locationPresente;
			return this;
		}

		public FiltroConsultarCobrancasImediatas SetStatus(string status) {
			this.Status = status;
			return this;
		}
	}
}
