using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Client;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Client {
        public static async Task<MessageResponse<List<DTO_Client_Consult>>> GetAllClients () {
            MessageResponse<List<DTO_Client_Consult>> responseConsultClients = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Client_Consult> dataRetrieved = await
                        context.Clients.
                        Select (clnt => new DTO_Client_Consult {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            CodeRFC = clnt.CodeRFC,
                            CodeCURP = clnt.CodeCURP,
                            IdStatusClient = clnt.StatusesClient.IdStatusClient,
                            StatusClient = clnt.StatusesClient.Status
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Success (
                            dataRetrieved.Count + " clients retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Failure ("Cannot retrieved clients.");
                    }
                } catch (Exception ex) {
                    responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultClients;
        }

        public static async Task<MessageResponse<List<DTO_Client_Consult>>> GetFilteredClients (string keyWord) {
            MessageResponse<List<DTO_Client_Consult>> responseFilteredClients = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Client_Consult> dataRetrieved = await
                        context.Clients.
                        Where (clnt =>
                            clnt.FirstName.Contains (keyWord) ||
                            clnt.MiddleName.Contains (keyWord) ||
                            clnt.LastName.Contains (keyWord) ||
                            clnt.CodeCURP.Equals (keyWord) ||
                            clnt.CodeRFC.Equals (keyWord)).
                        Select (clnt => new DTO_Client_Consult {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            CodeRFC = clnt.CodeRFC,
                            CodeCURP = clnt.CodeCURP,
                            IdStatusClient = clnt.StatusesClient.IdStatusClient,
                            StatusClient = clnt.StatusesClient.Status
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseFilteredClients = MessageResponse<List<DTO_Client_Consult>>.Success (
                            dataRetrieved.Count + " clients retrieved.",
                            dataRetrieved);
                    } else {
                        responseFilteredClients = MessageResponse<List<DTO_Client_Consult>>.Failure ("Cannot retrieved filtered clients..");
                    }
                } catch (Exception ex) {
                    responseFilteredClients = MessageResponse<List<DTO_Client_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseFilteredClients;
        }

        public static async Task<MessageResponse<string>> GetStatusClient (int idClient) {
            MessageResponse<string> responseStatus = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await context.Clients.
                        Where (clnt => clnt.IdClient == idClient).
                        Select (clnt => clnt.StatusesClient.Status).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseStatus = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        responseStatus = MessageResponse<string>.Failure ($"Client ID {idClient} without status.");
                    }
                } catch (Exception ex) {
                    responseStatus = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseStatus;
        }

        public static async Task<MessageResponse<bool>> ChangeStatusClient (int idClient, int idStatus) {
            MessageResponse<bool> responseUpdateStatusClient = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Clients currentClient = context.Clients.Find (idClient);

                    if (currentClient != null) {
                        context.Clients.Attach (currentClient);

                        currentClient.IdStatusClient = idStatus;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentClient).State = EntityState.Modified;
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

                        responseUpdateStatusClient = MessageResponse<bool>.Success (
                            $"Client ID {currentClient.IdClient} updated", true);
                    } else {
                        responseUpdateStatusClient = MessageResponse<bool>.Failure ($"Client ID {currentClient.IdClient} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateStatusClient = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateStatusClient;
        }

        public static async Task<MessageResponse<DTO_Client_Details>> GetDetailsClient (int idClient) {
            MessageResponse<DTO_Client_Details> responseDetails = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Client_Details dataRetrieved = await context.Clients
                        .Where (clnt => clnt.IdClient == idClient)
                        .Include (clnt => clnt.StatusesMarital)
                        .Include (clnt => clnt.ClientsAddresses)
                        .Include (clnt => clnt.WorkClients)
                        .Include (clnt => clnt.BankAccounts)
                        .Include (clnt => clnt.BankAccounts1)
                        .Include (clnt => clnt.ContactsReferencesClients)
                        .Include (clnt => clnt.ContactsReferencesClients1)
                        .Include (clnt => clnt.StatusesClient)
                        .Select (clnt => new DTO_Client_Details {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            DateBirth = clnt.DateBirth,
                            Gender = clnt.Gender,
                            IdMaritalStatus = clnt.IdStatusMarital,
                            MaritalStatus = clnt.StatusesMarital.Status,
                            CodeCURP = clnt.CodeCURP,
                            AddressClient = clnt.ClientsAddresses == null ? null : new DTO_ClientAddress {
                                IdAddressClient = clnt.ClientsAddresses.IdClientAddress,
                                ExteriorNumber = clnt.ClientsAddresses.ExteriorNumber ?? string.Empty,
                                InteriorNumber = clnt.ClientsAddresses.InteriorNumber ?? string.Empty,
                                Street = clnt.ClientsAddresses.Street ?? string.Empty,
                                Colony = clnt.ClientsAddresses.Colony ?? string.Empty,
                                PostalCode = clnt.ClientsAddresses.PostalCode ?? string.Empty,
                                Municipality = clnt.ClientsAddresses.Municipality ?? string.Empty,
                                IdState = clnt.ClientsAddresses.StatesAddresses.IdStateAddress,
                                State = clnt.ClientsAddresses.StatesAddresses.Name ?? string.Empty,
                                IdAddressType = clnt.ClientsAddresses.AddressesTypes.IdAddressType,
                                AddressType = clnt.ClientsAddresses.AddressesTypes.Type ?? string.Empty,
                            },
                            Email1 = clnt.Email1 ?? string.Empty,
                            Email2 = clnt.Email2 ?? string.Empty,
                            PhoneNumber1 = clnt.PhoneNumber1 ?? string.Empty,
                            PhoneNumber2 = clnt.PhoneNumber2 ?? string.Empty,
                            Work = clnt.WorkClients == null ? null : new DTO_WorkInfo {
                                IdWorkType = clnt.WorkClients.WorkTypes.IdWorkType,
                                WorkType = clnt.WorkClients.WorkTypes.Type ?? string.Empty,
                                WorkArea = clnt.WorkClients.WorkArea ?? string.Empty,
                                MonthlySalary = (float?)clnt.WorkClients.MonthlySalary ?? 0,
                            },
                            Reference1 = clnt.ContactsReferencesClients == null ? null : new DTO_ClientReference {
                                FirstName = clnt.ContactsReferencesClients.FirstName ?? string.Empty,
                                MiddleName = clnt.ContactsReferencesClients.MiddleName ?? string.Empty,
                                LastName = clnt.ContactsReferencesClients.LastName ?? string.Empty,
                                Email = clnt.ContactsReferencesClients.Email ?? string.Empty,
                                PhoneNumber = clnt.ContactsReferencesClients.PhoneNumber ?? string.Empty,
                                IdRelationshipType = clnt.ContactsReferencesClients.IdRelationshipType,
                                RelationshipType = clnt.ContactsReferencesClients.RelationshipsClientsTypes.Type ?? string.Empty,
                            },
                            Reference2 = clnt.ContactsReferencesClients1 == null ? null : new DTO_ClientReference {
                                FirstName = clnt.ContactsReferencesClients1.FirstName ?? string.Empty,
                                MiddleName = clnt.ContactsReferencesClients1.MiddleName ?? string.Empty,
                                LastName = clnt.ContactsReferencesClients1.LastName ?? string.Empty,
                                Email = clnt.ContactsReferencesClients1.Email ?? string.Empty,
                                PhoneNumber = clnt.ContactsReferencesClients1.PhoneNumber ?? string.Empty,
                                IdRelationshipType = clnt.ContactsReferencesClients1.IdRelationshipType,
                                RelationshipType = clnt.ContactsReferencesClients1.RelationshipsClientsTypes.Type ?? string.Empty,
                            },
                            CodeRFC = clnt.CodeRFC ?? string.Empty,
                            BankAccount1 = clnt.BankAccounts == null ? null : new DTO_BankAccountClient {
                                IdBankName = clnt.BankAccounts.Banks.IdBank,
                                BankName = clnt.BankAccounts.Banks.Name,
                                CLABE = clnt.BankAccounts.CodeCLABE ?? string.Empty,
                                CardNumber = clnt.BankAccounts.CardNumber ?? string.Empty,
                                IdCardType = clnt.BankAccounts.BankCardTypes.IdBankCardType,
                                CardType = clnt.BankAccounts.BankCardTypes.Type ?? string.Empty,
                            },
                            BankAccount2 = clnt.BankAccounts1 == null ? null : new DTO_BankAccountClient {
                                IdBankName = clnt.BankAccounts1.Banks.IdBank,
                                BankName = clnt.BankAccounts1.Banks.Name ?? string.Empty,
                                CLABE = clnt.BankAccounts1.CodeCLABE ?? string.Empty,
                                CardNumber = clnt.BankAccounts1.CardNumber ?? string.Empty,
                                IdCardType = clnt.BankAccounts1.BankCardTypes.IdBankCardType,
                                CardType = clnt.BankAccounts1.BankCardTypes.Type ?? string.Empty,
                            },
                            IdStatusClient = clnt.StatusesClient.IdStatusClient,
                            StatusClient = clnt.StatusesClient.Status,
                        })
                        .FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseDetails = MessageResponse<DTO_Client_Details>.Success (
                            $"Client name \"{dataRetrieved.FirstName}\" retrieved.",
                            dataRetrieved);
                    } else {
                        responseDetails = MessageResponse<DTO_Client_Details>.Failure ("Client not retrieved.");
                    }
                } catch (Exception ex) {
                    responseDetails = MessageResponse<DTO_Client_Details>.Failure (ex.ToString ());
                }
            }
            return responseDetails;
        }


        public static async Task<MessageResponse<bool>> RegistryNewClient (DTO_Client_Details newClient) {
            MessageResponse<bool> responseCreateClient = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Clients createdClient = new Clients {
                        FirstName = newClient.FirstName,
                        MiddleName = newClient.MiddleName,
                        LastName = newClient.LastName,
                        DateBirth = newClient.DateBirth,
                        Gender = newClient.Gender,
                        IdStatusMarital = newClient.IdMaritalStatus,
                        CodeCURP = newClient.CodeCURP,
                        ClientsAddresses = new ClientsAddresses {
                            IdState = newClient.AddressClient.IdState,
                            Municipality = newClient.AddressClient.Municipality,
                            PostalCode = newClient.AddressClient.PostalCode,
                            Colony = newClient.AddressClient.Colony,
                            Street = newClient.AddressClient.Street,
                            ExteriorNumber = newClient.AddressClient.ExteriorNumber,
                            InteriorNumber = newClient.AddressClient.InteriorNumber,
                            IdAddressType = newClient.AddressClient.IdAddressType
                        },
                        Email1 = newClient.Email1,
                        Email2 = newClient.Email2,
                        PhoneNumber1 = newClient.PhoneNumber1,
                        PhoneNumber2 = newClient.PhoneNumber2,
                        WorkClients = new WorkClients {
                            WorkArea = newClient.Work.WorkArea,
                            IdWorkType = newClient.Work.IdWorkType,
                            MonthlySalary = newClient.Work.MonthlySalary
                        },
                        ContactsReferencesClients = new ContactsReferencesClients {
                            FirstName = newClient.Reference1.FirstName,
                            MiddleName = newClient.Reference1.MiddleName,
                            LastName = newClient.Reference1.LastName,
                            Email = newClient.Reference1.Email,
                            PhoneNumber = newClient.Reference1.PhoneNumber,
                            IdRelationshipType = newClient.Reference1.IdRelationshipType
                        },
                        ContactsReferencesClients1 = new ContactsReferencesClients {
                            FirstName = newClient.Reference2.FirstName,
                            MiddleName = newClient.Reference2.MiddleName,
                            LastName = newClient.Reference2.LastName,
                            Email = newClient.Reference2.Email,
                            PhoneNumber = newClient.Reference2.PhoneNumber,
                            IdRelationshipType = newClient.Reference2.IdRelationshipType
                        },
                        CodeRFC = newClient.CodeRFC,
                        BankAccounts = new BankAccounts {
                            IdNameBank = newClient.BankAccount1.IdBankName,
                            CodeCLABE = newClient.BankAccount1.CLABE,
                            CardNumber = newClient.BankAccount1.CardNumber,
                            IdCardType = newClient.BankAccount1.IdCardType
                        },
                        BankAccounts1 = new BankAccounts {
                            IdNameBank = newClient.BankAccount2.IdBankName,
                            CodeCLABE = newClient.BankAccount2.CLABE,
                            CardNumber = newClient.BankAccount2.CardNumber,
                            IdCardType = newClient.BankAccount2.IdCardType
                        },
                        IdStatusClient = newClient.IdStatusClient
                    };

                    context.Clients.Add (createdClient);
                    await context.SaveChangesAsync ();

                    responseCreateClient = MessageResponse<bool>.Success (
                        $"Client {createdClient.FirstName} created.", true);
                } catch (Exception ex) {
                    responseCreateClient = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseCreateClient;
        }

        public static async Task<MessageResponse<bool>> SaveChangesDataClient (DTO_Client_Details newDataClient) {
            MessageResponse<bool> responseUpdateDataClient = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Clients currentClient = context.Clients.Find (newDataClient.IdClient);

                    if (currentClient != null) {
                        context.Clients.Attach (currentClient);

                        currentClient.Gender = newDataClient.Gender;
                        currentClient.IdStatusMarital = newDataClient.IdMaritalStatus;
                        currentClient.Email1 = newDataClient.Email1;
                        currentClient.Email2 = newDataClient.Email2;
                        currentClient.PhoneNumber1 = newDataClient.PhoneNumber1;
                        currentClient.PhoneNumber2 = newDataClient.PhoneNumber2;
                        currentClient.WorkClients.IdWorkType = newDataClient.Work.IdWorkType;
                        currentClient.WorkClients.WorkArea = newDataClient.Work.WorkArea;
                        currentClient.WorkClients.MonthlySalary = newDataClient.Work.MonthlySalary;
                        currentClient.ContactsReferencesClients.FirstName = newDataClient.Reference1.FirstName;
                        currentClient.ContactsReferencesClients.MiddleName = newDataClient.Reference1.MiddleName;
                        currentClient.ContactsReferencesClients.LastName = newDataClient.Reference1.LastName;
                        currentClient.ContactsReferencesClients.Email = newDataClient.Reference1.Email;
                        currentClient.ContactsReferencesClients.PhoneNumber = newDataClient.Reference1.PhoneNumber;
                        currentClient.ContactsReferencesClients.IdRelationshipType = newDataClient.Reference1.IdRelationshipType;
                        currentClient.ContactsReferencesClients1.FirstName = newDataClient.Reference2.FirstName;
                        currentClient.ContactsReferencesClients1.MiddleName = newDataClient.Reference2.MiddleName;
                        currentClient.ContactsReferencesClients1.LastName = newDataClient.Reference2.LastName;
                        currentClient.ContactsReferencesClients1.Email = newDataClient.Reference2.Email;
                        currentClient.ContactsReferencesClients1.PhoneNumber = newDataClient.Reference2.PhoneNumber;
                        currentClient.ContactsReferencesClients1.IdRelationshipType = newDataClient.Reference2.IdRelationshipType;
                        currentClient.BankAccounts.IdNameBank = newDataClient.BankAccount1.IdBankName;
                        currentClient.BankAccounts.CodeCLABE = newDataClient.BankAccount1.CLABE;
                        currentClient.BankAccounts.CardNumber = newDataClient.BankAccount1.CardNumber;
                        currentClient.BankAccounts.IdCardType = newDataClient.BankAccount1.IdCardType;
                        currentClient.BankAccounts1.IdNameBank = newDataClient.BankAccount2.IdBankName;
                        currentClient.BankAccounts1.CodeCLABE = newDataClient.BankAccount2.CLABE;
                        currentClient.BankAccounts1.CardNumber = newDataClient.BankAccount2.CardNumber;
                        currentClient.BankAccounts1.IdCardType = newDataClient.BankAccount2.IdCardType;
                        currentClient.IdStatusClient = newDataClient.IdStatusClient;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentClient).State = EntityState.Modified;
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

                        responseUpdateDataClient = MessageResponse<bool>.Success (
                            $"Client ID {currentClient.IdClient} updated", true);
                    } else {
                        responseUpdateDataClient = MessageResponse<bool>.Failure ($"Client ID {currentClient.IdClient} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateDataClient = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateDataClient;
        }

        public static async Task<MessageResponse<string>> GetRFCClient (int idClient) {
            MessageResponse<string> responseConsultRFC = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.IdClient == idClient).
                        Select (clnt => clnt.CodeRFC).
                        FirstOrDefaultAsync ();

                    if (responseConsultRFC != null) {
                        responseConsultRFC = MessageResponse<string>.Success (
                            $"cardNumber code \"{responseConsultRFC}\" retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultRFC = MessageResponse<string>.Failure ("cardNumber doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultRFC = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseConsultRFC;
        }

        public static async Task<bool> VerifyExistenceEmail (string email) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.Email1.Equals (email) || clnt.Email2.Equals (email)).
                        Select (clnt => clnt.Email1).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistencePhoneNumber (string phoneNumber) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.PhoneNumber1.Equals (phoneNumber) || clnt.PhoneNumber2.Equals (phoneNumber)).
                        Select (clnt => clnt.PhoneNumber1).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistenceCURP (string curp) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.CodeCURP.Equals (curp)).
                        Select (clnt => clnt.CodeCURP).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistenceRFC (string rfc) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.CodeRFC.Equals (rfc)).
                        Select (clnt => clnt.CodeRFC).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistenceCardNumber (string cardNumber) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.BankAccounts.CardNumber.Equals (cardNumber) || clnt.BankAccounts1.CardNumber.Equals (cardNumber)).
                        Select (clnt => clnt.BankAccounts.CardNumber).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistenceCLABE (string clabe) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.BankAccounts.CodeCLABE.Equals (clabe) ||
                                                      clnt.BankAccounts1.CodeCLABE.Equals (clabe)).
                        Select (clnt => clnt.BankAccounts.CodeCLABE).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static async Task<MessageResponse<DTO_Client_Search>> SearchClientCreditApplication (string name, string codeCurp, string codeRFC) {
            MessageResponse<DTO_Client_Search> responseSearch = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Client_Search dataRetrieved = await context.Clients.
                        Where (clnt =>
                            (clnt.FirstName + " " + clnt.MiddleName + " " + clnt.LastName).Equals (name) &&
                            clnt.CodeCURP.Equals (codeCurp) &&
                            clnt.CodeRFC.Equals (codeRFC)).
                        Select (clnt => new DTO_Client_Search {
                            IdClient = clnt.IdClient,
                            Name = clnt.FirstName + " " + clnt.MiddleName + " " + clnt.LastName,
                            IdStatus = clnt.IdStatusClient,
                            Status = clnt.StatusesClient.Status
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseSearch = MessageResponse<DTO_Client_Search>.Success (
                            $"Client ID {dataRetrieved.IdClient} retrieved.",
                            dataRetrieved);
                    } else {
                        responseSearch = MessageResponse<DTO_Client_Search>.Failure ($"Client doesn't retrieved.");
                    }
                } catch (Exception ex) {
                    responseSearch = MessageResponse<DTO_Client_Search>.Failure (ex.ToString ());
                }
            }
            return responseSearch;
        }
    }
}