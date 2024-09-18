using FinanciaRed.Model.DTO;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Employee {
        public static async Task<MessageResponse<DTO_Employee_Login>> GetLogin (String email, String password) {
            MessageResponse<DTO_Employee_Login> responseLogin = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Employee_Login retrievedUser = await
                        context.Employees.
                        Where (empl => empl.Email.Equals (email) &&
                                empl.Password.Equals (password)).
                        Select (empl => new DTO_Employee_Login {
                            IdEmployee = empl.IdEmployee,
                            FirstName = empl.FirstName,
                            MiddleName = empl.MiddleName,
                            LastName = empl.LastName,
                            Email = empl.Email,
                            IdRol = empl.RolesEmployees.IdRoleEmployee,
                            Rol = empl.RolesEmployees.Role
                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedUser != null) {
                        responseLogin = MessageResponse<DTO_Employee_Login>.Success (
                            $"Welcome {retrievedUser.FirstName}.",
                            retrievedUser
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

        public static async Task<MessageResponse<DTO_Employee_DetailsEmployee>> GetDetailsEmployee (int idEmployee, bool withPassword) {
            MessageResponse<DTO_Employee_DetailsEmployee> responseLogin = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    DTO_Employee_DetailsEmployee retrievedUser = await
                        context.Employees.
                        Where (empl => empl.IdEmployee == idEmployee).
                        Select (empl => new DTO_Employee_DetailsEmployee {
                            IdEmployee = empl.IdEmployee,
                            FirstName = empl.FirstName,
                            MiddleName = empl.MiddleName,
                            LastName = empl.LastName,
                            Email = empl.Email,
                            Password = withPassword ? empl.Password : "",
                            IdRol = empl.RolesEmployees.IdRoleEmployee,
                            Rol = empl.RolesEmployees.Role
                        }).
                        FirstOrDefaultAsync ();

                    if (retrievedUser != null) {
                        responseLogin = MessageResponse<DTO_Employee_DetailsEmployee>.Success (
                            $"Details of ID {retrievedUser.IdEmployee} retrieved.",
                            retrievedUser
                        );
                    } else {
                        responseLogin = MessageResponse<DTO_Employee_DetailsEmployee>.Failure ("Details doesn't retrieved.");
                    }
                } catch (Exception ex) {
                    responseLogin = MessageResponse<DTO_Employee_DetailsEmployee>.Failure (ex.ToString ());
                }
            }
            return responseLogin;
        }

        public static MessageResponse<int> ModifyDataEmployee (DTO_Employee_ModifyData newDataEmployee) {
            MessageResponse<int> responseModify = null;

            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    Employees currentEmployee = context.Employees.Find (newDataEmployee.IdEmployee);

                    if (currentEmployee != null) {
                        context.Employees.Attach (currentEmployee);
                        currentEmployee.Email = newDataEmployee.Email;
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
    }
}
