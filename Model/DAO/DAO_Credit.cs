using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Credit {
        public static async Task<MessageResponse<List<DTO_Credit_Consult>>> GetAllCredits () {
            MessageResponse<List<DTO_Credit_Consult>> responseConsultClients = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Credit_Consult> dataRetrieved = await
                        context.Credits.
                        Select (crdt => new DTO_Credit_Consult {
                            IdCredit = crdt.IdCredit,
                            Amount = crdt.Amount,
                            AmountLeft = crdt.AmountLeft,
                            IdClient = crdt.IdClient,
                            IdStateCredit = crdt.IdStateCredit,
                            //SignedDocument = (byte[]) crdt.SignedDocument
                            InterestRate = crdt.InterestRate,
                            StartDate = crdt.StartDate,
                            EndDate = crdt.EndDate,
                            IdEmployee = crdt.IdEmployee
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Success (
                            dataRetrieved.Count + " clients retrieved",
                            dataRetrieved);
                    } else {
                        responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Failure ("Wrong credentials");
                    }
                } catch (Exception ex) {
                    responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultClients;
        }
    }
}
