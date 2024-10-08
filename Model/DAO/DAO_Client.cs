using FinanciaRed.Model.DTO;
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
                            StatusActive = clnt.StatusActive ? "Activo" : "No activo"
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Success (
                            dataRetrieved.Count + " clients retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Failure ("Wrong credentials.");
                    }
                } catch (Exception ex) {
                    responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultClients;
        }

        public static async Task<MessageResponse<DTO_Client_DetailsClient>> GetDetailsClient (int idClient) {
            MessageResponse<DTO_Client_DetailsClient> responseDetails = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Client_DetailsClient dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.IdClient == idClient).
                        Include (ms => ms.MaritalStatuses).
                        Include (addr => addr.ClientsAddresses).
                        Select (clnt => new DTO_Client_DetailsClient {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            DateBirth = clnt.DateBirth,
                            Gender = clnt.Gender,
                            IdMaritalStatus = clnt.IdMaritalStatus,
                            MaritalStatus = clnt.MaritalStatuses.Status,
                            CodeCurp = clnt.CodeCURP,
                            AddressClient = new DTO_AddressClient {
                                IdAddressClient = clnt.ClientsAddresses.IdClientAddress,
                                ExteriorNumber = clnt.ClientsAddresses.ExteriorNumber,
                                InteriorNumber = clnt.ClientsAddresses.InteriorNumber,
                                Street = clnt.ClientsAddresses.Street,
                                Colony = clnt.ClientsAddresses.Colony,
                                PostalCode = clnt.ClientsAddresses.PostalCode,
                                Municipality = clnt.ClientsAddresses.Municipality,
                                IdState = clnt.ClientsAddresses.StatesAddresses.IdStateAddress,
                                State = clnt.ClientsAddresses.StatesAddresses.Name,
                                IdAddressType = clnt.ClientsAddresses.AddressesTypes.IdAddressType,
                                AddressType = clnt.ClientsAddresses.AddressesTypes.Type
                            },
                            Email1 = clnt.Email1,
                            Email2 = clnt.Email2,
                            PhoneNumber1 = clnt.PhoneNumber1,
                            PhoneNumber2 = clnt.PhoneNumber2,
                            IdWorkType = clnt.WorkAreas.WorkAreaTypes.IdWorkAreaType,
                            WorkType = clnt.WorkAreas.WorkAreaTypes.Type,
                            WorkArea = clnt.WorkAreas.WorkArea,
                            MonthlySalary = (float)clnt.WorkAreas.MonthlySalary,
                            Reference1FirstName = clnt.ContactsReferencesClients.FirstName,
                            Reference1MiddleName = clnt.ContactsReferencesClients.MiddleName,
                            Reference1LastName = clnt.ContactsReferencesClients.LastName,
                            Reference1Email = clnt.ContactsReferencesClients.Email,
                            Reference1PhoneNumber = clnt.ContactsReferencesClients.PhoneNumber,
                            IdReference1RelationshipType = clnt.ContactsReferencesClients.IdRelationshipType,
                            Reference1RelationshipType = clnt.ContactsReferencesClients.RelationshipsClientsTypes.Type,
                            Reference2FirstName = clnt.ContactsReferencesClients1.FirstName,
                            Reference2MiddleName = clnt.ContactsReferencesClients1.MiddleName,
                            Reference2LastName = clnt.ContactsReferencesClients1.LastName,
                            Reference2Email = clnt.ContactsReferencesClients1.Email,
                            Reference2PhoneNumber = clnt.ContactsReferencesClients1.PhoneNumber,
                            IdReference2RelationshipType = clnt.ContactsReferencesClients1.IdRelationshipType,
                            Reference2RelationshipType = clnt.ContactsReferencesClients1.RelationshipsClientsTypes.Type,
                            CodeRFC = clnt.CodeRFC,
                            IdBankAccount1Name = clnt.BankAccounts.Banks.IdBank,
                            BankAccount1Name = clnt.BankAccounts.Banks.Name,
                            BankAccount1CLABE = clnt.BankAccounts.CLABE,
                            BankAccount1CardNumber = clnt.BankAccounts.CardNumber,
                            IdBankAccount1CardType = clnt.BankAccounts.BankCardTypes.IdBankCardType,
                            BankAccount1CardType = clnt.BankAccounts.BankCardTypes.Type,
                            IdBankAccount2Name = clnt.BankAccounts1.Banks.IdBank,
                            BankAccount2Name = clnt.BankAccounts1.Banks.Name,
                            BankAccount2CLABE = clnt.BankAccounts1.CLABE,
                            BankAccount2CardNumber = clnt.BankAccounts1.CardNumber,
                            IdBankAccount2CardType = clnt.BankAccounts1.BankCardTypes.IdBankCardType,
                            BankAccount2CardType = clnt.BankAccounts1.BankCardTypes.Type,
                            StatusActive = clnt.StatusActive
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseDetails = MessageResponse<DTO_Client_DetailsClient>.Success (
                            $"Client name \"{dataRetrieved.FirstName}\" retrieved.",
                            dataRetrieved);
                    } else {
                        responseDetails = MessageResponse<DTO_Client_DetailsClient>.Failure ("Clients doesn't retrieved");
                    }
                } catch (Exception ex) {
                    responseDetails = MessageResponse<DTO_Client_DetailsClient>.Failure (ex.ToString ());
                }
            }
            return responseDetails;
        }

        public static async Task<MessageResponse<bool>> RegistryNewClient (DTO_Client_DetailsClient newClient) {
            MessageResponse<bool> responseCreateClient = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Clients createdClient = new Clients {
                        FirstName = newClient.FirstName,
                        MiddleName = newClient.MiddleName,
                        LastName = newClient.LastName,
                        DateBirth = newClient.DateBirth,
                        Gender = newClient.Gender,
                        IdMaritalStatus = newClient.IdMaritalStatus,
                        CodeCURP = newClient.CodeCurp,
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
                        WorkAreas = new WorkAreas {
                            WorkArea = newClient.WorkArea,
                            IdWorkAreaType = newClient.IdWorkType,
                            MonthlySalary = newClient.MonthlySalary
                        },
                        ContactsReferencesClients = new ContactsReferencesClients {
                            FirstName = newClient.Reference1FirstName,
                            MiddleName = newClient.Reference1MiddleName,
                            LastName = newClient.Reference1LastName,
                            Email = newClient.Reference1Email,
                            PhoneNumber = newClient.Reference1PhoneNumber,
                            IdRelationshipType = newClient.IdReference1RelationshipType
                        },
                        ContactsReferencesClients1 = new ContactsReferencesClients {
                            FirstName = newClient.Reference2FirstName,
                            MiddleName = newClient.Reference2MiddleName,
                            LastName = newClient.Reference2LastName,
                            Email = newClient.Reference2Email,
                            PhoneNumber = newClient.Reference2PhoneNumber,
                            IdRelationshipType = newClient.IdReference2RelationshipType
                        },
                        CodeRFC = newClient.CodeRFC,
                        BankAccounts = new BankAccounts {
                            IdNameBank = newClient.IdBankAccount1Name,
                            CLABE = newClient.BankAccount1CLABE,
                            CardNumber = newClient.BankAccount1CardNumber,
                            IdCardType = newClient.IdBankAccount1CardType
                        },
                        BankAccounts1 = new BankAccounts {
                            IdNameBank = newClient.IdBankAccount2Name,
                            CLABE = newClient.BankAccount2CLABE,
                            CardNumber = newClient.BankAccount2CardNumber,
                            IdCardType = newClient.IdBankAccount2CardType
                        },
                        StatusActive = true
                    };

                    context.Clients.Add (createdClient);
                    await context.SaveChangesAsync ();

                    responseCreateClient = MessageResponse<bool>.Success (
                        $"Client {createdClient.FirstName} created", true);
                } catch (Exception ex) {
                    responseCreateClient = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseCreateClient;
        }

        public static MessageResponse<bool> SaveChangesDataClient (DTO_Client_DetailsClient newDataClient) {
            MessageResponse<bool> responseUpdateDataClient = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Clients currentClient = context.Clients.Find (newDataClient.IdClient);

                    if (currentClient != null) {
                        context.Clients.Attach (currentClient);

                        currentClient.Gender = newDataClient.Gender;
                        currentClient.IdMaritalStatus = newDataClient.IdMaritalStatus;
                        currentClient.Email1 = newDataClient.Email1;
                        currentClient.Email2 = newDataClient.Email2;
                        currentClient.PhoneNumber1 = newDataClient.PhoneNumber1;
                        currentClient.PhoneNumber2 = newDataClient.PhoneNumber2;
                        currentClient.WorkAreas.IdWorkAreaType = newDataClient.IdWorkType;
                        currentClient.WorkAreas.WorkArea = newDataClient.WorkArea;
                        currentClient.WorkAreas.MonthlySalary = newDataClient.MonthlySalary;
                        currentClient.ContactsReferencesClients.FirstName = newDataClient.Reference1FirstName;
                        currentClient.ContactsReferencesClients.MiddleName = newDataClient.Reference1MiddleName;
                        currentClient.ContactsReferencesClients.LastName = newDataClient.Reference1LastName;
                        currentClient.ContactsReferencesClients.Email = newDataClient.Reference1Email;
                        currentClient.ContactsReferencesClients.PhoneNumber = newDataClient.Reference1PhoneNumber;
                        currentClient.ContactsReferencesClients.IdRelationshipType = newDataClient.IdReference1RelationshipType;
                        currentClient.ContactsReferencesClients1.FirstName = newDataClient.Reference2FirstName;
                        currentClient.ContactsReferencesClients1.MiddleName = newDataClient.Reference2MiddleName;
                        currentClient.ContactsReferencesClients1.LastName = newDataClient.Reference2LastName;
                        currentClient.ContactsReferencesClients1.Email = newDataClient.Reference2Email;
                        currentClient.ContactsReferencesClients1.PhoneNumber = newDataClient.Reference2PhoneNumber;
                        currentClient.ContactsReferencesClients1.IdRelationshipType = newDataClient.IdReference2RelationshipType;
                        currentClient.BankAccounts.IdNameBank = newDataClient.IdBankAccount1Name;
                        currentClient.BankAccounts.CLABE = newDataClient.BankAccount1CLABE;
                        currentClient.BankAccounts.CardNumber = newDataClient.BankAccount1CardNumber;
                        currentClient.BankAccounts.IdCardType = newDataClient.IdBankAccount1CardType;
                        currentClient.BankAccounts1.IdNameBank = newDataClient.IdBankAccount2Name;
                        currentClient.BankAccounts1.CLABE = newDataClient.BankAccount2CLABE;
                        currentClient.BankAccounts1.CardNumber = newDataClient.BankAccount2CardNumber;
                        currentClient.BankAccounts1.IdCardType = newDataClient.IdBankAccount2CardType;
                        currentClient.StatusActive = newDataClient.StatusActive;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentClient).State = EntityState.Modified;
                                context.SaveChanges ();

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
                        responseUpdateDataClient = MessageResponse<bool>.Failure ($"Client ID {currentClient.IdClient} doesn´t exists");
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
                        responseConsultRFC = MessageResponse<string>.Failure ("cardNumber doesn´t retrieved");
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
                } catch (Exception ex) {
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
                } catch (Exception ex) {
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

                    if (!string.IsNullOrEmpty(dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception ex) {
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

                    if (!string.IsNullOrEmpty(dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception ex) {
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
                } catch (Exception ex) {
                    return false;
                }
            }
        }

        public static async Task<bool> VerifyExistenceCLABE (string clabe) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Clients.
                        Where (clnt => clnt.BankAccounts.CLABE.Equals (clabe) || clnt.BankAccounts1.CLABE.Equals (clabe)).
                        Select (clnt => clnt.BankAccounts.CLABE).
                        FirstOrDefaultAsync ();

                    if (!string.IsNullOrEmpty (dataRetrieved)) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception ex) {
                    return false;
                }
            }
        }
    }
}