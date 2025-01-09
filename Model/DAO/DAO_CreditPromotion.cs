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
        public static async Task<MessageResponse<List<DTO_CreditPromotion_Consult>>> GetAsync () {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditPromotion_Consult>> response = null;
                try {
                    List<DTO_CreditPromotion_Consult> dataRetrieved = await context.Promotions.
                        Select (cp => new DTO_CreditPromotion_Consult {
                            IdCreditPromotion = cp.IdPromotion,
                            Name = cp.Name,
                            InterestRate = cp.InterestRate,
                            InterestPercentaje = ((cp.InterestRate * 100)).ToString () + " %",
                            NumberFortNigths = cp.NumberFortnights,
                            DateStart = cp.DateStart,
                            DateEnd = cp.DateEnd
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Success (
                            dataRetrieved.Count + " Promociones de crédito obtenidas.",
                            dataRetrieved
                        );
                    } else {
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ("No se logró obtener la lista de Promociones de crédito.");
                    }
                } catch (Exception e) {
                    response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ($"Error inesperado: {e.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_CreditPromotion_Consult>>> GetAsync (DateTime dateNow) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditPromotion_Consult>> response = null;
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
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Success (
                            dataRetrieved.Count + " Promociones de crédito obtenidas.",
                            dataRetrieved
                        );
                    } else {
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ("No se logró obtener la lista de promociones de crédito.");
                    }
                } catch (Exception e) {
                    response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ($"Error inesperado: {e.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_CreditPromotion_Consult>>> GetAsync (string keyText) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditPromotion_Consult>> response = null;
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
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Success (
                            dataRetrieved.Count + " Promociones de crédito obtenidos.",
                            dataRetrieved
                        );
                    } else {
                        response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ("No se logró obtener la lista de Promociones de crédito.");
                    }
                } catch (Exception e) {
                    response = MessageResponse<List<DTO_CreditPromotion_Consult>>.Failure ($"Error inesperado: {e.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PostAsync (DTO_CreditPromotion_Details newProm) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Promotions createdPromotion = new Promotions {
                        Name = newProm.Name,
                        NumberFortnights = newProm.NumberFortNigths,
                        InterestRate = newProm.InterestRate,
                        DateStart = newProm.DateStart,
                        DateEnd = newProm.DateEnd,
                        IsActive = true
                    };

                    context.Promotions.Add (createdPromotion);
                    await context.SaveChangesAsync ();

                    response = MessageResponse<bool>.Success (
                        $"Promoción de crédito creada.", true);
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<string>> GetStatusAsync (int idPromotion) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<string> response = null;
                try {
                    string dataRetrieved = await context.Promotions.
                        Where (prom => prom.IdPromotion == idPromotion).
                        Select (prom => prom.IsActive ? "Activo" : "Inactivo").
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        response = MessageResponse<string>.Failure ($"Promoción de crédito ID {idPromotion} sin estado.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<string>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutAsync (int idPromotion, bool isActive) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
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
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Promotions databaseEntity = (Promotions)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Promoción de crédito ID {currentPromotion.IdPromotion} actualizado.", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener la Promoción de crédito ID {currentPromotion.IdPromotion}.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_CreditPromotion_Details>> GetDetailsAsync (int idPromotion) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_CreditPromotion_Details> response = null;
                try {
                    DTO_CreditPromotion_Details retrievedData = await context.Promotions.
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
                        response = MessageResponse<DTO_CreditPromotion_Details>.Success (
                            $"{retrievedData.IdCreditPromotion} Promoción de crédito obtenido.",
                            retrievedData
                        );
                    } else {
                        response = MessageResponse<DTO_CreditPromotion_Details>.Failure ("No se logró obtener la Promoción de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_CreditPromotion_Details>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutAsync (DTO_CreditPromotion_Details modifyData) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Promotions currentPromotion = context.Promotions.Find (modifyData.IdCreditPromotion);

                    if (currentPromotion != null) {
                        context.Promotions.Attach (currentPromotion);

                        currentPromotion.Name = modifyData.Name;
                        currentPromotion.DateStart = modifyData.DateStart;
                        currentPromotion.DateEnd = modifyData.DateEnd;
                        currentPromotion.InterestRate = modifyData.InterestRate;
                        currentPromotion.NumberFortnights = modifyData.NumberFortNigths;
                        currentPromotion.IsActive = modifyData.IsActive;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentPromotion).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Clients databaseEntity = (Clients)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Promoción de crédito ID {currentPromotion.IdPromotion} actualizado", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener la Promoción de crédito ID {currentPromotion.IdPromotion}.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }
    }
}
