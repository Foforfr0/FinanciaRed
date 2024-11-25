﻿using FinanciaRed.Model.DTO;
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

        public static async Task<MessageResponse<List<DTO_CreditPolicy_Consult>>> GetFilteredCreditPolicies (string keyWord) {
            MessageResponse<List<DTO_CreditPolicy_Consult>> responseFilteredCreditPolicies = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
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
                            DateEnd = pol.DateEnd ?? pol.DateStart
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseFilteredCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Success (
                            dataRetrieved.Count + " credit policies retrieved.",
                            dataRetrieved);
                    } else {
                        responseFilteredCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure ("Cannot retrieved filtered credit policies.");
                    }
                } catch (Exception ex) {
                    responseFilteredCreditPolicies = MessageResponse<List<DTO_CreditPolicy_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseFilteredCreditPolicies;
        }

        public static async Task<MessageResponse<string>> GetStatusCreditPolicy (int idCreditPolicy) {
            MessageResponse<string> responseStatus = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await context.Policies.
                        Where (empl => empl.IdPolicy == idCreditPolicy).
                        Select (clnt => clnt.IsActive ? "Activo" : "Inactivo").
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseStatus = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        responseStatus = MessageResponse<string>.Failure ($"Credit credpolicy ID {idCreditPolicy} without status.");
                    }
                } catch (Exception ex) {
                    responseStatus = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseStatus;
        }

        public static async Task<MessageResponse<bool>> ChangeStatusCreditPolicy (int idCreditPolicy, bool isActive) {
            MessageResponse<bool> responseUpdateStatusCreditPolicy = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
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

                        responseUpdateStatusCreditPolicy = MessageResponse<bool>.Success (
                            $"Credit credpolicy ID {currentCreditPolicy.IdPolicy} updated", true);
                    } else {
                        responseUpdateStatusCreditPolicy = MessageResponse<bool>.Failure ($"Credit credpolicy ID {currentCreditPolicy.IdPolicy} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateStatusCreditPolicy = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateStatusCreditPolicy;
        }

        public static async Task<MessageResponse<bool>> RegistryNewCreditPolicy (DTO_CreditPolicy_Consult newCreditPolicy) {
            MessageResponse<bool> responseCreateCreditPolicy = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Policies createdCreditPolicy = new Policies {
                        Name = newCreditPolicy.Name,
                        Description = newCreditPolicy.Description,
                        DateStart = newCreditPolicy.DateStart,
                        DateEnd = newCreditPolicy.DateEnd
                    };

                    context.Policies.Add (createdCreditPolicy);
                    await context.SaveChangesAsync ();

                    responseCreateCreditPolicy = MessageResponse<bool>.Success (
                        $"Credit credpolicy created.", true);
                } catch (Exception ex) {
                    responseCreateCreditPolicy = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseCreateCreditPolicy;
        }

        public static async Task<MessageResponse<List<DTO_CreditApplication_CreditPolicies>>> GetCreditPolicies_CreditApplitacion (int idCreditApplication) {
            MessageResponse<List<DTO_CreditApplication_CreditPolicies>> responseConsultCreditPolicies = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditApplication_CreditPolicies> dataRetrieved = await
                        context.CreditApplications_Policies.
                        Where (cap => cap.CreditApplications.IdCreditApplication == idCreditApplication).
                        Select (credpolicy => new DTO_CreditApplication_CreditPolicies {
                            IdCreditPolicy = credpolicy.IdPolicy,
                            NameCreditPolicy = credpolicy.Policies.Name,
                            IdCreditApplication = credpolicy.IdCreditApplication,
                            IsApplied = credpolicy.IsAprobed
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditPolicies = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Success (
                            dataRetrieved.Count + " credits policies retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCreditPolicies = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Failure ("Cannot retrieve credit policies.");
                    }
                } catch (Exception ex) {
                    responseConsultCreditPolicies = MessageResponse<List<DTO_CreditApplication_CreditPolicies>>.Failure (ex.ToString ());
                }
            }
            return responseConsultCreditPolicies;
        }

        public static async Task<MessageResponse<DTO_CreditPolicy_Consult>> GetDetailsCreditPolicy (int idPolicy) {
            MessageResponse<DTO_CreditPolicy_Consult> responseConsultCreditPolicies = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_CreditPolicy_Consult dataRetrieved = await
                        context.Policies.
                        Where (pol => pol.IdPolicy == idPolicy).
                        Select (policy => new DTO_CreditPolicy_Consult {
                            IdCreditPolicy = policy.IdPolicy,
                            Name = policy.Name,
                            Description = policy.Description,
                            DateStart = policy.DateStart,
                            DateEnd = policy.DateEnd ?? policy.DateStart
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditPolicies = MessageResponse<DTO_CreditPolicy_Consult>.Success (
                            $"Credit credpolicy ID {idPolicy} retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCreditPolicies = MessageResponse<DTO_CreditPolicy_Consult>.Failure ("Cannot retrieve credit credpolicy.");
                    }
                } catch (Exception ex) {
                    responseConsultCreditPolicies = MessageResponse<DTO_CreditPolicy_Consult>.Failure (ex.ToString ());
                }
            }
            return responseConsultCreditPolicies;
        }
    }
}