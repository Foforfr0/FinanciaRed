using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.MonthlyEfficiencies;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Efficiencies {
        public static async Task<MessageResponse<DTO_Efficiencies>> GetEfficiencies (DTO_Efficiencies solicitedEfficiencies) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_Efficiencies> retrievedEfficiencies = null;
                DTO_Efficiencies dataRetrieved = new DTO_Efficiencies ();
                try {
                    // Aprobación de créditos
                    dataRetrieved.DateStart = solicitedEfficiencies.DateStart;
                    dataRetrieved.DateEnd = solicitedEfficiencies.DateEnd;
                    dataRetrieved.CreditsApplicatedNumb = await context.CreditApplications.
                        Where (ca =>
                            ca.DateApplication >= solicitedEfficiencies.DateStart &&
                            ca.DateApplication <= solicitedEfficiencies.DateEnd).
                        CountAsync ();
                    if (dataRetrieved.CreditsApplicatedNumb == 0) {
                        dataRetrieved.AllZero ();
                    } else {
                        dataRetrieved.CreditsApplicatedPercent = 100;
                        dataRetrieved.CreditsAwaitingNumb = await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Where (ca => ca.StatusesCreditApplication.IdStatusCreditApplication == 1).
                            CountAsync ();
                        dataRetrieved.CreditsAwaitingPercent = (dataRetrieved.CreditsAwaitingNumb * 100) / dataRetrieved.CreditsApplicatedNumb;
                        dataRetrieved.CreditsAcceptedNumb = await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Where (ca => ca.StatusesCreditApplication.IdStatusCreditApplication == 2).
                            CountAsync ();
                        dataRetrieved.CreditsAcceptedPercent = (dataRetrieved.CreditsAcceptedNumb * 100) / dataRetrieved.CreditsApplicatedNumb;
                        dataRetrieved.CreditsDeclinedNumb = await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Where (ca => ca.StatusesCreditApplication.IdStatusCreditApplication == 3).
                            CountAsync ();
                        dataRetrieved.CreditsDeclinedPercent = (dataRetrieved.CreditsDeclinedNumb * 100) / dataRetrieved.CreditsApplicatedNumb;

                        //Montos otorgados
                        dataRetrieved.TotalAmountCredits = await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Select (ca => ca.AmountTotal).
                            DefaultIfEmpty (0).
                            SumAsync ();
                        dataRetrieved.AverageAmountCredit = (float)(await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Select (ca => ca.AmountTotal).
                            DefaultIfEmpty (0).
                            AverageAsync ());
                        dataRetrieved.MaximumAmountCredit = await context.CreditApplications.
                            Where (ca =>
                                ca.DateApplication >= solicitedEfficiencies.DateStart &&
                                ca.DateApplication <= solicitedEfficiencies.DateEnd).
                            Select (ca => ca.AmountTotal).
                            DefaultIfEmpty (0).
                            MaxAsync ();
                    }

                    retrievedEfficiencies = MessageResponse<DTO_Efficiencies>.Success ("Efficiencies retrieved.", dataRetrieved);
                } catch (Exception) {
                    retrievedEfficiencies =
                        MessageResponse<DTO_Efficiencies>.
                        Failure ("Error al obtener las eficiencias soliitadas");
                }

                return retrievedEfficiencies;
            }
        }
    }
}
