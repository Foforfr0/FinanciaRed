using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Employee {
        public static async Task<MessageResponse<DTO_Employee_Login>> GetLogin (String email, String password) {
            MessageResponse<DTO_Employee_Login> responseLogin = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Employee_Login dataRetrieved = await
                        context.Employees.
                        Where (empl => empl.Email.Equals (email) &&
                                empl.Password.Equals (password)).
                        Select (empl => new DTO_Employee_Login {
                            IdEmployee = empl.IdEmployee,
                            FirstName = empl.FirstName,
                            MiddleName = empl.MiddleName,
                            LastName = empl.LastName,
                            ProfilePhoto = empl.ProfilePhoto,
                            IdRol = empl.RolesEmployees.IdRoleEmployee,
                            Rol = empl.RolesEmployees.Role
                        }).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseLogin = MessageResponse<DTO_Employee_Login>.Success (
                            $"Welcome {dataRetrieved.FirstName}.",
                            dataRetrieved
                        );
                    } else {
                        responseLogin = MessageResponse<DTO_Employee_Login>.Failure ("Wrong credentials.");
                    }
                } catch (Exception ex) {
                    responseLogin = MessageResponse<DTO_Employee_Login>.Failure (ex.ToString ());
                }
            }
            return responseLogin;
        }

        public static async Task<MessageResponse<List<DTO_Employee_Consult>>> GetAllEmployees () {
            MessageResponse<List<DTO_Employee_Consult>> responseConsultEmployees = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Employee_Consult> dataRetrieved = await
                        context.Employees.
                        Select (employee => new DTO_Employee_Consult {
                            IdEmployee = employee.IdEmployee,
                            FirstName = employee.FirstName,
                            MiddleName = employee.MiddleName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            Rol = employee.RolesEmployees.Role
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Success (
                            dataRetrieved.Count + " employees retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Failure ("Employess doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultEmployees;
        }

        public static async Task<MessageResponse<List<DTO_Employee_Consult>>> GetFilteredEmployees (string keyWord) {
            MessageResponse<List<DTO_Employee_Consult>> responseConsultEmployees = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    List<DTO_Employee_Consult> dataRetrieved = await
                        context.Employees.
                         Where (empl =>
                            empl.FirstName.Equals (keyWord) ||
                            empl.MiddleName.Equals (keyWord) ||
                            empl.LastName.Equals (keyWord) ||
                            empl.Email.Equals (keyWord) ||
                            empl.RolesEmployees.Role.Equals (keyWord)).
                        Select (employee => new DTO_Employee_Consult {
                            IdEmployee = employee.IdEmployee,
                            FirstName = employee.FirstName,
                            MiddleName = employee.MiddleName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            Rol = employee.RolesEmployees.Role
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Success (
                            dataRetrieved.Count + " employees retrieved.",
                            dataRetrieved);
                    } else {
                        responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Failure ("Employess doesn´t retrieved.");
                    }
                } catch (Exception ex) {
                    responseConsultEmployees = MessageResponse<List<DTO_Employee_Consult>>.Failure (ex.ToString ());
                }
            }
            return responseConsultEmployees;
        }

        public static async Task<MessageResponse<string>> GetStatusEmployee (int idEmployee) {
            MessageResponse<string> responseStatus = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await context.Employees.
                        Where (empl => empl.IdEmployee== idEmployee).
                        Select (clnt => clnt.StatusesEmployee.Status).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        responseStatus = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        responseStatus = MessageResponse<string>.Failure ($"Employee ID {idEmployee} without status.");
                    }
                } catch (Exception ex) {
                    responseStatus = MessageResponse<string>.Failure (ex.ToString ());
                }
            }
            return responseStatus;
        }

        public static async Task<MessageResponse<bool>> ChangeStatusEmployee (int idEmployee, int idStatus) {
            MessageResponse<bool> responseUpdateStatusEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees currentEmployee = context.Employees.Find (idEmployee);

                    if (currentEmployee != null) {
                        context.Employees.Attach (currentEmployee);

                        currentEmployee.IdStatusEmployee = idStatus;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentEmployee).State = EntityState.Modified;
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

                        responseUpdateStatusEmployee = MessageResponse<bool>.Success (
                            $"Employee ID {currentEmployee.IdEmployee} updated", true);
                    } else {
                        responseUpdateStatusEmployee = MessageResponse<bool>.Failure ($"Employee ID {currentEmployee.IdEmployee} doesn´t exists.");
                    }
                } catch (Exception ex) {
                    responseUpdateStatusEmployee = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateStatusEmployee;
        }

        public static async Task<MessageResponse<DTO_Employee_Details>> GetDetailsEmployee (int idEmployee, bool withPassword) {
            MessageResponse<DTO_Employee_Details> responseDetailsEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Employee_Details retrievedData = await
                        context.Employees.
                        Where (empl => empl.IdEmployee == idEmployee).
                        Select (empl => new DTO_Employee_Details {
                            IdEmployee = empl.IdEmployee,
                            ProfilePhoto = empl.ProfilePhoto,
                            FirstName = empl.FirstName,
                            MiddleName = empl.MiddleName,
                            LastName = empl.LastName,
                            DateBirth = empl.DateBirth,
                            Gender = empl.Gender,
                            CodeCURP = empl.CodeCURP,
                            CodeRFC = empl.CodeRFC,
                            Email = empl.Email,
                            Password = withPassword ? empl.Password : "",
                            IdRol = empl.RolesEmployees.IdRoleEmployee,
                            Rol = empl.RolesEmployees.Role
                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedData != null) {
                        responseDetailsEmployee = MessageResponse<DTO_Employee_Details>.Success (
                            $"Details of ID {retrievedData.IdEmployee} retrieved.",
                            retrievedData
                        );
                    } else {
                        responseDetailsEmployee = MessageResponse<DTO_Employee_Details>.Failure ("Details doesn't retrieved.");
                    }
                } catch (Exception ex) {
                    responseDetailsEmployee = MessageResponse<DTO_Employee_Details>.Failure (ex.ToString ());
                }
            }
            return responseDetailsEmployee;
        }

        public static async Task<MessageResponse<bool>> RegistryNewEmployee (DTO_Employee_Details newEmployee) {
            MessageResponse<bool> responseCreateEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees createdEmployee = new Employees {
                        FirstName = newEmployee.FirstName,
                        MiddleName = newEmployee.MiddleName,
                        LastName = newEmployee.LastName,
                        DateBirth = newEmployee.DateBirth,
                        Gender = newEmployee.Gender,
                        CodeCURP = newEmployee.CodeCURP,
                        CodeRFC = newEmployee.CodeRFC,
                        Email = newEmployee.Email,
                        Password = newEmployee.Password,
                        IdRole = newEmployee.IdRol,
                        ProfilePhoto = newEmployee.ProfilePhoto
                    };

                    context.Employees.Add (createdEmployee);
                    await context.SaveChangesAsync ();

                    responseCreateEmployee = MessageResponse<bool>.Success (
                        $"Employee {createdEmployee.FirstName} created", true);
                } catch (Exception ex) {
                    responseCreateEmployee = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseCreateEmployee;
        }

        public static MessageResponse<bool> SaveChangesDataEmployee (DTO_Employee_Details newDataEmployee, bool changePassword) {
            MessageResponse<bool> responseUpdateDataEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees currentEmployee = context.Employees.Find (newDataEmployee.IdEmployee);

                    if (currentEmployee != null) {
                        context.Employees.Attach (currentEmployee);

                        currentEmployee.Gender = newDataEmployee.Gender;
                        currentEmployee.CodeCURP = newDataEmployee.CodeCURP;
                        currentEmployee.CodeRFC = newDataEmployee.CodeRFC;
                        currentEmployee.Email = newDataEmployee.Email;
                        if (changePassword)
                            currentEmployee.Password = newDataEmployee.Password;
                        currentEmployee.ProfilePhoto = newDataEmployee.ProfilePhoto;

                        bool failedSave = false;
                        do {
                            try {
                                context.Entry (currentEmployee).State = EntityState.Modified;
                                context.SaveChanges ();

                            } catch (DbUpdateConcurrencyException ex) {
                                failedSave = true;
                                foreach (var entry in ex.Entries) {
                                    if (entry.Entity is Employees) {
                                        var proposedValues = entry.CurrentValues;
                                        var databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            var databaseEntity = (Employees)databaseValues.ToObject ();
                                            // Actualiza los valores originales con los valores actuales de la base de datos.
                                            entry.OriginalValues.SetValues (databaseValues);
                                            // Decide qué hacer con los valores propuestos.
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (failedSave);
                        responseUpdateDataEmployee = MessageResponse<bool>.Success (
                            $"ID {newDataEmployee.IdEmployee} data employee modified.",
                            true
                            );
                    } else {
                        responseUpdateDataEmployee = MessageResponse<bool>.Failure ("Modification no realized.");
                    }
                } catch (DbEntityValidationException ex) {
                    responseUpdateDataEmployee = MessageResponse<bool>.Failure (ex.StackTrace);
                } catch (Exception ex) {
                    responseUpdateDataEmployee = MessageResponse<bool>.Failure (ex.Message);
                }
            }
            return responseUpdateDataEmployee;
        }

        public static async Task<bool> VerifyExistenceEmail (string email) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    string dataRetrieved = await
                        context.Employees.
                        Where (employee => employee.Email.Equals (email)).
                        Select (employee => employee.Email).
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
                        context.Employees.
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
                        context.Employees.
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
    }
}
