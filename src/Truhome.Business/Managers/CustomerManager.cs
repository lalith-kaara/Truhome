using Microsoft.EntityFrameworkCore;
using System.Data;
using Truhome.Business.Comparers;
using Truhome.Business.Exceptions;
using Truhome.Business.Interfaces;
using Truhome.Business.Mappers;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;
using Truhome.Domain.Contexts;
using Truhome.Domain.Entities;
using MatchType = Truhome.Business.Enums.MatchType;

namespace Truhome.Business.Managers;

public class CustomerManager : ICustomerManager
{
    private readonly TruhomeDbContext _dbContext;
    public CustomerManager(TruhomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeduplicationResponse> CheckDeduplicationAsync(DeduplicationData request, string? correlationId, string? originSystem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(correlationId);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(originSystem);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(request.ExternalCustomerId);

        await _dbContext.Customeraudits.AddAsync(request.ToCustomerAudit(correlationId, originSystem), cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var exactlyMatchedCustomers = await FetchExactMatchCustomers(request, cancellationToken).ConfigureAwait(false);

        var ambigousMatchedCustomers = await FetchAmbiguousMatchCustomer(request, cancellationToken).ConfigureAwait(false);

        List<Match> matches = new List<Match>();

        matches.AddRange(exactlyMatchedCustomers.AsParallel().Select(x => x.ToMatch(MatchType.Exact.ToString())).ToList());

        matches.AddRange(ambigousMatchedCustomers.AsParallel().Select(x => x.ToMatch(MatchType.Ambiguous.ToString())).ToList());

        matches = matches.Distinct(new MatchCustomerIdComparer()).ToList();

        if (!matches.Any())
        {
            Customer customer = await CreateCustomerAsync(request, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.ExternalCustomerId))
            {
                await CreateCustomerMappingAsync(customer.Id, request.ExternalCustomerId, cancellationToken);
            }

            return new DeduplicationResponse
            {
                MatchCount = matches.Count,
                Matches = null
            };
        }
        else
        {
            return new DeduplicationResponse
            {
                MatchCount = matches.Count,
                Matches = matches
            };
        }
    }

    private async Task<List<Customer>> FetchExactMatchCustomers(DeduplicationData request, CancellationToken cancellationToken)
    {
        var exactMatchQuery = _dbContext.Customers.AsNoTracking().Where(c =>
            // Rule 1
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             request.DateOfBirth != null &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             c.Dateofbirth == request.DateOfBirth)

            ||

            // Rule 2
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             request.MobileNumber.HasValue &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             c.Mobilenumber == request.MobileNumber)

            ||

            // Rule 3
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             request.DateOfBirth.HasValue &&
             request.MobileNumber.HasValue &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             c.Dateofbirth == request.DateOfBirth &&
             c.Mobilenumber == request.MobileNumber)

            ||

            // Rule 4
            (!string.IsNullOrWhiteSpace(request.LastName) &&
             request.DateOfBirth.HasValue &&
             request.MobileNumber.HasValue &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             c.Dateofbirth == request.DateOfBirth &&
             c.Mobilenumber == request.MobileNumber)

            ||

            // Rule 5
            (!string.IsNullOrWhiteSpace(request.PassportNumber) &&
             !string.IsNullOrWhiteSpace(c.Passportnumber) &&
             c.Passportnumber.ToLower() == request.PassportNumber.ToLower())

            ||

            // Rule 6
            (!string.IsNullOrWhiteSpace(request.DrivingLicenseNumber) &&
             !string.IsNullOrWhiteSpace(c.Drivinglicensenumber) &&
             c.Drivinglicensenumber.ToLower() == request.DrivingLicenseNumber.ToLower())

            ||

            // Rule 7
            (!string.IsNullOrWhiteSpace(request.VoterId) &&
             !string.IsNullOrWhiteSpace(c.Voterid) &&
             c.Voterid.ToLower() == request.VoterId.ToLower())

            ||

            // Rule 8
            (!string.IsNullOrWhiteSpace(request.PanNumber) &&
             !string.IsNullOrWhiteSpace(c.Pannumber) &&
             c.Pannumber.ToLower() == request.PanNumber.ToLower())

            ||

            // Rule 9
            (!string.IsNullOrWhiteSpace(request.AadharNumber) &&
             !string.IsNullOrWhiteSpace(c.Aadharnumber) &&
             c.Aadharnumber.ToLower() == request.AadharNumber.ToLower())

            ||

