using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_CreditPolicies {
        public static async Task<MessageResponse<List<DTO_CreditPolicy_Consult>>> GetAllCreditPolicies () {
            MessageResponse<List<DTO_CreditPolicy_Consult>> responseConsultCreditPolicies = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditPolicy_Consult> dataRetrieved = await
                        context.Policies.
                        Select (policy => new DTO_CreditPolicy_Consult {
                            IdCreditPolicy = policy.IdPolicy,
                            Name = policy.Name,
                            Description = policy.Description,
                            DateStart = policy.DateStart,
                            DateEnd = policy.DateEnd ?? policy.DateStart
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Success (
                            dataRetrieved.Count + " credits policies retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ("Cannot retrieve credit policies.");
                    }
                } catch (Exception ex) {
                    responseConsultCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultCreditPolicies;
        }
    }
}