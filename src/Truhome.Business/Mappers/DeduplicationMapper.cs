using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;
using Truhome.Domain.Entities;

namespace Truhome.Business.Mappers
{
    public static class DeduplicationMapper
    {
        public static Customerrequestlog ToCustomerRequestLog(this DeduplicationData request, string? correlationId, string? originSystem)
        {
            return new Customerrequestlog
            {
                Firstname = request.FirstName,
                Middlename = request.MiddleName,
                Lastname = request.LastName,
                Dateofbirth = request.DateOfBirth,
                Mobilenumber = request.MobileNumber,
                Drivinglicensenumber = request.DrivingLicenseNumber,
                Passportnumber = request.PassportNumber,
                Pannumber = request.PanNumber,
                Aadharnumber = request.AadharNumber,
                Ckycnumber = request.CkycNumber,
                Voterid = request.VoterId,
                Fatherfirstname = request.FatherFirstName,
                Fathermiddlename = request.FatherMiddleName,
                Fatherlastname = request.FatherLastName,
                Husbandfirstname = request.HusbandFirstName,
                Husbandmiddlename = request.HusbandMiddleName,
                Husbandlastname = request.HusbandLastName,
                Systemorigin = originSystem,
                Correlationid = correlationId
            };
        }

        public static Match ToMatch(this Customer customer, string matchType)
        {
            DeduplicationData fields = new DeduplicationData
            {
                FirstName = customer.Firstname,
                MiddleName = customer.Middlename,
                LastName = customer.Lastname,
                DateOfBirth = customer.Dateofbirth,
                MobileNumber = customer.Mobilenumber,
                DrivingLicenseNumber = customer.Drivinglicensenumber,
                PassportNumber = customer.Passportnumber,
                PanNumber = customer.Pannumber,
                AadharNumber = customer.Aadharnumber,
                CkycNumber = customer.Ckycnumber,
                VoterId = customer.Voterid,
                FatherFirstName = customer.Fatherfirstname,
                FatherMiddleName = customer.Fathermiddlename,
                FatherLastName = customer.Fatherlastname,
                HusbandFirstName = customer.Husbandfirstname,
                HusbandMiddleName = customer.Husbandmiddlename,
                HusbandLastName = customer.Husbandlastname
            };

            return new Match
            {
                ExistingId = customer.Id.ToString(),
                MatchType = matchType,
                Fields = fields
            };
        }
    }
}