            // Rule 10
            (!string.IsNullOrWhiteSpace(request.CkycId) &&
             !string.IsNullOrWhiteSpace(c.Ckycid) &&
             c.Ckycid.ToLower() == request.CkycId.ToLower())
        );

        return await exactMatchQuery.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    private async Task<List<Customer>> FetchAmbiguousMatchCustomer(DeduplicationData request, CancellationToken cancellationToken)
    {
        var ambiguousMatchQuery = _dbContext.Customers.AsNoTracking().Where(c =>
            // Rule 1
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

            ||

            // Rule 2
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

            ||

            // Rule 3
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

            ||

            // Rule 4
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

            ||

            // Rule 5
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

            ||

            // Rule 6
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

            ||

            //Rule 7: in the ambiguous rule list it is duplicate

            // Rule 8
            (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

            ||

            // Rule 9
            (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

            ||

            // Rule 10
            (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

             ||

             //Rule 11
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

             ||

             //Rule 12
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

             ||

             //Rule 13
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower())))

             ||

             //Rule 14
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

             ||

             //Rule 15
             (!string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

             ||

             //Rule 16
             (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower())))

             ||

             //Rule 17

             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower()))

             ||

             //Rule 18
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Fathermiddlename) &&
             c.Fathermiddlename.ToLower() == request.FatherMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.FatherLastName) &&
             !string.IsNullOrWhiteSpace(c.Fatherlastname) &&
             c.Fatherlastname.ToLower() == request.FatherLastName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseMiddleName) &&
             !string.IsNullOrWhiteSpace(c.Spousemiddlename) &&
             c.Spousemiddlename.ToLower() == request.SpouseMiddleName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.SpouseLastName) &&
             !string.IsNullOrWhiteSpace(c.Spouselastname) &&
             c.Spouselastname.ToLower() == request.SpouseLastName.ToLower()))

             ||

             //Rule 19
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 20
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 21
             (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 22
             (!string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 23
             (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 24
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower())))

             ||

             //Rule 25
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower()))

             ||

             //Rule 26
             ((!string.IsNullOrWhiteSpace(request.FatherFirstName) &&
             !string.IsNullOrWhiteSpace(c.Fatherfirstname) &&
             c.Fatherfirstname.ToLower() == request.FatherFirstName.ToLower()) ||
             (!string.IsNullOrWhiteSpace(request.SpouseFirstName) &&
             !string.IsNullOrWhiteSpace(c.Spousefirstname) &&
             c.Spousefirstname.ToLower() == request.SpouseFirstName.ToLower()))

             ||

             //Rule 27
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower())

             ||

             //Rule 28
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower())

             ||

             //Rule 29
             (!string.IsNullOrWhiteSpace(request.FirstName) &&
             !string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Firstname) &&
             c.Firstname.ToLower() == request.FirstName.ToLower() &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower())

             ||

             //Rule 30
             (!string.IsNullOrWhiteSpace(request.LastName) &&
             !string.IsNullOrWhiteSpace(c.Lastname) &&
             c.Lastname.ToLower() == request.LastName.ToLower() &&
             !string.IsNullOrWhiteSpace(request.MiddleName) &&
             !string.IsNullOrWhiteSpace(c.Middlename) &&
             c.Middlename.ToLower() == request.MiddleName.ToLower())
        );

        return await ambiguousMatchQuery.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<Customer> CreateCustomerAsync(DeduplicationData request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Customer customer = request.ToCustomer();

        await _dbContext.Customers.AddAsync(customer, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return customer;
    }

    private async Task<Customermapping> CreateCustomerMappingAsync(int customerId, string externalCustomerId, CancellationToken cancellationToken)
    {
        Customermapping customerMapping = new Customermapping
        {
            Customerid = customerId,
            Externalcustomerid = externalCustomerId
        };
        await _dbContext.Customermappings.AddAsync(customerMapping, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return customerMapping;
    }

    public async Task<bool> UpdateCustomerAsync(DeduplicationData request, string? correlationId, string? originSystem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(correlationId);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(originSystem);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(request.ExternalCustomerId);

        if (request.CustomerId is 0)
        {
            throw TruhomeExceptions.TE404("Customer");
        }

        await _dbContext.Customeraudits.AddAsync(request.ToCustomerAudit(correlationId, originSystem), cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        Customer? customer = await _dbContext.Customers.FindAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);

        if (customer == null)
        {
            throw new Exception($"Customer not found with CustomerId {request.CustomerId}");
        }
        customer = request.ToCustomer();

        _dbContext.Entry(customer).State = EntityState.Modified;
        bool isSuccess = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;

        if (!await IsCustomerMappingExistsAsync(customer.Id, request.ExternalCustomerId))
        {
            Customermapping? customermapping = await CreateCustomerMappingAsync(customer.Id, request.ExternalCustomerId, cancellationToken);

            isSuccess = customermapping.Id > 0;
        }
        return true;
    }

    private async Task<bool> IsCustomerMappingExistsAsync(int customerId, string externalCustomerId)
    {
        Customermapping? customermapping = await _dbContext.Customermappings.FirstOrDefaultAsync(x => x.Customerid == customerId && x.Externalcustomerid == externalCustomerId).ConfigureAwait(false);

        return customermapping != null;
    }
}
