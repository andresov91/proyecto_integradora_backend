using System.DirectoryServices.AccountManagement;

namespace kairosApp.Models.Support
{
    
    [DirectoryRdnPrefix("CN")]

    [DirectoryObjectClass("User")]
    public class UserPrincipalEx : UserPrincipal

    {

        // Inplement the constructor using the base class constructor. 

        public UserPrincipalEx(PrincipalContext ctx) : base(ctx)

        { }


        // Create the "PhysicalDeliveryOfficeName " property.    

        [DirectoryProperty("physicalDeliveryOfficeName")]

        public string PhysicalDeliveryOfficeName

        {

            get

            {

                if (ExtensionGet("physicalDeliveryOfficeName").Length != 1)

                    return string.Empty;

                return (string)ExtensionGet("physicalDeliveryOfficeName")[0];

            }

            set { ExtensionSet("physicalDeliveryOfficeName", value); }

        }

    }
}
