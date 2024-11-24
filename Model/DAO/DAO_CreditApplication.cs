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
    internal class DAO_CreditApplication {
        public static async Task<MessageResponse<List<DTO_CreditApplication_Consult>>> GetAllCreditApplications () {
            MessageResponse<List<DTO_CreditApplication_Consult>> responseConsultCreditApplications = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditApplication_Consult> dataRetrieved = await
                        context.CreditApplications.
                        Select (ca => new DTO_CreditApplication_Consult {
                            IdCreditApplication = ca.IdCreditApplication,
                            AmountTotal = ca.AmountTotal,
                            InteresRate = ca.InteresRate ?? 0,
                            NumberFortNights = ca.NumberFortnights ?? 0,
                            DateRequest = ca.DateApplication,
                            IdStatus = ca.IdStatusCreditApplication,
                            Status = ca.StatusesCreditApplication.Status,
                            ClientName = ca.Clients.FirstName + " " + ca.Clients.MiddleName
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Success (
                            dataRetrieved.Count + " credits applications retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ("Cannot retrieve credit applications.");
                    }
                } catch (Exception ex) {
                    responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultCreditApplications;
        }

        public static async Task<MessageResponse<List<DTO_CreditApplication_Consult>>> GetFilteredCreditApplications (string keyWord) {
            MessageResponse<List<DTO_CreditApplication_Consult>> responseConsultCreditApplications = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CreditApplication_Consult> dataRetrieved = await
                        context.CreditApplications.
                        Where (ca =>
                            string.Concat (ca.AmountTotal).Contains (keyWord) ||
                            string.Concat (ca.DateAcepted).Contains (keyWord) ||
                            ca.StatusesCreditApplication.Status.Contains (keyWord)).
                        Select (ca => new DTO_CreditApplication_Consult {
                            IdCreditApplication = ca.IdCreditApplication,
                            AmountTotal = ca.AmountTotal,
                            InteresRate = ca.InteresRate ?? 0,
                            NumberFortNights = ca.NumberFortnights ?? 0,
                            DateRequest = ca.DateApplication,
                            IdStatus = ca.IdStatusCreditApplication,
                            Status = ca.StatusesCreditApplication.Status,
                            ClientName = ca.Clients.FirstName + " " + ca.Clients.MiddleName
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Success (
                            dataRetrieved.Count + " credits applications retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ("Cannot retrieve credit applications.");
                    }
                } catch (Exception ex) {
                    responseConsultCreditApplications = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultCreditApplications;
        }

        public static async Task<MessageResponse<DTO_CreditApplication_Details>> GetDetailsCreditApplication (int idCreditApplication) {
            MessageResponse<DTO_CreditApplication_Details> responseDetailsCreditApplications = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_CreditApplication_Details dataRetrieved = await context.CreditApplications.
                        Where (ca => ca.IdCreditApplication == idCreditApplication).
                        Select (ca => new DTO_CreditApplication_Details {
                            IdCreditApplication = ca.IdCreditApplication,
                            NameAdviser = ca.Employees.FirstName + " " + ca.Employees.MiddleName + " " + ca.Employees.LastName,
                            AmountSolicited = ca.AmountTotal,
                            AmountWithInteres = (int)(ca.AmountTotal + ca.AmountTotal * ca.InteresRate),
                            InterestRate = ca.InteresRate ?? 0,
                            DateSolicited = ca.DateApplication,
                            NumberFortNights = ca.NumberFortnights ?? 0,
                            ClientFirstName = ca.Clients.FirstName,
                            ClientMiddleName = ca.Clients.MiddleName,
                            ClientLastName = ca.Clients.LastName,
                            ClientDateBirth = ca.Clients.DateBirth,
                            CodeCURP = ca.Clients.CodeCURP,
                            CodeRFC= ca.Clients.CodeRFC,
                            ClientGender = ca.Clients.Gender.Equals ("M") ? "Masculino" : "Femenino",
                            ClientAddress = "Calle " + ca.Clients.ClientsAddresses.Street + ", " +
                                            "Interior " + ca.Clients.ClientsAddresses.InteriorNumber + ", " +
                                            "Exterior " + ca.Clients.ClientsAddresses.ExteriorNumber + ", " +
                                            "Colonia " + ca.Clients.ClientsAddresses.Colony + ", " +
                                            "Código postal " + ca.Clients.ClientsAddresses.PostalCode + ", " +
                                            "Tipo de vivienda " + ca.Clients.ClientsAddresses.AddressesTypes.Type + ", " +
                                            "Municipio " + ca.Clients.ClientsAddresses.Municipality + ", " +
                                            "Estado " + ca.Clients.ClientsAddresses.StatesAddresses.Name + " ",
                            ClientEmail1 = ca.Clients.Email1,
                            ClientEmail2 = ca.Clients.Email2 ?? "Sin correo alterno",
                            ClientPhoneNumber1 = ca.Clients.PhoneNumber1,
                            ClientPhoneNumber2 = ca.Clients.PhoneNumber2 ?? "Sin teléfono alterno",
                            ClientWorkType = ca.Clients.WorkClients.WorkTypes.Type,
                            ClientWork = ca.Clients.WorkClients.WorkArea,
                            ClientMonthlySalary = ca.Clients.WorkClients.MonthlySalary,
                            ClientReference1 = new DTO_ClientReference {
                                FirstName = ca.Clients.ContactsReferencesClients.FirstName,
                                MiddleName = ca.Clients.ContactsReferencesClients.MiddleName,
                                LastName = ca.Clients.ContactsReferencesClients.LastName,
                                Email = ca.Clients.ContactsReferencesClients.Email,
                                PhoneNumber = ca.Clients.ContactsReferencesClients.PhoneNumber,
                                RelationshipType = ca.Clients.ContactsReferencesClients.RelationshipsClientsTypes.Type
                            },
                            ClientReference2 = new DTO_ClientReference {
                                FirstName = ca.Clients.ContactsReferencesClients1.FirstName,
                                MiddleName = ca.Clients.ContactsReferencesClients1.MiddleName,
                                LastName = ca.Clients.ContactsReferencesClients1.LastName,
                                Email = ca.Clients.ContactsReferencesClients1.Email,
                                PhoneNumber = ca.Clients.ContactsReferencesClients1.PhoneNumber,
                                RelationshipType = ca.Clients.ContactsReferencesClients1.RelationshipsClientsTypes.Type
                            },
                            ClientBankAcount1 = new DTO_BankAccountClient {
                                BankName = ca.Clients.BankAccounts.Banks.Name,
                                CardType = ca.Clients.BankAccounts.BankCardTypes.Type,
                                CardNumber = ca.Clients.BankAccounts.CardNumber,
                                CLABE = ca.Clients.BankAccounts.CodeCLABE
                            },
                            ClientBankAcount2 = new DTO_BankAccountClient {
                                BankName = ca.Clients.BankAccounts1.Banks.Name,
                                CardType = ca.Clients.BankAccounts1.BankCardTypes.Type,
                                CardNumber = ca.Clients.BankAccounts1.CardNumber,
                                CLABE = ca.Clients.BankAccounts1.CodeCLABE
                            },
                            ProofINE = ca.ProofINE,
                            ProofAddress = ca.ProofAddress,
                            ProofLastVoucher = ca.ProofLastPayStub,
                            Valoration = ca.ValorationOpinion ?? "",
                            Status = ca.StatusesCreditApplication.Status
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseDetailsCreditApplications = MessageResponse<DTO_CreditApplication_Details>.Success (
                            $"Credit application ID {idCreditApplication} retrieved.",
                            dataRetrieved);
                    } else {
                        responseDetailsCreditApplications = MessageResponse<DTO_CreditApplication_Details>.Failure ("Cannot retrieve credit application.");
                    }
                } catch (Exception ex) {
                    responseDetailsCreditApplications = MessageResponse<DTO_CreditApplication_Details>.Failure (ex.ToString ());
                }
            }
            return responseDetailsCreditApplications;
        }

        public static async Task<MessageResponse<bool>> SaveCheckListPolicies (int idCreditApplication, List<DTO_CreditApplication_CreditPolicies> checkListPolicies, bool isApproved) {
            try {
                using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                    List<CreditApplications_Policies> relations = await context.CreditApplications_Policies.
                        Where (cap => cap.IdCreditApplication == idCreditApplication).
                        ToListAsync ();

                    Dictionary<int, bool?> policyDict = checkListPolicies.
                        Where (
                            cl => cl.IdCreditApplication == idCreditApplication
                        ).
                        ToDictionary (
                            cl => cl.IdCreditPolicy,
                            cl => cl.IsApplied
                        );

                    foreach (CreditApplications_Policies relation in relations) {
                        if (policyDict.TryGetValue (relation.IdPolicy, out bool? isApplied)) {
                            relation.IsAprobed = isApplied;
                        } else {
                            relation.IsAprobed = null;
                        }
                    }
                    await context.SaveChangesAsync ();



                    CreditApplications currentCreditApplication = context.CreditApplications.Attach (context.CreditApplications.Find (idCreditApplication));
                    currentCreditApplication.IdStatusCreditApplication = isApproved ? 2 : 3;
                    context.Entry (currentCreditApplication).State = EntityState.Modified;
                    await context.SaveChangesAsync ();


                    return MessageResponse<bool>.Success ("The policies was updated.", true);
                }
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Cannot save the changes: {ex.Message}");
            }
        }


        public static async Task<MessageResponse<bool>> DefineOpinion (int idCreditApplcation, string valoration) {
            MessageResponse<bool> responseUpdateValorationCrdApp = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    CreditApplications currentCreditApplication = context.CreditApplications.Find (idCreditApplcation);

                    if (currentCreditApplication != null) {
                        context.CreditApplications.Attach (currentCreditApplication);

                        currentCreditApplication.ValorationOpinion = valoration;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentCreditApplication).State = EntityState.Modified;
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

                        responseUpdateValorationCrdApp = MessageResponse<bool>.Success (
                            $"Credit applicacion ID {idCreditApplcation} updated", true);
                    } else {
                        responseUpdateValorationCrdApp = MessageResponse<bool>.Failure ($"Credit application ID {idCreditApplcation} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateValorationCrdApp = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateValorationCrdApp;
        }
    }
}
