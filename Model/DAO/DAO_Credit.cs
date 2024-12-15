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
                            AmountTotal = crdt.CreditApplications.AmountTotal,
                            AmountTotalS = "$ " + (crdt.CreditApplications.AmountTotal).ToString (),
                            AmountLeft = crdt.AmountLeft,
                            AmountLeftS = "$ " + (crdt.AmountLeft).ToString (),
                            DateStart = crdt.DateStart,
                            DateEnd = crdt.DateEnd,
                            InterestRate = crdt.CreditApplications.Promotions.InterestRate,
                            InterestPercentaje = (crdt.CreditApplications.Promotions.InterestRate * 100).ToString () + " %",
                            NumberFortnigths = crdt.CreditApplications.Promotions.NumberFortnights
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
                    // Filtra los créditos relacionados con el cliente por su `IdPromotion`
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

        public static async Task<MessageResponse<bool>> CreateCredit (DTO_CreditApplication_Details newCredit) {
            /*
             * Se crea un Credito pero sin inciar, hace falta el documento firmado por el cliente
             */
            MessageResponse<bool> responseCreateCredit = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Credits createdCredit = new Credits {
                        AmountLeft = newCredit.AmountSolicited,
                        IdStatusCredit = 1,
                        SignedDocument = null,
                        PaymentLayout = null,
                        DateStart = null,
                        DateEnd = null,
                        IdCreditApplication = newCredit.IdCreditApplication,
                    };

                    context.Credits.Add (createdCredit);
                    await context.SaveChangesAsync ();

                    responseCreateCredit = MessageResponse<bool>.Success (
                        $"Credit created.", true);
                } catch (Exception ex) {
                    responseCreateCredit = MessageResponse<bool>.Failure ("Exception: " + ex.Message);
                }
            }
            return responseCreateCredit;
        }
    }
}
