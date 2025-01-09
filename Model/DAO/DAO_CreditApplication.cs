using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Client;
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
        public static async Task<MessageResponse<List<DTO_CreditApplication_Consult>>> GetAsync () {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditApplication_Consult>> response = null;
                try {
                    List<DTO_CreditApplication_Consult> dataRetrieved = await
                        context.CreditApplications.
                        Select (ca => new DTO_CreditApplication_Consult {
                            IdCreditApplication = ca.IdCreditApplication,
                            AmountTotal = ca.AmountTotal,
                            AmountTotalS = "$ " + ca.AmountTotal.ToString (),
                            InteresRate = ca.Promotions.InterestRate,
                            InterestPercentaje = ((ca.Promotions.InterestRate * 100)).ToString () + " %",
                            NumberFortNights = ca.Promotions.NumberFortnights,
                            DateRequest = ca.DateApplication,
                            IdStatus = ca.IdStatusCreditApplication,
                            Status = ca.StatusesCreditApplication.Status,
                            ClientName = ca.Clients.FirstName + " " + ca.Clients.MiddleName
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditApplication_Consult>>.Success (
                            dataRetrieved.Count + " solicitudes de crédito obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ("No se logró obtenr la lista de solicitudes de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_CreditApplication_Consult>>> GetAsync (string keyWord) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_CreditApplication_Consult>> response = null;
                try {
                    List<DTO_CreditApplication_Consult> dataRetrieved = await context.CreditApplications.
                        Where (ca =>
                            string.Concat (ca.AmountTotal).Contains (keyWord) ||
                            string.Concat (ca.DateAcepted).Contains (keyWord) ||
                            ca.StatusesCreditApplication.Status.Contains (keyWord)).
                        Select (ca => new DTO_CreditApplication_Consult {
                            IdCreditApplication = ca.IdCreditApplication,
                            AmountTotal = ca.AmountTotal,
                            InteresRate = ca.Promotions.InterestRate,
                            NumberFortNights = ca.Promotions.NumberFortnights,
                            DateRequest = ca.DateApplication,
                            IdStatus = ca.IdStatusCreditApplication,
                            Status = ca.StatusesCreditApplication.Status,
                            ClientName = ca.Clients.FirstName + " " + ca.Clients.MiddleName
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_CreditApplication_Consult>>.Success (
                            dataRetrieved.Count + " solicitudes de crédito obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ("No se logró obtener la lista de solicitudes de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_CreditApplication_Consult>>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PostAsync (DTO_CreditApplication_Create newCreditApplication) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                if (newCreditApplication == null || newCreditApplication.IdClient <= 0) {
                    return MessageResponse<bool>.Failure ("Datos del crédito invélidos.");
                }
                MessageResponse<bool> response = null;
                using (var transaction = context.Database.BeginTransaction ()) {
                    try {
                        CreditApplications createdCreditApplication = new CreditApplications {
                            DateApplication = newCreditApplication.DateApplication,
                            AmountTotal = newCreditApplication.AmountSolicited,
                            IdStatusCreditApplication = 1,
                            IdPromotion = newCreditApplication.IdPromotion,
                            IdEmployeeApplication = newCreditApplication.IdEmployee,
                            IdClient = newCreditApplication.IdClient,
                            ProofINE = newCreditApplication.ProofINE,
                            ProofAddress = newCreditApplication.ProofAddress,
                            ProofLastPayStub = newCreditApplication.ProofLastPayStub,
                        };
                        context.CreditApplications.Add (createdCreditApplication);
                        await context.SaveChangesAsync ();

                        DateTime today = DateTime.Now;
                        List<Policies> activePolicies = await context.Policies
                            .Where (p =>
                                today >= p.DateStart &&
                                today <= (p.DateEnd ?? DateTime.Now))
                            .ToListAsync ();

                        foreach (Policies policy in activePolicies) {
                            CreditApplications_Policies applicationPolicy = new CreditApplications_Policies {
                                IdCreditApplication = createdCreditApplication.IdCreditApplication,
                                IdPolicy = policy.IdPolicy,
                                IsAprobed = null
                            };
                            context.CreditApplications_Policies.Add (applicationPolicy);
                            await context.SaveChangesAsync ();
                        }
                        transaction.Commit ();

                        response = MessageResponse<bool>.Success ("Solicitud de crédito creada correctamente.", true);
                    } catch (Exception ex) {
                        transaction.Rollback ();
                        response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                    }
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_CreditApplication_Details>> GetDetailsAsync (int idCreditApplication) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_CreditApplication_Details> response = null;
                try {
                    DTO_CreditApplication_Details dataRetrieved = await context.CreditApplications.
                        Where (ca => ca.IdCreditApplication == idCreditApplication).
                        Select (ca => new DTO_CreditApplication_Details {
                            IdCreditApplication = ca.IdCreditApplication,
                            NameAdviser = ca.Employees.FirstName + " " + ca.Employees.MiddleName + " " + ca.Employees.LastName,
                            AmountSolicited = ca.AmountTotal,
                            AmountWithInteres = (int)(ca.AmountTotal + ca.AmountTotal * ca.Promotions.InterestRate),
                            InterestRate = ca.Promotions.InterestRate,
                            DateSolicited = ca.DateApplication,
                            NumberFortNights = ca.Promotions.NumberFortnights,
                            IdClient = ca.Clients.IdClient,
                            ClientFirstName = ca.Clients.FirstName,
                            ClientMiddleName = ca.Clients.MiddleName,
                            ClientLastName = ca.Clients.LastName,
                            ClientDateBirth = ca.Clients.DateBirth,
                            CodeCURP = ca.Clients.CodeCURP,
                            CodeRFC = ca.Clients.CodeRFC,
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
                        response = MessageResponse<DTO_CreditApplication_Details>.Success (
                            $"Solicitud de crédito ID {idCreditApplication} obtenido.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<DTO_CreditApplication_Details>.Failure ("No se logró obtener la solicitud de crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_CreditApplication_Details>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutPoliciesAsync (int idCreditApplication, List<DTO_CreditApplication_CreditPolicies> checkListPolicies, bool isApproved) {
            try {
                using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                    List<CreditApplications_Policies> relations = await context.CreditApplications_Policies.
                        Where (cap => cap.IdCreditApplication == idCreditApplication).
                        ToListAsync ();

                    Dictionary<int, bool?> policyDict = checkListPolicies.
                        Where (cl => cl.IdCreditApplication == idCreditApplication).
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

                    CreditApplications currentCreditApplication =
                        context.CreditApplications.Attach (context.CreditApplications.Find (idCreditApplication));
                    currentCreditApplication.IdStatusCreditApplication = isApproved ? 2 : 3;
                    context.Entry (currentCreditApplication).State = EntityState.Modified;
                    await context.SaveChangesAsync ();

                    return MessageResponse<bool>.Success ("Las políticas han sido actualizadas.", true);
                }
            } catch (Exception ex) {
                return MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
            }
        }

        public static async Task<MessageResponse<bool>> PutOpinionAsync (int idCreditApplcation, string valoration) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    CreditApplications currentCreditApplication = context.CreditApplications.Find (idCreditApplcation);

                    if (currentCreditApplication != null) {
                        context.CreditApplications.Attach (currentCreditApplication);

                        currentCreditApplication.ValorationOpinion = valoration;
                        currentCreditApplication.DateAcepted = DateTime.Now;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentCreditApplication).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();
                                        if (databaseValues != null) {
                                            CreditApplications databaseEntity = (CreditApplications)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Solicitud de crédito ID {idCreditApplcation} actualizado.", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"Solicitud de crédito ID {idCreditApplcation} no se logró obtener.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }
    }
}
