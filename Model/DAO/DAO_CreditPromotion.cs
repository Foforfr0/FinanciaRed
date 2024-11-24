using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditPromotion;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_CreditPromotion {
        public static async Task<MessageResponse<List<DTO_CreditPromotion_Consult>>> GetAllCreditPromotions () {
            MessageResponse<List<DTO_CreditPromotion_Consult>> responseConsultCreditPromotions = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditPromotion_Consult> dataRetrieved = await context.Promotions.
                        Select (cp => new DTO_CreditPromotion_Consult {
                            IdCreditPromotion = cp.IdPromotion,
                            Name = cp.Name,
                            InterestRate = cp.InterestRate,
                            NumberFortNigths = cp.NumberFortnights,
                            DateStart = cp.DateStart,
                            DateEnd = cp.DateEnd
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Success (
                            dataRetrieved.Count + " credit promotions retrieved.",
                            dataRetrieved
                        );
                    } else {
                        responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ("Cannot retrieved credit promotions.");
                    }
                } catch (Exception e) {
                    responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ($"Exception: {e.Message}");
                }
            }
            return responseConsultCreditPromotions;
        }

        public static async Task<MessageResponse<List<DTO_CreditPromotion_Consult>>> GetFilteredCreditPromotions (string keyText) {
            MessageResponse<List<DTO_CreditPromotion_Consult>> responseConsultCreditPromotions = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditPromotion_Consult> dataRetrieved = await context.Promotions.
                        Select (cp => new DTO_CreditPromotion_Consult {
                            IdCreditPromotion = cp.IdPromotion,
                            Name = cp.Name,
                            InterestRate = cp.InterestRate,
                            NumberFortNigths = cp.NumberFortnights,
                            DateStart = cp.DateStart,
                            DateEnd = cp.DateEnd
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Success (
                            dataRetrieved.Count + " credit promotions retrieved.",
                            dataRetrieved
                        );
                    } else {
                        responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ("Cannot retrieved credit promotions.");
                    }
                } catch (Exception e) {
                    responseConsultCreditPromotions = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ($"Exception: {e.Message}");
                }
            }
            return responseConsultCreditPromotions;
        }

        public static async Task<MessageResponse<string>> GetStatusCreditPromotion (int idPromotion) {
            MessageResponse<string> responseStatus = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await context.Promotions.
                        Where (prom => prom.IdPromotion == idPromotion).
                        Select (prom => prom.IsActive ? "Activo" : "Inactivo").
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseStatus = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        responseStatus = MessageResponse<string>.Failure ($"Promotion ID {idPromotion} without status.");
                    }
                } catch (Exception ex) {
                    responseStatus = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseStatus;
        }

        public static async Task<MessageResponse<bool>> ChangeStatusCreditPromotion (int idPromotion, bool isActive) {
            MessageResponse<bool> responseUpdateStatusPromotion = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Promotions currentPromotion = context.Promotions.Find (idPromotion);

                    if (currentPromotion != null) {
                        context.Promotions.Attach (currentPromotion);

                        currentPromotion.IsActive = isActive;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentPromotion).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (var entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        var proposedValues = entry.CurrentValues;
                                        var databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            var databaseEntity = (Clients)databaseValues.ToObject ();
                                            // Actualiza los valores originales con los valores actuales de la base de datos.
                                            entry.OriginalValues.SetValues (databaseValues);
                                            // Decide qué hacer con los valores propuestos.
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        responseUpdateStatusPromotion = MessageResponse<bool>.Success (
                            $"Promotion ID {currentPromotion.IdPromotion} updated", true);
                    } else {
                        responseUpdateStatusPromotion = MessageResponse<bool>.Failure ($"Promotion ID {currentPromotion.IdPromotion} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateStatusPromotion = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateStatusPromotion;
        }

        public static async Task<MessageResponse<DTO_CreditPromotion_Details>> GetDetailsCreditPromotion (int idPromotion) {
            MessageResponse<DTO_CreditPromotion_Details> responseDetailsPromotion = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_CreditPromotion_Details retrievedData = await
                        context.Promotions.
                        Where (prom => prom.IdPromotion == idPromotion).
                        Select (prom => new DTO_CreditPromotion_Details {
                            IdCreditPromotion = prom.IdPromotion,
                            Name = prom.Name,
                            NumberFortNigths = prom.NumberFortnights,
                            InterestRate = prom.InterestRate,
                            DateStart = prom.DateStart,
                            DateEnd = prom.DateEnd
                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedData != null) {
                        responseDetailsPromotion = MessageResponse<DTO_CreditPromotion_Details>.Success (
                            $"Details of ID {retrievedData.IdCreditPromotion} promotion nretrieved.",
                            retrievedData
                        );
                    } else {
                        responseDetailsPromotion = MessageResponse<DTO_CreditPromotion_Details>.Failure ("Details doesn't retrieved.");
                    }
                } catch (Exception ex) {
                    responseDetailsPromotion = MessageResponse<DTO_CreditPromotion_Details>.Failure (ex.ToString ());
                }
            }
            return responseDetailsPromotion;
        }
    }
}
