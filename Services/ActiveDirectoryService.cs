
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models.Support;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace kairosApp.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        string path = "LDAP://192.168.253.3";
        DirectorySearcher directorySearcher;
        public string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry(path, "carfesal","EngyCfsm1011");
            try {
                directorySearcher = new DirectorySearcher(de);
            }
            catch(DirectoryServicesCOMException exception) { 
                return exception.Message.ToString();
            }
            

            return "LDAPS://"; //+ de.Properties["defaultNamingContext"][0].ToString();
        }

        public void GetAdditionalUserInfo()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.253.3", "csiusrpw", "T3st*12$");
            var contador = 0;
            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");
            // Email Address
            ds.PropertiesToLoad.Add("employeeid");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            ds.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            ds.PropertiesToLoad.Add("description");

            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                if (sr.Properties["physicalDeliveryOfficeName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad physicalDeliveryOfficeName: " + sr.Properties["physicalDeliveryOfficeName"][0].ToString());
                
                if (sr.Properties["employeeid"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad employeeid: " + sr.Properties["employeeid"][0].ToString());

                if (sr.Properties["name"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad name: " + sr.Properties["name"][0].ToString());

                if (sr.Properties["description"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad description: " + sr.Properties["description"][0].ToString());

                // If not filled in, then you will get an error
                if (sr.Properties["mail"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad mail: " + sr.Properties["mail"][0].ToString());

                if (sr.Properties["givenname"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad givenname: " + sr.Properties["givenname"][0].ToString());

                if (sr.Properties["sn"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad surname: " + sr.Properties["sn"][0].ToString());

                if (sr.Properties["userPrincipalName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad userPrincipalName: " + sr.Properties["userPrincipalName"][0].ToString());

                if (sr.Properties["distinguishedName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad distinguishedName: " + sr.Properties["distinguishedName"][0].ToString());
                contador++;
            }

            Debug.WriteLine("Numero de Usuarios: "+contador);
        }

        /**
         *
         */
        public string GetAUser(string userName)
        {
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.253.3", "csiusrpw", "T3st*12$");
            SearchResult sr;

            // Build User Searcher
            ds = BuildUserSearcher(de);
            // Set the filter to look for a specific user
            ds.Filter = "(&(objectCategory=User)(objectClass=person)(name=" + userName + "))";
            try
            {
                sr = ds.FindOne();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Ha ocurrido un problema: "+ ex.Message);
                return "No Encontrado";
            }

            if (sr != null)
            {
                Debug.WriteLine(sr.GetPropertyValue("name"));
                Debug.WriteLine(sr.GetPropertyValue("mail"));
                Debug.WriteLine(sr.GetPropertyValue("givenname"));
                Debug.WriteLine(sr.GetPropertyValue("sn"));
                Debug.WriteLine(sr.GetPropertyValue("userPrincipalName"));
                Debug.WriteLine(sr.GetPropertyValue("distinguishedName"));
                Debug.WriteLine(sr.GetPropertyValue("physicalDeliveryOfficeName"));
                Debug.WriteLine(sr.GetPropertyValue("telephoneNumber"));
                Debug.WriteLine(sr.GetPropertyValue("employeeid"));
                Debug.WriteLine(sr.GetPropertyValue("title"));
                return "Encontrado";
            }
            return "No encontrado";
        }

        public DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");
            // Email Address
            ds.PropertiesToLoad.Add("mail");
            //physicalDeliveryOfficeName
            ds.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            // First Name
            ds.PropertiesToLoad.Add("givenname");
            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");
            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");
            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");
            // telephoneNumber
            ds.PropertiesToLoad.Add("telephoneNumber");
            // EmployeeID
            ds.PropertiesToLoad.Add("employeeid");
            //Descripcion
            ds.PropertiesToLoad.Add("description");
            return ds;
        }
        public ADToDBUser FindUserByIdentification(string identificacion)
        {
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.253.3", "csiusrpw", "T3st*12$");
            SearchResult result;

            // Build User Searcher
            ds = BuildUserSearcher(de);
            // Set the filter to look for a specific user
            ds.Filter = "(&(objectCategory=User)(objectClass=person)(employeeid=" + identificacion + "))";
            try
            {
                result = ds.FindOne();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ocurrio un error: "+ ex.Message);
                return null;
            }

            if (result != null)
            {
                ADToDBUser paraCrear = new ADToDBUser { CuentaUsuario = new Models.CuentaUsuario(), Persona = new Models.Persona() };
                //Se agrega el username
                Debug.WriteLine(result.GetPropertyValue("name"));
                paraCrear.CuentaUsuario.Username = result.GetPropertyValue("name");

                //Por ahora se agrega el correo espol PD: Se deberia agregar un correo alterno
                Debug.WriteLine(result.GetPropertyValue("mail"));
                paraCrear.Persona.CorreoAlterno = result.GetPropertyValue("mail");
                // Se agregan los nombres
                Debug.WriteLine(result.GetPropertyValue("givenname"));
                paraCrear.Persona.Nombres = result.GetPropertyValue("givenname");

                // Se agregan los apellidos
                Debug.WriteLine(result.GetPropertyValue("sn"));
                paraCrear.Persona.Apellidos = result.GetPropertyValue("sn");

                Debug.WriteLine(result.GetPropertyValue("userPrincipalName"));
                Debug.WriteLine(result.GetPropertyValue("distinguishedName"));

                // Se agrega la Unidad
                Debug.WriteLine(result.GetPropertyValue("physicalDeliveryOfficeName"));
                paraCrear.Persona.Unidad = result.GetPropertyValue("physicalDeliveryOfficeName");

                // Se agrega el numero de telefono
                Debug.WriteLine(result.GetPropertyValue("telephoneNumber"));
                paraCrear.Persona.Telefono = result.GetPropertyValue("telephoneNumber");

                // Se agrega el numero de identificacion guardado
                Debug.WriteLine(result.GetPropertyValue("employeeid"));
                paraCrear.Persona.Identificacion = result.GetPropertyValue("employeeid");

                //Se agregar el rol
                paraCrear.Persona.Rol = result.GetPropertyValue("description");

                //Se crea y se agrega el alias
                paraCrear.CuentaUsuario.Alias = result.GetPropertyValue("givenname").Split(" ")[0].ToLower() + "." + result.GetPropertyValue("sn").Split(" ")[0].ToLower();
                return paraCrear;
            }
            else
            {
                return null;
            }
        }
        public ADToDBUser Login(string userName, string password)
        {
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://192.168.253.3", userName, password))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    // Full Name
                    searcher.PropertiesToLoad.Add("name");

                    // Email Address
                    searcher.PropertiesToLoad.Add("mail");

                    searcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
                    // First Name
                    searcher.PropertiesToLoad.Add("givenname");

                    // Last Name (Surname)
                    searcher.PropertiesToLoad.Add("sn");

                    // Login Name
                    searcher.PropertiesToLoad.Add("userPrincipalName");

                    // Distinguished Name
                    searcher.PropertiesToLoad.Add("distinguishedName");

                    searcher.PropertiesToLoad.Add("telephoneNumber");
                    
                    //Descripcion
                    searcher.PropertiesToLoad.Add("description");

                    // EmployeeID
                    searcher.PropertiesToLoad.Add("employeeid");

                    //Buscamos por la propiedad SamAccountName
                    searcher.Filter = "(samaccountname=" + userName + ")";

                    //Buscamos el usuario con la cuenta indicada
                    SearchResult result = null;
                    try
                    {
                        result = searcher.FindOne();
                    }
                    catch (DirectoryServicesCOMException e)
                    {
                        Debug.WriteLine(e.Message);
                        Debug.WriteLine("Cachea el errorÑ:");
                        return null;
                    }

                    if (result != null)
                    {
                        ADToDBUser paraCrear = new ADToDBUser { CuentaUsuario = new Models.CuentaUsuario(), Persona = new Models.Persona() };
                        //Se agrega el username
                        Debug.WriteLine(result.GetPropertyValue("name"));
                        paraCrear.CuentaUsuario.Username = result.GetPropertyValue("name");

                        //Por ahora se agrega el correo espol PD: Se deberia agregar un correo alterno
                        Debug.WriteLine(result.GetPropertyValue("mail"));
                        paraCrear.Persona.CorreoAlterno = result.GetPropertyValue("mail");
                        // Se agregan los nombres
                        Debug.WriteLine(result.GetPropertyValue("givenname"));
                        paraCrear.Persona.Nombres = result.GetPropertyValue("givenname");

                        // Se agregan los apellidos
                        Debug.WriteLine(result.GetPropertyValue("sn"));
                        paraCrear.Persona.Apellidos = result.GetPropertyValue("sn");

                        Debug.WriteLine(result.GetPropertyValue("userPrincipalName"));
                        Debug.WriteLine(result.GetPropertyValue("distinguishedName"));

                        // Se agrega la Unidad
                        Debug.WriteLine(result.GetPropertyValue("physicalDeliveryOfficeName"));
                        paraCrear.Persona.Unidad = result.GetPropertyValue("physicalDeliveryOfficeName");

                        // Se agrega el numero de telefono
                        Debug.WriteLine(result.GetPropertyValue("telephoneNumber"));
                        paraCrear.Persona.Telefono = result.GetPropertyValue("telephoneNumber");

                        // Se agrega el numero de identificacion guardado
                        Debug.WriteLine(result.GetPropertyValue("employeeid"));
                        paraCrear.Persona.Identificacion = result.GetPropertyValue("employeeid");

                        //Se agregar el rol
                        paraCrear.Persona.Rol = result.GetPropertyValue("description");

                        //Se crea y se agrega el alias
                        paraCrear.CuentaUsuario.Alias = result.GetPropertyValue("givenname").Split(" ")[0].ToLower() + "." + result.GetPropertyValue("sn").Split(" ")[0].ToLower();
                        return paraCrear;

                    }
                    else
                        return null;
                }
            }
        }

               

        public bool ResetPassword(string userName, string password)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, "192.168.253.3", "DC=espol,DC=edu,DC=ec", "csiusrpw", "T3st*12$");
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
                Debug.WriteLine("New Password: " + password);
                if (user == null)
                {
                    return false;
                }
                //Enable Account if it is disabled
                user.Enabled = true;
                //Reset User Password
                user.SetPassword(password);
                //Force user to change password at next logon
                //user.ExpirePasswordNow();
                user.Save();
                return true;
            }
            catch (Exception e)
            {
                throw e;
                Debug.WriteLine("Ocurrio un error: " + e.Message);
                return false;
            }
            

        }

        public bool CreateUser(ADCreateUser user)
        {
            // Creating the PrincipalContext
            PrincipalContext principalContext = null;
            try
            {
                principalContext = new PrincipalContext(ContextType.Domain, "192.168.253.3", "DC=espol,DC=edu,DC=ec", "csiusrpw", "T3st*12$");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to create PrincipalContext. Exception: " + e);
                return false;
            }

            // Check if user object already exists in the store
            UserPrincipal usr = UserPrincipal.FindByIdentity(principalContext, user.Username);
            if (usr != null)
            {
                Debug.WriteLine(user.Username + " already exists. Please use a different User Logon Name.");
                return false;
            }

            // Create the new UserPrincipal object
            UserPrincipalEx userPrincipal = new UserPrincipalEx(principalContext);
            
            if (user.Persona.Apellidos != null && user.Persona.Apellidos.Length > 0)
                userPrincipal.Surname = user.Persona.Apellidos;
                
            if (user.Persona.Nombres != null && user.Persona.Nombres.Length > 0)
                userPrincipal.GivenName = user.Persona.Nombres;
            
            if (user.Persona.Unidad != null && user.Persona.Unidad.Length > 0)
                userPrincipal.PhysicalDeliveryOfficeName = user.Persona.Unidad;

            if (user.Persona.Rol != null && user.Persona.Rol.Length > 0)
                userPrincipal.Description = user.Persona.Rol;
            
            if (user.Persona.Identificacion != null && user.Persona.Identificacion.Length > 0)
                userPrincipal.EmployeeId = user.Persona.Identificacion;
                

            if (user.Username != null && user.Username.Length > 0)
                userPrincipal.EmailAddress = user.Username+"@espol.edu.ec";
                userPrincipal.UserPrincipalName = user.Username+ "@espol.edu.ec";

            if (user.Persona.Telefono != null && user.Persona.Telefono.Length > 0)
                userPrincipal.VoiceTelephoneNumber = user.Persona.Telefono;
            
            if (user.Username != null && user.Username.Length > 0)
                userPrincipal.SamAccountName = user.Username;
                userPrincipal.Name = user.Username;

            
            
            //var pwdOfNewlyCreatedUser = "fweoSD654";
            userPrincipal.SetPassword(user.Password);

            userPrincipal.Enabled = true;
            //userPrincipal.ExpirePasswordNow();

            try
            {
                userPrincipal.Save();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception creating user object. " + e);
                return false;
            }
        }

       
    }

}
