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
                            CodeRFC = crdt.CreditApplications.Clients.CodeRFC,
                            AmountTotal = crdt.CreditApplications.AmountTotal,
                            AmountLeft = crdt.AmountLeft,
                            StartDate = crdt.DateStart,
                            EndDate = crdt.DateEnd,
                            IdStatusCredit = crdt.StatusesCredit.IdStatusCredit,
                            StatusCredit = crdt.StatusesCredit.Status
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Success (
                            dataRetrieved.Count + " credits retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Failure ("Cannot retrieve credits.");
                    }
                } catch (Exception ex) {
                    responseConsultClients = MessageResponse<List<DTO_Credit_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultClients;
        }

        public static async Task<MessageResponse<bool>> GetCreditActiveClient (int idClient) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    int dataRetrieved = await context.Credits.
                        Where (cred => cred.CreditApplications.Clients.IdClient == idClient).
                        CountAsync ();

                    if (dataRetrieved > 0) {
                        return MessageResponse<bool>.Success ("Client has an active credit.", true);
                    } else {
                        return MessageResponse<bool>.Failure ("Client doesn't have an active credit.");
                    }
                } catch (Exception ex) {
                    return MessageResponse<bool>.Failure (ex.ToString ());
                }
            }
        }

        public static async Task<MessageResponse<int>> ChangeStatusCreditsClient (int idClient, int idNewStatus) {
            MessageResponse<int> responseChangeStatusCredits;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    // Filtra los créditos relacionados con el cliente por su `idClient`
                    List<Credits> relatedCredits = await context.Credits.
                        Where (credit => credit.CreditApplications.Clients.IdClient == idClient).
                        ToListAsync ();

                    // Actualiza cada crédito con el nuevo estado
                    foreach (Credits credit in relatedCredits) {
                        credit.IdStatusCredit = idNewStatus; // Cambia 'Status' por el campo que necesitas actualizar
                    }

                    // Guarda todos los cambios
                    await context.SaveChangesAsync ();

                    responseChangeStatusCredits = MessageResponse<int>.Success (
                                            $"{relatedCredits.Count} have changed status.", relatedCredits.Count);
                } catch (Exception ex) {
                    responseChangeStatusCredits = MessageResponse<int>.Failure ("Exception" + ex.Message);
                }
            }
            return responseChangeStatusCredits;
        }
    }
}
