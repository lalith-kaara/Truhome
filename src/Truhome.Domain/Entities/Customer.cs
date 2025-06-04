using System;
using System.Collections.Generic;

namespace Truhome.Domain.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Lastname { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public long? Mobilenumber { get; set; }

    public string? Drivinglicensenumber { get; set; }

    public string? Passportnumber { get; set; }

    public string? Pannumber { get; set; }

    public string? Aadharnumber { get; set; }

    public string? Ckycid { get; set; }

    public string? Voterid { get; set; }

    public string? Fatherfirstname { get; set; }

    public string? Fathermiddlename { get; set; }

    public string? Fatherlastname { get; set; }

    public string? Spousefirstname { get; set; }

    public string? Spousemiddlename { get; set; }

    public string? Spouselastname { get; set; }

    public string? Mothermaidenname { get; set; }

    public string? Emailid { get; set; }

    public string? Gender { get; set; }

    public long? Alternatemobilenumber { get; set; }

    public string? Companyname { get; set; }

    public string? Cin { get; set; }

    public string? Sourcesystem { get; set; }

    public short? Customertype { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
