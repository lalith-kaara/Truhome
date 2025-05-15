using Microsoft.EntityFrameworkCore;
using Truhome.Business.Interfaces;
using Truhome.Business.Mappers;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;
using Truhome.Domain.Contexts;
using Truhome.Domain.Entities;
using MatchType = Truhome.Business.Enums.MatchType;

namespace Truhome.Business.Managers;

public class DeduplicationManager : IDeduplicationManager
{
    private readonly TruhomeDbContext _dbContext;
    public DeduplicationManager(TruhomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeduplicationResponse> CheckDeduplicationAsync(DeduplicationData request, string? correlationId, string? originSystem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _dbContext.Customerrequestlogs.AddAsync(request.ToCustomerRequestLog(correlationId, originSystem), cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        List<Customer> customers = new List<Customer>();

        List<Match> matches = new List<Match>();

        var query = _dbContext.Customers.AsNoTracking().Where(c =>
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
            (!string.IsNullOrWhiteSpace(request.CkycNumber) &&
             !string.IsNullOrWhiteSpace(c.Ckycnumber) &&
             c.Ckycnumber.ToLower() == request.CkycNumber.ToLower())
        );


        var exactlyMatchedCustomers = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

        matches.AddRange(exactlyMatchedCustomers.AsParallel().Select(x => x.ToMatch(MatchType.Exact.ToString())).ToList());

        return new DeduplicationResponse
        {
            MatchCount = matches.Count,
            Matches = matches
        };
    }
}
