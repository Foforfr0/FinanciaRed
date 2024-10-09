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

        public static async Task<MessageResponse<DTO_Employee_DetailsEmployee>> GetDetailsEmployee (int idEmployee, bool withPassword) {
            MessageResponse<DTO_Employee_DetailsEmployee> responseDetailsEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Employee_DetailsEmployee retrievedData = await
                        context.Employees.
                        Where (empl => empl.IdEmployee == idEmployee).
                        Select (empl => new DTO_Employee_DetailsEmployee {
                            IdEmployee = empl.IdEmployee,
                            FirstName = empl.FirstName,
                            MiddleName = empl.MiddleName,
                            LastName = empl.LastName,
                            ProfilePhoto = empl.ProfilePhoto,
                            Email = empl.Email,
                            Password = withPassword ? empl.Password : "",
                            IdRol = empl.RolesEmployees.IdRoleEmployee,
                            Rol = empl.RolesEmployees.Role
                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedData != null) {
                        responseDetailsEmployee = MessageResponse<DTO_Employee_DetailsEmployee>.Success (
                            $"Details of ID {retrievedData.IdEmployee} retrieved.",
                            retrievedData
                        );
                    } else {
                        responseDetailsEmployee = MessageResponse<DTO_Employee_DetailsEmployee>.Failure ("Details doesn't retrieved.");
                    }
                } catch (Exception ex) {
                    responseDetailsEmployee = MessageResponse<DTO_Employee_DetailsEmployee>.Failure (ex.ToString ());
                }
            }
            return responseDetailsEmployee;
        }

        public static MessageResponse<bool> SaveChangesDataEmployee (DTO_Employee_DetailsEmployee newDataEmployee) {
            MessageResponse<bool> responseUpdateDataEmployee = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees currentEmployee = context.Employees.Find (newDataEmployee.IdEmployee);

                    if (currentEmployee != null) {
                        context.Employees.Attach (currentEmployee);

                        currentEmployee.FirstName = newDataEmployee.FirstName;
                        currentEmployee.MiddleName = newDataEmployee.MiddleName;
                        currentEmployee.LastName = newDataEmployee.LastName;
                        currentEmployee.Email = newDataEmployee.Email;
                        currentEmployee.IdRole = newDataEmployee.IdRol;
                        currentEmployee.ProfilePhoto = newDataEmployee.ProfilePhoto;

                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentEmployee).State = EntityState.Modified;
                                context.SaveChanges ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (var entry in ex.Entries) {
                                    if (entry.Entity is Match) {
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
                        } while (SaveFailed);

                        responseUpdateDataEmployee = MessageResponse<bool>.Success (
                            $"Employee ID {currentEmployee.IdEmployee} updated", true);
                    } else {
                        responseUpdateDataEmployee = MessageResponse<bool>.Failure ($"Employee ID {currentEmployee.IdEmployee} doesn´t exists");
                    }
                } catch (Exception ex) {
                    responseUpdateDataEmployee = MessageResponse<bool>.Failure ("Exception" + ex.Message);
                }
            }
            return responseUpdateDataEmployee;
        }

        public static MessageResponse<int> ModifyDataEmployee (DTO_Employee_ModifyData newDataEmployee, bool changePassword) {
            MessageResponse<int> responseModify = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees currentEmployee = context.Employees.Find (newDataEmployee.IdEmployee);

                    if (currentEmployee != null) {
                        context.Employees.Attach (currentEmployee);

                        currentEmployee.ProfilePhoto = newDataEmployee.ProfilePhoto;
                        currentEmployee.Email = newDataEmployee.Email;
                        if (changePassword)
                            currentEmployee.Password = newDataEmployee.Password;

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
                        responseModify = MessageResponse<int>.Success (
                            $"ID {newDataEmployee.IdEmployee} data employee modified.",
                            1
                            );
                    } else {
                        responseModify = MessageResponse<int>.Failure ("Modification no realized.");
                    }
                } catch (Exception ex) {
                    responseModify = MessageResponse<int>.Failure (ex.ToString ());
                }
            }
            return responseModify;
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
    }
}
