using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Credit;
using FinanciaRed.Model.Model_Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanciaRed.Model.DAO {
    internal class DAO_Credit {
        public static async Task<MessageResponse<List<DTO_Credit_Consult>>> GetAsync () {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_Credit_Consult>> response = null;
                try {
                    List<DTO_Credit_Consult> dataRetrieved = await
                        context.Credits.
                        Select (crdt => new DTO_Credit_Consult {
                            IdCredit = crdt.IdCredit,
                            AmountTotal = crdt.CreditApplications.AmountTotal,
                            AmountTotalS = "$ " + (crdt.CreditApplications.AmountTotal).ToString (),
                            AmountLeft = crdt.AmountLeft,
                            AmountLeftS = "$ " + (crdt.AmountLeft).ToString (),
                            DateStart = crdt.DateStart,
                            DateEnd = crdt.DateEnd,
                            InterestRate = crdt.CreditApplications.Promotions.InterestRate,
                            InterestPercentaje = (crdt.CreditApplications.Promotions.InterestRate * 100).ToString () + " %",
                            NumberFortnigths = crdt.CreditApplications.Promotions.NumberFortnights,
                            IdStatus = crdt.IdStatusCredit,
                            Status = crdt.StatusesCredit.Status
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_Credit_Consult>>.Success (
                            dataRetrieved.Count + " créditos obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_Credit_Consult>>.Failure ("No se logró obtener la lista de créditos.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_Credit_Consult>>.Failure (ex.ToString ());
                }
                return response;
            }
        }

        public static async Task<MessageResponse<List<DTO_Credit_Consult>>> GetAsync (string keyWord) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<List<DTO_Credit_Consult>> response = null;
                try {
                    List<DTO_Credit_Consult> dataRetrieved = await
                        context.Credits.
                        Where (crdt =>
                            crdt.StatusesCredit.Status.Contains (keyWord) ||
                            crdt.AmountLeft.ToString ().Contains (keyWord) ||
                            crdt.DateStart.ToString ().Contains (keyWord) ||
                            crdt.DateEnd.ToString ().Contains (keyWord) ||
                            crdt.CreditApplications.AmountTotal.ToString ().Contains (keyWord) ||
                            crdt.CreditApplications.Promotions.InterestRate.ToString ().Contains (keyWord) ||
                            crdt.CreditApplications.Promotions.NumberFortnights.ToString ().Contains (keyWord)
                        ).
                        Select (crdt => new DTO_Credit_Consult {
                            IdCredit = crdt.IdCredit,
                            AmountTotal = crdt.CreditApplications.AmountTotal,
                            AmountTotalS = "$ " + (crdt.CreditApplications.AmountTotal).ToString (),
                            AmountLeft = crdt.AmountLeft,
                            AmountLeftS = "$ " + (crdt.AmountLeft).ToString (),
                            DateStart = crdt.DateStart,
                            DateEnd = crdt.DateEnd,
                            InterestRate = crdt.CreditApplications.Promotions.InterestRate,
                            InterestPercentaje = (crdt.CreditApplications.Promotions.InterestRate * 100).ToString () + " %",
                            NumberFortnigths = crdt.CreditApplications.Promotions.NumberFortnights,
                            IdStatus = crdt.StatusesCredit.IdStatusCredit,
                            Status = crdt.StatusesCredit.Status
                        }).
                        ToListAsync ();

                    if (dataRetrieved != null) {
                        response = MessageResponse<List<DTO_Credit_Consult>>.Success (
                            dataRetrieved.Count + " Créditos obtenidos.",
                            dataRetrieved);
                    } else {
                        response = MessageResponse<List<DTO_Credit_Consult>>.Failure ("No de logró obtener la lista de Créditos.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<List<DTO_Credit_Consult>>.Failure (ex.ToString ());
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_Credit_Details>> GetDetailsAsync (int idCredit) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_Credit_Details> response = null;
                try {
                    DTO_Credit_Details dataRetrieved = await context.Credits.
                        Where (crd => crd.IdCredit == idCredit).
                        Select (crd => new DTO_Credit_Details {
                            IdCredit = crd.IdCredit,
                            AmountLeft = crd.AmountLeft ?? 0,
                            DateStart = crd.DateStart,
                            DateEnd = crd.DateEnd,
                            IdStatus = crd.StatusesCredit.IdStatusCredit,
                            Status = crd.StatusesCredit.Status,
                            SignedDocument = crd.SignedDocument,
                            PaymentLayout = crd.PaymentLayout,
                            CreditApplication = new DTO_CreditApplication_Details {
                                IdCreditApplication = crd.CreditApplications.IdCreditApplication,
                                AmountSolicited = crd.CreditApplications.AmountTotal,
                                InterestRate = crd.CreditApplications.Promotions.InterestRate,
                                DateSolicited = crd.CreditApplications.DateApplication,
                                DateAccepted = crd.CreditApplications.DateAcepted ?? crd.CreditApplications.DateApplication
                            }
                        }).
                        FirstOrDefaultAsync ();
                    if (dataRetrieved != null) {
                        response = MessageResponse<DTO_Credit_Details>.Success (
                            $"Credito ID {dataRetrieved.IdCredit} obtenido.",
                            dataRetrieved
                        );
                    } else {
                        response = MessageResponse<DTO_Credit_Details>.Failure ($"No se logró obtener los datos del crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_Credit_Details>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> GetStatusCreditClientAsync (int idClient) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                try {
                    int dataRetrieved = await context.Credits.
                        Where (cred => cred.CreditApplications.Clients.IdClient == idClient).
                        CountAsync ();

                    if (dataRetrieved > 0) {
                        return MessageResponse<bool>.Success ("Cliente tiene un crédito activo.", true);
                    } else {
                        return MessageResponse<bool>.Failure ("Cliente no tiene un crédito activo.");
                    }
                } catch (Exception ex) {
                    return MessageResponse<bool>.Failure (ex.ToString ());
                }
            }
        }

        public static async Task<MessageResponse<int>> PutStatusCreditsClientAsync (int idClient, int idNewStatus) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<int> response;
                try {
                    List<Credits> relatedCredits = await context.Credits.
                        Where (credit => credit.CreditApplications.Clients.IdClient == idClient).
                        ToListAsync ();
                    foreach (Credits credit in relatedCredits) {
                        credit.IdStatusCredit = idNewStatus;
                    }

                    await context.SaveChangesAsync ();
                    response = MessageResponse<int>.Success (
                        $"Se ha cambiado el estado de {relatedCredits.Count} créditos.", relatedCredits.Count);
                } catch (Exception ex) {
                    response = MessageResponse<int>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<DTO_StatusCredit>> GetStatusCreditAsync (int idCredit) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<DTO_StatusCredit> response = null;
                try {
                    Credits currentCredit = await context.Credits.FindAsync (idCredit);
                    if (currentCredit != null) {
                        response = MessageResponse<DTO_StatusCredit>.Success ("Se ha obtenido el estado del crédito.",
                            new DTO_StatusCredit {
                                IdStatus = currentCredit.StatusesCredit.IdStatusCredit,
                                Status = currentCredit.StatusesCredit.Status
                            });
                    } else {
                        response = MessageResponse<DTO_StatusCredit>.Failure ("No se logró obtener el crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<DTO_StatusCredit>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PutStatusCreditAsync (int idCredit, int idStatus) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Credits currentCredit = await context.Credits.FindAsync (idCredit);
                    if (currentCredit != null) {
                        context.Credits.Attach (currentCredit);

                        currentCredit.IdStatusCredit = idStatus;

                        await context.SaveChangesAsync ();
                        bool SaveFailed = false;
                        do {
                            try {
                                context.Entry (currentCredit).State = EntityState.Modified;
                                await context.SaveChangesAsync ();

                            } catch (DbUpdateConcurrencyException ex) {
                                SaveFailed = true;
                                foreach (DbEntityEntry entry in ex.Entries) {
                                    if (entry.Entity is Match) {
                                        DbPropertyValues proposedValues = entry.CurrentValues;
                                        DbPropertyValues databaseValues = entry.GetDatabaseValues ();

                                        if (databaseValues != null) {
                                            Credits databaseEntity = (Credits)databaseValues.ToObject ();
                                            entry.OriginalValues.SetValues (databaseValues);
                                            entry.CurrentValues.SetValues (proposedValues);
                                        }
                                    }
                                }
                            }
                        } while (SaveFailed);
                        response = MessageResponse<bool>.Success ("Se ha cambiado el estado del crédito.", true);
                    } else {
                        response = MessageResponse<bool>.Failure ("No se logró obtener el crédito.");
                    }
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }

        public static async Task<MessageResponse<bool>> PostAsync (DTO_CreditApplication_Details newCredit) {
            /*
             * Se crea un Credito pero sin inciar, hace falta el documento firmado por el cliente
             */
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<bool> response = null;
                try {
                    Credits createdCredit = new Credits {
                        AmountLeft = newCredit.AmountSolicited,
                        IdStatusCredit = 1,
                        SignedDocument = null,
                        PaymentLayout = null,
                        DateStart = null,
                        DateEnd = null,
                        IdCreditApplication = newCredit.IdCreditApplication,
                    };

                    context.Credits.Add (createdCredit);
                    await context.SaveChangesAsync ();

                    response = MessageResponse<bool>.Success (
                        $"Credit created.", true);
                } catch (Exception ex) {
                    response = MessageResponse<bool>.Failure ("Exception: " + ex.Message);
                }
                return response;
            }
        }

        public static async Task<MessageResponse<byte[]>> GetPaymentLayoutAsync (int idCredit) {
            using (FinanciaRedEntities context = new FinanciaRedEntities ()) {
                MessageResponse<byte[]> response = null;
                try {
                    byte[] dataRetrieved = await context.Credits.
                        Where (crd => crd.IdCredit == idCredit).
                        Select (crd => crd.PaymentLayout).
                        FirstOrDefaultAsync ();
                    if (dataRetrieved != null)
                        response = MessageResponse<byte[]>.Success ("Se logró obtener registro de pagos.", dataRetrieved);
                    else
                        response = MessageResponse<byte[]>.Failure ("No se logró obtener el registro de pagos.");
                } catch (Exception ex) {
                    response = MessageResponse<byte[]>.Failure ($"Error inesperado: {ex.Message}");
                }
                return response;
            }
        }
    }
}
