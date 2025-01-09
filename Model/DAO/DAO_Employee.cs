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
        public static async Task<MessageResponse<DTO_Employee_Login>> GetAsync (String email, String password) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_Employee_Login> response = null;
                try {
                    DTO_Employee_Login dataRetrieved = await context.Employees.
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
                        response = MessageResponse<DTO_Employee_Login>.Success (
                            $"Bienvenid@ {dataRetrieved.FirstName}.",
                            dataRetrieved
                        );
                    } else {
                        response = MessageResponse<DTO_Employee_Login>.Failure ("Credenciales incorrectas.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_Employee_Login>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_Employee_Consult>>> GetAsync () {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_Employee_Consult>> response = null;
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
                        response = MessageResponse<List<DTO_Employee_Consult>>.Success (
                            dataRetrieved.Count + " Empleados obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_Employee_Consult>>.Failure ("No se logró obtener la lista de Empleado.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_Employee_Consult>>.Failure (ex.ToString ());
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_Employee_Consult>>> GetAsync (string keyWord) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_Employee_Consult>> response = null;
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
                        response = MessageResponse<List<DTO_Employee_Consult>>.Success (
                            dataRetrieved.Count + " Empleado obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_Employee_Consult>>.Failure ("No se logró obtener la lista de Empleados.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_Employee_Consult>>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<string>> GetStatusAsync (int idEmployee) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<string> response = null;
                try {
                    string dataRetrieved = await context.Employees.
                        Where (empl => empl.IdEmployee == idEmployee).
                        Select (clnt => clnt.StatusesEmployee.Status).
                        FirstOrDefaultAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<string>.Success (
                            dataRetrieved,
                            dataRetrieved);
                    } else {
                        response = MessageResponse<string>.Failure ($"Empleado ID {idEmployee} sin estado.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<string>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutStatusAsync (int idEmployee, int idStatus) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
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
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Employees databaseEntity = (Employees)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);

                        response = MessageResponse<bool>.Success (
                            $"Empleado ID {currentEmployee.IdEmployee} actualizado.", true);
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener Empleado ID {currentEmployee.IdEmployee}.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_Employee_Details>> GetAsync (int idEmployee, bool withPassword) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_Employee_Details> response = null;
                try {
                    DTO_Employee_Details retrievedData = await context.Employees.
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
                        response = MessageResponse<DTO_Employee_Details>.Success (
                            $"{retrievedData.IdEmployee} Empleado obtenido.",
                            retrievedData
                        );
                    } else {
                        response = MessageResponse<DTO_Employee_Details>.Failure ("No se logró obtener el Empleado.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_Employee_Details>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PostAsync (DTO_Employee_Details newEmployee) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
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
                        ProfilePhoto = newEmployee.ProfilePhoto,
                        IdStatusEmployee = 1
                    };

                    context.Employees.Add (createdEmployee);
                    await context.SaveChangesAsync ();

                    response = MessageResponse<bool>.Success (
                        $"Empleado {createdEmployee.FirstName} creado.", true);
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static MessageResponse<bool> PutAsync (DTO_Employee_Details newDataEmployee, bool changePassword) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
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
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Employees) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Employees databaseEntity = (Employees)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (failedSave);
                        response = MessageResponse<bool>.Success (
                            $"Empleado ID {newDataEmployee.IdEmployee} actualizado.",
                            true
                            );
                    } else {
                        response = MessageResponse<bool>.Failure ($"No se logró obtener el Empleado Id {currentEmployee.IdEmployee}.");
                    }
                } catch (DbEntityValidationException ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
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
