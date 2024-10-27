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
                            IdStatusClient = clnt.StatusesClient.IdStatusClient,
                            StatusClient = clnt.StatusesClient.Status
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
                        Include (ms => ms.StatusesMarital).
                        Include (addr => addr.ClientsAddresses).
                        Select (clnt => new DTO_Client_DetailsClient {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            DateBirth = clnt.DateBirth,
                            Gender = clnt.Gender,
                            IdMaritalStatus = clnt.IdStatusMarital,
                            MaritalStatus = clnt.StatusesMarital.Status,
                            CodeCURP = clnt.CodeCURP,
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
                            Work = new DTO_WorkInfo {
                                IdWorkType = clnt.WorkClients.WorkTypes.IdWorkType,
                                WorkType = clnt.WorkClients.WorkTypes.Type,
                                WorkArea = clnt.WorkClients.WorkArea,
                                MonthlySalary = (float)clnt.WorkClients.MonthlySalary
                            },
                            Reference1 = new DTO_ReferenceClient {
                                FirstName = clnt.ContactsReferencesClients.FirstName,
                                MiddleName = clnt.ContactsReferencesClients.MiddleName,
                                LastName = clnt.ContactsReferencesClients.LastName,
                                Email = clnt.ContactsReferencesClients.Email,
                                PhoneNumber = clnt.ContactsReferencesClients.PhoneNumber,
                                IdRelationshipType = clnt.ContactsReferencesClients.IdRelationshipType,
                                RelationshipType = clnt.ContactsReferencesClients.RelationshipsClientsTypes.Type,
                            },
                            Reference2 = new DTO_ReferenceClient {
                                FirstName = clnt.ContactsReferencesClients1.FirstName,
                                MiddleName = clnt.ContactsReferencesClients1.MiddleName,
                                LastName = clnt.ContactsReferencesClients1.LastName,
                                Email = clnt.ContactsReferencesClients1.Email,
                                PhoneNumber = clnt.ContactsReferencesClients1.PhoneNumber,
                                IdRelationshipType = clnt.ContactsReferencesClients1.IdRelationshipType,
                                RelationshipType = clnt.ContactsReferencesClients1.RelationshipsClientsTypes.Type,
                            },
                            CodeRFC = clnt.CodeRFC,
                            BankAccount1 = new DTO_BankAccountClient {
                                IdBankName = clnt.BankAccounts.Banks.IdBank,
                                BankName = clnt.BankAccounts.Banks.Name,
                                CLABE = clnt.BankAccounts.CodeCLABE,
                                CardNumber = clnt.BankAccounts.CardNumber,
                                IdCardType = clnt.BankAccounts.BankCardTypes.IdBankCardType,
                                CardType = clnt.BankAccounts.BankCardTypes.Type,
                            },
                            BankAccount2 = new DTO_BankAccountClient {
                                IdBankName = clnt.BankAccounts1.Banks.IdBank,
                                BankName = clnt.BankAccounts1.Banks.Name,
                                CLABE = clnt.BankAccounts1.CodeCLABE,
                                CardNumber = clnt.BankAccounts1.CardNumber,
                                IdCardType = clnt.BankAccounts1.BankCardTypes.IdBankCardType,
                                CardType = clnt.BankAccounts1.BankCardTypes.Type,
                            },
                            IdStatusClient = clnt.StatusesClient.IdStatusClient,
                            StatusClient = clnt.StatusesClient.Status
                            
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
                        WorkClients = new WorkClients{
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
                        currentClient.IdStatusMarital= newDataClient.IdMaritalStatus;
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
    }
}