using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditApplication;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_CreditPolicy {
        public static async Task<MessageResponse<List<DTO_CreditPolicy_Consult>>> GetAsync () {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditPolicy_Consult>> response = null;
                try {
                    List<DTO_CreditPolicy_Consult> dataRetrieved = (await context.Policies.
                        Select (policy => new {
                            policy.IdPolicy,
                            policy.Name,
                            policy.Description,
                            policy.DateStart,
                            policy.DateEnd
                        }).
                        ToListAsync ()).
                        Select (policy => new DTO_CreditPolicy_Consult {
                            IdCreditPolicy = policy.IdPolicy,
                            Name = policy.Name,
                            Description = policy.Description,
                            DateStart = policy.DateStart,
                            DateEnd = policy.DateEnd,
                            DateEndS = policy.DateEnd.HasValue
                                ? policy.DateEnd.Value.ToString ("dd/MM/yyyy")
                                : "Sin vigencia"
                        }).
                        ToList ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Success (
                            dataRetrieved.Count + " Políticas de crédito obtenidas.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ("No se logró obtener la lista de Políticas de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_CreditPolicy_Consult>>> GetAsync (string keyWord) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditPolicy_Consult>> response = null;
                try {
                    List<DTO_CreditPolicy_Consult> dataRetrieved = await context.Policies.
                        Where (pol =>
                            pol.Name.Contains (keyWord) ||
                            pol.Description.Contains (keyWord)
                        ).
                        Select (pol => new DTO_CreditPolicy_Consult {
                            IdCreditPolicy = pol.IdPolicy,
                            Name = pol.Name,
                            Description = pol.Description,
                            DateStart = pol.DateStart,
                            DateEnd = pol.DateEnd
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Success (
                            dataRetrieved.Count + " políticas de crédito obtenidas.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ("No se logró obtener la lista de políticas de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<string>> GetStatusAsync (int idCreditPolicy) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<string> response = null;
                try {
                    string dataRetrieved = await context.Policies.
                        Where (empl => empl.IdPolicy == idCreditPolicy).
                        Select (clnt => clnt.IsActive ? "Activo" : "Inactivo").
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        response = MessageResponse<string>.Failure ($"Política de crédito ID {idCreditPolicy} sin estado.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<string>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutStatusAsync (int idCreditPolicy, bool isActive) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Policies currentCreditPolicy = context.Policies.Find (idCreditPolicy);

                    if (currentCreditPolicy != null) {
                        context.Policies.Attach (currentCreditPolicy);

                        currentCreditPolicy.IsActive = isActive;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentCreditPolicy).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Policies databaseEntity = (Policies)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Política de crédito ID {currentCreditPolicy.IdPolicy} actualizado", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener la Política de crédito ID {currentCreditPolicy.IdPolicy}.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PostAsync (DTO_CreditPolicy_Consult newCreditPolicy) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Policies createdCreditPolicy = new Policies {
                        Name = newCreditPolicy.Name,
                        Description = newCreditPolicy.Description,
                        DateStart = newCreditPolicy.DateStart,
                        DateEnd = newCreditPolicy.DateEnd,
                        IsActive = true
                    };

                    context.Policies.Add (createdCreditPolicy);
                    await context.SaveChangesAsync ();

                    response = MessageResponse<bool>.Success (
                        $"Política de crédito creado.", true);
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Errror inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_CreditApplication_CreditPolicies>>> GetPolicies_ApplitacionAsync (int idCreditApplication) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditApplication_CreditPolicies>> response = null;
                try {
                    List<DTO_CreditApplication_CreditPolicies> dataRetrieved = await context.CreditApplications_Policies.
                        Where (cap => cap.CreditApplications.IdCreditApplication == idCreditApplication).
                        Select (credpolicy => new DTO_CreditApplication_CreditPolicies {
                            IdCreditPolicy = credpolicy.IdPolicy,
                            NameCreditPolicy = credpolicy.Policies.Name,
                            IdCreditApplication = credpolicy.IdCreditApplication,
                            IsApplied = credpolicy.IsAprobed
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Success (
                            dataRetrieved.Count + " politicas de crédito obtenidas.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Failure ("No se logró obtener la lista de políticas de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Failure (ex.ToString ());
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_CreditPolicy_Consult>> GetDetailsAsync (int idPolicy) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_CreditPolicy_Consult> response = null;
                try {
                    DTO_CreditPolicy_Consult dataRetrieved = await context.Policies.
                        Where (pol => pol.IdPolicy == idPolicy).
                        Select (policy => new DTO_CreditPolicy_Consult {
                            IdCreditPolicy = policy.IdPolicy,
                            Name = policy.Name,
                            Description = policy.Description,
                            DateStart = policy.DateStart,
                            DateEnd = policy.DateEnd
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<DTO_CreditPolicy_Consult>.Success (
                            $"Política de crédito {idPolicy} obtenido.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<DTO_CreditPolicy_Consult>.Failure ("No se logró obtener la política de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_CreditPolicy_Consult>.Failure (ex.ToString ());
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutAsync (DTO_CreditPolicy_Consult modifyPolicy) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Policies currentCreditPolicy = context.Policies.Find (modifyPolicy.IdCreditPolicy);

                    if (currentCreditPolicy != null) {
                        context.Policies.Attach (currentCreditPolicy);

                        currentCreditPolicy.Name = modifyPolicy.Name;
                        currentCreditPolicy.Description = modifyPolicy.Description;
                        currentCreditPolicy.DateStart = modifyPolicy.DateStart;
                        currentCreditPolicy.DateEnd = modifyPolicy.DateEnd;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentCreditPolicy).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Policies databaseEntity = (Policies)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Política de crédito ID {currentCreditPolicy.IdPolicy} actualizado.", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener la Política de crédito ID {currentCreditPolicy.IdPolicy}.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }
    }
}