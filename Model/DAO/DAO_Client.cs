using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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
                                IdAddressClient = clnt.IdAddress,
                                ExteriorNumber = clnt.ClientsAddresses.ExteriorNumber,
                                InteriorNumber = clnt.ClientsAddresses.InteriorNumber,
                                Street = clnt.ClientsAddresses.Street,
                                Colony = clnt.ClientsAddresses.Colony,
                                PostalCode = clnt.ClientsAddresses.PostalCode,
                                State = clnt.ClientsAddresses.State,
                                IdAddressType = clnt.ClientsAddresses.IdAddressType,
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
                            StatusActive = clnt.StatusActive ? "Activo" : "No activo"
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
                        currentClient.StatusActive = newDataClient.StatusActive.Equals ("Activo");

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
                                            var databaseEntity = (Match)databaseValues.ToObject ();
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
                    responseUpdateDataClient = MessageResponse<bool>.Failure ("Exception"+ex.Message);
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
                            $"RFC code \"{responseConsultRFC}\" retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultRFC = MessageResponse<string>.Failure ("RFC doesn´t retrieved");
                    }
                } catch (Exception ex) {
                    responseConsultRFC = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseConsultRFC;
        }
    }
}