using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_GeneralVariables {
        public static async Task<MessageResponse<List<DTO_MaritalStatus>>> GetAllMaritalStatuses () {
            MessageResponse<List<DTO_MaritalStatus>> responseConsultMaritalStatuses = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_MaritalStatus> dataRetrieved = await
                        context.MaritalStatuses.
                        Select (ms => new DTO_MaritalStatus {
                            IdMaritalStatus = ms.IdMaritalStatus,
                            Status = ms.Status
                        }).
                        OrderBy (ms => ms.IdMaritalStatus).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultMaritalStatuses = MessageResponse<List<DTO_MaritalStatus>>.Success (
                            dataRetrieved.Count + " marital statuses retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultMaritalStatuses = MessageResponse<List<DTO_MaritalStatus>>.Failure ("Marital statuses doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultMaritalStatuses = MessageResponse<List<DTO_MaritalStatus>>.Failure (ex.ToString ());
                }
            }
            return responseConsultMaritalStatuses;
        }

        public static async Task<MessageResponse<List<DTO_AddressState>>> GetAllAddressStates () {
            MessageResponse<List<DTO_AddressState>> responseConsultStatesAddress = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_AddressState> dataRetrieved = await
                        context.StatesAddresses.
                        Select (state => new DTO_AddressState {
                            IdAddressState = state.IdStateAddress,
                            Name = state.Name
                        }).
                        OrderBy (wt => wt.IdAddressState).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultStatesAddress = MessageResponse<List<DTO_AddressState>>.Success (
                            dataRetrieved.Count + " states address retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultStatesAddress = MessageResponse<List<DTO_AddressState>>.Failure ("States address doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultStatesAddress = MessageResponse<List<DTO_AddressState>>.Failure (ex.ToString ());
                }
            }
            return responseConsultStatesAddress;
        }

        public static async Task<MessageResponse<List<DTO_WorkType>>> GetAllWorkTypes () {
            MessageResponse<List<DTO_WorkType>> responseConsultWorkType = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_WorkType> dataRetrieved = await
                        context.WorkAreaTypes.
                        Select (wt => new DTO_WorkType {
                            IdWorkType = wt.IdWorkAreaType,
                            Type = wt.Type
                        }).
                        OrderBy (wt => wt.IdWorkType).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultWorkType = MessageResponse<List<DTO_WorkType>>.Success (
                            dataRetrieved.Count + " work types retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultWorkType = MessageResponse<List<DTO_WorkType>>.Failure ("Work types doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultWorkType = MessageResponse<List<DTO_WorkType>>.Failure (ex.ToString ());
                }
            }
            return responseConsultWorkType;
        }

        public static async Task<MessageResponse<List<DTO_RelationshipType>>> GetAllRelationshipsTypes () {
            MessageResponse<List<DTO_RelationshipType>> responseConsultRelationTypes = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_RelationshipType> dataRetrieved = await
                        context.RelationshipsClientsTypes.
                        Select (rst => new DTO_RelationshipType {
                            IdRelationshipType = rst.IdRelationshipClient,
                            Type = rst.Type
                        }).
                        OrderBy (rst => rst.IdRelationshipType).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultRelationTypes = MessageResponse<List<DTO_RelationshipType>>.Success (
                            dataRetrieved.Count + " relationships types retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultRelationTypes = MessageResponse<List<DTO_RelationshipType>>.Failure ("Relationships types doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultRelationTypes = MessageResponse<List<DTO_RelationshipType>>.Failure (ex.ToString ());
                }
            }
            return responseConsultRelationTypes;
        }

        public static async Task<MessageResponse<List<DTO_Bank>>> GetAllBanks () {
            MessageResponse<List<DTO_Bank>> responseConsultBanks = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Bank> dataRetrieved = await
                        context.Banks.
                        Select (bank => new DTO_Bank {
                            IdBank = bank.IdBank,
                            Name = bank.Name
                        }).
                        OrderBy (bank => bank.IdBank).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultBanks = MessageResponse<List<DTO_Bank>>.Success (
                            dataRetrieved.Count + " banks retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultBanks = MessageResponse<List<DTO_Bank>>.Failure ("Banks doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultBanks = MessageResponse<List<DTO_Bank>>.Failure (ex.ToString ());
                }
            }
            return responseConsultBanks;
        }

        public static async Task<MessageResponse<List<DTO_CardType>>> GetAllCardTypes () {
            MessageResponse<List<DTO_CardType>> responseConsultCardTypes = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_CardType> dataRetrieved = await
                        context.BankCardTypes.
                        Select (type => new DTO_CardType {
                            IdCardType = type.IdBankCardType,
                            Type = type.Type
                        }).
                        OrderBy (type => type.IdCardType).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultCardTypes = MessageResponse<List<DTO_CardType>>.Success (
                            dataRetrieved.Count + " card types retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultCardTypes = MessageResponse<List<DTO_CardType>>.Failure ("Card types doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultCardTypes = MessageResponse<List<DTO_CardType>>.Failure (ex.ToString ());
                }
            }
            return responseConsultCardTypes;
        }

        public static async Task<MessageResponse<List<DTO_AddressType>>> GetAllAdressesTypes () {
            MessageResponse<List<DTO_AddressType>> responseConsultAddressTypes = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_AddressType> dataRetrieved = await
                        context.AddressesTypes.
                        Select (type => new DTO_AddressType {
                            IdAddressType = type.IdAddressType,
                            Type = type.Type
                        }).
                        OrderBy (type => type.IdAddressType).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultAddressTypes = MessageResponse<List<DTO_AddressType>>.Success (
                            dataRetrieved.Count + " addresses types retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultAddressTypes = MessageResponse<List<DTO_AddressType>>.Failure ("Addresses types doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultAddressTypes = MessageResponse<List<DTO_AddressType>>.Failure (ex.ToString ());
                }
            }
            return responseConsultAddressTypes;
        }

        public static async Task<MessageResponse<List<DTO_EmployeeRol>>> GetAllEmployeeRoles () {
            MessageResponse<List<DTO_EmployeeRol>> responseConsultAddressTypes = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_EmployeeRol> dataRetrieved = await
                        context.RolesEmployees.
                        Select (rol => new DTO_EmployeeRol {
                            IdRol = rol.IdRoleEmployee,
                            Rol = rol.Role
                        }).
                        OrderBy (rol => rol.IdRol).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultAddressTypes = MessageResponse<List<DTO_EmployeeRol>>.Success (
                            dataRetrieved.Count + " roles retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultAddressTypes = MessageResponse<List<DTO_EmployeeRol>>.Failure ("Roles employee doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultAddressTypes = MessageResponse<List<DTO_EmployeeRol>>.Failure (ex.ToString ());
                }
            }
            return responseConsultAddressTypes;
        }
    }
}
