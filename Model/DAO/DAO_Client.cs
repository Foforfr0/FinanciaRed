using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Client {
        public static async Task<MessageResponse<List<DTO_Client_Consult>>> GetAllClients () {
            MessageResponse<List<DTO_Client_Consult>> responseConsultClients = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Client_Consult> retrievedConsultClients = await
                        context.Clients.
                        Select (clnt => new DTO_Client_Consult {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            CodeRFC = clnt.CodeRFC,
                            CodeCURP = clnt.CodeCURP
                        }).
                        ToListAsync ();

                    if (retrievedConsultClients != null) {
                        responseConsultClients = MessageResponse<List<DTO_Client_Consult>>.Success (
                            retrievedConsultClients.Count + " clients retrieved.",
                            retrievedConsultClients);
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
            MessageResponse<DTO_Client_DetailsClient> responseConsultClients = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Client_DetailsClient retrievedDetailsClient = await
                        context.Clients.
                        Where (clnt => clnt.IdClient == idClient).
                        Include (ms => ms.MaritalStatuses).
                        Include (addr => addr.ClientsAddresses).
                        Select (clnt => new DTO_Client_DetailsClient {
                            IdClient = clnt.IdClient,
                            FirstName = clnt.FirstName,
                            MiddleName = clnt.MiddleName,
                            LastName = clnt.LastName,
                            DateBirth = clnt.DateBirth.ToString (),
                            Gender = clnt.Gender,
                            MaritalStatus = clnt.MaritalStatuses.Status,
                            CodeCurp = clnt.CodeCURP,
                            PersonalAddress = clnt.ClientsAddresses.ExteriorNumber + " " + clnt.ClientsAddresses.ExteriorNumber + " " +
                                              clnt.ClientsAddresses.Street + " " + clnt.ClientsAddresses.Colony + " " + clnt.ClientsAddresses.PostalCode + " " +
                                              clnt.ClientsAddresses.State + ". Tipo de vivienda: " + clnt.ClientsAddresses.AddressesTypes.Type,
                            Email1 = clnt.Email1,
                            Email2 = clnt.Email2,
                            PhoneNumber1 = clnt.PhoneNumber1,
                            PhoneNumber2 = clnt.PhoneNumber2,
                            WorkType = clnt.WorkAreas.WorkAreaTypes.Type,
                            WorkArea = clnt.WorkAreas.WorkArea,
                            MonthlySalary = (float)clnt.WorkAreas.MonthlySalary,
                            Reference1FirstName = clnt.ContactsReferencesClients.FirstName,
                            Reference1MiddleName = clnt.ContactsReferencesClients.MiddleName,
                            Reference1LastName = clnt.ContactsReferencesClients.LastName,
                            Reference1Email = clnt.ContactsReferencesClients.Email,
                            Reference1PhoneNumber = clnt.ContactsReferencesClients.PhoneNumber,
                            Reference1RelationshipType = clnt.ContactsReferencesClients.RelationshipsClientsTypes.Type,
                            Reference2FirstName = clnt.ContactsReferencesClients1.FirstName,
                            Reference2MiddleName = clnt.ContactsReferencesClients1.MiddleName,
                            Reference2LastName = clnt.ContactsReferencesClients1.LastName,
                            Reference2Email = clnt.ContactsReferencesClients1.Email,
                            Reference2PhoneNumber = clnt.ContactsReferencesClients1.PhoneNumber,
                            Reference2RelationshipType = clnt.ContactsReferencesClients1.RelationshipsClientsTypes.Type,
                            CodeRFC = clnt.CodeRFC,
                            BankAccount1Name = clnt.BankAccounts.Banks.Name,
                            BankAccount1CLABE = clnt.BankAccounts.CLABE,
                            BankAccount1CardNumber = clnt.BankAccounts.CardNumber,
                            BankAccount1CardType = clnt.BankAccounts.BankCardTypes.Type,
                            BankAccount2Name = clnt.BankAccounts1.Banks.Name,
                            BankAccount2CLABE = clnt.BankAccounts1.CLABE,
                            BankAccount2CardNumber = clnt.BankAccounts1.CardNumber,
                            BankAccount2CardType = clnt.BankAccounts1.BankCardTypes.Type

                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedDetailsClient != null) {
                        responseConsultClients = MessageResponse<DTO_Client_DetailsClient>.Success (
                            $"Client name \"{retrievedDetailsClient.FirstName}\" retrieved.",
                            retrievedDetailsClient);
                    } else {
                        responseConsultClients = MessageResponse<DTO_Client_DetailsClient>.Failure ("Clients doesn't retrieved");
                    }
                } catch (Exception ex) {
                    responseConsultClients = MessageResponse<DTO_Client_DetailsClient>.Failure (ex.ToString ());
                }
            }
            return responseConsultClients;
        }

        public static async Task<MessageResponse<string>> GetRFCClient (int idClient) {
            MessageResponse<string> responseConsultRFC = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string retrievedConsultRFC = await
                        context.Clients.
                        Where(clnt => clnt.IdClient == idClient).
                        Select (clnt => clnt.CodeRFC).
                        FirstOrDefaultAsync ();

                    if (responseConsultRFC != null) {
                        responseConsultRFC = MessageResponse<string>.Success (
                            $"RFC code \"{responseConsultRFC}\" retrieved.",
                            retrievedConsultRFC);
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