using Truhome.Business.Enums;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;
using Truhome.Domain.Entities;

namespace Truhome.Business.Mappers
{
    public static class Mapper
    {
        public static Customeraudit ToCustomerAudit(this DeduplicationData request, string? correlationId, string? originSystem)
        {
            return new Customeraudit
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
                Ckycid = request.CkycId,
                Voterid = request.VoterId,
                Fatherfirstname = request.FatherFirstName,
                Fathermiddlename = request.FatherMiddleName,
                Fatherlastname = request.FatherLastName,
                Spousefirstname = request.SpouseFirstName,
                Spousemiddlename = request.SpouseMiddleName,
                Spouselastname = request.SpouseLastName,
                Sourcesystem = originSystem,
                Correlationid = correlationId,
                Mothermaidenname = request.MotherMaidenName,
                Gender = request.Gender,
                Cin=request.Gender,
                Alternatemobilenumber = request.AlternateMobileNumber,
                Companyname = request.CompanyName,
                Customerid= request.CustomerId,
                Customertype = (short)(CustomerType)Enum.Parse(typeof(CustomerType), request.CustomerType),
                Emailid=request.EmailId
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
                CkycId = customer.Ckycid,
                VoterId = customer.Voterid,
                FatherFirstName = customer.Fatherfirstname,
                FatherMiddleName = customer.Fathermiddlename,
                FatherLastName = customer.Fatherlastname,
                SpouseFirstName = customer.Spousefirstname,
                SpouseMiddleName = customer.Spousemiddlename,
                SpouseLastName = customer.Spouselastname,
                AlternateMobileNumber = customer.Alternatemobilenumber,
                CIn= customer.Cin,
                CompanyName = customer.Companyname,
                CustomerType= Enum.GetName(typeof(CustomerType), customer.Customertype),
                EmailId= customer.Emailid,
                Gender= customer.Gender,
                MotherMaidenName= customer.Mothermaidenname,
                SourceSystem= customer.Sourcesystem,
                CustomerId = customer.Id
            };

            return new Match
            {
                ExistingId = customer.Id.ToString(),
                MatchType = matchType,
                Fields = fields
            };
        }

        public static Customer ToCustomer(this DeduplicationData customer)
        {
            Customer fields = new Customer
            {
                Firstname = customer.FirstName,
                Middlename = customer.MiddleName,
                Lastname = customer.LastName,
                Dateofbirth = customer.DateOfBirth,
                Mobilenumber = customer.MobileNumber,
                Drivinglicensenumber = customer.DrivingLicenseNumber,
                Passportnumber = customer.PassportNumber,
                Pannumber = customer.PanNumber,
                Aadharnumber = customer.AadharNumber,
                Ckycid = customer.CkycId,
                Voterid = customer.VoterId,
                Fatherfirstname = customer.FatherFirstName,
                Fathermiddlename = customer.FatherMiddleName,
                Fatherlastname = customer.FatherLastName,
                Spousefirstname = customer.SpouseFirstName,
                Spousemiddlename = customer.SpouseMiddleName,
                Spouselastname = customer.SpouseLastName,
                Alternatemobilenumber = customer.AlternateMobileNumber,
                Cin= customer.CIn,
                Companyname = customer.CompanyName,
                Customertype= (short)(CustomerType)Enum.Parse(typeof(CustomerType), customer.CustomerType),
                Emailid= customer.EmailId,
                Gender= customer.Gender,
                Mothermaidenname= customer.MotherMaidenName,
                Sourcesystem= customer.SourceSystem,
                Id = customer.CustomerId
            };

            return fields;
        }

    }
}
